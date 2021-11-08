using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SSC
{

    /// <summary>
    /// Bezier curve
    /// </summary>
    [ExecuteInEditMode]
    public class BezierCurveScript : MonoBehaviour
    {

        /// <summary>
        /// Bezier sampling info
        /// </summary>
        public class SamplingInfo
        {

            /// <summary>
            /// Position
            /// </summary>
            public Vector3 position = Vector3.zero;

            /// <summary>
            /// Rotation
            /// </summary>
            public Quaternion rotation = Quaternion.identity;

            /// <summary>
            /// Set values
            /// </summary>
            /// <param name="_position">position</param>
            /// <param name="_rotation">rotation</param>
            public void Set(Vector3 _position, Quaternion _rotation)
            {
                this.position = _position;
                this.rotation = _rotation;
            }

        }

        /// <summary>
        /// Bezier sampling info
        /// </summary>
        protected class SamplingInfoWithLength : SamplingInfo
        {

            /// <summary>
            /// Total meter for this instance
            /// </summary>
            public float currentTotalMeter = 0.0f;

        }

        /// <summary>
        /// Bezier sampling info struct
        /// </summary>
        [Obsolete("Bad performance struct", false)]
        public struct SamplingInfoStruct
        {

            /// <summary>
            /// Position
            /// </summary>
            public Vector3 position;

            /// <summary>
            /// Rotation
            /// </summary>
            public Quaternion rotation;

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="_position">position</param>
            /// <param name="_rotation">rotation</param>
            public SamplingInfoStruct(Vector3 _position, Quaternion _rotation)
            {
                this.position = _position;
                this.rotation = _rotation;
            }

        }

        /// <summary>
        /// Bezter curve sampling rate
        /// </summary>
        [SerializeField]
        [Tooltip("Bezter curve sampling rate")]
        [Range(1, 50)]
        protected int m_sampling = 20;

        /// <summary>
        /// Close bezter curve
        /// </summary>
        [SerializeField]
        [Tooltip("Close bezter curve")]
        protected bool m_close = false;

        /// <summary>
        /// BezierHandleScript list
        /// </summary>
        [SerializeField]
        [ReadOnly]
        protected List<BezierHandleScript> m_bezierHandleList = new List<BezierHandleScript>();

        /// <summary>
        /// SamplingInfo list
        /// </summary>
        [SerializeField]
        [HideInInspector]
        protected List<SamplingInfoWithLength> m_samplingInfoList = new List<SamplingInfoWithLength>();

        Vector3 p0 = Vector3.zero;
        Vector3 p1 = Vector3.zero;
        Vector3 p2 = Vector3.zero;
        Vector3 p3 = Vector3.zero;

        float omt = 0f;
        float omt2 = 0f;
        float t2 = 0f;

#if UNITY_EDITOR

        /// <summary>
        /// Add BezierHandleScript EditorOnly
        /// </summary>
        /// <param name="bhs">BezierHandleScript</param>
        // -------------------------------------------------------------------------------------------
        public void addBezierHandleScriptEditorOnly(BezierHandleScript bhs)
        {
            this.m_bezierHandleList.Add(bhs);
        }

        /// <summary>
        /// Init EditorOnly
        /// </summary>
        // -------------------------------------------------------------------------------------------
        public void initEditorOnly()
        {
            this.init();
        }

        /// <summary>
        /// Clear bezier handle list EditorOnly
        /// </summary>
        // -------------------------------------------------------------------------------------------
        public void clearBezierHandleListEditorOnly()
        {
            this.m_bezierHandleList.Clear();
        }

        /// <summary>
        /// Create new bezier EditorOnly
        /// </summary>
        // -------------------------------------------------------------------------------------------
        public static void createNewBezierCurveEditorOnly()
        {

            GameObject bezier = new GameObject("Bezier Curve");

            // points
            {

                GameObject point1 = new GameObject("Bezier Handle (1)");
                GameObject point2 = new GameObject("Bezier Handle (2)");

                // SetParent
                {
                    point1.transform.SetParent(bezier.transform);
                    point2.transform.SetParent(bezier.transform);
                }

                // position
                {
                    point1.transform.position = Vector3.zero;
                    point2.transform.position = new Vector3(5.0f, 0, 0);
                }

                // BezierHandleScript
                {
                    point1.AddComponent<BezierHandleScript>();
                    point2.AddComponent<BezierHandleScript>();
                }

            }

            // BezierCurveScript
            {
                bezier.AddComponent<BezierCurveScript>().initEditorOnly();
            }

        }

#endif

        /// <summary>
        /// Awake
        /// </summary>
        // -------------------------------------------------------------------------------------------
        protected virtual void Awake()
        {

            this.init();

        }

#if UNITY_EDITOR

        /// <summary>
        /// Update
        /// </summary>
        // -------------------------------------------------------------------------------------------
        void Update()
        {

            if (UnityEditor.Selection.activeTransform == this.transform && !Application.isPlaying && this.transform.hasChanged)
            {
                this.init();
            }

        }

#endif

        /// <summary>
        /// Sampled bezier meter length
        /// </summary>
        /// <returns>bezier meter length</returns>
        // -------------------------------------------------------------------------------------------
        public float sampledBezierMeterLength()
        {

            if(this.m_samplingInfoList.Count > 0)
            {
                return this.m_samplingInfoList[this.m_samplingInfoList.Count - 1].currentTotalMeter;
            }

            return 0.0f;

        }

        /// <summary>
        /// init
        /// </summary>
        // -------------------------------------------------------------------------------------------
        protected void init()
        {
            
            // m_sampling
            {
                this.m_sampling = Mathf.Max(1, this.m_sampling);
            }

            // m_bezierHandleList
            {

                this.m_bezierHandleList.Clear();

                BezierHandleScript temp = null;
                
                foreach(Transform child in this.transform)
                {

                    temp = child.GetComponent<BezierHandleScript>();

                    if(temp)
                    {
                        this.m_bezierHandleList.Add(temp);
                    }

                }

            }

            // m_close
            {
                if(this.m_close && this.m_bezierHandleList.Count >= 2)
                {
                    this.m_bezierHandleList.Add(this.m_bezierHandleList[0]);
                }
            }

            // check
            {

                if (Application.isPlaying)
                {

                    for (int i = this.m_bezierHandleList.Count - 1; i >= 0; i--)
                    {

                        if (!this.m_bezierHandleList[i])
                        {
                            Debug.LogWarning("m_bezierHandleList has empty element : " + Funcs.CreateHierarchyPath(this.transform));
                            this.m_bezierHandleList.RemoveAt(i);
                        }

                    }

                }

            }

            // samplingBezierLength
            {
                this.samplingBezierLength();
            }

        }

        /// <summary>
        /// Sampling
        /// </summary>
        // -------------------------------------------------------------------------------------------
        protected void samplingBezierLength()
        {

            this.m_samplingInfoList.Clear();

            // -------------------

            int size = this.m_bezierHandleList.Count;

            float total = 0.0f;

            Vector3 point0 = Vector3.zero;
            Vector3 point1 = Vector3.zero;

            SamplingInfoWithLength temp = null;

            BezierHandleScript start = null;
            BezierHandleScript end = null;

            float t01 = 0.0f;

            // -------------------

            for (int i = 0; i < size - 1; i++)
            {

                // start end
                {
                    start = this.m_bezierHandleList[i];
                    end = this.m_bezierHandleList[i + 1];
                }

                // first
                {

                    point0 = 
                        (i > 0) ?
                        this.m_samplingInfoList[this.m_samplingInfoList.Count - 1].position :
                        start.point()
                        ;

                }

                // meter sampling
                {

                    for (int j = 0; j < this.m_sampling; j++)
                    {

                        // t01
                        {
                            t01 = (float)j / (float)this.m_sampling;
                        }

                        // new
                        {
                            temp = new SamplingInfoWithLength();
                        }

                        // point1
                        {
                            point1 = this.pointAtOneBezierCurve(start, end, t01);
                        }

                        // total
                        {
                            total += Vector3.Distance(point0, point1);
                        }

                        // set
                        {
                            temp.position = point1;
                            temp.currentTotalMeter = total;
                            temp.rotation = Quaternion.Lerp(start.transform.rotation, end.transform.rotation, t01);
                        }

                        // add
                        {
                            this.m_samplingInfoList.Add(temp);
                        }

                        // change
                        {
                            point0 = point1;
                        }

                    }

                }

            }

            // last
            {

                if (size >= 2 && this.m_samplingInfoList.Count >= 1)
                {

                    temp = new SamplingInfoWithLength();

                    temp.position = this.m_bezierHandleList[size - 1].point();
                    temp.rotation = this.m_bezierHandleList[size - 1].transform.rotation;
                    temp.currentTotalMeter = total + Vector3.Distance(this.m_samplingInfoList[this.m_samplingInfoList.Count - 1].position, temp.position);

                    this.m_samplingInfoList.Add(temp);

                }

            }

        }

        /// <summary>
        /// Find sampled point
        /// </summary>
        /// <param name="t01">normalized t</param>
        /// <param name="refSamplingInfo">ref SamplingInfo</param>
        // -------------------------------------------------------------------------------------------
        public void findSampledPointByNormalizedValue(float t01, SamplingInfo refSamplingInfo)
        {

            t01 = Mathf.Clamp01(t01);

            this.findSampledPointByMeter(this.sampledBezierMeterLength() * t01, refSamplingInfo);

        }


        /// <summary>
        /// Find sampled point
        /// </summary>
        /// <param name="targetMeterLength">target meter length</param>
        /// <param name="refSamplingInfo">ref SamplingInfo</param>
        // -------------------------------------------------------------------------------------------
        public void findSampledPointByMeter(float targetMeterLength, SamplingInfo refSamplingInfo)
        {

            if (
                this.m_samplingInfoList.Count <= 1 ||
                targetMeterLength >= this.m_samplingInfoList[this.m_samplingInfoList.Count - 1].currentTotalMeter
                )
            {
                return;
            }

            // --------------

            if (refSamplingInfo == null)
            {
                refSamplingInfo = new SamplingInfo();
            }

            // --------------


            int halfsize = this.m_samplingInfoList.Count / 2;

            SamplingInfoWithLength half = this.m_samplingInfoList[halfsize];

            int first_i = (targetMeterLength < half.currentTotalMeter) ? 0 : halfsize - 1;
            int last_i = (targetMeterLength < half.currentTotalMeter) ? halfsize : this.m_samplingInfoList.Count - 1;

            SamplingInfoWithLength current = null;
            SamplingInfoWithLength next = null;

            float new_t = 0.0f;

            // --------------

            for (int i = first_i; i < last_i; i++)
            {

                current = this.m_samplingInfoList[i];
                next = this.m_samplingInfoList[i + 1];

                if (targetMeterLength <= next.currentTotalMeter)
                {

                    new_t = Mathf.InverseLerp(current.currentTotalMeter, next.currentTotalMeter, targetMeterLength);

                    refSamplingInfo.Set(
                        Vector3.Lerp(current.position, next.position, new_t),
                        Quaternion.Slerp(current.rotation, next.rotation, new_t)
                        );

                    break;

                }

            }

        }


        /// <summary>
        /// Find sampled point
        /// </summary>
        /// <param name="t01">normalized t</param>
        /// <returns>SamplingInfoStruct</returns>
        [Obsolete("Bad performance", false)]
        // -------------------------------------------------------------------------------------------
        public SamplingInfoStruct findSampledPointByNormalizedValue(float t01)
        {

            t01 = Mathf.Clamp01(t01);

            return this.findSampledPointByMeter(this.sampledBezierMeterLength() * t01);

        }

        /// <summary>
        /// Find sampled point
        /// </summary>
        /// <param name="targetMeterLength">target meter length</param>
        /// <returns>SamplingInfoStruct</returns>
        [Obsolete("Bad performance", false)]
        // -------------------------------------------------------------------------------------------
        public SamplingInfoStruct findSampledPointByMeter(float targetMeterLength)
        {

            if (this.m_samplingInfoList.Count <= 1)
            {
                return new SamplingInfoStruct();
            }

            if(targetMeterLength >= this.m_samplingInfoList[this.m_samplingInfoList.Count - 1].currentTotalMeter)
            {
                return new SamplingInfoStruct(
                    this.m_samplingInfoList[this.m_samplingInfoList.Count - 1].position,
                    this.m_samplingInfoList[this.m_samplingInfoList.Count - 1].rotation
                    );
            }

            // --------------


            int halfsize = this.m_samplingInfoList.Count / 2;

            SamplingInfoWithLength half = this.m_samplingInfoList[halfsize];

            int first_i = (targetMeterLength < half.currentTotalMeter) ? 0 : halfsize - 1;
            int last_i = (targetMeterLength < half.currentTotalMeter) ? halfsize : this.m_samplingInfoList.Count - 1;

            SamplingInfoWithLength current = null;
            SamplingInfoWithLength next = null;

            float new_t = 0.0f;

            // --------------

            for (int i = first_i; i < last_i; i++)
            {

                current = this.m_samplingInfoList[i];
                next = this.m_samplingInfoList[i + 1];

                if(targetMeterLength <= next.currentTotalMeter)
                {

                    new_t = Mathf.InverseLerp(current.currentTotalMeter, next.currentTotalMeter, targetMeterLength);

                    return new SamplingInfoStruct(
                        Vector3.Lerp(current.position, next.position, new_t),
                        Quaternion.Lerp(current.rotation, next.rotation, new_t)
                        );

                }

            }

            return new SamplingInfoStruct();

        }

        /// <summary>
        /// Get point at one bezier curve
        /// </summary>
        /// <param name="start">start BezierHandleScript</param>
        /// <param name="end">end BezierHandleScript</param>
        /// <param name="t">t</param>
        /// <returns>point</returns>
        // -------------------------------------------------------------------------------------------
        protected Vector3 pointAtOneBezierCurve(BezierHandleScript start, BezierHandleScript end, float t)
        {

            if(!start && !end)
            {
                return Vector3.zero;
            }

            else if(!start)
            {
                return end.point();
            }

            else if (!end)
            {
                return start.point();
            }

            // ----------------------------

            t = Mathf.Clamp01(t);

            p0 = start.point();
            p1 = start.startTangentHandlePoint();
            p2 = end.endTangentHandlePoint();
            p3 = end.point();

            omt = 1f - t;
            omt2 = omt * omt;
            t2 = t * t;

            return
                (p0 * (omt2 * omt)) +
                (p1 * (3f * omt2 * t)) +
                (p2 * (3f * omt * t2)) +
                (p3 * (t2 * t))
                ;

        }

        /// <summary>
        /// OnDrawGizmos
        /// </summary>
        // -------------------------------------------------------------------------------------------
        protected virtual void OnDrawGizmos()
        {

#if UNITY_EDITOR

            BezierHandleScript current = null;
            BezierHandleScript next = null;

            int size = this.m_bezierHandleList.Count;

            UnityEditor.Handles.color = Color.green;

            Vector3 arrow0 = Vector3.zero;
            Vector3 arrow1 = Vector3.zero;
            Vector3 arrow2 = Vector3.zero;

            for (int i = 0; i < size - 1; i++)
            {

                if(!this.m_bezierHandleList[i] || !this.m_bezierHandleList[i + 1])
                {
                    continue;
                }

                // ----------------

                current = this.m_bezierHandleList[i];
                next = this.m_bezierHandleList[i + 1];

                if (!current || !next)
                {
                    continue;
                }

                // ---------

                // DrawAAConvexPolygon
                {
                    arrow0 = current.point() + (current.transform.right * 0.25f);
                    arrow1 = current.point() + (current.transform.forward * 1.0f);
                    arrow2 = current.point() - (current.transform.right * 0.25f);

                    UnityEditor.Handles.DrawAAConvexPolygon(arrow0, arrow1, arrow2);
                }

                // DrawBezier
                {

                    UnityEditor.Handles.DrawBezier(
                        current.transform.position,
                        next.transform.position,
                        current.startTangentHandlePoint(),
                        next.endTangentHandlePoint(),
                        Color.red,
                        null,
                        3.0f
                        );

                }

            }

            if (size > 0)
            {

                current = this.m_bezierHandleList[this.m_bezierHandleList.Count - 1];

                if(current)
                {

                    arrow0 = current.point() + (current.transform.right * 0.25f);
                    arrow1 = current.point() + (current.transform.forward * 1.0f);
                    arrow2 = current.point() - (current.transform.right * 0.25f);

                    UnityEditor.Handles.DrawAAConvexPolygon(arrow0, arrow1, arrow2);

                }

            }

#endif

        }

    }

}
