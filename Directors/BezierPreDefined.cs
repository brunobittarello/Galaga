using GalagaFramework.Framework;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace GalagaFramework.Directors
{
    class BezierPreDefined
    {
        static internal BezierCurve Exit()
        {
            var bezier = new BezierCurve();
            var controlPoints = new List<Vector2>();

            controlPoints.Add(new Vector2(0, 0));
            controlPoints.Add(new Vector2(0, -25));
            controlPoints.Add(new Vector2(25, -25));
            controlPoints.Add(new Vector2(25, 0));

            bezier.SetControlPoints(controlPoints);
            return bezier;
        }

        static internal BezierCurve Attack()
        {
            var bezier = new BezierCurve();
            var controlPoints = new List<Vector2>();

            controlPoints.Add(new Vector2(0, 0));
            controlPoints.Add(new Vector2(13, 31));
            controlPoints.Add(new Vector2(24, 36));
            controlPoints.Add(new Vector2(44, 37));
            controlPoints.Add(new Vector2(74, 48));
            controlPoints.Add(new Vector2(81, 29));
            controlPoints.Add(new Vector2(101, 63));

            /*
            controlPoints.Add(new Vector2(0, 0));
            controlPoints.Add(new Vector2(60, 30));
            controlPoints.Add(new Vector2(20, 40));
            controlPoints.Add(new Vector2(123, 83));
            controlPoints.Add(new Vector2(152, 133));
            controlPoints.Add(new Vector2(175, 122));
            controlPoints.Add(new Vector2(189, 196));
            controlPoints.Add(new Vector2(52, 206));
            controlPoints.Add(new Vector2(46, 195));
            controlPoints.Add(new Vector2(25, 188));
            controlPoints.Add(new Vector2(13, 131));
            controlPoints.Add(new Vector2(0, 134));
            controlPoints.Add(new Vector2(12, 30));
            */
            bezier.SetControlPoints(controlPoints);
            return bezier;
        }
    }
}
