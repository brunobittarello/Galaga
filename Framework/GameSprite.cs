using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GalagaFramework.Framework
{
    public class GameSprite
    {
        internal Vector2 positionEnd;
        internal Texture2D texture;
        internal Vector2 TextureSize { get; private set; }
        internal Rectangle? Frame { get; set; }
        internal float Rotation { get; set; }
        internal Vector2 RotationPoint { get; set; }

        protected float scale;
        public Vector2 Position { get; protected set; }
        public Color Color { get; set; }

        internal virtual float Scale
        {
            get
            {
                return scale;
            }
            set
            {
                scale = value;
            }
        }

        public GameSprite(Texture2D newTexture, Vector2? position = null, float newScale = 1, Color? color = null, Rectangle? frame = null)
        {
            texture = newTexture;
            TextureSize = new Vector2(texture.Width, texture.Height);
            positionEnd = new Vector2(texture.Width, texture.Height);
            scale = newScale;
            if (position != null)
                SetPosition(position.Value);
            if (color != null)
                Color = color.Value;
            else
                Color = Color.White;
            Frame = frame;
        }

        public void SetPosition(Vector2 newPosition)
        {
            Position = newPosition;
        }

        public void CenterInPosition(Vector2 position)
        {
            Position = position - (Size() / 2);
        }

        public bool IsPointInTexture(Vector2 point)
        {
            return (point.X >= Position.X && point.Y >= Position.Y && point.X < positionEnd.X && point.Y < positionEnd.Y);
        }

        public bool IsPointInRealSizeTexture(Vector2 point)
        {
            var position = (Position * ApplicationMemory.ScreenController.ScreenScale) + ApplicationMemory.ScreenController.Margin;
            var endPosition = position + RealSize();
            return (point.X >= position.X && point.Y >= position.Y && point.X < endPosition.X && point.Y < endPosition.Y);
        }

        public Vector2 Size()
        {
            return (Frame == null) ? (TextureSize * Scale) : (new Vector2(Frame.Value.Width, Frame.Value.Height) * Scale);
        }

        public Vector2 RealSize()
        {
            return Size() * ApplicationMemory.ScreenController.ScreenScale;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            var scale = Scale * ApplicationMemory.ScreenController.ScreenScale;
            var position = (Position * ApplicationMemory.ScreenController.ScreenScale) + ApplicationMemory.ScreenController.Margin;
            spriteBatch.Draw(texture, position, Frame, Color, Rotation, RotationPoint, scale, SpriteEffects.None, 0f);
        }
    }
}
