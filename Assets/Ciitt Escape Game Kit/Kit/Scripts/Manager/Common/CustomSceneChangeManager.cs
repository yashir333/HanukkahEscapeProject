using System.Collections;
using System.Collections.Generic;
using SSC;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ciitt.EscapeGameKit
{

    /// <summary>
    /// Custom SceneChangeManager
    /// </summary>
    public class CustomSceneChangeManager : SceneChangeManager
    {

        /// <summary>
        /// Instance
        /// </summary>
        public static CustomSceneChangeManager CustomSceneChangeManagerInstance { get { return (CustomSceneChangeManager)CustomSceneChangeManager.Instance; } }

        /// <summary>
        /// Identifier for simple now loading
        /// </summary>
        [SerializeField]
        [Tooltip("Identifier for simple now loading")]
        protected string m_simpleNowLoadingUiIdentifier = "SimpleNowLoading";

        /// <summary>
        /// Current loading target UserProgressDataSO
        /// </summary>
        protected UserProgressDataSO m_refCurrentUserProgressDataSO = null;

        /// <summary>
        /// Default now loading ui identifier
        /// </summary>
        protected string m_defaultNowLoadingUiIdentifier = "";

        // -----------------------------------------------------------------------------------------------

        /// <summary>
        /// Awake
        /// </summary>
        // -----------------------------------------------------------------------------------------------
        protected override void Awake()
        {

            base.Awake();

            // m_defaultNowLoadingUiIdentifier
            {
                this.m_defaultNowLoadingUiIdentifier = this.m_currentNowLoadingUiIdentifier;
            }

        }

        /// <summary>
        /// Start
        /// </summary>
        // -------------------------------------------------------------------------
        void Start()
        {

            // CustomReduxManager
            {
                CustomReduxManager.CustomReduxManagerInstance.addSceneChangeStateReceiver(this.onSceneChangeStateReceiver);
            }

        }

        /// <summary>
        /// SceneChangeState receiver
        /// </summary>
        /// <param name="scState">SceneChangeState</param>
        // ------------------------------------------------------------------------------------------
        void onSceneChangeStateReceiver(SceneChangeState scState)
        {

            if (scState.stateEnum == SceneChangeState.StateEnum.AllStartupsDone)
            {
                this.m_currentNowLoadingUiIdentifier = this.m_defaultNowLoadingUiIdentifier;
            }

            else if(scState.stateEnum == SceneChangeState.StateEnum.ScenePlaying)
            {
                this.m_refCurrentUserProgressDataSO = null;
            }

        }

        /// <summary>
        /// Set simple now loading
        /// </summary>
        // -------------------------------------------------------------------------------------------------------
        public void setSimpleNowLoadingFlagOnce()
        {
            this.m_currentNowLoadingUiIdentifier = this.m_simpleNowLoadingUiIdentifier;
        }

        /// <summary>
        /// Back to title
        /// </summary>
        // -------------------------------------------------------------------------------------------------------
        public override void backToTitleScene()
        {
            base.loadNextScene(this.m_titleSceneName);
        }

        /// <summary>
        /// Get json data from current user progress data
        /// </summary>
        /// <typeparam name="T">any</typeparam>
        /// <param name="trans">Transform</param>
        /// <param name="mb">MonoBehaviour</param>
        /// <returns>data</returns>
        // -------------------------------------------------------------------------------------
        public T getDataFromCurrentUserProgressData<T>(Transform trans, MonoBehaviour mb) where T : new()
        {

            string json = this.getJsonDataFromCurrentUserProgressData(trans, mb);

            return SystemManager.Instance.convertJson<T>(json);

        }

        /// <summary>
        /// Get json data from current user progress data
        /// </summary>
        /// <param name="trans">Transform</param>
        /// <param name="mb">MonoBehaviour</param>
        /// <returns>data</returns>
        // -------------------------------------------------------------------------------------
        public string getJsonDataFromCurrentUserProgressData(Transform trans, MonoBehaviour mb)
        {

            if (this.m_refCurrentUserProgressDataSO == null)
            {
                return "";
            }

            // ---------------

            string key = SystemManager.Instance.createKeyPath(trans, mb);

            if (!string.IsNullOrEmpty(key))
            {

                if (this.m_refCurrentUserProgressDataSO.dataDict.ContainsKey(key))
                {

                    var temp = this.m_refCurrentUserProgressDataSO.dataDict[key] as KeyAndData;

                    if (temp != null)
                    {
                        return temp.stringOrJson;
                    }

                }

#if UNITY_EDITOR

                else
                {
                    Debug.LogWarning("(#if UNITY_EDITOR) Not contain a key : " + key + " : Did you edit the scene or script?");
                }
#endif

            }

            return "";

        }

        /// <summary>
        /// Load a scene with user progress data
        /// </summary>
        // -------------------------------------------------------------------------------------------------------
        public virtual void loadSceneWithUserProgressData(UserProgressDataSO data)
        {

            if (!data)
            {
                return;
            }

            // --------------------

            // set
            {
                this.m_refCurrentUserProgressDataSO = data;
            }

            // loadNextScene
            {
                this.loadNextScene(this.m_refCurrentUserProgressDataSO.sceneName);
            }

        }

        /// <summary>
        /// Is loading current scene with user progress data
        /// </summary>
        /// <returns>yesno</returns>
        // -------------------------------------------------------------------------------------------------------
        public bool isLoadingCurrentSceneWithUserProgressData()
        {
            return (this.m_refCurrentUserProgressDataSO != null);
        }

    }

}
