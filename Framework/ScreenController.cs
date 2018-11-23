using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace GalagaFramework.Framework
{
    public class ScreenController
    {
        internal Vector2 GridPosition { get; private set; }
        internal Vector2 Resolution { get; private set; }
        internal Vector2 InternalResolution { get; private set; }
        internal Vector2 Margin { get; private set; }
        internal float ScreenScale { get; private set; }

        Texture2D Background;
        GraphicsDeviceManager Graphics;
        bool update;
        Vector2? preMinimize;

        internal ScreenController(GraphicsDeviceManager graphics, float widht, float height)
        {
            //GridPosition = new Vector2(240, 320);
            GridPosition = new Vector2(224, 288);
            Graphics = graphics;
            SetResolution(widht, height);
        }

        internal void SetResolution(float widht, float height)
        {
            if ((height == 0 && update) || (widht == Resolution.X && height == Resolution.Y))
                return;

            update = true;
            if (widht == 0 && height == 0 && preMinimize == null)
            {
                preMinimize = Resolution;
                Resolution = Vector2.Zero;
                //TODO: Pause
            }
            else if (preMinimize != null)
            {
                Resolution = preMinimize.Value;
                preMinimize = null;
            }
            else
                Resolution = new Vector2(widht, height);

            InternalResolution = new Vector2((int)((height / 4) * 3), height);
            if (InternalResolution.X > Resolution.X)
                Resolution = new Vector2(InternalResolution.X, height);

            ScreenScale = InternalResolution.X / GridPosition.X;
            Margin = new Vector2((Resolution.X / 2) - (InternalResolution.X / 2), 0);
        }

        internal Vector2 PointInInternalResolution(Vector2 point)
        {
            return (point - Margin) / ScreenScale;
        }

        internal void Update()
        {
            if (update)
            {
                Graphics.PreferredBackBufferWidth = (int)Resolution.X;
                Graphics.PreferredBackBufferHeight = (int)Resolution.Y;
                Graphics.ApplyChanges();
                update = false;
            }
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            if (Background == null)
                Background = TextureManager.FleetGrinded;

            var size = new Vector2(InternalResolution.X / Background.Width, InternalResolution.Y / Background.Height);
            var position = new Vector2((Resolution.X / 2) - (InternalResolution.X / 2), 0);
            //spriteBatch.Draw(Background, position, null, Color.Gray, 0f, Vector2.Zero, size, SpriteEffects.None, 0f);
        }

        internal void LateDraw(SpriteBatch spriteBatch)
        {
            var texture = TextureManager.Empty;
            var size = new Vector2((((Resolution.X / 2) - (InternalResolution.X / 2)) / texture.Width), Resolution.Y / texture.Height);
            spriteBatch.Draw(texture, Vector2.Zero, null, Color.Black, 0f, Vector2.Zero, size, SpriteEffects.None, 0f);
            var position = new Vector2((Resolution.X / 2) + (InternalResolution.X / 2), 0);
            spriteBatch.Draw(texture, position, null, Color.Black, 0f, Vector2.Zero, size, SpriteEffects.None, 0f);
        }

        internal void InternalBorders(SpriteBatch spriteBatch)
        {
            var texture = TextureManager.Empty;
            var size = new Vector2(InternalResolution.X / texture.Width, (InternalResolution.Y / (texture.Height * 20)));
            var position = new Vector2((Resolution.X / 2) - (InternalResolution.X / 2), 0);
            spriteBatch.Draw(texture, position, null, Color.Black, 0f, Vector2.Zero, size, SpriteEffects.None, 0f);

            position = new Vector2((Resolution.X / 2) - (InternalResolution.X / 2), InternalResolution.Y - 20 * ScreenScale);
            size.Y *= 1.4f;
            spriteBatch.Draw(texture, position, null, Color.Black, 0f, Vector2.Zero, size, SpriteEffects.None, 0f);
        }
    }
}
