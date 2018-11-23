using GalagaFramework.Actors.Kinemactic;
using GalagaFramework.Framework;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GalagaFramework.Directors.SplashScreens
{
    class LifeEarningInstructionScreen : DirectorBase
    {
        GameSprite[] arrGalagaIcon;
        DisplayText[] arrText;
        Space Space;

        public LifeEarningInstructionScreen()
        {
            Space = new Space();
            arrGalagaIcon = new GameSprite[3];
            arrText = new DisplayText[5];

            var screenSize = ApplicationMemory.ScreenController.GridPosition;

            arrText[0] = new DisplayText(new Vector2(screenSize.X * 0.25f, screenSize.Y * 0.25f), "PUSH START BUTTON", 0.3f, Color.Cyan);
            arrGalagaIcon[0] = new GameSprite(TextureManager.Galaga, Vector2.Zero);
            arrGalagaIcon[0].CenterInPosition(new Vector2(screenSize.X * 0.05f, screenSize.Y * 0.4f));
            arrText[1] = new DisplayText(new Vector2(screenSize.X * 0.15f, screenSize.Y * 0.4f), "1ST BONUS FOR 20000 PTS", 0.3f, Color.Yellow);
            arrGalagaIcon[1] = new GameSprite(TextureManager.Galaga, Vector2.Zero);
            arrGalagaIcon[1].CenterInPosition(new Vector2(screenSize.X * 0.05f, screenSize.Y * 0.5f));
            arrText[2] = new DisplayText(new Vector2(screenSize.X * 0.15f, screenSize.Y * 0.5f), "2ST BONUS FOR 70000 PTS", 0.3f, Color.Yellow);
            arrGalagaIcon[2] = new GameSprite(TextureManager.Galaga, Vector2.Zero);
            arrGalagaIcon[2].CenterInPosition(new Vector2(screenSize.X * 0.05f, screenSize.Y * 0.6f));
            arrText[3] = new DisplayText(new Vector2(screenSize.X * 0.15f, screenSize.Y * 0.6f), "AND for EVERY 70000 PTS", 0.3f, Color.Yellow);
            arrText[4] = new DisplayText(new Vector2(screenSize.X * 0.25f, screenSize.Y * 0.8f), "© 1981 NAMCO LTD.", 0.3f, Color.White);
        }

        public override void Update(GameTime gameTime)
        {
            Space.Update(gameTime);
            if (ArcadeControl.ActionButtonPressedDown())
                IsDone = true;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Space.Draw(spriteBatch);
            arrText[0].Draw(spriteBatch);
            arrGalagaIcon[0].Draw(spriteBatch);
            arrText[1].Draw(spriteBatch);
            arrGalagaIcon[1].Draw(spriteBatch);
            arrText[2].Draw(spriteBatch);
            arrGalagaIcon[2].Draw(spriteBatch);
            arrText[3].Draw(spriteBatch);
            arrText[4].Draw(spriteBatch);
        }
    }
}
