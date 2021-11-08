using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ciitt.EscapeGameKit
{

    /// <summary>
    /// Button input common
    /// </summary>
    [RequireComponent(typeof(MeshFilter))]
    public class ButtonInputScript : ClickableColliderScript
    {

        /// <summary>
        /// Reference to MeshFilter
        /// </summary>
        protected MeshFilter m_refMeshFilter = null;

        /// <summary>
        /// Original colors
        /// </summary>
        protected Color32[] m_oriColors = null;

        /// <summary>
        /// Grey colors
        /// </summary>
        protected Color32[] m_greyColors = null;

        /// <summary>
        /// Start
        /// </summary>
        // --------------------------------------------------------------------------------------------
        protected override void Start()
        {

            base.Start();

            // m_refMeshFilter
            {
                this.m_refMeshFilter = this.GetComponent<MeshFilter>();
            }

            // colors
            {

                Color32[] colors = this.m_refMeshFilter.mesh.colors32;

                this.m_oriColors = new Color32[colors.Length];
                this.m_greyColors = new Color32[colors.Length];

                for (int i = colors.Length - 1; i >= 0; i--)
                {
                    this.m_oriColors[i] = colors[i];
                    this.m_greyColors[i] = Color.grey;
                }

            }

        }

        /// <summary>
        /// OnDestroy
        /// </summary>
        // --------------------------------------------------------------------------------------------
        protected virtual void OnDestroy()
        {

            if (this.m_refMeshFilter)
            {
                Destroy(this.m_refMeshFilter.mesh);
            }

        }

        /// <summary>
        /// Change and resume color
        /// </summary>
        /// <param name="resumeInvokeTime">time to resume</param>
        // ------------------------------------------------------------------------------------------
        protected virtual void changeAndResumeColor(float resumeInvokeTime = 0.1f)
        {

            // changeColor
            {
                this.changeColor();
            }

            // resumeColor
            {
                CancelInvoke("resumeColor");
                Invoke("resumeColor", resumeInvokeTime);
            }

        }

        /// <summary>
        /// Change color
        /// </summary>
        // ------------------------------------------------------------------------------------------
        protected virtual void changeColor()
        {

            if (this.m_refMeshFilter)
            {
                this.m_refMeshFilter.mesh.colors32 = this.m_greyColors;
            }

        }

        /// <summary>
        /// Resume color
        /// </summary>
        // ------------------------------------------------------------------------------------------
        protected virtual void resumeColor()
        {

            if (this.m_refMeshFilter)
            {
                this.m_refMeshFilter.mesh.colors32 = this.m_oriColors;
            }

        }

    }

}
