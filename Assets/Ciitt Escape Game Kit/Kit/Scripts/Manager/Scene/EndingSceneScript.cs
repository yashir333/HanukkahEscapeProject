using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ciitt.EscapeGameKit
{

    /// <summary>
    /// Ending scene script
    /// </summary>
    public class EndingSceneScript : MonoBehaviour
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
        /// Next scene name
        /// </summary>
        [HideInInspector]
        [SerializeField]
        string m_nextSceneName = "";

        /// <summary>
        /// Update
        /// </summary>
        // -----------------------------------------------------------------------
        void Update()
        {

            if(Time.timeSinceLevelLoad < 5.0f)
            {
                return;
            }

            // -----------

            if(Input.GetMouseButtonDown(0))
            {
                //CustomSceneChangeManager.Instance.backToTitleScene();
                CustomSceneChangeManager.Instance.loadNextScene(this.m_nextSceneName);
                this.enabled = false;
            }

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
