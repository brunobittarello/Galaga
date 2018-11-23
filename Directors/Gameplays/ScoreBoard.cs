using Newtonsoft.Json;
using System.Linq;

namespace GalagaFramework.Directors.Gameplays
{
    class ScoreBoard
    {
        public Score[] Top5Score;

        internal int PutScoreInTop5(Score score)
        {
            var top5Score = Top5Score.ToList();
            int position;
            for (position = 0; position < 5; position++)
                if (score.Points > Top5Score[position].Points)
                {
                    top5Score.Insert(position, score);
                    break;
                }

            top5Score.RemoveAt(5);
            Top5Score = top5Score.ToArray();
            return position;
        }

        internal void Save()
        {
            var output = JsonConvert.SerializeObject(this);
            var file = new System.IO.StreamWriter("HighScore.txt");
            file.WriteLine(output);
            file.Close();
        }

        static internal ScoreBoard Load()
        {
            try
            {
                var file = new System.IO.StreamReader("HighScore.txt");
                var json = file.ReadLine();
                file.Close();

                return JsonConvert.DeserializeObject<ScoreBoard>(json);
            }
            catch
            {
                return Default();
            }
        }

        static ScoreBoard Default()
        {
            var entity = new ScoreBoard();
            entity.Top5Score = new Score[5];
            entity.Top5Score[0] = new Score() { Name = new char[3] { 'N', '.', 'N' }, Points = 20000 };
            entity.Top5Score[1] = new Score() { Name = new char[3] { 'A', '.', 'A' }, Points = 20000 };
            entity.Top5Score[2] = new Score() { Name = new char[3] { 'N', '.', 'N' }, Points = 20000 };
            entity.Top5Score[3] = new Score() { Name = new char[3] { 'C', '.', 'C' }, Points = 20000 };
            entity.Top5Score[4] = new Score() { Name = new char[3] { 'O', '.', 'O' }, Points = 20000 };
            return entity;
        }
    }
}
