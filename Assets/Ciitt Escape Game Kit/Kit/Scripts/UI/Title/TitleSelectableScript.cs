using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ciitt.EscapeGameKit
{

    [RequireComponent(typeof(Selectable))]
    public class TitleSelectableScript : MonoBehaviour
    {

        /// <summary>
        /// Reference to Selectable
        /// </summary>
        protected Selectable m_refSelectable = null;

        /// <summary>
        /// Start
        /// </summary>
        // -------------------------------------------------------------------------
        protected virtual void Start()
        {

            // m_refSelectable
            {
                this.m_refSelectable = this.GetComponent<Selectable>();
            }

            // addMainGameSceneStateReceiver
            {
                CustomReduxManager.CustomReduxManagerInstance.addMainGameSceneStateReceiver(this.onMainGameSceneStateReceiver);
            }

        }

        /// <summary>
        /// MainGameSceneState receiver
        /// </summary>
        /// <param name="mgsState">MainGameSceneState</param>
        // -----------------------------------------------------------------------
        protected virtual void onMainGameSceneStateReceiver(MainGameSceneState mgsState)
        {

            this.m_refSelectable.interactable = (mgsState.stateEnum == MainGameSceneState.StateEnum.TitleSceneMain);

        }

    }

}
