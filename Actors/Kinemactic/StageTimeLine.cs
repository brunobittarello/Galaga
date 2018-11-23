using GalagaFramework.Actors.Kinemactic.Stage;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace GalagaFramework.Actors.Kinemactic
{
    public class StageTimeLine
    {
        public List<StageEntity> Stages;

        //Stage1
        //Demograph- Time
        //HHHH     - 0
        //MMMM     - 0
        //BMBMBMBM - 1
        //MMMMMM   - 2
        //HHHHHH   - 3
        //HHHHHH   - 4

        public StageTimeLine()
        {
            
        }

        public void Default()
        {
            Stages = new List<StageEntity>();
            var stage = new StageEntity()
            {
                IsBonus = false,
                Squads = new List<Squad.SquadEntity>()
            };

            var squad = new Squad.SquadEntity()
            {
                BezierPoints = new List<Vector2>(),
                EnemyType = new List<byte>() {
                    20, 21, 22, 23
                },
                DivePoint = 10,
                Syncronous = true
            };
            squad.BezierPoints.Add(new Vector2(0,40));
            squad.BezierPoints.Add(new Vector2(56,37));
            squad.BezierPoints.Add(new Vector2(38,66));
            squad.BezierPoints.Add(new Vector2(107,89));

            squad.BezierPoints.Add(new Vector2(184,88));
            squad.BezierPoints.Add(new Vector2(168,74));
            squad.BezierPoints.Add(new Vector2(242,62));

            var bezierPoints = squad.BezierPoints;
            stage.Squads.Add(squad);

            squad = new Squad.SquadEntity()
            {
                BezierPoints = bezierPoints,
                EnemyType = new List<byte>() {
                    4, 5, 6, 7
                },
                DivePoint = 10,
                Syncronous = false
            };
            stage.Squads.Add(squad);

            squad = new Squad.SquadEntity()
            {
                BezierPoints = bezierPoints,
                EnemyType = new List<byte>() {
                    0, 8, 1, 9, 2, 10, 3, 11
                },
                DivePoint = 10,
                Syncronous = true
            };
            stage.Squads.Add(squad);

            squad = new Squad.SquadEntity()
            {
                BezierPoints = bezierPoints,
                EnemyType = new List<byte>() {
                    12, 13, 14, 15, 16, 17, 18, 19
                },
                DivePoint = 10,
                Syncronous = true
            };
            stage.Squads.Add(squad);

            squad = new Squad.SquadEntity()
            {
                BezierPoints = bezierPoints,
                EnemyType = new List<byte>() {
                    24, 25, 26, 27, 28, 29, 30, 31
                },
                DivePoint = 10,
                Syncronous = true
            };
            stage.Squads.Add(squad);

            squad = new Squad.SquadEntity()
            {
                BezierPoints = bezierPoints,
                EnemyType = new List<byte>() {
                    32, 33, 34, 35, 36, 37, 38, 39
                },
                DivePoint = 10,
                Syncronous = true
            };
            stage.Squads.Add(squad);

            Stages.Add(stage);
        }

        public void Save()
        {
            /*
            var output = JsonConvert.SerializeObject(this);
            var file = new System.IO.StreamWriter("StageTL.txt");
            file.WriteLine(output);
            file.Close();
             * */
        }

        static public StageTimeLine Load()
        {
            //var file = new System.IO.StreamReader("StageTL.txt");
            //var json = file.ReadLine();
            //file.Close();

            var json = DefaultStage();
            return JsonConvert.DeserializeObject<StageTimeLine>(json);
        }

        static string DefaultStage()
        {
            return "{\"Stages\":[{\"IsBonus\":false,\"Squads\":[{\"BezierPoints\":[\"125.688896, -5.60000038\",\"113.244446, 75.91112\",\"100.8, 69.68889\",\"18.6666679, 138.755554\",\"13.0666676, 186.044449\",\"56.0000038, 197.244446\",\"70.31111, 163.644455\",\"88.35556, 146.844452\",\"82.13334, 145.6\",\"95.82223, 126.311119\"],\"EnemyType\":[20,21,22,23],\"DivePoint\":10,\"Syncronous\":true},{\"BezierPoints\":[\"97.06667, -1.86666679\",\"135.022232, 56.0000038\",\"130.044449, 72.8\",\"207.822235, 136.8889\",\"209.066681, 189.155563\",\"171.111115, 204.711121\",\"155.555557, 167.377777\",\"140, 146.222229\",\"149.333344, 146.222229\",\"130.666672, 123.822227\"],\"EnemyType\":[4,5,6,7],\"DivePoint\":10,\"Syncronous\":false},{\"BezierPoints\":[\"-7.466667, 258.222229\",\"92.71111, 240.17778\",\"89.6000061, 208.444458\",\"113.866669, 166.755554\",\"108.26667, 123.200005\",\"70.9333344, 129.422226\",\"70.31111, 166.133347\",\"77.1555557, 194.755569\",\"116.977783, 197.244446\",\"115.733337, 139.377777\"],\"EnemyType\":[0,8,1,9,2,10,3,11],\"DivePoint\":10,\"Syncronous\":true},{\"BezierPoints\":[\"228.977783, 262.5778\",\"155.555557, 237.6889\",\"138.133331, 242.666672\",\"115.111115, 168.622223\",\"113.866669, 120.711113\",\"171.111115, 128.17778\",\"171.733337, 159.911118\",\"163.022232, 197.866669\",\"116.977783, 197.244446\",\"115.733337, 139.377777\"],\"EnemyType\":[12,13,14,15,16,17,18,19],\"DivePoint\":10,\"Syncronous\":true},{\"BezierPoints\":[\"125.688896, -5.60000038\",\"113.244446, 75.91112\",\"100.8, 69.68889\",\"18.6666679, 138.755554\",\"13.0666676, 186.044449\",\"56.0000038, 197.244446\",\"70.31111, 163.644455\",\"88.35556, 146.844452\",\"82.13334, 145.6\",\"95.82223, 126.311119\"],\"EnemyType\":[24,25,26,27,28,29,30,31],\"DivePoint\":10,\"Syncronous\":true},{\"BezierPoints\":[\"97.06667, -1.86666679\",\"135.022232, 56.0000038\",\"130.044449, 72.8\",\"207.822235, 136.8889\",\"209.066681, 189.155563\",\"171.111115, 204.711121\",\"155.555557, 167.377777\",\"140, 146.222229\",\"149.333344, 146.222229\",\"130.666672, 123.822227\"],\"EnemyType\":[32,33,34,35,36,37,38,39],\"DivePoint\":10,\"Syncronous\":true}]}]}";
        }
    }
}
