using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace GalagaFramework.Framework
{
    abstract public class Entity
    {
        protected GameTime GameTime;
        public Vector2 Position;

        protected float Deltatime
        {
            get
            {
                return (float)ApplicationMemory.GameTime.ElapsedGameTime.TotalSeconds;
            }
        }

        protected float TotalSeconds
        {
            get
            {
                return (float)ApplicationMemory.GameTime.TotalGameTime.TotalSeconds;
            }
        }

        public Entity()
        {
        }

        public virtual void Update(GameTime gameTime) { }
        public virtual void Draw(SpriteBatch spriteBatch) { }
    }
}
