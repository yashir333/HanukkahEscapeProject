using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace SSC
{

    /// <summary>
    /// Class for WWW startup
    /// </summary>
    public partial class WwwStartupManager : SingletonMonoBehaviour<WwwStartupManager>
    {

        /// <summary>
        /// Class for loading UWR startup (UnityWebRequest)
        /// </summary>
        protected class WwwStartupContentsUwr : WwwStartupContentsBase
        {

            /// <summary>
            /// UnityWebRequest
            /// </summary>
            public UnityWebRequest uwr = null;

            /// <summary>
            /// Dispose uwr
            /// </summary>
            public bool disposeUwr = true;

            /// <summary>
            /// Success Action
            /// </summary>
            public Action<UnityWebRequest> successAction = null;

            /// <summary>
            /// Failed Action
            /// </summary>
            public Action<UnityWebRequest> failedAction = null;

            /// <summary>
            /// Progress Action
            /// </summary>
            public Action<UnityWebRequest> progressAction = null;

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="_urw">UnityWebRequest</param>
            /// <param name="_disposeUwr">dispose uwr</param>
            /// <param name="_successAction">successAction</param>
            /// <param name="_failedAction">failedAction</param>
            /// <param name="_progressAction">progressAction</param>
            public WwwStartupContentsUwr(
                UnityWebRequest _uwr,
                bool _disposeUwr,
                Action<UnityWebRequest> _successAction,
                Action<UnityWebRequest> _failedAction,
                Action<UnityWebRequest> _progressAction
                ) : base()
            {
                this.uwr = _uwr;
                this.disposeUwr = _disposeUwr;
                this.successAction = _successAction;
                this.failedAction = _failedAction;
                this.progressAction = _progressAction;
            }

        }

        /// <summary>
        /// Add startup data (UnityWebRequest)
        /// </summary>
        /// <param name="uwr">UnityWebRequest</param>
        /// <param name="disposeUwr">dispose uwr</param>
        /// <param name="success_func">success function</param>
        /// <param name="failed_func">failed function</param>
        /// <param name="progress_func">progress function</param>
        // -------------------------------------------------------------------------------------------------------
        public void addSceneStartupWwwUwr(UnityWebRequest uwr, bool disposeUwr, Action<UnityWebRequest> success_func, Action<UnityWebRequest> failed_func, Action<UnityWebRequest> progress_func)
        {
            this.m_wwwsList.Add(new WwwStartupContentsUwr(uwr, disposeUwr, success_func, failed_func, progress_func));
        }

        /// <summary>
        /// Load WwwStartupContents (UnityWebRequest)
        /// </summary>
        /// <param name="wwws">WwwStartupContentsUwr</param>
        /// <returns>IEnumerator</returns>
        // -------------------------------------------------------------------------------------------------------
        protected IEnumerator loadWwwStartupContentsUwr(WwwStartupContentsUwr wwws)
        {

            if (wwws == null || wwws.uwr == null)
            {
                yield break;
            }

            // ---------------------

            // UnityWebRequest
            {

                float noProgressTimer = 0.0f;
                float previousProgress = 0.0f;

                // ---------------------

#if UNITY_2017_2_OR_NEWER
                UnityWebRequestAsyncOperation ao = wwws.uwr.SendWebRequest();
#else
                AsyncOperation ao = wwws.uwr.Send();
#endif

                // ---------------------

                if (ao == null)
                {

                    if (wwws.disposeUwr)
                    {
                        wwws.uwr.Dispose();
                    }

                    yield break;

                }

                // ---------------------

                // set progressValueFunc
                {
                    wwws.progressValueFunc = () =>
                    {
                        return ao.progress;
                    };
                }

                // set urlIfNeeded
                {
                    wwws.urlIfNeeded = wwws.uwr.url;
                }

                // wait www done
                {

                    while (!ao.isDone)
                    {

                        if (wwws.progressAction != null)
                        {
                            wwws.progressAction(wwws.uwr);
                        }

                        // timeout
                        {

                            if (this.m_noProgressTimeOutSeconds > 0.0f)
                            {

                                if (Mathf.Approximately(previousProgress, ao.progress))
                                {
                                    noProgressTimer += Time.deltaTime;
                                }

                                else
                                {
                                    noProgressTimer = 0.0f;
                                }

                                previousProgress = ao.progress;

                                if (noProgressTimer >= this.m_noProgressTimeOutSeconds)
                                {
                                    wwws.errorMessage = this.messageTimeout();
                                    break;
                                }

                            }

                        }

                        yield return null;

                    } // while (!ao.isDone)

                    if (wwws.progressAction != null)
                    {
                        wwws.progressAction(wwws.uwr);
                    }

                    yield return null;

                }

                // success or fail
                {

                    // set errorMessage
                    {

                        if (string.IsNullOrEmpty(wwws.errorMessage))
                        {
                            wwws.errorMessage = wwws.uwr.error;
                        }

                    }

                    // success
                    if (string.IsNullOrEmpty(wwws.errorMessage))
                    {
                        if (wwws.successAction != null)
                        {
                            wwws.successAction(wwws.uwr);
                        }
                    }

                    // fail
                    else
                    {

                        if (wwws.failedAction != null)
                        {
                            wwws.failedAction(wwws.uwr);
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

            }

            // Dispose
            {

                if (wwws.disposeUwr)
                {
                    wwws.uwr.Dispose();
                }

            }

        }

    }

}
