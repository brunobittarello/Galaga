using GalagaFramework.Actors.Kinemactic;
using Microsoft.Xna.Framework.Graphics;

namespace GalagaFramework.Directors.Gameplays
{
    class GameplayStatePlaying: GameplayStateBase
    {
        public GameplayStatePlaying(Gameplay gameplay)
            : base(gameplay)
        {
            if (gameplay.Fleet == null)
                gameplay.Fleet = new Fleet(StageTimeLine.Load().Stages[0]);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Gameplay.DrawStageUI(spriteBatch);
        }

        internal override GameplayStatus NextState()
        {
            return GameplayStatus.Playing;
        }
    }
}
