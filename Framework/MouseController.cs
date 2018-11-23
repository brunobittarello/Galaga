using GalagaFramework.Framework.MouseUtil;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GalagaFramework.Framework
{
    public static class MouseController
    {
        static MouseButton Left;
        static MouseButton Right;

        //Left
        static public bool LeftClickDown
        {
            get
            {
                return Left.ClickDown;
            }
        }
        static public bool LeftClickReleased
        {
            get
            {
                return Left.ClickReleased;
            }
        }
        static public bool LeftDraging
        {
            get
            {
                return Left.Draging;
            }
        }
        static public Vector2 LeftClickPosition
        {
            get
            {
                return Left.ClickPosition;
            }
        }

        //Right
        static public bool RightClickDown
        {
            get
            {
                return Right.ClickDown;
            }
        }
        static public bool RightClickReleased
        {
            get
            {
                return Right.ClickReleased;
            }
        }
        static public bool RightDraging
        {
            get
            {
                return Right.Draging;
            }
        }
        static public Vector2 RightClickPosition
        {
            get
            {
                return Right.ClickPosition;
            }
        }

        static MouseState State;
        static MouseState LastState;
        static public Vector2 CurrentPosition;
        static public Vector2 CurrentIntPosition;

        static MouseController()
        {
            State = new MouseState(0, 0, 0, ButtonState.Released, ButtonState.Released, ButtonState.Released, ButtonState.Released, ButtonState.Released);
            Left = new MouseButton();
            Right = new MouseButton();
        }

        public static void Update()
        {
            LastState = State;
            State = Mouse.GetState();
            CurrentPosition = new Vector2(State.X, State.Y);
            CurrentIntPosition = ApplicationMemory.ScreenController.PointInInternalResolution(CurrentPosition);

            UpdateMouseButton(Left, State.LeftButton, LastState.LeftButton);
            UpdateMouseButton(Right, State.RightButton, LastState.RightButton);
        }

        static void UpdateMouseButton(MouseButton button, ButtonState current, ButtonState last)
        {
            if (current == ButtonState.Pressed && last == ButtonState.Released)
            {
                button.ClickDown = true;
                button.ClickReleased = false;
                button.ClickPosition = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
            }

            if (!button.Draging && button.ClickDown && Vector2.Distance(button.ClickPosition, CurrentPosition) > 2)
            {
                button.Draging = true;
            }

            if (current == ButtonState.Released && last == ButtonState.Pressed)
            {
                button.ClickReleased = true;
                button.ClickDown = false;
                button.Draging = false;
                button.ClickPosition = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
            }
        }
    }
}
