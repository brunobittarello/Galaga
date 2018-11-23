using GalagaFramework.Framework;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GalagaFramework.Directors.Gameplays
{
    abstract class GameplayStateBase : Entity
    {
        protected Gameplay Gameplay;
        protected internal bool IsDone { get; protected set; }

        public GameplayStateBase(Gameplay gameplay)
        {
            Gameplay = gameplay;
        }

        abstract internal GameplayStatus NextState();
    }
}
