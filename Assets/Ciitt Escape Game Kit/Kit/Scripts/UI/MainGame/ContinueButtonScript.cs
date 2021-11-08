using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ciitt.EscapeGameKit
{

    /// <summary>
    /// Continue button
    /// </summary>
    public class ContinueButtonScript : TitleSelectableScript
    {

        /// <summary>
        /// Start
        /// </summary>
        /// <returns>IEnumerator</returns>
        // ----------------------------------------------------------------------
        protected override void Start()
        {

            base.Start();

            StartCoroutine(this.checkFirst());

        }

        /// <summary>
        /// Check at first
        /// </summary>
        /// <returns>IEnumerator</returns>
        // ----------------------------------------------------------------------
        IEnumerator checkFirst()
        {

            while (!SystemManager.Instance.isStartupDone)
            {
                yield return null;
            }

            this.m_refSelectable.interactable = SystemManager.Instance.isContinueDataAvailable();

        }

        /// <summary>
        /// MainGameSceneState receiver
        /// </summary>
        /// <param name="mgsState">MainGameSceneState</param>
        // -----------------------------------------------------------------------
        protected override void onMainGameSceneStateReceiver(MainGameSceneState mgsState)
        {

            this.m_refSelectable.interactable =
                (mgsState.stateEnum == MainGameSceneState.StateEnum.TitleSceneMain) &&
                SystemManager.Instance.isContinueDataAvailable()
                ;

        }

    }

}
