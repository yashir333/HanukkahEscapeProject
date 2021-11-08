using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ciitt.EscapeGameKit
{

    /// <summary>
    /// Simple bezier path
    /// </summary>
    public class SimpleBezierPath
    {

        class LengthAndStartAndEnd
        {
            public float startTotalLengthMeter = 0.0f;
            public float endTotalLengthMeter = 0.0f;
            public SimpleBezierHandlePoint refStart = null;
            public SimpleBezierHandlePoint refEnd = null;
        }

        /// <summary>
        /// Sampling meters list
        /// </summary>
        List<LengthAndStartAndEnd> m_samplingMeters = new List<LengthAndStartAndEnd>();

        /// <summary>
        /// Total meter length
        /// </summary>
        float m_totalMeterLength = 0.0f;

        Vector3 p0 = Vector3.zero;
        Vector3 p1 = Vector3.zero;
        Vector3 p2 = Vector3.zero;
        Vector3 p3 = Vector3.zero;

        float omt = 0f;
        float omt2 = 0f;
        float t2 = 0f;

        /// <summary>
        /// SimpleBezierHandlePoint list
        /// </summary>
        List<SimpleBezierHandlePoint> m_simpleBezierPointList = new List<SimpleBezierHandlePoint>();

        /// <summary>
        /// Temp Transform list
        /// </summary>
        List<Transform> m_tempTransformList = new List<Transform>();

        /// <summary>
        /// Temp Vector3 list
        /// </summary>
        List<Vector3> m_tempVector3List = new List<Vector3>();

        // -------------------------------------------------------------------------------------------

        /// <summary>
        /// Total meter length
        /// </summary>
        public float totalMeterLength { get { return this.m_totalMeterLength; } }

        /// <summary>
        /// SimpleBezierPath
        /// </summary>
        // -------------------------------------------------------------------------------------------
        public SimpleBezierPath()
        {

        }

        /// <summary>
        /// SimpleBezierPath
        /// </summary>
        /// <param name="from">from</param>
        /// <param name="to">to</param>
        /// <param name="wayPointList">way point list</param>
        /// <param name="sampling">bezier sampling</param>
        /// <param name="tangentStrength">tangent strength</param>
        // -------------------------------------------------------------------------------------------
        public SimpleBezierPath(Transform from, Transform to, List<Transform> wayPointList, int sampling = 10, float tangentStrength = 0.3f)
        {
            this.setValues(from, to, wayPointList, sampling, tangentStrength);
        }


        /// <summary>
        /// SimpleBezierPath
        /// </summary>
        /// <param name="from">from</param>
        /// <param name="to">to</param>
        /// <param name="toRot">to Rotstion</param>
        /// <param name="wayPointList">way point list</param>
        /// <param name="sampling">bezier sampling</param>
        /// <param name="tangentStrength">tangent strength</param>
        // -------------------------------------------------------------------------------------------
        public SimpleBezierPath(Vector3 from, Vector3 to, Quaternion toRot, List<Transform> wayPointList, int sampling = 10, float tangentStrength = 0.3f)
        {
            this.setValues(from, to, toRot, wayPointList, sampling, tangentStrength);
        }

        /// <summary>
        /// Clear
        /// </summary>
        // -------------------------------------------------------------------------------------------
        protected void clear()
        {

            this.m_samplingMeters.Clear();

            this.m_totalMeterLength = 0.0f;

            p0 = Vector3.zero;
            p1 = Vector3.zero;
            p2 = Vector3.zero;
            p3 = Vector3.zero;

            omt = 0f;
            omt2 = 0f;
            t2 = 0f;

            this.m_simpleBezierPointList.Clear();

            this.m_tempTransformList.Clear();
            this.m_tempVector3List.Clear();

        }

        /// <summary>
        /// Set values
        /// </summary>
        /// <param name="from">from</param>
        /// <param name="to">to</param>
        /// <param name="wayPointList">way point list</param>
        /// <param name="sampling">bezier sampling</param>
        /// <param name="tangentStrength">tangent strength</param>
        // -------------------------------------------------------------------------------------------
        public void setValues(Transform from, Transform to, List<Transform> wayPointList, int sampling = 10, float tangentStrength = 0.3f)
        {

            this.clear();

            // add
            {

                if (from)
                {
                    this.m_tempTransformList.Add(from);
                }

                this.m_tempTransformList.AddRange(wayPointList);

                if (to)
                {
                    this.m_tempTransformList.Add(to);
                }

            }

            // m_simpleBezierPointList
            {

                int size = this.m_tempTransformList.Count;

                for (int i = 0; i < size; i++)
                {

                    this.m_simpleBezierPointList.Add(
                        SimpleBezierHandlePoint.create(
                            this.m_tempTransformList[Mathf.Max(0, i - 1)],
                            this.m_tempTransformList[i],
                            this.m_tempTransformList[Mathf.Min(size - 1, i + 1)],
                            tangentStrength
                            )
                        );

                }

            }

            // samplingBezierLength
            {
                this.samplingBezierLength(sampling);
            }

        }

        /// <summary>
        /// Set values
        /// </summary>
        /// <param name="from">from</param>
        /// <param name="to">to</param>
        /// <param name="toRot">to Rotstion</param>
        /// <param name="wayPointList">way point list</param>
        /// <param name="sampling">bezier sampling</param>
        /// <param name="tangentStrength">tangent strength</param>
        // -------------------------------------------------------------------------------------------
        public void setValues(Vector3 from, Vector3 to, Quaternion toRot, List<Transform> wayPointList, int sampling = 10, float tangentStrength = 0.3f)
        {

            this.clear();

            //
            {

                this.m_tempVector3List.Add(from);

                foreach (var val in wayPointList)
                {
                    this.m_tempVector3List.Add(val.position);
                }

                this.m_tempVector3List.Add(to);

            }

            // m_simpleBezierPointList
            {

                int size = this.m_tempVector3List.Count;

                for (int i = 0; i < size; i++)
                {

                    this.m_simpleBezierPointList.Add(
                        SimpleBezierHandlePoint.create(
                            this.m_tempVector3List[Mathf.Max(0, i - 1)],
                            this.m_tempVector3List[i],
                            this.m_tempVector3List[Mathf.Min(size - 1, i + 1)],
                            toRot,
                            tangentStrength
                            )
                        );

                }

            }

            // samplingBezierLength
            {
                this.samplingBezierLength(sampling);
            }

        }

        /// <summary>
        /// Sampling
        /// </summary>
        // -------------------------------------------------------------------------------------------
        protected void samplingBezierLength(int sampling)
        {

            this.m_totalMeterLength = 0.0f;
            this.m_samplingMeters.Clear();

            // ---------------

            int size = this.m_simpleBezierPointList.Count;

            Vector3 point0 = Vector3.zero;
            Vector3 point1 = Vector3.zero;

            LengthAndStartAndEnd temp = null;

            sampling = sampling / size;

            for (int i = 0; i < size - 1; i++)
            {

                temp = new LengthAndStartAndEnd();

                temp.refStart = this.m_simpleBezierPointList[i];
                temp.refEnd = this.m_simpleBezierPointList[i + 1];
                temp.startTotalLengthMeter = this.m_totalMeterLength;

                // editor
                {

                    if (temp.refStart == null || temp.refEnd == null)
                    {
                        Debug.LogError("temp.refStart == null || temp.refEnd == null");
                        continue;
                    }

                }

                point0 = temp.refStart.worldPoint;

                for (int j = 1; j <= sampling; j++)
                {

                    point1 = this.pointAtOneBezierCurve(temp.refStart, temp.refEnd, (float)j / (float)sampling, true);

                    this.m_totalMeterLength += Vector3.Distance(point0, point1);

                    point0 = point1;

                }

                temp.endTotalLengthMeter = this.m_totalMeterLength;

                this.m_samplingMeters.Add(temp);

            }

        }

        /// <summary>
        /// Get point at bezier curve
        /// </summary>
        /// <param name="t">distance</param>
        /// <param name="linear">linear</param>
        /// <returns>point</returns>
        // -------------------------------------------------------------------------------------------
        public Vector3 pointAtBezierCurve(float distance, bool linear)
        {

            SimpleBezierHandlePoint start = null;
            SimpleBezierHandlePoint end = null;
            float new_t = 0.0f;

            this.convert(distance, out start, out end, out new_t);

            return this.pointAtOneBezierCurve(
                start,
                end,
                new_t,
                linear
                );

        }

        /// <summary>
        /// Get point at one bezier curve
        /// </summary>
        /// <param name="start">start BezierHandleScript</param>
        /// <param name="end">end BezierHandleScript</param>
        /// <param name="t">t</param>
        /// <param name="linear">linear</param>
        /// <returns>point</returns>
        // -------------------------------------------------------------------------------------------
        protected Vector3 pointAtOneBezierCurve(SimpleBezierHandlePoint start, SimpleBezierHandlePoint end, float t, bool linear)
        {

            if (start == null && end == null)
            {
                return Vector3.zero;
            }

            else if (start == null)
            {
                return end.worldPoint;
            }

            else if (end == null)
            {
                return start.worldPoint;
            }

            // ----------------------------

            t = Mathf.Clamp01(t);

            p0 = start.worldPoint;
            p1 = start.worldStartTangentPoint;
            p2 = end.worldEndTangentPoint;
            p3 = end.worldPoint;

            if(linear)
            {
                return Vector3.Lerp(p0, p3, t);
            }

            omt = 1f - t;
            omt2 = omt * omt;
            t2 = t * t;

            return
                p0 * (omt2 * omt) +
                p1 * (3f * omt2 * t) +
                p2 * (3f * omt * t2) +
                p3 * (t2 * t)
                ;

        }

        /// <summary>
        /// Get BezierPoint at bezier curve
        /// </summary>
        /// <param name="t">t</param>
        /// <param name="linear">linear</param>
        /// <returns>BezierPoint</returns>
        // -------------------------------------------------------------------------------------------
        public BezierPoint pointAtBezierCurveDetail(float t, bool linear)
        {

            BezierPoint ret = new BezierPoint();

            SimpleBezierHandlePoint start = null;
            SimpleBezierHandlePoint end = null;
            float new_t = 0.0f;

            this.convert(t, out start, out end, out new_t);

            ret.point = this.pointAtOneBezierCurve(
                start,
                end,
                new_t,
                linear
                );

            ret.tangent = this.normalizedTangent(t);

            if (start != null && end != null)
            {
                ret.rotation = Quaternion.Slerp(start.rotation, end.rotation, new_t);
            }

            else
            {
                ret.rotation = Quaternion.identity;
            }

            return ret;

        }

        /// <summary>
        /// Get tangent at point
        /// </summary>
        /// <param name="t">t</param>
        /// <returns>tangent</returns>
        // -------------------------------------------------------------------------------------------
        public Vector3 normalizedTangent(float t)
        {

            t = Mathf.Clamp(t, 0.0f, 0.999f);

            Vector3 p0 = this.pointAtBezierCurve(t, false);
            Vector3 p1 = this.pointAtBezierCurve(t + 0.001f, false);

            return (p1 - p0).normalized;

        }

        /// <summary>
        /// Convert t
        /// </summary>
        /// <param name="distance">distance</param>
        /// <param name="start">SimpleBezierHandlePoint start</param>
        /// <param name="end">SimpleBezierHandlePoint end</param>
        /// <param name="converted_t">new t</param>
        // -------------------------------------------------------------------------------------------
        void convert(float distance, out SimpleBezierHandlePoint start, out SimpleBezierHandlePoint end, out float converted_t)
        {

            if (this.m_samplingMeters.Count <= 0 || this.m_totalMeterLength <= 0.0f)
            {

                if (this.m_simpleBezierPointList.Count > 0)
                {
                    start = this.m_simpleBezierPointList[0];
                    end = this.m_simpleBezierPointList[0];
                    converted_t = 0.0f;
                }

                else
                {
                    start = null;
                    end = null;
                    converted_t = 0.0f;
                }

                return;

            }

            // -------------------------------

            distance = Mathf.Clamp(distance, 0.0f, this.m_totalMeterLength);

            // -------------------------------

            // this.m_samplingMeters.Count >= 2
            {

                //float targetLength = this.m_samplingMeters[this.m_samplingMeters.Count - 1].endTotalLengthMeter * t;

                for (int i = this.m_samplingMeters.Count - 1; i >= 0; i--)
                {

                    if (distance >= this.m_samplingMeters[i].startTotalLengthMeter)
                    {

                        start = this.m_samplingMeters[i].refStart;
                        end = this.m_samplingMeters[i].refEnd;

                        converted_t = Mathf.InverseLerp(
                            this.m_samplingMeters[i].startTotalLengthMeter,
                            this.m_samplingMeters[i].endTotalLengthMeter,
                            distance
                            );

                        return;

                    }

                }

            }

            // this.m_samplingMeters.Count == 1
            {

                start = this.m_samplingMeters[0].refStart;
                end = this.m_samplingMeters[0].refEnd;
                converted_t = Mathf.InverseLerp(
                    this.m_samplingMeters[0].startTotalLengthMeter,
                    this.m_samplingMeters[0].endTotalLengthMeter,
                    distance
                    );

            }

        }

    }

}
