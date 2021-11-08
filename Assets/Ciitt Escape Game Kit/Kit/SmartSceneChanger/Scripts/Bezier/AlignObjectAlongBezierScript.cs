using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SSC
{

    /// <summary>
    /// Align GameObjects along a bezier
    /// </summary>
    public class AlignObjectAlongBezierScript : MonoBehaviour
    {

        public enum LookAtType
        {
            None,
            HandleRotation,
            LookAtObject,
            LookAtObjectInverse,
        }

        /// <summary>
        /// Reference to BezierCurveScript
        /// </summary>
        [SerializeField]
        [Tooltip("Reference to BezierCurveScript")]
        protected BezierCurveScript m_refBezierCurveScript = null;

        /// <summary>
        /// Offset meter from start point
        /// </summary>
        [SerializeField]
        [Tooltip("Offset meter from start point")]
        protected float m_startOffsetMeter = 0.0f;

        /// <summary>
        /// Offset meter between objects
        /// </summary>
        [SerializeField]
        [Tooltip("Offset meter between objects")]
        protected float m_betweenOffsetMeter = 0.0f;

        /// <summary>
        /// LookAtType
        /// </summary>
        [SerializeField]
        [Tooltip("LookAtType")]
        protected LookAtType m_lookAtType = LookAtType.HandleRotation;

        /// <summary>
        /// Reference to object to lookat
        /// </summary>
        [SerializeField]
        [Tooltip("Reference to object to lookat")]
        protected Transform m_refLookAt = null;

        /// <summary>
        /// BezierCurveScript.SamplingInfo
        /// </summary>
        protected BezierCurveScript.SamplingInfo m_samplingInfo = new BezierCurveScript.SamplingInfo();

#if UNITY_EDITOR

        /// <summary>
        /// OnValidate Sync EditorOnly
        /// </summary>
        [SerializeField]
        [Tooltip("OnValidate Sync EditorOnly")]
        protected bool m_onValidateSyncEditorOnly = false;

#endif

        /// <summary>
        /// Awake
        /// </summary>
        // --------------------------------------------------------------------------------------------
        protected virtual void Awake()
        {
            Destroy(this);
        }

        /// <summary>
        /// OnValidate
        /// </summary>
        // --------------------------------------------------------------------------------------------
        protected virtual void OnValidate()
        {

#if UNITY_EDITOR

            if (!Application.isPlaying && this.m_onValidateSyncEditorOnly)
            {
                this.alignChildren();
            }

#endif

        }

        /// <summary>
        /// Align children along bezier
        /// </summary>
        // --------------------------------------------------------------------------------------------
        public void alignChildren()
        {

            if(!this.enabled || !this.m_refBezierCurveScript)
            {
                return;
            }

            // --------------

            float bezierLength = this.m_refBezierCurveScript.sampledBezierMeterLength();

            // --------------

            if (bezierLength > 0.0f)
            {

                float currentPos = this.m_startOffsetMeter % bezierLength;

                Transform child = null;

                for(int i = 0; i < this.transform.childCount; i++)
                {

                    // ------------------

                    child = this.transform.GetChild(i);

                    // ------------------

                    // bounds1
                    {
                        if (i > 0)
                        {
                            Bounds bounds1 = this.getBounds(child.gameObject);
                            currentPos = (currentPos + (bounds1.size.x / 2f)) % bezierLength;
                        }
                    }

                    // set
                    {

                        // findSampledPointByMeter
                        {
                            this.m_refBezierCurveScript.findSampledPointByMeter(currentPos, this.m_samplingInfo);
                        }

                        // position
                        {
                            child.position = this.m_samplingInfo.position;
                        }

                        // rotation
                        {

                            if (this.m_lookAtType == LookAtType.HandleRotation)
                            {
                                child.rotation = this.m_samplingInfo.rotation;
                            }

                            else if(this.m_lookAtType == LookAtType.LookAtObject)
                            {
                                if(this.m_refLookAt)
                                {
                                    child.LookAt(this.m_refLookAt, Vector3.up);
                                }
                            }

                            else if (this.m_lookAtType == LookAtType.LookAtObjectInverse)
                            {
                                if (this.m_refLookAt)
                                {
                                    Vector3 direction = (child.position - this.m_refLookAt.position).normalized;
                                    child.LookAt(child.position + direction, Vector3.up);
                                }
                            }

                        }
                        
                    }

                    // bounds2
                    {
                        Bounds bounds2 = this.getBounds(child.gameObject);
                        currentPos = (currentPos + this.m_betweenOffsetMeter + (bounds2.size.x / 2f)) % bezierLength;
                    }

                }

            }

        }

        /// <summary>
        /// Calc bounds
        /// </summary>
        /// <param name="obj">GameObject</param>
        /// <returns>Bounds</returns>
        // --------------------------------------------------------------------------------------------
        protected virtual Bounds getBounds(GameObject obj)
        {

            if (!obj)
            {
                return new Bounds();
            }

            // ----------------------

            Vector3 min = Vector3.zero;
            Vector3 max = Vector3.zero;

            MeshFilter[] mfArray = obj.GetComponentsInChildren<MeshFilter>();

            Vector3 tempMin = new Vector3(100000f, 100000f, 100000f);
            Vector3 tempMax = new Vector3(-100000f, -100000f, -100000f);

            // ----------------------

            foreach (var mf in mfArray)
            {

                tempMin = mf.sharedMesh.bounds.min;
                tempMax = mf.sharedMesh.bounds.max;

                tempMin.x *= mf.transform.lossyScale.x;
                tempMin.y *= mf.transform.lossyScale.y;
                tempMin.z *= mf.transform.lossyScale.z;

                tempMax.x *= mf.transform.lossyScale.x;
                tempMax.y *= mf.transform.lossyScale.y;
                tempMax.z *= mf.transform.lossyScale.z;

                min.x = Mathf.Min(min.x, tempMin.x);
                min.y = Mathf.Min(min.y, tempMin.y);
                min.z = Mathf.Min(min.z, tempMin.z);

                max.x = Mathf.Max(max.x, tempMax.x);
                max.y = Mathf.Max(max.y, tempMax.y);
                max.z = Mathf.Max(max.z, tempMax.z);

            }

            Vector3 center = (min + max) / 2f;
            Vector3 size = max - min;

            return new Bounds(center, size);

        }

    }

}
