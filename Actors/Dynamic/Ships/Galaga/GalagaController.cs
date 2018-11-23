using GalagaFramework.Actors.Dynamic.Bullets;
using GalagaFramework.Framework;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace GalagaFramework.Actors.Dynamic.Ships.Galaga
{
    public class GalagaController : Dynamic
    {
        const float MAX_VELOCITY = 90;
        float MaxLeft;
        float MaxRight;

        GalagaPosition FleetPosition;
        internal GalagaStatus Status { get; set; }

        public GalagaController(Vector2 position)
            : base(TextureManager.Galaga)
        {
            SetFleetPosition(GalagaPosition.Center);
            Status = GalagaStatus.PlayerControling;

            Position = position;
            
            Image.CenterInPosition(Position);
        }

        public override void Update(GameTime gameTime)
        {
            GameTime = gameTime;

            switch (Status)
            {
                case GalagaStatus.PlayerControling:
                    UpdatePlayerControling();
                    break;
                case GalagaStatus.Spining:
                    UpdateSpining();
                    break;
            }
        }

        void UpdatePlayerControling()
        {
            if (ArcadeControl.LeftButtonPressed())
                Move(false);
            if (ArcadeControl.RightButtonPressed())
                Move(true);
        }

        void UpdateSpining()
        {
            Image.RotationPoint = new Vector2(TextureManager.Galaga.Width / 2, TextureManager.Galaga.Height / 2);
            Image.Rotation += 0.25f;
            Image.CenterInPosition(Position);
        }

        internal void SetAbductionConditions(Vector2 newPosition)
        {
            Position = newPosition;
            Image.Scale -= 0.075f * Deltatime;
            Image.CenterInPosition(Position);
        }

        internal void SetFleetPosition(GalagaPosition fleetPosition)
        {
            Image.RotationPoint = Vector2.Zero;
            FleetPosition = GalagaPosition.Center;
            switch (fleetPosition)
            {
                case GalagaPosition.Center:
                    MaxLeft = TextureManager.Galaga.Width / 2;
                    MaxRight = ApplicationMemory.ScreenController.GridPosition.X - MaxLeft;
                    break;
                case GalagaPosition.Left:
                    MaxLeft = TextureManager.Galaga.Width / 2;
                    MaxRight = (ApplicationMemory.ScreenController.GridPosition.X - MaxLeft) - TextureManager.Galaga.Width;
                    break;
                case GalagaPosition.Right:
                    MaxLeft = TextureManager.Galaga.Width + (TextureManager.Galaga.Width / 2);
                    MaxRight = ApplicationMemory.ScreenController.GridPosition.X - (TextureManager.Galaga.Width / 2);
                    break;
            }
        }

        internal bool Align(Vector2 targetPosition)
        {
            if (Position.X != targetPosition.X)
            {
                Position.X += ((Position.X > targetPosition.X) ? -100 : 100) * Deltatime;
                if (Math.Abs(Position.X - targetPosition.X) < 1)
                    Position.X = targetPosition.X;
                Image.CenterInPosition(Position);
                return false;
            }

            if (Position.Y != targetPosition.Y)
            {
                Position.Y += 100 * Deltatime;
                if (Position.Y > targetPosition.Y)
                    Position.Y = targetPosition.Y;
                Image.CenterInPosition(Position);
                return false;
            }

            return true;
        }

        void Move(bool right)
        {
            var direction = (((right) ? -MAX_VELOCITY : MAX_VELOCITY) * Deltatime);
            Position.X = MathHelper.Clamp(Position.X - direction, MaxLeft, MaxRight);
            Image.CenterInPosition(Position);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Image.Draw(spriteBatch);
        }
    }

    enum GalagaPosition
    {
        Center,
        Left,
        Right,
    }

    enum GalagaStatus
    {
        PlayerControling,
        Spining,
        Aligning,
        Exploding,
    }
}
