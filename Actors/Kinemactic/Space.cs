using GalagaFramework.Framework;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GalagaFramework.Actors.Kinemactic
{
    public class Space : ActorBase
    {
        GameSprite[] Images;
        GameSprite[] Borders;
        float FrameTimer;
        float FrameIndex;
        float Scale;
        internal SpaceStatus Status;

        readonly int Weight;
        readonly int Height;

        public Space()
        {
            Scale = 0.45f;
            Images = new GameSprite[2];
            Weight = (TextureManager.Space.Width / 4);
            Height = TextureManager.Space.Height;

            Images[0] = new GameSprite(TextureManager.Space, new Vector2(0, 8));
            Images[0].Scale = Scale;
            Images[0].Frame = new Rectangle(0, 0, Weight, Height);

            Images[1] = new GameSprite(TextureManager.Space, new Vector2(0, 8 + Height));
            Images[1].Scale = Scale;
            Images[1].Frame = new Rectangle(0, 0, Weight, Height);
            Status = SpaceStatus.Stoped;
        }

        public override void Update(GameTime gameTime)
        {
            GameTime = gameTime;

            if (Status == SpaceStatus.Playing)
                Move(50);
            else if (Status == SpaceStatus.Reversing)
                Move(-150);

            FrameTimer += Deltatime;
            if (FrameTimer < 0.2f)
                return;

            FrameTimer -= 0.2f;
                NextFrame();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Images[0].Draw(spriteBatch);
            Images[1].Draw(spriteBatch);
            ApplicationMemory.ScreenController.InternalBorders(spriteBatch);
        }

        void NextFrame()
        {
            FrameIndex++;
            if (FrameIndex == 4)
                FrameIndex = 0;

            Images[0].Frame = Images[1].Frame = new Rectangle((int)(488 * FrameIndex), 0, Weight, Height);
        }

        void Move(int amount)
        {
            var y = amount * Deltatime;
            var y0 = Images[0].Position.Y + y;
            var y1 = Images[1].Position.Y + y;
            var height = Height * Scale;

            if (y0 < 8 - height)
                y0 = height + 8;
            else if (y0 > height + 8)
                y0 = 8 - height;

            if (y1 < 8 - height)
                y1 = height + 8;
            else if (y1 > height + 8)
                y1 = 8 - height;

            Images[0].SetPosition(new Vector2(0, y0));
            Images[1].SetPosition(new Vector2(0, y1));
        }
    }

    enum SpaceStatus
    {
        Stoped,
        Playing,
        Reversing,
    }
}
