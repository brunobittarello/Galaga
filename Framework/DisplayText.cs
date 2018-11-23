using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GalagaFramework.Framework
{
    public class DisplayText
    {
        public Vector2 Position;
        public string Text;
        public float Scale;
        public Color Color;

        public DisplayText(Vector2? position = null, string text = null, float scale = 1, Color? color = null)
        {
            Position = (position.HasValue) ? position.Value : Vector2.Zero;
            Text = text;
            Scale = scale;
            Color = (color.HasValue) ? color.Value : Color.White;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            var scale = Scale * ApplicationMemory.ScreenController.ScreenScale;
            var position = (Position * ApplicationMemory.ScreenController.ScreenScale) + ApplicationMemory.ScreenController.Margin;
            spriteBatch.DrawString(ApplicationMemory.Font, Text, position, Color, 0, Vector2.Zero, scale, SpriteEffects.None, 0f);
        }
    }
}
