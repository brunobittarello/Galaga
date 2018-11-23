using GalagaFramework.Actors.Dynamic.Powers;
using GalagaFramework.Actors.Kinemactic.Squad;
using GalagaFramework.Directors.Gameplays;
using GalagaFramework.Framework;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;

namespace GalagaFramework.Actors.Dynamic.Ships.EnemyShip
{
    class Boss : EnemyShip
    {
        internal bool Hit { get; private set; }
        Vector2 attackSpot;
        float powerTimer;
        BossAttackStatus AttackStatus;
        PowerForce Power;

        public Boss(BezierCurve bezierCurve, int index, SquadController squadCtr = null)
            : base(TextureManager.Boss, bezierCurve, index, squadCtr)
        {
            Image.Frame = new Rectangle(0, 0, 15, 16);
            Image.RotationPoint = new Vector2(15, 16) / 2;
            Image.SetPosition(Position);
        }

        protected override void SpriteToDirection(Vector2 direction)
        {
            base.SpriteToDirection(direction);
        }

        protected override void Animation()
        {
            if (AnimationTime && ((int)TotalSeconds) % 2 == 1)
            {
                Image.Frame = new Rectangle(0, 0, 15, 16);
                AnimationTime = false;
            }
            else if (!AnimationTime && ((int)TotalSeconds) % 2 == 0)
            {
                Image.Frame = new Rectangle(16, 0, 15, 16);
                AnimationTime = true;
            }
        }

        internal void MarkHit()
        {
            Hit = true;
            Image = new GameSprite(TextureManager.BossBlue);
            Image.Frame = new Rectangle(0, 0, 15, 16);
            Image.RotationPoint = new Vector2(15, 16) / 2;
            Image.SetPosition(Position);
        }

        internal override void SetAttack()
        {
            base.SetAttack();
            attackSpot = GalagaFleet.NearPositionToHitGalaga(Position) + new Vector2(0, -72);
            AttackStatus = BossAttackStatus.ExitingFleet;
        }

        protected override void UpdateAttackig()
        {
            switch (AttackStatus)
            {
                case BossAttackStatus.ExitingFleet: UpdateExitingFleet(); break;
                case BossAttackStatus.GoingToSpot: UpdateGoingToSpot(); break;
                case BossAttackStatus.UsingPower: UpdateUsingPower(); break;
                case BossAttackStatus.GoingBottom: UpdateGoingBottom(); break;
            }

            Image.SetPosition(Position);
        }

        void UpdateExitingFleet()
        {
            base.PerformExitFleet();
            if (fleetExited == true)
                AttackStatus = BossAttackStatus.GoingToSpot;
        }

        void UpdateGoingToSpot()
        {
            var direction = attackSpot - Position;
            direction.Normalize();
            Position += direction * DIVE_VELOCITY * Deltatime;

            if (Vector2.Distance(Position, attackSpot) < 1.5f)
            {
                powerTimer = TotalSeconds;
                Power = new PowerForce(Position);
                AttackStatus = BossAttackStatus.UsingPower;
            }
        }

        void UpdateUsingPower()
        {
            Power.Update(GameTime);

            if (Power.IsDone)
            {
                Power = null;
                AttackStatus = BossAttackStatus.GoingBottom;
            }
        }

        void UpdateGoingBottom()
        {
            if (Gameplay.Status == GameplayStatus.Playing && DetectCollisions(base.Galagas.ToList<Dynamic>()))
            {
                ExplodeWithGalaga();
                return;
            }

            Position += new Vector2(0, 1) * DIVE_VELOCITY * Deltatime;
            if (Position.Y > ApplicationMemory.ScreenController.GridPosition.Y + Image.TextureSize.Y)
            {
                Position.Y = -Image.TextureSize.Y;
                base.TryToEnterInFleet();
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Power != null)
                Power.Draw(spriteBatch);

            base.Draw(spriteBatch);
        }
    }

    enum BossAttackStatus
    {
        ExitingFleet,
        GoingToSpot,
        UsingPower,
        GoingBottom,
    }
}
