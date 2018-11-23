using GalagaFramework.Actors.Dynamic.Ships.EnemyShip;
using GalagaFramework.Actors.Dynamic.Ships.Galaga;
using GalagaFramework.Actors.Kinemactic;
using GalagaFramework.Directors;
using GalagaFramework.Directors.Gameplays;
using GalagaFramework.Framework;

namespace GalagaFramework.Actors
{
    public abstract class ActorBase : Entity
    {
        GameContainer GameContainer;

        protected Fleet Fleet
        {
            get
            {
                return (GameContainer == null) ? null : GameContainer.Fleet;
            }
        }

        protected EnemyShip[] Ships
        {
            get
            {
                return (GameContainer == null) ? null : GameContainer.Fleet.Ships;
            }
        }

        protected GalagaFleet GalagaFleet
        {
            get
            {
                return (GameContainer == null) ? null : GameContainer.GalagaFleet;
            }
        }
        
        protected GalagaController[] Galagas
        {
            get
            {
                return (GameContainer == null) ? null : GameContainer.GalagaFleet.Galagas;
            }
        }
        
        protected Gameplay Gameplay
        {
            get
            {
                if (GameContainer != null && GameContainer is Gameplay)
                    return (GameContainer as Gameplay);
                return null;
            }
        }
        
        internal ActorBase()
        {
            GameContainer = ApplicationMemory.GameContainer as GameContainer;
        }
    }
}
