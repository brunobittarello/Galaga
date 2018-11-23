using GalagaFramework.Framework;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace GalagaFramework.Actors.Kinemactic.Squad
{
    public class SquadEntity
    {
        public List<Vector2> BezierPoints;
        //
        // Fleet fixed: 1-40
        // Other1: 96
        // Other2: 97
        // Honet: 98
        // Moth: 99
        // Boss: 100
        //
        public List<byte> EnemyType;
        public int DivePoint;
        public bool Syncronous;
        public int FirePoint;
    }
}
