using System.Collections;
using System.Collections.Generic;
using SSC;
using UnityEngine;

namespace ciitt.EscapeGameKit
{

    /// <summary>
    /// Main game scene script
    /// </summary>
    public class MainGameSceneScript : MonoBehaviour
    {

        /// <summary>
        /// Main game UI identifier
        /// </summary>
        [SerializeField]
        [Tooltip("Main game UI identifier")]
        string m_mainGameUi = "MainGame";

        /// <summary>
        /// Item showroom UI identifier
        /// </summary>
        [SerializeField]
        [Tooltip("Item showroom UI identifier")]
        string m_itemShowroomUi = "ItemShowroom";

        /// <summary>
        /// Field object evolution UI identifier
        /// </summary>
        [SerializeField]
        [Tooltip("Field object evolution UI identifier")]
        string m_fieldObjectEvolutionUi = "FieldObjectEvolution";

        /// <summary>
        /// Item evolution UI identifier
        /// </summary>
        [SerializeField]
        [Tooltip("Item evolution UI identifier")]
        string m_itemEvolutionUi = "ItemEvolution";

        /// <summary>
        /// Options UI identifier
        /// </summary>
        [SerializeField]
        [Tooltip("Options UI identifier")]
        string m_optionsUi = "Options";

        /// <summary>
        /// How to play UI identifier
        /// </summary>
        [SerializeField]
        [Tooltip("How to play UI identifier")]
        string m_howToPlayUi = "HowToPlay";

        /// <summary>
        /// Show how to play first
        /// </summary>
        [SerializeField]
        [Tooltip("Show how to play first")]
        bool m_showHowToPlayFirst = true;

        /// <summary>
        /// Reference to StateWatcher<MainGameSceneState>
        /// </summary>
        StateWatcher<MainGameSceneState> m_refMainGameSceneStateWatcher = null;

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


            // m_refMainGameSceneStateWatcher
            {
                this.m_refMainGameSceneStateWatcher = CustomReduxManager.CustomReduxManagerInstance.MainGameSceneStateWatcher;
            }

            // addMainGameSceneStateReceiver
            {
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

                CustomSceneChangeManager cscManager = CustomSceneChangeManager.Instance as CustomSceneChangeManager;

                if (cscManager.isLoadingCurrentSceneWithUserProgressData() || !this.m_showHowToPlayFirst)
                {
                    mgsState.setState(MainGameSceneState.StateEnum.MainGameSceneMain);
                }

                else
                {
                    mgsState.setState(MainGameSceneState.StateEnum.MainGameSceneHowToPlay);
                }

            }

        }

        /// <summary>
        /// MainGameSceneState receiver
        /// </summary>
        /// <param name="mgsState">MainGameSceneState</param>
        // -----------------------------------------------------------------------
        void onMainGameSceneStateReceiver(MainGameSceneState mgsState)
        {

            if (mgsState.stateEnum == MainGameSceneState.StateEnum.MainGameSceneMain)
            {

                UiManager.Instance.showUi(this.m_mainGameUi, true, false);

                if (
                    mgsState.previousStateEnum == MainGameSceneState.StateEnum.MainGameSceneHowToPlay ||
                    mgsState.previousStateEnum == MainGameSceneState.StateEnum.MainGameSceneOptions ||
                    mgsState.previousStateEnum == MainGameSceneState.StateEnum.MainGameSceneItemShowroom
                    )
                {
                    SoundManager.Instance.playSe(SoundManager.SeType.CloseItem);
                }

            }

            else if (mgsState.stateEnum == MainGameSceneState.StateEnum.MainGameSceneItemShowroom)
            {
                UiManager.Instance.showUi(this.m_itemShowroomUi, true, false);
                SoundManager.Instance.playSe(SoundManager.SeType.ShowItem);
            }

            else if (mgsState.stateEnum == MainGameSceneState.StateEnum.MainGameSceneFieldObjectEvolution)
            {
                UiManager.Instance.showUi(this.m_fieldObjectEvolutionUi, true, false);
            }

            else if (mgsState.stateEnum == MainGameSceneState.StateEnum.MainGameSceneItemEvolution)
            {
                UiManager.Instance.showUi(this.m_itemEvolutionUi, true, false);
            }

            else if (mgsState.stateEnum == MainGameSceneState.StateEnum.MainGameSceneOptions)
            {
                UiManager.Instance.showUi(this.m_optionsUi, true, false);
                SoundManager.Instance.playSe(SoundManager.SeType.ShowItem);
            }

            else if (mgsState.stateEnum == MainGameSceneState.StateEnum.MainGameSceneHowToPlay)
            {
                UiManager.Instance.showUi(this.m_howToPlayUi, true, false);
                SoundManager.Instance.playSe(SoundManager.SeType.ShowItem);
            }

            else
            {
                UiManager.Instance.showUi("", true, false);
            }

        }

        /// <summary>
        /// Send MainGameSceneState signal
        /// </summary>
        /// <param name="stateEnum">MainGameSceneState.StateEnum</param>
        // ------------------------------------------------------------------------------------------
        void sendMainGameSceneStateSignal(MainGameSceneState.StateEnum stateEnum)
        {

            MainGameSceneState mgsState = this.m_refMainGameSceneStateWatcher.state();

            mgsState.setState(stateEnum);

        }

        /// <summary>
        /// Show how to play UI
        /// </summary>
        // ------------------------------------------------------------------------------------------
        public void showHowToPlayUi()
        {
            this.sendMainGameSceneStateSignal(MainGameSceneState.StateEnum.MainGameSceneHowToPlay);
        }

        /// <summary>
        /// Close how to play UI
        /// </summary>
        // ------------------------------------------------------------------------------------------
        public void closeHowToPlayUi()
        {
            this.sendMainGameSceneStateSignal(MainGameSceneState.StateEnum.MainGameSceneMain);
        }

        /// <summary>
        /// Show options UI
        /// </summary>
        // -------------------------------------------------------------------------------------------
        public void showOptionsUi()
        {
            this.sendMainGameSceneStateSignal(MainGameSceneState.StateEnum.MainGameSceneOptions);
        }

        /// <summary>
        /// Close options UI
        /// </summary>
        // ------------------------------------------------------------------------------------------
        public void closeOptionsUi()
        {
            this.sendMainGameSceneStateSignal(MainGameSceneState.StateEnum.MainGameSceneMain);
        }

        /// <summary>
        /// Save user progress
        /// </summary>
        // ------------------------------------------------------------------------------------------
        public void saveUserProgress()
        {
            SystemManager.Instance.saveCurrentUserProgressData();
            SoundManager.Instance.playSe(SoundManager.SeType.Save);
        }

    }

}
