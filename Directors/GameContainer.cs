using GalagaFramework.Actors.Kinemactic;

namespace GalagaFramework.Directors
{
    public abstract class GameContainer : DirectorBase
    {
        public GalagaFleet GalagaFleet;
        public Fleet Fleet;
        public Space Space;
    }
}
