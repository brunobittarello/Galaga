using Microsoft.Xna.Framework.Input;

namespace GalagaFramework.Framework
{
    public static class ArcadeControl
    {
        static KeyboardState Last;
        static KeyboardState KeyboardState;

        static public void Update()
        {
            Last = KeyboardState;
            KeyboardState = Keyboard.GetState();
        }

        static public bool LeftButtonPressed()
        {
            return Keyboard.GetState().IsKeyDown(Keys.A) || Keyboard.GetState().IsKeyDown(Keys.Left);
        }

        static public bool LeftButtonPressedDown()
        {
            return (Keyboard.GetState().IsKeyDown(Keys.A) && !Last.IsKeyDown(Keys.A)) || (Keyboard.GetState().IsKeyDown(Keys.Left) && !Last.IsKeyDown(Keys.Left));
        }

        static public bool RightButtonPressed()
        {
            return Keyboard.GetState().IsKeyDown(Keys.D) || Keyboard.GetState().IsKeyDown(Keys.Right);
        }

        static public bool RightButtonPressedDown()
        {
            return (Keyboard.GetState().IsKeyDown(Keys.D) && !Last.IsKeyDown(Keys.D)) || (Keyboard.GetState().IsKeyDown(Keys.Right) && !Last.IsKeyDown(Keys.Right));
        }

        static public int HorizontalPressedDown()
        {
            return ((LeftButtonPressedDown())?-1:0) + ((RightButtonPressedDown()?1:0));
        }

        static public bool ActionButtonPressed()
        {
            return Keyboard.GetState().IsKeyDown(Keys.Space) || Keyboard.GetState().IsKeyDown(Keys.LeftControl);
        }

        static public bool ActionButtonPressedDown()
        {
            return (Keyboard.GetState().IsKeyDown(Keys.Space) && !Last.IsKeyDown(Keys.Space)) || (Keyboard.GetState().IsKeyDown(Keys.LeftControl) && !Last.IsKeyDown(Keys.LeftControl));
        }

        static public bool DebugButtonPressedDown()
        {
            return Keyboard.GetState().IsKeyDown(Keys.B) && !Last.IsKeyDown(Keys.B);
        }
        /*
        static public bool StartButtonPressed()
        {
            return Keyboard.GetState().IsKeyDown(Keys.Enter);
        }
         * */
    }
}
