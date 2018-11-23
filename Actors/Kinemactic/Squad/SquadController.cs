using GalagaFramework.Actors.Dynamic.Ships.EnemyShip;
using GalagaFramework.Framework;

namespace GalagaFramework.Actors.Kinemactic.Squad
{
    public class SquadController
    {
        internal SquadEntity Entity { get; private set; }
        internal bool SquadCreated { get; private set; }
        BezierCurve Bezier;
        int numShipsCreated;
        int numShipOut;
        int count;

        public bool IsDone
        {
            get
            {
                return SquadCreated && numShipsCreated == numShipOut;
            }
        }

        internal SquadController(SquadEntity entity)
        {
            Entity = entity;
            Bezier = new BezierCurve();
            Bezier.SetControlPoints(entity.BezierPoints);
        }

        internal EnemyShip CreateShip()
        {
            if (count != 0)
            {
                count--;
                return null;
            }
            count = 10;

            if (SquadCreated)
                return null;

            EnemyShip ship;
            if (Entity.EnemyType[numShipsCreated] < 4)
                ship = new Boss(Bezier, Entity.EnemyType[numShipsCreated], this);
            else if (Entity.EnemyType[numShipsCreated] > 19)
                ship = new Honet(Bezier, Entity.EnemyType[numShipsCreated], this);
            else 
                ship = new Moth(Bezier, Entity.EnemyType[numShipsCreated], this);

            numShipsCreated++;

            if (numShipsCreated == Entity.EnemyType.Count)
                SquadCreated = true;

            return ship;
        }

        internal void SquadShipOut()
        {
            numShipOut++;
        }
    }
}
