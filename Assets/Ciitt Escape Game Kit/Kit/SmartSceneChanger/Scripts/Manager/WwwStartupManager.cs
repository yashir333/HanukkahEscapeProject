#pragma warning disable 0618

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;

namespace SSC
{

    /// <summary>
    /// Class for WWW startup
    /// </summary>
    public partial class WwwStartupManager : SingletonMonoBehaviour<WwwStartupManager>
    {

        /// <summary>
        /// Class for loading WWW startup base
        /// </summary>
        protected class WwwStartupContentsBase : StartupContents
        {

            /// <summary>
            /// Progress value function
            /// </summary>
            public Func<float> progressValueFunc = null;

            /// <summary>
            /// Constructor
            /// </summary>
            public WwwStartupContentsBase()
            {
                
            }

        }

        /// <summary>
        /// WwwStartupContents list
        /// </summary>
        protected List<WwwStartupContentsBase> m_wwwsList = new List<WwwStartupContentsBase>();

        /// <summary>
        /// The number of parallel loading coroutines
        /// </summary>
        [SerializeField]
        [Tooltip("The number of parallel loading coroutines")]
        protected int m_numberOfCo = 4;

        /// <summary>
        /// Ignore error
        /// </summary>
        [SerializeField]
        [Tooltip("Ignore error")]
        protected bool m_ignoreError = false;

        /// <summary>
        /// Error seconds for timeout
        /// </summary>
        [SerializeField]
        [Tooltip("Error seconds for timeout")]
        protected float m_noProgressTimeOutSeconds = 0.0f;

        /// <summary>
        /// DialogMessages
        /// </summary>
        protected DialogMessages m_messages = new DialogMessages();

        /// <summary>
        /// Current error
        /// </summary>
        protected StartupContents m_currentError = null;

        /// <summary>
        /// WaitForSeconds
        /// </summary>
        protected WaitForSeconds m_waitForSeconds = new WaitForSeconds(0.5f);

        /// <summary>
        /// override
        /// </summary>
        // -------------------------------------------------------------------------------------------------------
        protected override void initOnAwake()
        {
            this.m_numberOfCo = Math.Max(1, this.m_numberOfCo);
        }

        /// <summary>
        /// Message for timeout
        /// </summary>
        /// <returns>message</returns>
        // -------------------------------------------------------------------------------------------------------
        protected virtual string messageTimeout()
        {
            return "Connection Timeout";
        }

        /// <summary>
        /// Denominator of progress
        /// </summary>
        /// <returns>value</returns>
        // -------------------------------------------------------------------------------------------------------
        public int progressDenominator()
        {
            return this.m_wwwsList.Count;
        }

        /// <summary>
        /// Numerator of progress
        /// </summary>
        /// <returns>value</returns>
        // -------------------------------------------------------------------------------------------------------
        public float progressNumerator()
        {

            float ret = 0.0f;

            foreach (var wwws in this.m_wwwsList)
            {

                if (wwws.currentWorkingState == StartupContents.WorkingState.DoneSuccessOrError)
                {
                    ret += 1.0f;
                }

                else if (wwws.progressValueFunc != null)
                {
                    ret += wwws.progressValueFunc();
                }

            }

            return ret;

        }

        /// <summary>
        /// Has NotYet content
        /// </summary>
        /// <returns>detected</returns>
        // -------------------------------------------------------------------------------------------------------
        public bool hasNotYetContent()
        {
            return this.m_wwwsList.Find(x => x.currentWorkingState == StartupContents.WorkingState.NotYet) != null;
        }

        /// <summary>
        /// Create error message for dialog
        /// </summary>
        /// <returns>error message object</returns>
        // -------------------------------------------------------------------------------------------------------
        public virtual System.Object createErrorMessage()
        {

            this.m_messages.clear();

            this.m_messages.category = DialogMessages.MessageCategory.Error;

            string errorMessage = (this.m_currentError != null) ? this.m_currentError.errorMessage : "Unknown Error";
            string errorUrl = (this.m_currentError != null) ? this.m_currentError.urlIfNeeded : "Unknown Error";

            if (LanguageManager.isAvailable())
            {

                LanguageManager lm = LanguageManager.Instance;

                this.m_messages.title = lm.getFormattedString(LanguageManager.SSCLanguageKeys.Dialog_Title_Error);
                this.m_messages.mainMessage = lm.getFormattedString(LanguageManager.SSCLanguageKeys.Error_Www_Startup, errorMessage);
                this.m_messages.subMessage = lm.getFormattedString(LanguageManager.SSCLanguageKeys.Dialog_Sub_Retry);
                this.m_messages.urlIfNeeded = errorUrl;

            }

            else
            {
                this.m_messages.title = "WWW Error";
                this.m_messages.mainMessage = errorMessage;
                this.m_messages.subMessage = "Retry ?";
                this.m_messages.urlIfNeeded = errorUrl;
            }

            return this.m_messages;

        }

