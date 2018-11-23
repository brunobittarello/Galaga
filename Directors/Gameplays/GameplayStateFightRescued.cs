using GalagaFramework.Actors.Kinemactic;
using GalagaFramework.Framework;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace GalagaFramework.Directors.Gameplays
{
    class GameplayStateFightRescued : GameplayStateBase
    {
        SoundEffectInstance sound;

        public GameplayStateFightRescued(Gameplay gameplay)
            : base(gameplay)
        {
            sound = SoundManager.FigherCaptured.CreateInstance();
        }

        public override void Update(GameTime gameTime)
        {
            if (sound.State == SoundState.Stopped)
                sound.Play();

            if (Gameplay.GalagaFleet.Status == GalagaFleetStatus.Playing)
                IsDone = true;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Gameplay.DrawStageUI(spriteBatch);
        }

        internal override GameplayStatus NextState()
        {
            return GameplayStatus.Playing;
        }
    }
}
