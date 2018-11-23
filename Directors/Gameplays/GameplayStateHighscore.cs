using GalagaFramework.Framework;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GalagaFramework.Directors.Gameplays
{
    class GameplayStateHighscore : GameplayStateBase
    {
        int CharIndex;
        char[] Characters;
        int PlayerCharIndex;
        bool NameDefined;
        float Timer;

        DisplayText[] FixedLabels;
        DisplayText[] InitialsLabels;
        DisplayText PlayerNameText;

        public GameplayStateHighscore(Gameplay gameplay)
            : base(gameplay)
        {
            var top5Place = gameplay.ScoreBoard.PutScoreInTop5(gameplay.Score);

            Characters = new char[28] {
                'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', ' ', '.'
            };

            FixedLabels = new DisplayText[15];
            InitialsLabels = new DisplayText[3];

            Vector2 position = Vector2.Zero;

            FixedLabels[0] = new DisplayText(position = new Vector2(18, 64), "ENTER YOUR INITIALS !", 0.35f, Color.Red);
            FixedLabels[1] = new DisplayText(position += new Vector2(24, 16), "SCORE       NAME", 0.35f, Color.Cyan);
            FixedLabels[2] = new DisplayText(position += new Vector2(0, 16), 34530.ToString(), 0.35f, Color.Cyan);

            InitialsLabels[0] = new DisplayText(position += new Vector2(117, 0), "A", 0.35f, Color.Cyan);
            InitialsLabels[1] = new DisplayText(position += new Vector2(9, 0), "A", 0.35f, Color.Cyan);
            InitialsLabels[2] = new DisplayText(position += new Vector2(9, 0), "A", 0.35f, Color.Cyan);

            FixedLabels[3] = new DisplayText(position = (ApplicationMemory.ScreenController.GridPosition / 2) + new Vector2(-24, 8), "TOP 5", 0.35f, Color.Red);
            FixedLabels[4] = new DisplayText(position += new Vector2(-20, 16), "SCORE    NAME", 0.35f, Color.Cyan);

            position += new Vector2(-48, 16);
            var labelIndex = 5;
            //CreateTop5ScoreTexts
            for (int i = 0; i < 5; i++)
            {
                string posName = (i + 1) + "TH  ";
                if (i == 0)
                    posName = "1ST  ";
                else if (i == 1)
                    posName = "2ND  ";
                else if (i == 2)
                    posName = "3RD  ";

                if (top5Place == i)
                {
                    FixedLabels[labelIndex++] = new DisplayText(position, posName + gameplay.ScoreBoard.Top5Score[i].Points, 0.35f, Color.Yellow);
                    PlayerNameText = FixedLabels[labelIndex++] = new DisplayText(position + new Vector2(134, 0), gameplay.ScoreBoard.Top5Score[i].NameString, 0.35f, Color.Yellow);
                }
                else
                {
                    FixedLabels[labelIndex++] = new DisplayText(position, posName + gameplay.ScoreBoard.Top5Score[i].Points, 0.35f, Color.Cyan);
                    FixedLabels[labelIndex++] = new DisplayText(position + new Vector2(134, 0), gameplay.ScoreBoard.Top5Score[i].NameString, 0.35f, Color.Cyan);
                }
                position += new Vector2(0, 16);
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (NameDefined == true)
            {
                if (Timer > 5)
                    IsDone = true;
                else
                    Timer += Deltatime;

                return;
            }

            if (IsDone)
                return;

            if ((int)TotalSeconds % 2 == 0)
                InitialsLabels[PlayerCharIndex].Color = Color.Yellow;
            else
                InitialsLabels[PlayerCharIndex].Color = Color.Cyan;

            if (ArcadeControl.ActionButtonPressedDown())
                WriteCharacter();

            if (ArcadeControl.HorizontalPressedDown() != 0)
                ChangeCharacter(ArcadeControl.HorizontalPressedDown() > 0 ? 1 : -1);
        }

        void WriteCharacter()
        {
            Gameplay.Score.Name[PlayerCharIndex] = Characters[CharIndex];
            PlayerNameText.Text = Gameplay.Score.NameString;
            InitialsLabels[PlayerCharIndex].Color = Color.Cyan;
            PlayerCharIndex++;

            if (PlayerCharIndex == 3)
            {
                Gameplay.ScoreBoard.Save();
                NameDefined = true;
            }
        }

        void ChangeCharacter(int direction)
        {
            CharIndex += direction;
            if (CharIndex < 0)
                CharIndex = Characters.Length - 1;
            else if (CharIndex >= Characters.Length)
                CharIndex = 0;

            InitialsLabels[PlayerCharIndex].Text = Characters[CharIndex].ToString();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            foreach (var label in FixedLabels)
                label.Draw(spriteBatch);

            foreach (var label in InitialsLabels)
                label.Draw(spriteBatch);

            Gameplay.DrawGameLabelUI(spriteBatch);
        }

        internal override GameplayStatus NextState()
        {
            return GameplayStatus.End;
        }
    }
}
