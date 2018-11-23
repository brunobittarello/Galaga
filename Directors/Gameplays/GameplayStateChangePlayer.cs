using GalagaFramework.Framework;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace GalagaFramework.Directors.Gameplays
{
    class GameplayStateChangePlayer : GameplayStateBase
    {
        DisplayText Text;
        float Timer;

        public GameplayStateChangePlayer(Gameplay gameplay)
            : base(gameplay)
        {
            var position = (ApplicationMemory.ScreenController.GridPosition / 2) - new Vector2(35, 0); 
            Text = new DisplayText(position, "PLAYER 1", 0.35f, Color.Cyan);
            SoundManager.Start.Play();
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
            if (Gameplay.Stage == 0)
                return GameplayStatus.ChangeStage;

            return GameplayStatus.ChangeStage;//TODO: verify when this game has 2 playable players
        }
    }
}
