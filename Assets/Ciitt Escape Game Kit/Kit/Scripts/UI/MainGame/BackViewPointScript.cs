using System.Collections;
using System.Collections.Generic;
using SSC;
using UnityEngine;
using UnityEngine.UI;

namespace ciitt.EscapeGameKit
{

    /// <summary>
    /// Back ViewPoint
    /// </summary>
    [RequireComponent(typeof(Button))]
    public class BackViewPointScript : MonoBehaviour
    {

        /// <summary>
        /// Reference to Button
        /// </summary>
        Button m_refButton = null;

        /// <summary>
        /// Start
        /// </summary>
        // -----------------------------------------------------------------------
        void Start()
        {

            // m_refButton
            {
                this.m_refButton = this.GetComponent<Button>();
            }

            // addMainGameSceneStateReceiver
            {
                CustomReduxManager.CustomReduxManagerInstance.addMainGameSceneStateReceiver(this.onMainGameSceneStateReceiver);
                CustomReduxManager.CustomReduxManagerInstance.addSceneChangeStateReceiver(this.onSceneChangeStateReceiver);
            }

        }

        /// <summary>
        /// MainGameSceneState receiver
        /// </summary>
        /// <param name="mgsState">MainGameSceneState</param>
        // -----------------------------------------------------------------------
        void onMainGameSceneStateReceiver(MainGameSceneState mgsState)
        {

            if (mgsState.stateEnum == MainGameSceneState.StateEnum.MainGameSceneChangeCameraView)
            {

                this.m_refButton.interactable =
                    mgsState.changeCameraViewInfo.currentTargetViewPoint &&
                    mgsState.changeCameraViewInfo.currentTargetViewPoint.viewPointParent
                    ;

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

                var mgsState = CustomReduxManager.CustomReduxManagerInstance.MainGameSceneStateWatcher.state();

                this.m_refButton.interactable =
                    mgsState.changeCameraViewInfo.currentTargetViewPoint &&
                    mgsState.changeCameraViewInfo.currentTargetViewPoint.viewPointParent
                    ;

            }

        }

    }

}
