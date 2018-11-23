using GalagaFramework.Actors.Kinemactic;
using GalagaFramework.Framework;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GalagaFramework.Directors.Gameplays
{
    class GameplayStateReady : GameplayStateBase
    {
        DisplayText Text;
        float Timer;

        public GameplayStateReady(Gameplay gameplay)
            : base(gameplay)
        {
            var position = (ApplicationMemory.ScreenController.GridPosition / 2) - new Vector2(30, 0);
            Text = new DisplayText(position, "READY", 0.35f, Color.Cyan);
            gameplay.Space.Status = SpaceStatus.Playing;
        }

        public override void Update(GameTime gameTime)
        {
            if (Gameplay.Fleet != null && Gameplay.Fleet.Status != FleetStatus.Idle)
                return;

            if (Gameplay.GalagaFleet == null)
                Gameplay.GalagaFleet = new GalagaFleet();

            Timer += Deltatime;
            if (Timer > 3)
            {
                if (Gameplay.Fleet != null)
                    Gameplay.Fleet.BackToActivity();
                IsDone = true;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Gameplay.Fleet != null && Gameplay.Fleet.Status == FleetStatus.Idle)
                Text.Draw(spriteBatch);
            Gameplay.DrawStageUI(spriteBatch);
        }

        internal override GameplayStatus NextState()
        {
            return GameplayStatus.Playing;
        }
    }
}
