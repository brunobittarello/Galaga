using GalagaFramework.Framework;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GalagaFramework.Directors.SplashScreens
{
    class ControlsScreen : DirectorBase
    {
        DisplayText Text;
        DisplayText Text1;
        DisplayText Text2;
        DisplayText Text3;

        public ControlsScreen()
        {
            Text = new DisplayText(new Vector2(15, 96),  "  <-  or     A           = Move Left", 0.2f, Color.Cyan);
            Text1 = new DisplayText(new Vector2(15, 112), "  ->  or     D           = Move Right", 0.2f, Color.Magenta);
            Text2 = new DisplayText(new Vector2(15, 128), "Space or Right Ctrl      = Shoot", 0.2f, Color.Red);
            Text3 = new DisplayText(new Vector2(15, 232), "Fire to continue...", 0.2f);
        }
        public override void Update(GameTime gameTime)
        {
            if (ArcadeControl.ActionButtonPressedDown())
                IsDone = true;
        }

        public override void Draw(SpriteBatch spriteBatch) {
            Text.Draw(spriteBatch);
            Text1.Draw(spriteBatch);
            Text2.Draw(spriteBatch);
            Text3.Draw(spriteBatch);
        }
    }
}
