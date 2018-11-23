using GalagaFramework.Framework;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;

namespace GalagaFramework.Directors.SplashScreens
{
    abstract class VideoSplashScreen : DirectorBase
    {
        protected Video Video;
        protected float Timer;
        protected float Scale;

        public VideoSplashScreen(Video video)
        {
            Position = ApplicationMemory.ScreenController.GridPosition / 2;
            Video = video;
            VideoManager.Player.Play(video);
            Scale = 1;
        }

        public override void Update(GameTime gameTime)
        {
            if (ArcadeControl.ActionButtonPressedDown())
            {
                Timer = 0.05f;
                VideoManager.Player.Stop();
            }

            Timer = MathHelper.Clamp(Timer - Deltatime, 0, 1);

            if (Timer == 0 && VideoManager.Player.State == MediaState.Stopped)
                IsDone = true;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            try
            {
                var image = new GameSprite(VideoManager.Player.GetTexture(), Position, Scale);
                image.CenterInPosition(Position);
                image.Draw(spriteBatch);
            }
            catch
            {
                Console.WriteLine("Frame jumped!");
            }

        }
    }
}
