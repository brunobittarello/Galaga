using GalagaFramework.Actors.Kinemactic;
using GalagaFramework.Directors;
using GalagaFramework.Directors.Gameplays;
using GalagaFramework.Directors.SplashScreens;
using GalagaFramework.Framework;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GalagaFramework
{
    public class Game1 : GameHelper
    {
        DirectorBase Director;

        public Game1()
            : base()
        {

        }

        protected override void LoadScene()
        {
            Director = new StudioLogo();
            //Director = new Gameplay(this);
        }

        protected override void UpdateScene(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (Keyboard.GetState().IsKeyDown(Keys.R))
                Director = new Gameplay(this);

            Director.Update(gameTime);
            if (Director.IsDone)
                NextDirector();
        }

        void NextDirector()
        {
            if (Director is StudioLogo)
                Director = new ControlsScreen();
            else if (Director is ControlsScreen)
                Director = new AvengersVideo();
            else if (Director is AvengersVideo)
                Director = new LoadGameVideo();
            else if (Director is LoadGameVideo)
                Director = new GameDemoVideo();
            else if (Director is GameDemoVideo)
                Director = new LifeEarningInstructionScreen();
            else if (Director is LifeEarningInstructionScreen)
                Director = new Gameplay(this);
            else if (Director is Gameplay)
                Director = new GameDemoVideo();
        }

        protected override void DrawScene(GameTime gameTime)
        {
            Director.Draw(SpriteBatch);
        }
    }
}
