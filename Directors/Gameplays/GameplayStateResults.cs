using GalagaFramework.Framework;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GalagaFramework.Directors.Gameplays
{
    class GameplayStateResults : GameplayStateBase
    {
        DisplayText ResultsText;
        DisplayText ShotsText;
        DisplayText HitsText;
        DisplayText RatioText;
        float Timer;

        public GameplayStateResults(Gameplay gameplay)
            : base(gameplay)
        {
            gameplay.Fleet = null;
            gameplay.GalagaFleet = null;
            float ratio = 0;
            if (gameplay.Shots > 0)
                ratio = ((float)gameplay.Hits / (float)gameplay.Shots) * 100;
            var position = (ApplicationMemory.ScreenController.GridPosition / 2) - new Vector2(35, 0);

            ResultsText = new DisplayText(position, "-RESULTS-", 0.3f, Color.Red);
            ShotsText = new DisplayText(position += new Vector2(-48, 16), "SHOTS FIRED     " + gameplay.Shots.ToString(), 0.3f, Color.Yellow);
            HitsText = new DisplayText(position += new Vector2(0, 16), "NUMBER OF HITS  " + gameplay.Hits.ToString(), 0.3f, Color.Yellow);
            RatioText = new DisplayText(position += new Vector2(0, 16), "HIT-MISS RATIO  " + ratio.ToString("00.00") + " %", 0.3f, Color.White);
        }

        public override void Update(GameTime gameTime)
        {
            Timer += Deltatime;
            if (Timer > 7)
                IsDone = true;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            ResultsText.Draw(spriteBatch);
            ShotsText.Draw(spriteBatch);
            HitsText.Draw(spriteBatch);
            RatioText.Draw(spriteBatch);

            Gameplay.DrawGameLabelUI(spriteBatch);
        }

        internal override GameplayStatus NextState()
        {
            if (Gameplay.Score.Points > Gameplay.ScoreBoard.Top5Score[4].Points)
                return GameplayStatus.HighScore;
            return GameplayStatus.End;
        }
    }
}
