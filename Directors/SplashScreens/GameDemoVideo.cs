using GalagaFramework.Framework;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;

namespace GalagaFramework.Directors.SplashScreens
{
    class GameDemoVideo : VideoSplashScreen
    {
        public GameDemoVideo()
            : base(VideoManager.VideoDemo)
        {
            Scale = 0.4f;
        }

        public override void Update(GameTime gameTime)
        {
            if (VideoManager.Player.State == MediaState.Stopped)
                VideoManager.Player.Play(Video);

            if (ArcadeControl.ActionButtonPressedDown())
            {
                VideoManager.Player.Stop();
                IsDone = true;
            }
        }
    }
}
