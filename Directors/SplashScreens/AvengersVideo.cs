using GalagaFramework.Framework;

namespace GalagaFramework.Directors.SplashScreens
{
    class AvengersVideo : VideoSplashScreen
    {
        public AvengersVideo()
            : base(VideoManager.VideoAvengers)
        {
            Scale = 0.4f;
        }
    }
}
