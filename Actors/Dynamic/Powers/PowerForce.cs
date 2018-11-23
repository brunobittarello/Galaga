using GalagaFramework.Framework;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Drawing;
using System.Linq;

namespace GalagaFramework.Actors.Dynamic.Powers
{
    class PowerForce : Dynamic
    {
        bool Abducting;

        int frameIndex;
        int frameHigh;
        int highDirection;
        float frameTimer;
        float maxPowerTimer;
        SoundEffectInstance Sound;
        RectangleF AbductionArea;

        internal bool IsDone
        {
            get { return frameHigh < 0; }
        }

        public PowerForce(Vector2 position)
            : base(TextureManager.BossPower)
        {
            Position = position + new Vector2(0, 0);
            Image.Frame = new Microsoft.Xna.Framework.Rectangle(0, 0, 46, frameHigh);
            Image.RotationPoint = new Vector2(23, 0);
            Image.SetPosition(Position);
            highDirection = 8;
            Sound = SoundManager.BossPower.CreateInstance();

            AbductionArea = new RectangleF(Position.X - TextureManager.Galaga.Width * 1.5f, Position.Y + 60, TextureManager.Galaga.Width * 2, TextureManager.Galaga.Height);
        }

        public override void Update(GameTime gameTime)
        {
            if (Sound.State == SoundState.Stopped)
                Sound.Play();

            Animation();
            if (Abducting == false && frameHigh > 70)
                TryToAbduct();
        }

        void Animation()
        {
            frameTimer += Deltatime;
            if (frameTimer < 0.2f)
                return;

            frameHigh = MathHelper.Clamp(frameHigh + highDirection, -80, 80);
            if (frameHigh == 80)
                if (maxPowerTimer < 2)
                    maxPowerTimer += frameTimer;
                else
                {
                    maxPowerTimer = 0;
                    highDirection *= -1;
                }
            
            frameTimer = frameTimer - 0.2f;
            frameIndex++;
            if (frameIndex == 3)
                frameIndex = 0;

            Image.Frame = new Microsoft.Xna.Framework.Rectangle(46 * frameIndex, 0, 46, frameHigh);
        }

        void TryToAbduct()
        {
            if (DetectCollisions(base.Galagas.ToList<Dynamic>()))
            {
                Abducting = true;
                Sound.Stop();
                Sound.Dispose();
                Sound = SoundManager.BossPowerAbducting.CreateInstance();
                Sound.Play();

                maxPowerTimer = 2;
                highDirection *= -1;
                GalagaFleet.AbdutionBegins(Position + new Vector2(15, 0));
            }

        }

        internal override RectangleF Collider()
        {
            return AbductionArea;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Image.Draw(spriteBatch);
        }
    }
}
