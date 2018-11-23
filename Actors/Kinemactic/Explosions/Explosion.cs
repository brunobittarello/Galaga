using GalagaFramework.Framework;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GalagaFramework.Actors.Kinemactic.Explosions
{
    class Explosion : Entity
    {
        GameSprite Image;
        float FrameTime;
        int frame;
        int totalFrames;

        public bool IsDone
        {
            get
            {
                return frame >= totalFrames;

            }
        }

        public Explosion(Vector2 position, bool IsEnemy, GameTime gameTime)
        {
            if (IsEnemy)
            {
                Image = new GameSprite(TextureManager.EnemyExplosion);
                totalFrames = 5;
            }
            else
            {
                totalFrames = 4;
                Image = new GameSprite(TextureManager.GalagaExplosion);
            }
            GameTime = gameTime;
            Image.Frame = new Rectangle(0, 0, 32, 32);
            Position = position;
            Image.CenterInPosition(position);
            FrameTime = TotalSeconds;
        }

        public override void Update(GameTime gameTime)
        {
            GameTime = gameTime;
            if (IsDone || TotalSeconds - FrameTime < 0.08f)
                return;

            FrameTime = TotalSeconds;
            frame++;
            Image.Frame = new Rectangle(Image.Frame.Value.X + 32, 0, 32, 32);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Image.Draw(spriteBatch);
        }
    }
}
