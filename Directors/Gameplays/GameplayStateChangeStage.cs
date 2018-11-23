using GalagaFramework.Framework;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GalagaFramework.Directors.Gameplays
{
    class GameplayStateChangeStage : GameplayStateBase
    {
        DisplayText Text;
        float Timer;

        public GameplayStateChangeStage(Gameplay gameplay)
            : base(gameplay)
        {
            var position = (ApplicationMemory.ScreenController.GridPosition / 2) - new Vector2(30, 0);
            Text = new DisplayText(position, "STAGE " + Gameplay.Stage, 0.35f, Color.Cyan);
        }

        public override void Update(GameTime gameTime)
        {
            Timer += Deltatime;
            if (Timer > 3)
                IsDone = true;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Text.Draw(spriteBatch);
            Gameplay.DrawStageUI(spriteBatch);
        }

        internal override GameplayStatus NextState()
        {
            if (Gameplay.Stage == 1)
                return GameplayStatus.Ready;
            else
                return GameplayStatus.Playing;
        }
    }
}
