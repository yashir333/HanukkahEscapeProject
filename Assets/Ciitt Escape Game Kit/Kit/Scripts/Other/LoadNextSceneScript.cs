using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ciitt.EscapeGameKit
{

    /// <summary>
    /// Load next scene
    /// </summary>
    public class LoadNextSceneScript : MonoBehaviour
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
        /// Auto load after Start
        /// </summary>
        [SerializeField]
        [Tooltip("Auto load after Start")]
        bool m_autoLoadAfterStart = false;

        /// <summary>
        /// Use simple LoadScene
        /// </summary>
        [SerializeField]
        [Tooltip("Use simple LoadScene")]
        bool m_useSimpleLoadScene = false;

        /// <summary>
        /// Start
        /// </summary>
        // -------------------------------------------------------------------------------------
        IEnumerator Start()
        {

            while (!SystemManager.Instance.isStartupDone)
            {
                yield return null;
            }

            yield return null;

            if(this.m_autoLoadAfterStart)
            {
                this.loadNextScene();
            }

        }

        /// <summary>
        /// Load next scene
        /// </summary>
        // -------------------------------------------------------------------------------------
        public void loadNextScene()
        {

            if (this.m_useSimpleLoadScene)
            {
                CustomSceneChangeManager.CustomSceneChangeManagerInstance.setSimpleNowLoadingFlagOnce();
            }

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
