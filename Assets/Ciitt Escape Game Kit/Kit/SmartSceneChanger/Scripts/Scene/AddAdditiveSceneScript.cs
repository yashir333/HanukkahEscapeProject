using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace SSC
{

    /// <summary>
    /// Add additive scene
    /// </summary>
    public class AddAdditiveSceneScript : MonoBehaviour
    {

#if UNITY_EDITOR

        /// <summary>
        /// Scene to add
        /// </summary>
        [SerializeField]
        [Tooltip("Scene to add")]
        protected UnityEngine.Object m_additiveScene;

#endif

        /// <summary>
        /// Loaded state
        /// </summary>
        protected enum LoadedState
        {
            Unloaded,
            Loaded
        }

        /// <summary>
        /// Event when add finished
        /// </summary>
        [SerializeField]
        [Tooltip("Event when add finished")]
        protected UnityEvent m_addFinishedEvent;

        /// <summary>
        /// Event when unload finished
        /// </summary>
        [SerializeField]
        [Tooltip("Event when unload finished")]
        protected UnityEvent m_unloadFinishedEvent;

        /// <summary>
        /// Additive scene name
        /// </summary>
        [HideInInspector]
        [SerializeField]
        protected string m_additiveSceneName = "";

        /// <summary>
        /// addSceneIE
        /// </summary>
        protected IEnumerator m_addAndUnloadSceneIE = null;

        /// <summary>
        /// Current LoadedState
        /// </summary>
        protected LoadedState m_currentLoadedState = LoadedState.Unloaded;

        /// <summary>
        /// Add additive scene
        /// </summary>
        // -------------------------------------------------------------------
        public void addScene()
        {

            if(this.m_addAndUnloadSceneIE == null && this.m_currentLoadedState == LoadedState.Unloaded)
            {
                StartCoroutine(this.m_addAndUnloadSceneIE = this.addSceneIE());
            }

        }

        /// <summary>
        /// Unload additive scene
        /// </summary>
        // -------------------------------------------------------------------
        public void unloadScene()
        {

            if (this.m_addAndUnloadSceneIE == null && this.m_currentLoadedState == LoadedState.Loaded)
            {
                StartCoroutine(this.m_addAndUnloadSceneIE = this.unloadSceneIE());
            }

        }

        /// <summary>
        /// Add additive scene IEnumerator
        /// </summary>
        /// <returns>IEnumerator</returns>
        // -------------------------------------------------------------------
        protected virtual IEnumerator addSceneIE()
        {

            if(string.IsNullOrEmpty(this.m_additiveSceneName))
            {
                this.m_addAndUnloadSceneIE = null;
                yield break;
            }

            // ------------------
            
            AsyncOperation ao = SceneManager.LoadSceneAsync(this.m_additiveSceneName, LoadSceneMode.Additive);

            if(ao == null)
            {
                this.m_addAndUnloadSceneIE = null;
                yield break;
            }

            // ------------------

            // wait
            {
                while (!ao.isDone)
                {
                    yield return null;
                }
            }

            // m_addFinishedEvent
            {
                this.m_addFinishedEvent.Invoke();
            }

            // finish
            {
                this.m_addAndUnloadSceneIE = null;
                this.m_currentLoadedState = LoadedState.Loaded;
            }

        }

        /// <summary>
        /// Unload additive scene IEnumerator
        /// </summary>
        /// <returns>IEnumerator</returns>
        // -------------------------------------------------------------------
        protected virtual IEnumerator unloadSceneIE()
        {

            if (string.IsNullOrEmpty(this.m_additiveSceneName))
            {
                this.m_addAndUnloadSceneIE = null;
                yield break;
            }

            // ------------------

            AsyncOperation ao = SceneManager.UnloadSceneAsync(this.m_additiveSceneName);

            if (ao == null)
            {
                this.m_addAndUnloadSceneIE = null;
                yield break;
            }

            // ------------------

            // wait
            {
                while (!ao.isDone)
                {
                    yield return null;
                }
            }

            // m_unloadFinishedEvent
            {
                this.m_unloadFinishedEvent.Invoke();
            }

            // finish
            {
                this.m_addAndUnloadSceneIE = null;
                this.m_currentLoadedState = LoadedState.Unloaded;
            }

        }

        /// <summary>
        /// OnValidate
        /// </summary>
        // -------------------------------------------------------------------
        protected virtual void OnValidate()
        {

#if UNITY_EDITOR
            this.m_additiveSceneName = (this.m_additiveScene && !string.IsNullOrEmpty(this.m_additiveScene.name)) ? this.m_additiveScene.name : "";
#endif

        }

    }

}
