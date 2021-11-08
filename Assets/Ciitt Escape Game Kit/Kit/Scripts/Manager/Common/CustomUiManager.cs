using System.Collections;
using System.Collections.Generic;
using SSC;
using UnityEngine;
using UnityEngine.UI;

namespace ciitt.EscapeGameKit
{

    /// <summary>
    /// Custom UI manager
    /// </summary>
    public class CustomUiManager : UiManager
    {

        /// <summary>
        /// Instance
        /// </summary>
        public static CustomUiManager CustomUiManagerInstance { get { return (CustomUiManager)CustomUiManager.Instance; } }

        [Space(10.0f)]

        /// <summary>
        /// Reference to NotificationUiControllerScript
        /// </summary>
        [SerializeField]
        [Tooltip("Reference to NotificationUiControllerScript")]
        NotificationUiControllerScript m_refTempMessageUi = null;

        /// <summary>
        /// Reference to StateWatcher<MainGameSceneState>
        /// </summary>
        StateWatcher<MainGameSceneState> m_refMainGameSceneStateWatcher = null;

        /// <summary>
        /// Start
        /// </summary>
        // -------------------------------------------------------------------------------------------
        void Start()
        {

#if UNITY_EDITOR

            if (!this.m_refTempMessageUi)
            {
                Debug.LogError("m_refTempMessageUi is null : " + Funcs.createHierarchyPath(this.transform));
            }

#endif

            // m_refMainGameSceneStateWatcher
            {
                this.m_refMainGameSceneStateWatcher = CustomReduxManager.CustomReduxManagerInstance.MainGameSceneStateWatcher;
            }

        }

        /// <summary>
        /// Show message
        /// </summary>
        /// <param name="text">text</param>
        /// <param name="autoHideTime">auto hide invoke</param>
        // -------------------------------------------------------------------------------------------
        public void showTempMessageUi(string text, float autoHideTime = 1.0f)
        {

            if (this.m_refTempMessageUi)
            {
                this.m_refTempMessageUi.setText(text);
                this.m_refTempMessageUi.startShowing(true, autoHideTime, null);
            }

        }

        /// <summary>
        /// Send signal for options
        /// </summary>
        // -------------------------------------------------------------------------------------------
        public void sendSignalForShowingOptions()
        {

            MainGameSceneState mgsState = this.m_refMainGameSceneStateWatcher.state();

            if (mgsState.stateEnum == MainGameSceneState.StateEnum.TitleSceneMain)
            {

                mgsState.setState(MainGameSceneState.StateEnum.TitleSceneOptions);

                SoundManager.Instance.playSe(SoundManager.SeType.ShowItem);

            }

            else if (mgsState.stateEnum == MainGameSceneState.StateEnum.MainGameSceneMain)
            {

                mgsState.setState(MainGameSceneState.StateEnum.MainGameSceneOptions);

                SoundManager.Instance.playSe(SoundManager.SeType.ShowItem);

            }

        }

    }

}
