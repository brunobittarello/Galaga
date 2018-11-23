
namespace GalagaFramework.Directors.Gameplays
{
    class Score
    {
        public int Points;
        public char[] Name;

        public Score()
        {
            Name = new char[3] { ' ', ' ', ' ' };
            Points = 0;
        }

        public string NameString
        {
            get
            {
                return new string(Name);
            }
        }
    }
}
