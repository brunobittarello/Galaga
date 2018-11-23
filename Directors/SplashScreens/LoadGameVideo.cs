using GalagaFramework.Framework;

namespace GalagaFramework.Directors.SplashScreens
{
    class LoadGameVideo : VideoSplashScreen
    {
        public LoadGameVideo()
            : base(VideoManager.VideoLoad)
        {
            Scale = 0.4f;
        }
    }
}
