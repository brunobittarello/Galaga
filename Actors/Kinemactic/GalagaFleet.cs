using GalagaFramework.Actors.Dynamic.Bullets;
using GalagaFramework.Actors.Dynamic.Ships.Galaga;
using GalagaFramework.Actors.Kinemactic.Explosions;
using GalagaFramework.Directors.Gameplays;
using GalagaFramework.Framework;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace GalagaFramework.Actors.Kinemactic
{
    public class GalagaFleet : ActorBase
    {
        internal GalagaController[] Galagas;
        internal List<Bullet> Bullets;
        internal List<Explosion> Explosions;
        GalagaController NewGalaga;
        Vector2[] AlignPositions;
        internal GalagaFleetStatus Status { get; private set; }
        float LastShoot;

        float Timer;

        Vector2 AbductionPoint;

        public GalagaFleet()
        {
            Galagas = new GalagaController[2];
            Explosions = new List<Explosion>();
            Bullets = new List<Bullet>();
            var position = new Vector2(ApplicationMemory.ScreenController.GridPosition.X / 2, ApplicationMemory.ScreenController.GridPosition.Y - TextureManager.Galaga.Height);
            Galagas[0] = new GalagaController(position);
            //
            // AssociateNewGalaga(new Vector2(100, 100));

            AlignPositions = new Vector2[2] {
                new Vector2((ApplicationMemory.ScreenController.GridPosition.X / 2) - (TextureManager.Galaga.Width / 2), ApplicationMemory.ScreenController.GridPosition.Y - TextureManager.Galaga.Height),
                new Vector2((ApplicationMemory.ScreenController.GridPosition.X / 2) + (TextureManager.Galaga.Width / 2), ApplicationMemory.ScreenController.GridPosition.Y - TextureManager.Galaga.Height)
            };
            Status = GalagaFleetStatus.Playing;
        }

        public override void Update(GameTime gameTime)
        {
            switch (Status)
            {
                case GalagaFleetStatus.Playing: UpdatePlay(); break;
                case GalagaFleetStatus.Aligning: UpdateAlign(); break;
                case GalagaFleetStatus.BeingAbducted: UpdateBeingAbducted(); break;
                case GalagaFleetStatus.Dead: UpdateDead(); break;
            }

            foreach (var galaga in Galagas)
                if (galaga != null)
                    galaga.Update(gameTime);

            var explosions = Explosions.ToArray();
            foreach (var explosion in explosions)
            {
                explosion.Update(gameTime);
                if (explosion.IsDone)
                    Explosions.Remove(explosion);
            }

            var bullets = Bullets.ToArray();
            foreach (var bullet in bullets)
            {
                bullet.Update(GameTime);
                if (bullet.IsDone)
                    Bullets.Remove(bullet);
            }
        }

        void UpdatePlay()
        {
            if (NewGalaga != null)
                StartAligningGalagas();

            if (ArcadeControl.DebugButtonPressedDown())
                AssociateNewGalaga(new Vector2(100, 100));

            if (ArcadeControl.ActionButtonPressedDown() && Gameplay.Status == GameplayStatus.Playing)
                Fire();
        }

        void UpdateAlign()
        {
            Align();

            if (NewGalaga != null)
                NewGalaga.Update(GameTime);
        }

        void UpdateDead()
        {
            Timer += Deltatime;
            if (Timer > 3)
                LostLife();
        }

        void UpdateBeingAbducted()
        {
            var galaga = Galagas[0];

            if (Vector2.Distance(AbductionPoint, galaga.Position) < 30)
            {

                return;
            }
            var x = MathHelper.Lerp(galaga.Position.X, AbductionPoint.X, 0.2f * Deltatime);
            var y = MathHelper.Lerp(galaga.Position.Y, AbductionPoint.Y, 0.5f * Deltatime);

            galaga.SetAbductionConditions(new Vector2(x, y));
        }

        internal void AbdutionBegins(Vector2 position)
        {
            AbductionPoint = position;
            Gameplay.SetGalagaCaptured();
            Status = GalagaFleetStatus.BeingAbducted;
            Galagas[0].Status = GalagaStatus.Spining;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (NewGalaga != null)
                NewGalaga.Draw(spriteBatch);

            foreach (var galaga in Galagas)
                if (galaga != null)
                    galaga.Draw(spriteBatch);

            foreach (var explosion in Explosions)
                explosion.Draw(spriteBatch);

            foreach (var bullet in Bullets)
                bullet.Draw(spriteBatch);
        }

        internal void AssociateNewGalaga(Vector2 position)
        {
            NewGalaga = new GalagaController(position);
            NewGalaga.Position = position;
            NewGalaga.Status = GalagaStatus.Spining;
        }

        internal void DestroyGalaga(GalagaController galaga, bool explosion = true)
        {
            if (explosion)
            {
                SoundManager.GalagaExplosion.Play();
                Explosions.Add(new Explosion(galaga.Position, false, GameTime));
            }
            if (Galagas[1] == null && Galagas[0] == galaga)
            {
                Galagas[0] = null;
                Status = GalagaFleetStatus.Dead;
                return;
            }

            if (Galagas[1] != null && Galagas[0] == galaga)
                Galagas[0] = Galagas[1];

            Galagas[1] = null;
            Galagas[0].SetFleetPosition(GalagaPosition.Center);
        }

        void LostLife()
        {
            Gameplay.LostLife();
        }

        internal Vector2 NearPositionToHitGalaga(Vector2 position)
        {
            if (Galagas[0] == null)
                    return Vector2.Zero;//Avoid errors
            if (Galagas[1] == null || Vector2.Distance(Galagas[1].Position, position) > Vector2.Distance(Galagas[0].Position, position))
                    return Galagas[0].Position;
            return Galagas[1].Position;
        }

        void StartAligningGalagas()
        {
            Gameplay.SetGalagaRescued();
            Status = GalagaFleetStatus.Aligning;
            if (NewGalaga.Position.X < Galagas[0].Position.X)
            {
                var temp = Galagas[0];
                Galagas[0] = NewGalaga;
                Galagas[1] = temp;
            }
            else
                Galagas[1] = NewGalaga;

            Galagas[0].SetFleetPosition(GalagaPosition.Left);
            Galagas[1].SetFleetPosition(GalagaPosition.Right);
            Galagas[0].Status = Galagas[1].Status = GalagaStatus.Aligning;
            NewGalaga = null;
        }

        void Align()
        {
            if (Galagas[0].Align(AlignPositions[0]) & Galagas[1].Align(AlignPositions[1]))
            {
                Status = GalagaFleetStatus.Playing;
                Galagas[0].Status = Galagas[1].Status = GalagaStatus.PlayerControling;
            }
        }

        void Fire()
        {
            if (TotalSeconds - LastShoot <= 0.1f)
                return;

            if ((Galagas[1] == null && Bullets.Count >= 2) || Bullets.Count > 2)
                return;

            LastShoot = TotalSeconds;
            Gameplay.IncrementShots();
            if (Galagas[1] == null)
            {
                Bullets.Add(new Bullet(Galagas[0].Position));
                return;
            }

            var bullet1 = new Bullet(Galagas[0].Position);
            var bullet2 = new Bullet(Galagas[1].Position);
            bullet1.Pair = bullet2;
            bullet2.Pair = bullet1;
            Bullets.Add(bullet1);
            Bullets.Add(bullet2);
        }
    }

    enum GalagaFleetStatus
    {
        Playing,
        BeingAbducted,
        Aligning,
        Dead,
    }
}
