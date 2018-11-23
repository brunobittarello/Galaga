using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace GalagaFramework.Framework
{
    public class BezierCurve
    {
        Texture2D texture;
        const int SEGMENTS_PER_CURVE = 10;
        const float MINIMUM_SQR_DISTANCE = 0.01f;

        // This corresponds to about 172 degrees, 8 degrees from a traight line
        const float DIVISION_THRESHOLD = -0.99f;

        List<Vector2> controlPoints;
        List<GameSprite> ListImagePoints;

        int curveCount; //how many bezier curves in this path?

        /**
            Constructs a new empty Bezier curve. Use one of these methods
            to add points: SetControlPoints, Interpolate, SamplePoints.
        */
        public BezierCurve()
        {
            texture = TextureManager.Empty;
            controlPoints = new List<Vector2>();
            ListImagePoints = new List<GameSprite>();
        }

        /**
            Sets the control points of this Bezier path.
            Points 0-3 forms the first Bezier curve, points 
            3-6 forms the second curve, etc.
        */
        public void SetControlPoints(List<Vector2> newControlPoints)
        {
            controlPoints.Clear();
            controlPoints.AddRange(newControlPoints);
            curveCount = (controlPoints.Count - 1) / 3;
        }

        /**
            Returns the control points for this Bezier curve.
        */
        public List<Vector2> GetControlPoints()
        {
            return controlPoints;
        }

        /**
            Calculates a Bezier interpolated path for the given points.
        */
        public void Interpolate(List<Vector2> segmentPoints, float scale)
        {
            controlPoints.Clear();

            if (segmentPoints.Count < 2)
            {
                return;
            }

            for (int i = 0; i < segmentPoints.Count; i++)
            {
                if (i == 0) // is first
                {
                    var p1 = segmentPoints[i];
                    var p2 = segmentPoints[i + 1];

                    var tangent = (p2 - p1);
                    var q1 = p1 + scale * tangent;

                    controlPoints.Add(p1);
                    controlPoints.Add(q1);
                }
                else if (i == segmentPoints.Count - 1) //last
                {
                    var p0 = segmentPoints[i - 1];
                    var p1 = segmentPoints[i];
                    var tangent = (p1 - p0);
                    var q0 = p1 - scale * tangent;

                    controlPoints.Add(q0);
                    controlPoints.Add(p1);
                }
                else
                {
                    var p0 = segmentPoints[i - 1];
                    var p1 = segmentPoints[i];
                    var p2 = segmentPoints[i + 1];
                    var tangent = p2 - p0;
                    tangent.Normalize();
                    var q0 = p1 - scale * tangent * (p1 - p0).LengthSquared();
                    var q1 = p1 + scale * tangent * (p2 - p1).LengthSquared();

                    controlPoints.Add(q0);
                    controlPoints.Add(p1);
                    controlPoints.Add(q1);
                }
            }

            curveCount = (controlPoints.Count - 1) / 3;
        }

        /**
            Sample the given points as a Bezier path.
        */
        public void SamplePoints(List<Vector2> sourcePoints, float minSqrDistance, float maxSqrDistance, float scale)
        {
            if (sourcePoints.Count < 2)
            {
                return;
            }

            var samplePoints = new Stack<Vector2>();

            samplePoints.Push(sourcePoints[0]);

            var potentialSamplePoint = sourcePoints[1];

            int i = 2;

            for (i = 2; i < sourcePoints.Count; i++)
            {
                if (
                    ((potentialSamplePoint - sourcePoints[i]).LengthSquared() > minSqrDistance) &&
                    ((samplePoints.Peek() - sourcePoints[i]).LengthSquared() > maxSqrDistance))
                {
                    samplePoints.Push(potentialSamplePoint);
                }

                potentialSamplePoint = sourcePoints[i];
            }

            //now handle last bit of curve
            var p1 = samplePoints.Pop(); //last sample point
            var p0 = samplePoints.Peek(); //second last sample point
            var tangent = (p0 - potentialSamplePoint);
            tangent.Normalize();
            var d2 = (potentialSamplePoint - p1).LengthSquared();
            var d1 = (p1 - p0).LengthSquared();
            p1 = p1 + tangent * ((d1 - d2) / 2);

            samplePoints.Push(p1);
            samplePoints.Push(potentialSamplePoint);


            Interpolate(new List<Vector2>(samplePoints), scale);
        }

        /**
            Caluclates a point on the path.
        
            @param curveIndex The index of the curve that the point is on. For example, 
            the second curve (index 1) is the curve with controlpoints 3, 4, 5, and 6.
        
            @param t The paramater indicating where on the curve the point is. 0 corresponds 
            to the "left" point, 1 corresponds to the "right" end point.
        */
        public Vector2 CalculateBezierPoint(int curveIndex, float t)
        {
            int nodeIndex = curveIndex * 3;

            var p0 = controlPoints[nodeIndex];
            var p1 = controlPoints[nodeIndex + 1];
            var p2 = controlPoints[nodeIndex + 2];
            var p3 = controlPoints[nodeIndex + 3];

            return CalculateBezierPoint(t, p0, p1, p2, p3);
        }

        /**
            Gets the drawing points. This implementation simply calculates a certain number
            of points per curve.
        */
        public List<Vector2> GetDrawingPoints0()
        {
            var drawingPoints = new List<Vector2>();

            for (int curveIndex = 0; curveIndex < curveCount; curveIndex++)
            {
                if (curveIndex == 0) //Only do this for the first end point. 
                //When i != 0, this coincides with the 
                //end point of the previous segment,
                {
                    drawingPoints.Add(CalculateBezierPoint(curveIndex, 0));
                }

                for (int j = 1; j <= SEGMENTS_PER_CURVE; j++)
                {
                    float t = j / (float)SEGMENTS_PER_CURVE;
                    drawingPoints.Add(CalculateBezierPoint(curveIndex, t));
                }
            }

            return drawingPoints;
        }

        /**
            Gets the drawing points. This implementation simply calculates a certain number
            of points per curve.

            This is a lsightly different inplementation from the one above.
        */
        public List<Vector2> GetDrawingPoints1()
        {
            List<Vector2> drawingPoints = new List<Vector2>();

            for (int i = 0; i < controlPoints.Count - 3; i += 3)
            {
                var p0 = controlPoints[i];
                var p1 = controlPoints[i + 1];
                var p2 = controlPoints[i + 2];
                var p3 = controlPoints[i + 3];

                if (i == 0) //only do this for the first end point. When i != 0, this coincides with the end point of the previous segment,
                {
                    drawingPoints.Add(CalculateBezierPoint(0, p0, p1, p2, p3));
                }

                for (int j = 1; j <= SEGMENTS_PER_CURVE; j++)
                {
                    float t = j / (float)SEGMENTS_PER_CURVE;
                    drawingPoints.Add(CalculateBezierPoint(t, p0, p1, p2, p3));
                }
            }

            return drawingPoints;
        }

        /**
            This gets the drawing points of a bezier curve, using recursive division,
            which results in less points for the same accuracy as the above implementation.
        */
        public List<Vector2> GetDrawingPoints2()
        {
            var drawingPoints = new List<Vector2>();

            for (int curveIndex = 0; curveIndex < curveCount; curveIndex++)
            {
                var bezierCurveDrawingPoints = FindDrawingPoints(curveIndex);

                if (curveIndex != 0)
                {
                    //remove the fist point, as it coincides with the last point of the previous Bezier curve.
                    bezierCurveDrawingPoints.RemoveAt(0);
                }

                drawingPoints.AddRange(bezierCurveDrawingPoints);
            }

            return drawingPoints;
        }

        List<Vector2> FindDrawingPoints(int curveIndex)
        {
            var pointList = new List<Vector2>();

            var left = CalculateBezierPoint(curveIndex, 0);
            var right = CalculateBezierPoint(curveIndex, 1);

            pointList.Add(left);
            pointList.Add(right);

            FindDrawingPoints(curveIndex, 0, 1, pointList, 1);

            return pointList;
        }


        /**
            @returns the number of points added.
        */
        int FindDrawingPoints(int curveIndex, float t0, float t1,
            List<Vector2> pointList, int insertionIndex)
        {
            var left = CalculateBezierPoint(curveIndex, t0);
            var right = CalculateBezierPoint(curveIndex, t1);

            if ((left - right).LengthSquared() < MINIMUM_SQR_DISTANCE)
            {
                return 0;
            }

            var tMid = (t0 + t1) / 2;
            var mid = CalculateBezierPoint(curveIndex, tMid);

            var leftDirection = (left - mid);
            leftDirection.Normalize();
            var rightDirection = (right - mid);
            rightDirection.Normalize();

            if (Vector2.Dot(leftDirection, rightDirection) > DIVISION_THRESHOLD || Math.Abs(tMid - 0.5f) < 0.0001f)
            {
                int pointsAddedCount = 0;

                pointsAddedCount += FindDrawingPoints(curveIndex, t0, tMid, pointList, insertionIndex);
                pointList.Insert(insertionIndex + pointsAddedCount, mid);
                pointsAddedCount++;
                pointsAddedCount += FindDrawingPoints(curveIndex, tMid, t1, pointList, insertionIndex + pointsAddedCount);

                return pointsAddedCount;
            }

            return 0;
        }

        /**
            Caluclates a point on the Bezier curve represented with the four controlpoints given.
        */
        private Vector2 CalculateBezierPoint(float t, Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3)
        {
            float u = 1 - t;
            float tt = t * t;
            float uu = u * u;
            float uuu = uu * u;
            float ttt = tt * t;

            var p = uuu * p0; //first term

            p += 3 * uu * t * p1; //second term
            p += 3 * u * tt * p2; //third term
            p += ttt * p3; //fourth term

            return p;
        }

        internal void Mirror()
        {
            var count = controlPoints.Count;
            for (int i = 0; i < count; i++)
                controlPoints[i] *= new Vector2(-1, 1);
            Apply();
        }

        internal void RelativeBezierToPoint(Vector2 point)
        {
            var count = controlPoints.Count;
            for (int i = 0; i < count; i++)
                controlPoints[i] += point;
            Apply();
        }

        public void Apply()
        {
            ListImagePoints.Clear();
            var points = GetDrawingPoints0();
            foreach (var point in points)
                ListImagePoints.Add(new GameSprite(texture, point, 0.1f));
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var imagePoint in ListImagePoints)
                imagePoint.Draw(spriteBatch);
        }
    }
}
