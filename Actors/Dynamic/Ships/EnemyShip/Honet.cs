using GalagaFramework.Actors.Kinemactic.Squad;
using GalagaFramework.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GalagaFramework.Actors.Dynamic.Ships.EnemyShip
{
    class Honet : EnemyShip
    {
        public Honet(BezierCurve bezierCurve, int index, SquadController squadCtr = null)
            : base(TextureManager.Honet, bezierCurve, index, squadCtr)
        {

        }
    }
}
