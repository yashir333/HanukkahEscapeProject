using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ciitt.EscapeGameKit
{

    /// <summary>
    /// Exit
    /// </summary>
    public class ExitScript : ClickableColliderScript
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
        /// Function when unlocked
        /// </summary>
        // ----------------------------------------------------------------------------------
        protected override void actionWhenUnlocked()
        {

            CustomSceneChangeManager.Instance.loadNextScene(this.m_nextSceneName);

        }

#if UNITY_EDITOR

        void Update()
        {
            
            if(Input.GetMouseButtonDown(2))
            {
                print("(#if UNITY_EDITOR) Debug loadNextScene");
                CustomSceneChangeManager.Instance.loadNextScene(this.m_nextSceneName);
            }

        }

#endif

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
