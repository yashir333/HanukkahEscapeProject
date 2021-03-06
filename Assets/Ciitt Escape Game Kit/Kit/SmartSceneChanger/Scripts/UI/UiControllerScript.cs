using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SSC
{

    /// <summary>
    /// UI Controller
    /// </summary>
    public abstract class UiControllerScript : MonoBehaviour
    {


        /// <summary>
        /// UI identifier to belong
        /// </summary>
        [SerializeField]
        [Tooltip("UI identifier to belong")]
        protected List<string> m_uiIdentifierList = new List<string>();

        /// <summary>
        /// Delay seconds for showing
        /// </summary>
        [SerializeField]
        [Tooltip("Delay seconds for showing")]
        protected float m_delaySecondsForShowing = 0.0f;

        /// <summary>
        /// Delay seconds for hiding
        /// </summary>
        [SerializeField]
        [Tooltip("Delay seconds for hiding")]
        protected float m_delaySecondsForHiding = 0.0f;

        /// <summary>
        /// Show IEnumerator
        /// </summary>
        /// <returns>IEnumerator</returns>
        protected abstract IEnumerator show();

        /// <summary>
        /// Hide IEnumerator
        /// </summary>
        /// <returns>IEnumerator</returns>
        protected abstract IEnumerator hide();

        /// <summary>
        /// Show or hide state
        /// </summary>
        public enum ShowHideState
        {
            NowShowing,
            NowHiding,
            NowShowingTransition,
            NowHidingTransition
        }

        /// <summary>
        /// Current ShowHideState
        /// </summary>
        protected ShowHideState m_shState = ShowHideState.NowShowing;

        // ------------------------------------------------------------------------------------------

        /// <summary>
        /// Current ShowHideState getter
        /// </summary>
        public ShowHideState currentShowHideState { get { return this.m_shState; } }

        // ------------------------------------------------------------------------------------------

        /// <summary>
        /// Start
        /// </summary>
        // ------------------------------------------------------------------------------------------
        protected virtual void Start()
        {

            foreach(string identifier in this.m_uiIdentifierList)
            {
                if(!string.IsNullOrEmpty(identifier))
                {
                    UiManager.Instance.addUiControllerScript(identifier, this);
                }
            }

        }

        /// <summary>
        /// OnDestroy
        /// </summary>
        // ------------------------------------------------------------------------------------------
        protected virtual void OnDestroy()
        {

            if(UiManager.isAvailable())
            {
                foreach (string identifier in this.m_uiIdentifierList)
                {
                    if (!string.IsNullOrEmpty(identifier))
                    {
                        UiManager.Instance.removeUiControllerScript(identifier, this);
                    }
                }
            }

        }

        /// <summary>
        /// Start showing for button
        /// </summary>
        // ------------------------------------------------------------------------------------------
        public virtual void startShowingForButton()
        {
            this.startShowing(false, 0.0f, null);
        }

        /// <summary>
        /// Start showing
        /// </summary>
        /// <param name="showDoneCallback">callback when showing done</param>
        /// <returns>start or not</returns>
        // ------------------------------------------------------------------------------------------
        public virtual bool startShowing(Action showDoneCallback = null)
        {
            return this.startShowing(false, 0.0f, showDoneCallback);
        }

        /// <summary>
        /// Start showing
        /// </summary>
        /// <param name="restartShowing">show UI forcibly</param>
        /// <param name="autoHideInvoke">Invoke hide function</param>
        /// <param name="showDoneCallback">callback when showing done</param>
        /// <returns>start or not</returns>
        // ------------------------------------------------------------------------------------------
        public virtual bool startShowing(bool restartShowing, float autoHideInvoke, Action showDoneCallback)
        {

            if(!restartShowing)
            {
                if(this.m_shState == ShowHideState.NowShowing || this.m_shState == ShowHideState.NowShowingTransition)
                {

                    if(showDoneCallback != null)
                    {
                        showDoneCallback();
                    }

                    return false;

                }
            }

            // StartCoroutine
            {
                StopAllCoroutines();
                StartCoroutine(this.showBase(showDoneCallback));
            }

            if(autoHideInvoke > 0.0f)
            {
                CancelInvoke("startHidingInvoke");
                Invoke("startHidingInvoke", autoHideInvoke);
            }

            return true;

        }

        /// <summary>
        /// Start hiding for invoke
        /// </summary>
        // ------------------------------------------------------------------------------------------
        protected virtual void startHidingInvoke()
        {
            this.startHiding(null);
        }

        /// <summary>
        /// Start hiding for button
        /// </summary>
        // ------------------------------------------------------------------------------------------
        public virtual void startHidingForButton()
        {
            this.startHiding(null);
        }

        /// <summary>
        /// Start hiding
        /// </summary>
        /// <param name="hideDoneCallback">callback when hiding done</param>
        /// <returns>start or not</returns>
        // ------------------------------------------------------------------------------------------
        public virtual bool startHiding(Action hideDoneCallback = null)
        {

            if (this.m_shState == ShowHideState.NowHiding || this.m_shState == ShowHideState.NowHidingTransition)
            {

                if(hideDoneCallback != null)
                {
                    hideDoneCallback();
                }

                return false;

            }

            // StartCoroutine
            {
                StopAllCoroutines();
                StartCoroutine(this.hideBase(hideDoneCallback));
            }

            return true;

        }

        /// <summary>
        /// Show IEnumerator base
        /// </summary>
        /// <returns>IEnumerator</returns>
        // ------------------------------------------------------------------------------------------
        protected virtual IEnumerator showBase(Action showDoneCallback)
        {

            this.m_shState = ShowHideState.NowShowingTransition;

            yield return null;

            if(this.m_delaySecondsForShowing > 0.0f)
            {
                yield return new WaitForSeconds(this.m_delaySecondsForShowing);
            }

            {
                yield return this.show();
            }

            this.m_shState = ShowHideState.NowShowing;

            if (showDoneCallback != null)
            {
                showDoneCallback();
            }

        }

        /// <summary>
        /// Hide IEnumerator base
        /// </summary>
        /// <returns>IEnumerator</returns>
        // ------------------------------------------------------------------------------------------
        protected virtual IEnumerator hideBase(Action hideDoneCallback)
        {

            this.m_shState = ShowHideState.NowHidingTransition;

            yield return null;

            if (this.m_delaySecondsForHiding > 0.0f)
            {
                yield return new WaitForSeconds(this.m_delaySecondsForHiding);
            }

            {
                yield return this.hide();
            }

            this.m_shState = ShowHideState.NowHiding;

            if (hideDoneCallback != null)
            {
                hideDoneCallback();
            }

        }

    }

}
