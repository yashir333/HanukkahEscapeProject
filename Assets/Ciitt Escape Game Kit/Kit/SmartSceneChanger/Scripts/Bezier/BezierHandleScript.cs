using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SSC
{

    /// <summary>
    /// Bezier handle
    /// </summary>
    [ExecuteInEditMode]
    public class BezierHandleScript : MonoBehaviour
    {

        /// <summary>
        /// Handle type
        /// </summary>
        public enum HandleType
        {

            /// <summary>
            /// Absolute (from parent)
            /// </summary>
            SemiAbsolute,

            /// <summary>
            /// Relative
            /// </summary>
            Relative
        }

        /// <summary>
        /// HandleType
        /// </summary>
        public HandleType handleType = HandleType.SemiAbsolute;

        /// <summary>
        /// Start tangent
        /// </summary>
        public Vector3 startTangent = Vector3.forward;

        /// <summary>
        /// End tangent
        /// </summary>
        public Vector3 endTangent = -Vector3.forward;

        /// <summary>
        /// Get point position (this.transform.position)
        /// </summary>
        /// <returns>point position</returns>
        // ---------------------------------------------------------------------------------------------
        public Vector3 point()
        {
            return this.transform.position;
        }

        /// <summary>
        /// Start tangent handle point
        /// </summary>
        /// <returns>start tangent handle point</returns>
        // ---------------------------------------------------------------------------------------------
        public Vector3 startTangentHandlePoint()
        {

            if (this.handleType == HandleType.SemiAbsolute)
            {
                return this.transform.position + (this.transform.parent.rotation * this.startTangent);
            }

            return this.transform.TransformPoint(this.startTangent);

        }

        /// <summary>
        /// End tangent handle point
        /// </summary>
        /// <returns>end tangent handle point</returns>
        // ---------------------------------------------------------------------------------------------
        public Vector3 endTangentHandlePoint()
        {

            if (this.handleType == HandleType.SemiAbsolute)
            {
                return this.transform.position + (this.transform.parent.rotation * this.endTangent);
            }

            return this.transform.TransformPoint(this.endTangent);

        }

#if UNITY_EDITOR

        /// <summary>
        /// Awake
        /// </summary>
        // ---------------------------------------------------------------------------------------------
        void Awake()
        {

            // InitParentBezierCurveIfNotPlayingEditorOnly
            {
                this.InitParentBezierCurveIfNotPlayingEditorOnly();
            }

        }

        /// <summary>
        /// Update
        /// </summary>
        // -------------------------------------------------------------------------------------------
        void Update()
        {

            if (UnityEditor.Selection.activeTransform == this.transform && !Application.isPlaying && this.transform.hasChanged)
            {
                this.InitParentBezierCurveIfNotPlayingEditorOnly();
            }

        }

#endif

        /// <summary>
        /// OnValidate
        /// </summary>
        // ---------------------------------------------------------------------------------------------
        protected virtual void OnValidate()
        {

#if UNITY_EDITOR

            // InitParentBezierCurveIfNotPlayingEditorOnly
            {
                this.InitParentBezierCurveIfNotPlayingEditorOnly();
            }

#endif

        }

#if UNITY_EDITOR

        /// <summary>
        /// Initialize parent bezier curve if not playing (EditorOnly)
        /// </summary>
        // ---------------------------------------------------------------------------------------------
        protected void InitParentBezierCurveIfNotPlayingEditorOnly()
        {

            if(Application.isPlaying)
            {
                return;
            }

            // ------------------------

            BezierCurveScript bcs = this.transform.parent.GetComponent<BezierCurveScript>();

            if (bcs)
            {
                bcs.initEditorOnly();
            }

        }

        /// <summary>
        /// Import (EditorOnly)
        /// </summary>
        /// <param name="bif">BezierImportFormat</param>
        /// <param name="info">BezierImportInfo</param>
        // ---------------------------------------------------------------------------------------------
        public void importEditorOnly(BezierImportFormat bif, BezierImportFormat.BezierImportInfo info)
        {

            this.transform.position = info.point;

            if(bif.relativeTangentPos)
            {
                this.startTangent = info.startTangent;
                this.endTangent = info.endTangent;
            }

            else
            {
                this.startTangent = info.startTangent - info.point;
                this.endTangent = info.endTangent - info.point;
            }

        }

#endif

    }

}
