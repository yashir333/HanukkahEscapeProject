using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ciitt.EscapeGameKit
{

    /// <summary>
    /// Evolve anim
    /// </summary>
    public class EvolveAnimatorHolder : EvolveAnimHolder
    {

        /// <summary>
        /// Open state name
        /// </summary>
        [SerializeField]
        [Tooltip("Open state name")]
        string m_evolveStateName = "Evolve";

        /// <summary>
        /// Evolve trigger name
        /// </summary>
        [SerializeField]
        [Tooltip("Evolve trigger name")]
        string m_evolveTriggerName = "Evolve Trigger";

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
        /// Current EvolveAnimState
        /// </summary>
        // ----------------------------------------------------------------------------------
        public override EvolveAnimState currentEvolveAnimState()
        {

            AnimatorStateInfo asi0 = this.m_refAnimator.GetCurrentAnimatorStateInfo(0);

            if (this.m_refAnimator.IsInTransition(0) || asi0.normalizedTime < 1.0f)
            {
                return EvolveAnimState.Transition;
            }

            else if (asi0.IsName(this.m_evolveStateName))
            {
                return EvolveAnimState.Evolved;
            }

            else
            {
                return EvolveAnimState.NotEvolved;
            }

        }

        /// <summary>
        /// Play evolve anim
        /// </summary>
        /// <param name="immediately">immediately</param>
        // ------------------------------------------------------------------------------
        public override void playEvolveAnim(bool immediately)
        {

            if (immediately)
            {
                this.m_refAnimator.Play(this.m_evolveStateName, 0, 1.0f);
            }

            else if (this.m_delay > 0.0f)
            {
                CancelInvoke("playEvolveAnimInternal");
                Invoke("playEvolveAnimInternal", this.m_delay);
            }

            else
            {
                this.playEvolveAnimInternal();
            }

        }

        /// <summary>
        /// Play evolve anim internal
        /// </summary>
        // ------------------------------------------------------------------------------
        void playEvolveAnimInternal()
        {
            this.m_refAnimator.SetTrigger(this.m_evolveTriggerName);
        }

    }

}
