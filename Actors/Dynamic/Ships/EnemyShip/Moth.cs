using GalagaFramework.Actors.Kinemactic.Squad;
using GalagaFramework.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GalagaFramework.Actors.Dynamic.Ships.EnemyShip
{
    class Moth : EnemyShip
    {
        public Moth(BezierCurve bezierCurve, int index, SquadController squadCtr = null)
            : base(TextureManager.Moth, bezierCurve, index, squadCtr)
        {

        }


    }
}
