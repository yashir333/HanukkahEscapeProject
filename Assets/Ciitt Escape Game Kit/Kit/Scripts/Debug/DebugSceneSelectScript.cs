using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ciitt.EscapeGameKit
{

    public class DebugSceneSelectScript : MonoBehaviour
    {

#if UNITY_EDITOR

        /// <summary>
        /// Next scene
        /// </summary>
        [SerializeField]
        [Tooltip("Next scene")]
        UnityEngine.Object m_nextScene;

#endif

        /// <summary>
        /// Reference to button text
        /// </summary>
        [SerializeField]
        [Tooltip("Reference to button text")]
        Text m_refButtonText = null;

        /// <summary>
        /// Next scene name
        /// </summary>
        [HideInInspector]
        [SerializeField]
        string m_nextSceneName = "";

        /// <summary>
        /// Start
        /// </summary>
        // -------------------------------------------------------------------------------------
        void Start()
        {
            
            if(!this.m_refButtonText)
            {
                Debug.LogWarning("m_refButtonText is null : " + this.gameObject.name);
            }

            else
            {
                this.m_refButtonText.text = this.m_nextSceneName;
            }

        }

        /// <summary>
        /// Load next scene
        /// </summary>
        // -------------------------------------------------------------------------------------
        public void loadNextScene()
        {
            CustomSceneChangeManager.Instance.loadNextScene(this.m_nextSceneName);
        }

        /// <summary>
        /// OnValidate
        /// </summary>
        // -------------------------------------------------------------------------------------
        void OnValidate()
        {

#if UNITY_EDITOR

            if (this.m_nextScene && !string.IsNullOrEmpty(this.m_nextScene.name))
            {
                this.m_nextSceneName = this.m_nextScene.name;
            }

            else
            {
                this.m_nextSceneName = "";
            }

#endif

        }

    }

}
