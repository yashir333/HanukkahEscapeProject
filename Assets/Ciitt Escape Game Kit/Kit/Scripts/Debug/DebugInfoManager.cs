using System.Collections;
using System.Collections.Generic;
using SSC;
using UnityEngine;
using UnityEngine.UI;

namespace ciitt.EscapeGameKit
{

    public class DebugInfoManager : SingletonMonoBehaviour<DebugInfoManager>
    {

        /// <summary>
        /// Available language list
        /// </summary>
        [SerializeField]
        [Tooltip("Available language list")]
        Text m_refDebugText = null;

        /// <summary>
        /// Awake
        /// </summary>
        // -------------------------------------------------------------------------------------------
        protected override void initOnAwake()
        {

#if UNITY_EDITOR

            if(!this.CompareTag("EditorOnly"))
            {
                Debug.LogWarning("(#if UNITY_EDITOR) DebugInfoManager is not : EditorOnly" + Funcs.createHierarchyPath(this.transform));
            }

#endif

        }

        /// <summary>
        /// Set text
        /// </summary>
        /// <param name="text">text</param>
        // -------------------------------------------------------------------------------------------
        public void setDebugText(string text)
        {

            if(this.m_refDebugText)
            {
                this.m_refDebugText.text = text;
            }

            //else
            //{
            //    Debug.LogWarning("(#if UNITY_EDITOR) m_refDebugText is null : " + Funcs.createHierarchyPath(this.transform));
            //}

        }

    }

}
