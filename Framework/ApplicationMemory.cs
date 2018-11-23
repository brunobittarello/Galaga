using GalagaFramework.Directors;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace GalagaFramework.Framework
{
    static public class ApplicationMemory
    {
        static public GameTime GameTime;
        static public ScreenController ScreenController;
        static public SpriteFont Font;
        static public GameContainer GameContainer;
        static public Random Random;
        //static public List<Entity> Entities;

        static ApplicationMemory()
        {
            Random = new Random();
            //Entities = new List<Entity>();
        }
    }
}
