using GalagaFramework.Framework;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Drawing;

namespace GalagaFramework.Actors.Dynamic.Powers
{
    class CapturedGalaga : ActorBase
    {
        GameSprite Image;

        public CapturedGalaga(Vector2 position)
        {
            Position = position + new Vector2(0, 0);
            Image = new GameSprite(TextureManager.GalagaRed);
            Image.Frame = new Microsoft.Xna.Framework.Rectangle(0, 0, 15, 15);
            Image.RotationPoint = new Vector2(23, 0);
            Image.SetPosition(Position);
        }

        internal RectangleF Collider()
        {
            return new RectangleF(Position.X, Position.Y, 15, 15);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Image.Draw(spriteBatch);
        }
    }
}
