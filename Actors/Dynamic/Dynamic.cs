using GalagaFramework.Framework;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Drawing;

namespace GalagaFramework.Actors.Dynamic
{
    public class Dynamic : ActorBase
    {
        protected GameSprite Image;
        
        protected List<Dynamic> Collisions;

        

        public Dynamic(Texture2D texture)
            : base()
        {
            Image = new GameSprite(texture);
            Collisions = new List<Dynamic>();
        }

        protected bool DetectCollisions(List<Dynamic> listDynamic)
        {
            Collisions.Clear();

            foreach (var dynamic in listDynamic)
                if (dynamic !=  null && Collider().IntersectsWith(dynamic.Collider()))
                    Collisions.Add(dynamic);

            return Collisions.Count > 0;
        }

        internal virtual RectangleF Collider()
        {
            var size = Image.Size();
            var halfSize = size / 2;
            return new RectangleF(Position.X - halfSize.X, Position.Y - halfSize.Y, size.X, size.Y);
        }
    }
}
