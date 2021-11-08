#pragma warning disable 0618

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SSC
{

    /// <summary>
    /// Class for WWW startup
    /// </summary>
    public partial class WwwStartupManager : SingletonMonoBehaviour<WwwStartupManager>
    {

        /// <summary>
        /// Class for loading WWW startup (WWW)
        /// </summary>
        [Obsolete("Obsoleted", false)]
        protected class WwwStartupContentsWww : WwwStartupContentsBase
        {

            /// <summary>
            /// Url
            /// </summary>
            public string url = "";

            /// <summary>
            /// Success Action
            /// </summary>
            public Action<WWW> successAction = null;

            /// <summary>
            /// Failed Action
            /// </summary>
            public Action<WWW> failedAction = null;

            /// <summary>
            /// Progress Action
            /// </summary>
            public Action<WWW> progressAction = null;

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="_url">url</param>
            /// <param name="_successAction">successAction</param>
            /// <param name="_failedAction">failedAction</param>
            /// <param name="_progressAction">progressAction</param>
            public WwwStartupContentsWww(
                string _url,
                Action<WWW> _successAction,
                Action<WWW> _failedAction,
                Action<WWW> _progressAction
                ) : base()
            {
                this.url = _url;
                this.successAction = _successAction;
                this.failedAction = _failedAction;
                this.progressAction = _progressAction;
            }

        }

        /// <summary>
        /// ThreadPriority
        /// </summary>
        [SerializeField]
        [Tooltip("ThreadPriority")]
        [Obsolete("Obsoleted", false)]
        protected UnityEngine.ThreadPriority m_threadPriority = UnityEngine.ThreadPriority.Low;

        /// <summary>
        /// Add startup data (WWW)
        /// </summary>
        /// <param name="url">url</param>
        /// <param name="success_func">success function</param>
        /// <param name="failed_func">failed function</param>
        /// <param name="progress_func">progress function</param>
        [Obsolete("Use addSceneStartupWwwUwr", false)]
        // -------------------------------------------------------------------------------------------------------
        public void addSceneStartupWww(string url, Action<WWW> success_func, Action<WWW> failed_func, Action<WWW> progress_func)
        {
            this.m_wwwsList.Add(new WwwStartupContentsWww(url, success_func, failed_func, progress_func));
        }

        /// <summary>
        /// Load WwwStartupContents (WWW)
        /// </summary>
        /// <param name="wwws">WwwStartupContentsWww</param>
        /// <returns>IEnumerator</returns>
        [Obsolete("Use loadWwwStartupContentsUwr", false)]
        // -------------------------------------------------------------------------------------------------------
        protected IEnumerator loadWwwStartupContentsWww(WwwStartupContentsWww wwws)
        {

            if (wwws == null)
            {
                yield break;
            }

            // ---------------------

            // WWW
            {

                float noProgressTimer = 0.0f;
                float previousProgress = 0.0f;

                using (WWW www = new WWW(wwws.url))
                {

#if !UNITY_WEBGL
                    www.threadPriority = this.m_threadPriority;
#endif

                    // set progressValueFunc
                    {
                        wwws.progressValueFunc = () =>
                        {
                            return (www != null) ? www.progress : 0.0f;
                        };
                    }

                    // set urlIfNeeded
                    {
                        wwws.urlIfNeeded = wwws.url;
                    }

                    // wait www done
                    {

                        while (!www.isDone)
                        {

                            if (wwws.progressAction != null)
                            {
                                wwws.progressAction(www);
                            }

                            // timeout
                            {

                                if (this.m_noProgressTimeOutSeconds > 0.0f)
                                {

                                    if (Mathf.Approximately(previousProgress, www.progress))
                                    {
                                        noProgressTimer += Time.deltaTime;
                                    }

                                    else
                                    {
                                        noProgressTimer = 0.0f;
                                    }

                                    previousProgress = www.progress;

                                    if (noProgressTimer >= this.m_noProgressTimeOutSeconds)
                                    {
                                        wwws.errorMessage = this.messageTimeout();
                                        break;
                                    }

                                }

                            }

                            yield return null;

                        } // while (!www.isDone)

                        if (wwws.progressAction != null)
                        {
                            wwws.progressAction(www);
                        }

                        yield return null;

                    }

                    // success or fail
                    {

                        // set errorMessage
                        {

                            if (string.IsNullOrEmpty(wwws.errorMessage))
                            {
                                wwws.errorMessage = www.error;
                            }

                        }

                        // success
                        if (string.IsNullOrEmpty(wwws.errorMessage))
                        {
                            if (wwws.successAction != null)
                            {
                                wwws.successAction(www);
                            }
                        }

                        // fail
                        else
                        {

                            if (wwws.failedAction != null)
                            {
                                wwws.failedAction(www);
                            }

                            // updateError
                            {
                                this.updateError(wwws);
                            }

                        }

                    }

                    // set progressValueFunc to null
                    {
                        wwws.progressValueFunc = null;
                    }

                } // using

            }

        }

    }

}
