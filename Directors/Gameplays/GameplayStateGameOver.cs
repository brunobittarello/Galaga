using GalagaFramework.Framework;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GalagaFramework.Directors.Gameplays
{
    class GameplayStateGameOver : GameplayStateBase
    {
        DisplayText Text;
        float Timer;

        public GameplayStateGameOver(Gameplay gameplay)
            : base(gameplay)
        {
            var position = (ApplicationMemory.ScreenController.GridPosition / 2) - new Vector2(35, 0);
            Text = new DisplayText(position, "GAME OVER", 0.35f, Color.Cyan);
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
            Gameplay.DrawGameLabelUI(spriteBatch);
        }

        internal override GameplayStatus NextState()
        {
            return GameplayStatus.Results;
        }
    }
}
