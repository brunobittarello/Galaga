using GalagaFramework.Actors.Dynamic.Bullets;
using GalagaFramework.Actors.Dynamic.Ships.EnemyShip;
using GalagaFramework.Actors.Kinemactic.Explosions;
using GalagaFramework.Actors.Kinemactic.Stage;
using GalagaFramework.Directors.Gameplays;
using GalagaFramework.Framework;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GalagaFramework.Actors.Kinemactic
{
    public class Fleet : ActorBase
    {
        // 0-3 - Boss
        // 4-19 - Moth
        // 20-39 - Hornet
        internal EnemyShip[] Ships { get; private set; }
        bool[] ShipsDestroied;
        internal List<EnemyShip> ShipToDestroy;
        internal List<Bullet> Bullets;
        internal List<Explosion> Explosions;

        bool allAling;
        bool goingRight;
        bool demonstration;
        const float MAX_VELOCITY = 20;//20
        const float MARGIN = 80;

        internal FleetStatus Status { get; set; }

        float expandValue;
        float FleetSoundTimer;

        StageController StageCtr;

        public Fleet(StageEntity stage)
        {
            Position = new Vector2((ApplicationMemory.ScreenController.GridPosition.X / 2), 40);
            Ships = new EnemyShip[40];
            ShipsDestroied = new bool[40];
            ShipToDestroy = new List<EnemyShip>();
            Bullets = new List<Bullet>();
            Explosions = new List<Explosion>();
            StageCtr = new StageController(stage);
        }

        public void Reset()
        {
            Position = new Vector2((ApplicationMemory.ScreenController.GridPosition.X / 2), 40);
            Ships = new EnemyShip[40];
            ShipsDestroied = new bool[40];
            ShipToDestroy = new List<EnemyShip>();
            Explosions = new List<Explosion>();
            allAling = false;
            goingRight = false;
            demonstration = false;
            expandValue = 0;
            StageCtr.Reset();
        }

        public void SetDemonstration()
        {
            demonstration = true;
            for (var i = 0; i < 40; i++)
            {
                var relativePos = RelativePosition(i);
                Ships[i] = new EnemyShip(Position + relativePos, i);
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (demonstration)
                return;

            GameTime = gameTime;

            switch (Status)
            {
                case FleetStatus.Forming: UpdateForming(); break;
                case FleetStatus.Attacking: UpdateAttack(); break;
                case FleetStatus.Idle: Expand(); break;
            }

            foreach (var ship in Ships)
                if (ship != null)
                    ship.Update(gameTime);

            UpdateDesotryedShips();
            UpdateExplosions();
            UpdateBullets();
        }

        void UpdateAttack()
        {
            if (GalagaFleet == null || GalagaFleet.Status == GalagaFleetStatus.Dead)
            {
                Status = FleetStatus.Idle;
                return;
            }

            Expand();
            Attack();
        }

        void UpdateForming()
        {
            Move();
            if (StageCtr.IsDone == true)
            {
                if (IsAllAling() && IsCentralized())
                    Status = FleetStatus.Attacking;
                return;
            }

            var shipsCreated = StageCtr.CreateStage();
            if (shipsCreated == null || shipsCreated.Count == 0)
                return;

            foreach (var ship in shipsCreated)
                Ships[ship.FleetIndex] = ship;
        }

        internal void BackToActivity()
        {
            if (IsAllAling() && IsCentralized())
                Status = FleetStatus.Attacking;
            else
                Status = FleetStatus.Forming;
        }

        void UpdateDesotryedShips()
        {
            if (ShipToDestroy.Count == 0)
                return;

            var ships = ShipToDestroy.ToArray();
            foreach (var ship in ships)
                if (TotalSeconds - ship.HitTime > 0.1f)
                {

                    ShipToDestroy.Remove(ship);
                    Explosions.Add(new Explosion(ship.Position, true, GameTime));
                }
        }

        void UpdateBullets()
        {
            if (Bullets.Count == 0)
                return;

            var bullets = Bullets.ToArray();
            foreach (var bullet in bullets)
            {
                bullet.Update(GameTime);
                if (bullet.IsDone)
                    Bullets.Remove(bullet);
            }
        }

        void UpdateExplosions()
        {
            if (Explosions.Count == 0)
                return;

            var explosions = Explosions.ToArray();
            foreach (var explosion in explosions)
            {
                explosion.Update(GameTime);
                if (explosion.IsDone)
                    Explosions.Remove(explosion);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            foreach (var ship in Ships)
                if (ship != null)
                    ship.Draw(spriteBatch);

            foreach (var ship in ShipToDestroy)
                ship.Draw(spriteBatch);

            foreach (var explosion in Explosions)
                explosion.Draw(spriteBatch);

            foreach (var bullet in Bullets)
                bullet.Draw(spriteBatch);
        }

        internal void AddShipInFleet(EnemyShip enemy, int index)
        {
            var relativePosition = RelativePosition(index);
            Vector2 futurePosition;
            if (Status == FleetStatus.Forming)
            {
                futurePosition = EnterPositionMoving(relativePosition, enemy.Position);
                enemy.SetFleetPosition(futurePosition, relativePosition, false);
            }
            else
            {
                futurePosition = EnterPositionExpanding(relativePosition, enemy.Position);
                enemy.SetFleetPosition(futurePosition, relativePosition, true);
            }
        }

        internal void AddBullet(Bullet bullet)
        {
            Bullets.Add(bullet);
        }

        Vector2 RelativePosition(int index)
        {
            if (index < 4)
            {
                return new Vector2((index * 16) - 32 + 8, 0);
            }
            else if (index < 20)
            {
                index -= 4;
                return new Vector2(((index > 7) ? ((index - 8) * 16) : (index * 16)) - (16 * 4) + 8, (index > 7) ? 28 : 14);
            }
            else
            {
                index -= 20;
                return new Vector2(((index > 9) ? ((index - 10) * 16) : (index * 16)) - (16 * 5) + 8, (index > 9) ? 56 : 42);
            }
        }

        internal void DesotroyShip(EnemyShip ship, bool IsFromShip = false)
        {
            Gameplay.IncrementHits();
            if (IsFromShip == false && ship is Boss && !(ship as Boss).Hit)
            {
                (ship as Boss).MarkHit();
                SoundManager.EnemyShipHit.Play();
                return;
            }

            if (ship is Boss)
                SoundManager.BossExplosion.Play();
            else
                SoundManager.EnemyShipHit.Play();

            ship.StartToExplode();
            Gameplay.ScorePoint(ship);
            ShipToDestroy.Add(ship);
            for (var i = 0; i < 40; i++)
                if (Ships[i] == ship)
                {
                    Ships[i] = null;
                    ShipsDestroied[i] = true;
                    return;
                }
        }

        void Move()
        {
            var direction = (float)(((goingRight) ? MAX_VELOCITY : -MAX_VELOCITY) * Deltatime);

            Position.X += direction;
            if (Position.X < MARGIN)
            {
                Position.X = MARGIN + (MARGIN - Position.X);
                goingRight = !goingRight;
            }
            else if (Position.X > ApplicationMemory.ScreenController.GridPosition.X - MARGIN)
            {
                Position.X += (ApplicationMemory.ScreenController.GridPosition.X - MARGIN) - Position.X;
                goingRight = !goingRight;
            }

            //TODO: test this code instead the one above
            //Position.X = MathHelper.Clamp(Position.X + direction, MARGIN, ApplicationMemory.ScreenController.GridPosition.X - MARGIN);
            //if (Position.X == MARGIN || Position.X == ApplicationMemory.ScreenController.GridPosition.X - MARGIN)
            //    goingRight = !goingRight;

            foreach (var ship in Ships)
                if (ship != null && ship.State == EnemyShipState.InFleet)
                    ship.Move(Position.X);
        }

        Vector2 EnterPositionMoving(Vector2 relativePosition, Vector2 shipPosition)
        {
            //font: http://www.gamasutra.com/blogs/KainShin/20090515/83954/Predictive_Aim_Mathematics_for_AI_Targeting.php
            var targetPosition0 = Position + relativePosition;
            var theta = Math.Atan2(targetPosition0.Y - shipPosition.Y, targetPosition0.X - shipPosition.X);

            var distance = Vector2.Distance(targetPosition0, shipPosition);
            var V = (float)((goingRight) ? MAX_VELOCITY : -MAX_VELOCITY);
            var shipVelocity = EnemyShip.MAX_VELOCITY;

            float t = (float)((-2 * distance * V * Math.Cos(theta) - Math.Sqrt((2 * distance * V * Math.Cos(theta)) * (2 * distance * V * Math.Cos(theta)) + 4 * ((shipVelocity * shipVelocity) - (V * V)) * (distance * distance))) / (2 * ((shipVelocity * shipVelocity) - (V * V))));
            if (t < 0)
                t = (float)((-2 * distance * V * Math.Cos(theta) + Math.Sqrt((2 * distance * V * Math.Cos(theta)) * (2 * distance * V * Math.Cos(theta)) + 4 * ((shipVelocity * shipVelocity) - (V * V)) * (distance * distance))) / (2 * ((shipVelocity * shipVelocity) - (V * V))));
            //var Vb = ((targetPosition0 - shipPosition) / t);
            //Vb.Normalize();


            var fleetFuturePositionX = (Position + (new Vector2(V, 0) * t)).X;
            if (goingRight && fleetFuturePositionX > ApplicationMemory.ScreenController.GridPosition.X - MARGIN)
                targetPosition0.X = (targetPosition0 + (new Vector2(V, 0) * t)).X;
            else if (!goingRight && fleetFuturePositionX < MARGIN)
                targetPosition0.X = (targetPosition0 + (new Vector2(V, 0) * t)).X;
            else//No bounce
                return targetPosition0 + (new Vector2(V, 0) * t);


            theta = Math.Atan2(targetPosition0.Y - shipPosition.Y, targetPosition0.X - shipPosition.X);

            distance = Vector2.Distance(targetPosition0, shipPosition);
            V *= -1;

            t = (float)((-2 * distance * V * Math.Cos(theta) - Math.Sqrt((2 * distance * V * Math.Cos(theta)) * (2 * distance * V * Math.Cos(theta)) + 4 * ((shipVelocity * shipVelocity) - (V * V)) * (distance * distance))) / (2 * ((shipVelocity * shipVelocity) - (V * V))));
            if (t < 0)
                t = (float)((-2 * distance * V * Math.Cos(theta) + Math.Sqrt((2 * distance * V * Math.Cos(theta)) * (2 * distance * V * Math.Cos(theta)) + 4 * ((shipVelocity * shipVelocity) - (V * V)) * (distance * distance))) / (2 * ((shipVelocity * shipVelocity) - (V * V))));


            var targetPositionT = targetPosition0 + (new Vector2(V, 0) * t);
            return targetPositionT;

        }

        Vector2 EnterPositionExpanding(Vector2 relativePosition, Vector2 shipPosition)
        {
            var expandFuture = expandValue + 0.2f * ((goingRight) ? 0.5f : -0.5f);
            if (expandFuture < 0 || expandFuture > 1.4f)
                expandFuture = expandValue + 0.2f * ((goingRight) ? -0.5f : 0.5f);

            return Position + relativePosition * expandFuture;
        }

        bool IsAllAling()
        {
            if (allAling == true)
                return true;

            for (var i = 0; i < 40; i++)
                if (Ships[i] == null)
                {
                    if (ShipsDestroied[i] == false)
                        return false;
                }
                else if (Ships[i].State != EnemyShipState.InFleet)
                    return false;

            allAling = true;
            return true;
        }

        bool IsCentralized()
        {
            if (Position.X == (ApplicationMemory.ScreenController.GridPosition.X / 2))
                return true;

            if (Math.Abs(Position.X - (ApplicationMemory.ScreenController.GridPosition.X / 2)) < 1)
            {
                Position.X = ApplicationMemory.ScreenController.GridPosition.X / 2;
                return true;
            }

            return false;
        }

        internal bool IsAllDesotryed()
        {
            return !ShipsDestroied.Any(x => x == false) && Explosions.Count == 0 && Bullets.Count == 0 && ShipToDestroy.Count == 0;
        }

        void Expand()
        {
            if (TotalSeconds - FleetSoundTimer > (float)SoundManager.FleetSound.Duration.TotalSeconds)
            {
                FleetSoundTimer = TotalSeconds;
            SoundManager.FleetSound.Play();
            }

            expandValue = MathHelper.Clamp(expandValue + ((goingRight) ? 0.2f : -0.2f) * Deltatime, 1, 1.4F);
            if (expandValue == 1.4F)
                goingRight = false;
            else if (expandValue == 1)
                goingRight = true;

            foreach (var ship in Ships)
                if (ship != null && ship.State == EnemyShipState.InFleet)
                    ship.Expand(Position, expandValue);
        }

        void Attack()
        {
            var attackingShips = Ships.Where(x => x != null && x.State == EnemyShipState.Attacking).ToArray();
            if (attackingShips.Length < 2)
                DetermineShipAttack();
        }

        void DetermineShipAttack()
        {
            var availableShips = Ships.Where(x => x != null && x.State == EnemyShipState.InFleet).ToArray();

            if (availableShips.Length == 0)
                return;
            if (availableShips.Length == 2)
            {
                foreach (var ship in availableShips)
                    ship.SetAttack();
                return;
            }

            var preferenceCopy = ((int[])preference.Clone()).ToList();
            var numChoose = 0;
            var fitness = 0;
            do
            {
                if (availableShips.Contains(Ships[preferenceCopy[0]]))
                {
                    numChoose++;
                    Ships[preferenceCopy[0]].SetAttack();
                }
                preferenceCopy.RemoveAt(0);
            } while (numChoose != 2 && preferenceCopy.Count != 0);
        }

        int[] preference = new int[40] { 4, 20, 0, 11, 29, 12, 30, 19, 39, 05, 21, 03, 10, 28, 13, 31, 18, 38, 6, 22, 9, 27, 14, 32, 1, 8, 37, 7, 17, 26, 36, 15, 23, 33, 2, 16, 25, 35, 24, 34 };
    }

    enum FleetStatus
    {
        Forming,
        Attacking,
        Idle
    }
}

//--------00-01-02-03-
//---04-05-06-07-08-09-10-11
//---12-13-14-15-16-17-18-19
//20-21-22-23-24-25-26-27-28-29
//30-31-32-33-34-35-36-37-38-39
