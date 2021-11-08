using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ciitt.EscapeGameKit
{

    /// <summary>
    /// Open and close Animator
    /// </summary>
    [RequireComponent(typeof(Animator))]
    public class OpenCloseAnimatorHolder : OpenCloseAnimHolder
    {

        /// <summary>
        /// Open state name
        /// </summary>
        [SerializeField]
        [Tooltip("Open state name")]
        string m_openStateName = "Open";

        /// <summary>
        /// Close state name
        /// </summary>
        [SerializeField]
        [Tooltip("Close state name")]
        string m_closeStateName = "Close";

        /// <summary>
        /// Open trigger name
        /// </summary>
        [SerializeField]
        [Tooltip("Open trigger name")]
        string m_openTriggerName = "Open Trigger";

        /// <summary>
        /// Close trigger name
        /// </summary>
        [SerializeField]
        [Tooltip("Close trigger name")]
        string m_closeTriggerName = "Close Trigger";

        /// <summary>
        /// Reference to Animator
        /// </summary>
        Animator m_refAnimator = null;

        /// <summary>
        /// Awake
        /// </summary>
        // ------------------------------------------------------------------------------
        void Awake()
        {

            this.m_refAnimator = this.GetComponent<Animator>();

        }

        /// <summary>
        /// OpenCloseState
        /// </summary>
        // ------------------------------------------------------------------------------
        public override OpenCloseState currentOpenCloseState()
        {

            AnimatorStateInfo asi0 = this.m_refAnimator.GetCurrentAnimatorStateInfo(0);

            if (this.m_refAnimator.IsInTransition(0) || asi0.normalizedTime < 1.0f)
            {
                return OpenCloseState.Transition;
            }

            else if(asi0.IsName(this.m_openStateName))
            {
                return OpenCloseState.Open;
            }

            else
            {
                return OpenCloseState.Close;
            }

        }

        /// <summary>
        /// Play open anim
        /// </summary>
        /// <param name="immediately">immediately</param>
        // ------------------------------------------------------------------------------
        public override void playOpenAnim(bool immediately)
        {

            if (immediately)
            {
                this.m_refAnimator.Play(this.m_openStateName, 0, 1.0f);
            }

            else if(this.m_delay > 0.0f)
            {
                CancelInvoke("playOpenAnimInternal");
                Invoke("playOpenAnimInternal", this.m_delay);
            }

            else
            {
                this.playOpenAnimInternal();
            }

        }

        /// <summary>
        /// Play close anim
        /// </summary>
        /// <param name="immediately">immediately</param>
        // ------------------------------------------------------------------------------
        public override void playCloseAnim(bool immediately)
        {
            
            if (immediately)
            {
                this.m_refAnimator.Play(this.m_closeStateName, 0, 1.0f);
            }

            else if (this.m_delay > 0.0f)
            {
                CancelInvoke("playCloseAnimInternal");
                Invoke("playCloseAnimInternal", this.m_delay);
            }

            else
            {
                this.playCloseAnimInternal();
            }

        }

        /// <summary>
        /// Play open anim internal
        /// </summary>
        // ------------------------------------------------------------------------------
        void playOpenAnimInternal()
        {
            this.m_refAnimator.SetTrigger(this.m_openTriggerName);
        }

        /// <summary>
        /// Play close anim internal
        /// </summary>
        // ------------------------------------------------------------------------------
        void playCloseAnimInternal()
        {
            this.m_refAnimator.SetTrigger(this.m_closeTriggerName);
        }

    }

}
