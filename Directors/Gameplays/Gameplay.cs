using GalagaFramework.Actors.Dynamic.Ships.EnemyShip;
using GalagaFramework.Actors.Kinemactic;
using GalagaFramework.Framework;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GalagaFramework.Directors.Gameplays
{
    public class Gameplay : GameContainer
    {
        internal Game1 Game { get; private set; }

        internal int Lives { get; private set; }
        internal int Stage { get; private set; }

        internal ScoreBoard ScoreBoard { get; private set; }
        internal Score Score { get; private set; }
        int HighScore;

        internal int Shots { get; private set; }
        internal int Hits { get; private set; }

        internal GameplayStatus Status { get; private set; }
        GameplayStateBase State;
        GameSprite[] LivesImage;
        GameSprite[] StageImage;
        bool stageIconsCreated;
        float stageIconTimer;

        DisplayText Player1Label;
        DisplayText HighScoreLabel;
        //DisplayText Player2Label;
        DisplayText ScoreText;
        DisplayText HighScoreText;

        public Gameplay(Game1 game)
        {
            Game = game;
            Score = new Score();
            Space = new Space();

            Lives = 2;
            SetLives();
            PassStage();
            ScoreBoard = ScoreBoard.Load();
            HighScore = ScoreBoard.Top5Score[0].Points;

            State = new GameplayStateChangePlayer(this);
            ApplicationMemory.GameContainer = this;

            Player1Label = new DisplayText(new Vector2(16, 0), "1UP", 0.3f, Color.Red);
            HighScoreLabel = new DisplayText(new Vector2(80, 0), "HIGH SCORE", 0.3f, Color.Red);
            ScoreText = new DisplayText(new Vector2(30, 8), Score.Points.ToString("D2"), 0.3f, Color.White);
            HighScoreText = new DisplayText(new Vector2(103, 8), HighScore.ToString(), 0.3f, Color.White);
        }

        internal void LostLife()
        {
            Lives--;
            if (Lives < 0)
            {
                LoadNextState(GameplayStatus.GameOver);
                return;
            }

            SetLives();
            GalagaFleet = null;
            LoadNextState(GameplayStatus.Ready);
        }


        internal void PassStage()
        {
            Fleet = null;
            Stage++;
            StageImage = new GameSprite[Stage];
            stageIconsCreated = false;
        }

        void SetLives()
        {
            LivesImage = new GameSprite[Lives];
            var texture = TextureManager.Galaga;
            var position = new Vector2(0, ApplicationMemory.ScreenController.GridPosition.Y - (texture.Height / 2));
            for (var i = 0; i < Lives; i++)
            {
                LivesImage[i] = new GameSprite(texture, position);
                position.X += texture.Width + 4;
            }
        }

        void SetStage()
        {
            if (stageIconsCreated == true || TotalSeconds - stageIconTimer < (float)SoundManager.Stage.Duration.TotalSeconds)
                return;

            var texture = TextureManager.Stage1;
            var position = new Vector2(ApplicationMemory.ScreenController.GridPosition.X - texture.Width, ApplicationMemory.ScreenController.GridPosition.Y - (texture.Height / 2));
            for (var i = 0; i < Stage; i++)
            {
                if (StageImage[i] == null)
                {
                    SoundManager.Stage.Play();
                    stageIconTimer = TotalSeconds;
                    StageImage[i] = new GameSprite(texture, position);
                    return;
                }
                position.X -= texture.Width + 2;
            }
            stageIconsCreated = true;
        }

        public override void Update(GameTime gameTime)
        {
            State.Update(gameTime);
            SetStage();
            if (Space != null)
                Space.Update(gameTime);

            if (Fleet != null)
            {
                Fleet.Update(gameTime);
                if (Fleet.IsAllDesotryed())
                {
                    PassStage();
                    LoadNextState(GameplayStatus.ChangeStage);
                }
            }

            if (GalagaFleet != null)
                GalagaFleet.Update(gameTime);

            if (State.IsDone)
                LoadNextState(State.NextState());
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Space != null)
                Space.Draw(spriteBatch);

            State.Draw(spriteBatch);

            if (Fleet != null)
                Fleet.Draw(spriteBatch);

            if (GalagaFleet != null)
                GalagaFleet.Draw(spriteBatch);
        }

        internal void DrawStageUI(SpriteBatch spriteBatch)
        {
            DrawGameLabelUI(spriteBatch);
            foreach (var image in LivesImage)
                image.Draw(spriteBatch);
            foreach (var image in StageImage)
                if (image != null)
                    image.Draw(spriteBatch);
        }

        internal void DrawGameLabelUI(SpriteBatch spriteBatch)
        {
            if ((int)TotalSeconds % 2 == 0)
                Player1Label.Draw(spriteBatch);
            HighScoreLabel.Draw(spriteBatch);
            ScoreText.Draw(spriteBatch);
            HighScoreText.Draw(spriteBatch);
        }

        internal void IncrementShots()
        {
            Shots++;
        }

        internal void IncrementHits()
        {
            Hits++;
        }

        internal void ScorePoint(EnemyShip ship)
        {
            int score = 0;
            if (ship is Honet)
                if (ship.State == EnemyShipState.InFleet)
                    score = 50;
                else
                    score = 100;
            else if (ship is Moth)
                if (ship.State == EnemyShipState.InFleet)
                    score = 80;
                else
                    score = 160;
            else if (ship is Boss)
                if (ship.State == EnemyShipState.InFleet)
                    score = 150;
                else
                    score = 400;

            if ((Score.Points < 20000 && Score.Points + score > 20000) || (Score.Points % 70000 > (Score.Points + score) % 70000))
            {
                SoundManager.Life.Play();
                Lives++;
                SetLives();
            }

            Score.Points += score;
            ScoreText.Text = Score.Points.ToString("D2");
            if (Score.Points > HighScore)
                HighScoreText.Text = Score.Points.ToString();
        }

        internal void SetGalagaCaptured()
        {
            LoadNextState(GameplayStatus.FightCaptured);
        }

        internal void SetGalagaRescued()
        {
            LoadNextState(GameplayStatus.FightRecued);
        }

        void LoadNextState(GameplayStatus status)
        {
            Status = status;
            switch (status)
            {
                case GameplayStatus.ChangePlayer: State = new GameplayStateChangePlayer(this); break;
                case GameplayStatus.ChangeStage: State = new GameplayStateChangeStage(this); break;
                case GameplayStatus.Ready: State = new GameplayStateReady(this); break;
                case GameplayStatus.Playing: State = new GameplayStatePlaying(this); break;
                case GameplayStatus.FightCaptured: State = new GameplayStateFightCaptured(this); break;
                case GameplayStatus.FightRecued: State = new GameplayStateFightRescued(this); break;
                case GameplayStatus.GameOver: State = new GameplayStateGameOver(this); GalagaFleet = null; break;
                case GameplayStatus.Results: State = new GameplayStateResults(this); break;
                case GameplayStatus.HighScore: State = new GameplayStateHighscore(this); break;
                case GameplayStatus.End: IsDone = true; break;
            }
        }
    }

    enum GameplayStatus
    {
        ChangePlayer,
        ChangeStage,
        Ready,
        Playing,
        FightCaptured,
        FightRecued,
        GameOver,
        Results,
        HighScore,
        End,
    }
}
