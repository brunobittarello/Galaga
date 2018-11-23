using GalagaFramework.Actors.Dynamic.Ships.EnemyShip;
using GalagaFramework.Actors.Kinemactic.Squad;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace GalagaFramework.Actors.Kinemactic.Stage
{
    public class StageController
    {
        StageEntity Entity;
        List<SquadController> ActiveSquads;
        int squadIndex;
        internal bool IsDone { get; private set; }

        public StageController(StageEntity entity)
        {
            Entity = entity;
            SetSquads();
        }

        public void Reset()
        {
            IsDone = false;
            squadIndex = 0;
            SetSquads();
        }

        internal List<EnemyShip> CreateStage()
        {
            if (IsDone)
                return null;

            var shipsCreated = new List<EnemyShip>();
            var nextSquad = true;
            foreach (var squadCtr in ActiveSquads)
            {
                if (!squadCtr.SquadCreated)
                {
                    var ship = squadCtr.CreateShip();
                    if (ship != null)
                        shipsCreated.Add(ship);
                }
                if (!squadCtr.IsDone)
                    nextSquad = false;
            }

            if (nextSquad)
            {
                squadIndex++;
                SetSquads();
            }
            return shipsCreated;
        }

        void SetSquads()
        {
            ActiveSquads = new List<SquadController>();
            if (squadIndex >= Entity.Squads.Count)
            {
                IsDone = true;
                return;
            }

            ActiveSquads.Add(new SquadController(Entity.Squads[squadIndex]));

            if (squadIndex + 1 >= Entity.Squads.Count || Entity.Squads[squadIndex + 1].Syncronous == true)
                return;

            squadIndex++;
            ActiveSquads.Add(new SquadController(Entity.Squads[squadIndex]));
        }


    }
}
