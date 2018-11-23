using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace GalagaFramework.Framework
{
    static public class TextureManager
    {
        //--------------Game sprites
        //Ships
        static public Texture2D Galaga;
        static public Texture2D GalagaRed;
        static public Texture2D Boss;
        static public Texture2D BossBlue;
        static public Texture2D Moth;
        static public Texture2D Honet;

        //Bullets
        static public Texture2D BossPower;
        static public Texture2D GalagaBullet;
        static public Texture2D EnemyBullet;

        //Explosion
        static public Texture2D GalagaExplosion;
        static public Texture2D EnemyExplosion;

        //Stages
        static public Texture2D Stage1;
        static public Texture2D Stage5;
        static public Texture2D Stage10;

        //--------------Game editor
        static public Texture2D Empty;
        static public Texture2D BezierPoint;

        //Backgrounds
        static public Texture2D Space; 
        static public Texture2D Grid;
        static public Texture2D Fleet;
        static public Texture2D FleetGrinded;

        static ContentManager ContentManager;

        static public void LoadTextures(ContentManager content)
        {
            ContentManager = content;

            Galaga = content.Load<Texture2D>("Sprites/Ships/Galaga");
            GalagaRed = content.Load<Texture2D>("Sprites/Ships/GalagaRed");
            Boss = content.Load<Texture2D>("Sprites/Ships/BossGreenIdle");
            BossBlue = content.Load<Texture2D>("Sprites/Ships/BossBlueIdle");
            Moth = content.Load<Texture2D>("Sprites/Ships/MothAll");
            Honet = content.Load<Texture2D>("Sprites/Ships/HonetAll");

            //Bullets
            BossPower = content.Load<Texture2D>("Sprites/Bullets/BossPower");
            GalagaBullet = content.Load<Texture2D>("Sprites/Bullets/GalagaBullet");
            EnemyBullet = content.Load<Texture2D>("Sprites/Bullets/GalagaBullet");

            //Explosion
            GalagaExplosion = content.Load<Texture2D>("Sprites/Explosions/GalagaExplosion_32x32");
            EnemyExplosion = content.Load<Texture2D>("Sprites/Explosions/Explosion_32x32");

            //Stages
            Stage1 = content.Load<Texture2D>("Sprites/StageIcons/Stage1");
            Stage5 = content.Load<Texture2D>("Sprites/StageIcons/Stage5");
            Stage10 = content.Load<Texture2D>("Sprites/StageIcons/Stage10");

            //Game editor
            Empty = content.Load<Texture2D>("Sprites/Empty");
            BezierPoint = content.Load<Texture2D>("Sprites/Empty");

            //Backgrounds
            Space = content.Load<Texture2D>("Sprites/Backgrounds/Space");
            Grid = content.Load<Texture2D>("Sprites/Backgrounds/GalagaGrid"); 
            Fleet = content.Load<Texture2D>("Sprites/Backgrounds/GalagaFleet");
            FleetGrinded = content.Load<Texture2D>("Sprites/Backgrounds/GalagaFleetGrinded");
        }
    }
}
