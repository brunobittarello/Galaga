using GalagaFramework.Actors.Dynamic.Ships.EnemyShip;
using GalagaFramework.Actors.Dynamic.Ships.Galaga;
using GalagaFramework.Directors;
using GalagaFramework.Directors.Gameplays;
using GalagaFramework.Framework;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;

namespace GalagaFramework.Actors.Dynamic.Bullets
{
    class Bullet : Dynamic
    {
        internal bool IsDone { get; private set; }
        bool IsEnemyBullet;
        Vector2 Direction;
        internal Bullet Pair { get; set; }

        public Bullet(Vector2 position, bool isEnemyBullet = false)
            : base(TextureManager.GalagaBullet)
        {
            SoundManager.Shot.Play();
            IsEnemyBullet = isEnemyBullet;
            Position = position;
            Image.CenterInPosition(Position);

            if (isEnemyBullet)
            {
                if (GalagaFleet == null)
                    return;
                Direction = GalagaFleet.NearPositionToHitGalaga(Position) - Position;
                Direction.Normalize();
                Direction *= 175;
            }
            else
                Direction = new Vector2(0, -300);
        }

        public override void Update(GameTime gameTime)
        {
            if (IsDone)
                return;

            Position += Direction * Deltatime;
            Image.CenterInPosition(Position);

            if (Pair != null && Pair.IsDone)
                IsDone = true;

            if (Position.Y < 0 || Position.Y > ApplicationMemory.ScreenController.GridPosition.Y)
                IsDone = true;

            if (Gameplay.Status != GameplayStatus.Playing)
                return;

            if (IsEnemyBullet && DetectCollisions(base.Galagas.ToList<Dynamic>()))
            {
                IsDone = true;
                GalagaFleet.DestroyGalaga(Collisions.First() as GalagaController);
            }
            else if (!IsEnemyBullet && DetectCollisions(base.Ships.ToList<Dynamic>()))
            {
                IsDone = true;
                Fleet.DesotroyShip(Collisions.First() as EnemyShip);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Image.Draw(spriteBatch);
        }
    }
}
