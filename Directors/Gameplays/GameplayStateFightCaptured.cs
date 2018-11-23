using GalagaFramework.Framework;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GalagaFramework.Directors.Gameplays
{
    class GameplayStateFightCaptured : GameplayStateBase
    {
        DisplayText Text;
        bool ShowText;
        float Timer;
        float Duration;
        bool Waiting;

        public GameplayStateFightCaptured(Gameplay gameplay)
            : base(gameplay)
        {
            var position = (ApplicationMemory.ScreenController.GridPosition / 2) + new Vector2(-64, 0);
            Text = new DisplayText(position, "FIGHTER CAPTURED", 0.35f, Color.Red);
            Duration = SoundManager.FigherCaptured.Duration.Seconds;
        }

        public override void Update(GameTime gameTime)
        {
            if (Waiting)
                return;

            Timer += Deltatime;
            if (ShowText == false && Timer > 1.5f)
                Activate();
            else if (Timer > Duration + 2f)
            {
                Waiting = true;
                Gameplay.LostLife();
            }
        }

        public void Activate()
        {
            Gameplay.GalagaFleet = null;
            ShowText = true;
            SoundManager.FigherCaptured.Play();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (ShowText)
                Text.Draw(spriteBatch);
            Gameplay.DrawStageUI(spriteBatch);
        }

        internal override GameplayStatus NextState()
        {
            return GameplayStatus.Playing;//Useless
        }
    }
}
