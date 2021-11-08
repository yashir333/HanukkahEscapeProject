using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SSC;
using UnityEngine.UI;

namespace ciitt.EscapeGameKit
{

    /// <summary>
    /// SimpleUiControllerScript for notification
    /// </summary>
    public class NotificationUiControllerScript : SimpleUiControllerScript
    {

        /// <summary>
        /// Reference to Text
        /// </summary>
        [SerializeField]
        [Tooltip("Reference to Text")]
        Text m_refText = null;

        /// <summary>
        /// Start
        /// </summary>
        // ----------------------------------------------------------------------------------
        protected override void Start()
        {

#if UNITY_EDITOR

            if (!this.m_refText)
            {
                Debug.LogError("m_refText is null : " + Funcs.createHierarchyPath(this.transform));
            }

#endif

        }

        /// <summary>
        /// Set text
        /// </summary>
        /// <param name="text"></param>
        // ----------------------------------------------------------------------------------
        public void setText(string text)
        {

            if(this.m_refText)
            {
                this.m_refText.text = text;
            }

        }

    }

}
