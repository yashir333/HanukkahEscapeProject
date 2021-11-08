using System.Collections;
using System.Collections.Generic;
using SSC;
using UnityEngine;

namespace ciitt.EscapeGameKit
{

    /// <summary>
    /// Title scene script
    /// </summary>
    public class TitleSceneScript : MonoBehaviour
    {

#if UNITY_EDITOR

        /// <summary>
        /// Scene for start
        /// </summary>
        [SerializeField]
        [Tooltip("Scene for start")]
        UnityEngine.Object m_sceneForStart;

#endif

        /// <summary>
        /// Scene for start name
        /// </summary>
        [HideInInspector]
        [SerializeField]
        string m_sceneForStartName = "";

        /// <summary>
        /// Options UI identifier
        /// </summary>
        [SerializeField]
        [Tooltip("Options UI identifier")]
        string m_optionsUi = "Options";

        /// <summary>
        /// Start
        /// </summary>
        // -------------------------------------------------------------------------
        void Start()
        {

            // CustomReduxManager
            {
                CustomReduxManager.CustomReduxManagerInstance.addSceneChangeStateReceiver(this.onSceneChangeStateReceiver);
                CustomReduxManager.CustomReduxManagerInstance.addMainGameSceneStateReceiver(this.onMainGameSceneStateReceiver);
            }

        }

        /// <summary>
        /// SceneChangeState receiver
        /// </summary>
        /// <param name="scState">SceneChangeState</param>
        // ------------------------------------------------------------------------------------------
        void onSceneChangeStateReceiver(SceneChangeState scState)
        {

            if (scState.stateEnum == SceneChangeState.StateEnum.ScenePlaying)
            {

                MainGameSceneState mgsState = CustomReduxManager.CustomReduxManagerInstance.MainGameSceneStateWatcher.state();

                mgsState.setState(MainGameSceneState.StateEnum.TitleSceneMain);

            }

        }

        /// <summary>
        /// MainGameSceneState receiver
        /// </summary>
        /// <param name="mgsState">MainGameSceneState</param>
        // -----------------------------------------------------------------------
        void onMainGameSceneStateReceiver(MainGameSceneState mgsState)
        {

            if (mgsState.stateEnum == MainGameSceneState.StateEnum.TitleSceneOptions)
            {
                UiManager.Instance.showUi(this.m_optionsUi, true, false);
            }

            else
            {
                CustomUiManager.Instance.showUi("", true, false);
            }

        }

        /// <summary>
        /// Continue
        /// </summary>
        // -------------------------------------------------------------------------
        public void startGame()
        {

            CustomSceneChangeManager.Instance.loadNextScene(this.m_sceneForStartName);

            SoundManager.Instance.playSe(SoundManager.SeType.StartAndContinueInTitle);

        }

        /// <summary>
        /// Continue
        /// </summary>
        // -------------------------------------------------------------------------
        public void continueGame()
        {

            if(SystemManager.Instance.isContinueDataAvailable())
            {
                CustomSceneChangeManager.CustomSceneChangeManagerInstance.loadSceneWithUserProgressData(
                    SystemManager.Instance.userProgressData
                    );
            }

            SoundManager.Instance.playSe(SoundManager.SeType.StartAndContinueInTitle);

        }

        /// <summary>
        /// Show options UI
        /// </summary>
        // -------------------------------------------------------------------------------------------
        public void showOptionsUi()
        {

            MainGameSceneState mgsState = CustomReduxManager.CustomReduxManagerInstance.MainGameSceneStateWatcher.state();

            mgsState.setState(MainGameSceneState.StateEnum.TitleSceneOptions);

            SoundManager.Instance.playSe(SoundManager.SeType.ShowItem);

        }

        /// <summary>
        /// Close options UI
        /// </summary>
        // ------------------------------------------------------------------------------------------
        public void closeOptionsUi()
        {

            MainGameSceneState mgsState = CustomReduxManager.CustomReduxManagerInstance.MainGameSceneStateWatcher.state();

            mgsState.setState(MainGameSceneState.StateEnum.TitleSceneMain);

            SoundManager.Instance.playSe(SoundManager.SeType.CloseItem);

        }

        /// <summary>
        /// OnValidate
        /// </summary>
        // -------------------------------------------------------------------------------------
        void OnValidate()
        {

#if UNITY_EDITOR

            if (this.m_sceneForStart && !string.IsNullOrEmpty(this.m_sceneForStart.name))
            {
                this.m_sceneForStartName = this.m_sceneForStart.name;
            }

            else
            {
                this.m_sceneForStartName = "";
            }

#endif

        }

    }

}
