using System.Collections;
using System.Collections.Generic;
using SSC;
using UnityEngine;
using UnityEngine.UI;

namespace ciitt.EscapeGameKit
{

    /// <summary>
    /// Item showroom ui
    /// </summary>
    public class ItemShowroomUiScript : MonoBehaviour
    {

        /// <summary>
        /// Reference to item count text
        /// </summary>
        [SerializeField]
        [Tooltip("Reference to item count text")]
        Text m_refItemCountText = null;

        /// <summary>
        /// Reference to item description text
        /// </summary>
        [SerializeField]
        [Tooltip("Reference to item description text")]
        Text m_refItemDescriptionText = null;

        /// <summary>
        /// Start
        /// </summary>
        // ---------------------------------------------------------------------
        void Start()
        {

            // addMainGameSceneStateReceiver
            {

                StateWatcher<MainGameSceneState> watcher = CustomReduxManager.CustomReduxManagerInstance.MainGameSceneStateWatcher;

                CustomReduxManager.CustomReduxManagerInstance.addMainGameSceneStateReceiver(this.onMainGameSceneStateReceiver);

            }

#if UNITY_EDITOR

            // LogError
            {

                if (!this.m_refItemDescriptionText)
                {
                    Debug.LogError("m_refItemDescriptionText is null : " + Funcs.createHierarchyPath(this.transform));
                }

                if (!this.m_refItemCountText)
                {
                    Debug.LogError("m_refItemCountText is null : " + Funcs.createHierarchyPath(this.transform));
                }

            }

#endif

        }

        /// <summary>
        /// MainGameSceneState receiver
        /// </summary>
        /// <param name="mgsState">MainGameSceneState</param>
        // -----------------------------------------------------------------------
        void onMainGameSceneStateReceiver(MainGameSceneState mgsState)
        {

            if (mgsState.stateEnum == MainGameSceneState.StateEnum.MainGameSceneItemShowroom)
            {

                if(this.m_refItemCountText && mgsState.currentSelectedItemInfo.currentShowroomItem)
                {
                    this.m_refItemCountText.text =
                        string.Format(
                            "x {0}",
                            mgsState.currentSelectedItemInfo.currentShowroomItem.currentItemCount()
                            );
                }

                if (this.m_refItemDescriptionText && mgsState.currentSelectedItemInfo.currentShowroomItem)
                {
                    this.m_refItemDescriptionText.text =
                        mgsState.currentSelectedItemInfo.currentShowroomItem.descriptionText()
                        ;
                }

            }

        }

    }

}