        /// <summary>
        /// Clear contents
        /// </summary>
        // -------------------------------------------------------------------------------------------------------
        public void clearContents()
        {

            this.m_wwwsList.Clear();

            this.clearErrorForRestart();

        }

        /// <summary>
        /// Clear error for restart
        /// </summary>
        // -------------------------------------------------------------------------------------------------------
        protected void clearErrorForRestart()
        {

            // m_currentError
            {
                this.m_currentError = null;
            }

            // m_wwwsList
            {

                foreach (var val in this.m_wwwsList)
                {

                    if (!string.IsNullOrEmpty(val.errorMessage))
                    {
                        val.currentWorkingState = StartupContents.WorkingState.NotYet;
                    }

                    val.errorMessage = "";

                }

            }

        }

        /// <summary>
        /// Update error
        /// </summary>
        /// <param name="startupContents">error StartupContents</param>
        // -------------------------------------------------------------------------------------------------------
        protected void updateError(StartupContents startupContents)
        {

            if (this.m_ignoreError)
            {
                return;
            }

            // ----------

            if (startupContents != null)
            {
                this.m_currentError = startupContents;
            }

        }

        /// <summary>
        /// Has error
        /// </summary>
        /// <returns>has</returns>
        // -------------------------------------------------------------------------------------------------------
        public bool hasError()
        {
            return this.m_currentError != null;
        }

        /// <summary>
        /// Start www loadings
        /// </summary>
        /// <returns>IEnumerator</returns>
        // -------------------------------------------------------------------------------------------------------
        public IEnumerator startWwwStartup()
        {

            if (this.m_wwwsList.Count <= 0 || !this.hasNotYetContent())
            {
                yield break;
            }

            // ---------------

            // clearErrorForRestart
            {
                this.clearErrorForRestart();
            }

            // StartCoroutine
            {

                for (int i = 0; i < this.m_numberOfCo; i++)
                {
                    StartCoroutine(this.startWwwStartupSub());
                }

            }

            // wait 1 frame
            {
                yield return null;
            }

            // wait coroutines
            {

                while (this.m_wwwsList.Find(x => x.currentWorkingState == StartupContents.WorkingState.NowWorking) != null)
                {
                    yield return this.m_waitForSeconds;
                }

            }

        }

        /// <summary>
        /// Start www loadings sub
        /// </summary>
        /// <returns>IEnumerator</returns>
        // -------------------------------------------------------------------------------------------------------
        protected IEnumerator startWwwStartupSub()
        {

            int size = this.m_wwwsList.Count;

            WwwStartupContentsBase wwws = null;

            // ----------------------

            for (int i = 0; i < size; i++)
            {

                // hasError break
                {
                    if (this.hasError())
                    {
                        break;
                    }
                }

                // ----------------------

                wwws = this.m_wwwsList[i];

                // ----------------------

                // continue if not NotYet
                {
                    if (wwws.currentWorkingState != StartupContents.WorkingState.NotYet)
                    {
                        continue;
                    }
                }

                // NowWorking
                {
                    wwws.currentWorkingState = StartupContents.WorkingState.NowWorking;
                }

                // loadWwwStartupContents
                {

                    if (wwws is WwwStartupContentsWww)
                    {
                        yield return this.loadWwwStartupContentsWww(wwws as WwwStartupContentsWww);
                    }

                    else if(wwws is WwwStartupContentsUwr)
                    {
                        yield return this.loadWwwStartupContentsUwr(wwws as WwwStartupContentsUwr);
                    }

#if UNITY_EDITOR
                    else
                    {
                        Debug.LogWarning("TODO");
                    }
#endif

                }

                // DoneSuccessOrError
                {
                    wwws.currentWorkingState = StartupContents.WorkingState.DoneSuccessOrError;
                }

            }

        }

    }

}
