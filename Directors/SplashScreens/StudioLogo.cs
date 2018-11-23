using GalagaFramework.Framework;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GalagaFramework.Directors.SplashScreens
{
    class StudioLogo : DirectorBase
    {
        DisplayText Text;
        DisplayText Text2;
        float FadeFactor;
        float Timer;
        bool IsFadingOut;

        public StudioLogo()
        {
            Text = new DisplayText(new Vector2(0, 110), "BITTARELLO", 0.8f, Color.Black);
            Text2 = new DisplayText(new Vector2(100, 160), "STUDIO", 0.8f, Color.Black);
        }

        public override void Update(GameTime gameTime)
        {
            if (IsFadingOut)
                FadeOut();
            else
                FadeIn();
        }

        void FadeIn()
        {
            Timer += Deltatime;
            if (Keyboard.GetState().GetPressedKeys().Length > 0 || Timer > 3)
                IsFadingOut = true;

            FadeFactor = MathHelper.Clamp(FadeFactor + 0.01f, 0, 1);
            Text.Color = Text2.Color = new Color(FadeFactor * Vector3.One);
        }

        void FadeOut()
        {
            FadeFactor = MathHelper.Clamp(FadeFactor - 0.01f, 0, 1);
            Text.Color = Text2.Color = new Color(FadeFactor * Vector3.One);

            if (FadeFactor == 0)
                IsDone = true;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Text.Draw(spriteBatch);
            Text2.Draw(spriteBatch);
        }
    }
}
