using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

namespace GalagaFramework.Framework
{
    static class SoundManager
    {
        static internal SoundEffect Start;
        static internal SoundEffect Stage;
        static internal SoundEffect ChallangeStage;
        static internal SoundEffect ChallangeStageResults;
        static internal SoundEffect FigherCaptured;
        static internal SoundEffect FigherRescue;
        static internal SoundEffect Highscore;

        static internal SoundEffect BossExplosion;
        static internal SoundEffect BossPower;
        static internal SoundEffect BossPowerAbducting;
        static internal SoundEffect EnemyShipHit;
        static internal SoundEffect EnemyShipAttacking;
        static internal SoundEffect FleetSound;

        static internal SoundEffect Coin;
        static internal SoundEffect Life;
        static internal SoundEffect Shot;
        static internal SoundEffect GalagaExplosion;


        static public void LoadSounds(ContentManager content)
        {
            Start = content.Load<SoundEffect>("Sounds/Start");
            Stage = content.Load<SoundEffect>("Sounds/StageSound");
            ChallangeStage = content.Load<SoundEffect>("Sounds/ChallangeStage");
            ChallangeStageResults = content.Load<SoundEffect>("Sounds/ChallangeStageResults");
            FigherCaptured = content.Load<SoundEffect>("Sounds/FighterCaptured");
            FigherRescue = content.Load<SoundEffect>("Sounds/FighterCaptured");
            Highscore = content.Load<SoundEffect>("Sounds/Highscore");

            BossExplosion = content.Load<SoundEffect>("Sounds/BossExplosion");
            BossPower = content.Load<SoundEffect>("Sounds/BossPower2");
            BossPowerAbducting = content.Load<SoundEffect>("Sounds/BossPowerAbducting");
            EnemyShipHit = content.Load<SoundEffect>("Sounds/EnemyShipHit");
            EnemyShipAttacking = content.Load<SoundEffect>("Sounds/ShipAttacking");
            FleetSound = content.Load<SoundEffect>("Sounds/FleetSound");

            Coin = content.Load<SoundEffect>("Sounds/Coin");
            Life = content.Load<SoundEffect>("Sounds/Life");
            Shot = content.Load<SoundEffect>("Sounds/Shot");
            GalagaExplosion = content.Load<SoundEffect>("Sounds/GalagaExplosion");
        }
    }
}
