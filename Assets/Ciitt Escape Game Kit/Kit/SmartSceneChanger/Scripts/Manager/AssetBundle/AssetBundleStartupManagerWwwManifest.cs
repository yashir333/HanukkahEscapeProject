#pragma warning disable 0618

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SSC
{

    /// <summary>
    /// Class for AssetBundle startup
    /// </summary>
    public partial class AssetBundleStartupManager : SingletonMonoBehaviour<AssetBundleStartupManager>
    {

        /// <summary>
        /// Download manifest
        /// </summary>
        /// <param name="forcibly">forcibly</param>
        /// <returns>IEnumerator</returns>
        [Obsolete("Use downloadManifestUwr", false)]
        // -------------------------------------------------------------------------------------------------------
        protected IEnumerator downloadManifest(bool forcibly)
        {

            if(string.IsNullOrEmpty(this.m_manifestInfo.manifestFileUrl))
            {
#if UNITY_EDITOR
                Debug.LogWarning("(#if UNITY_EDITOR) : manifestFileUrl is empty");
#endif
                yield break;
            }

            // -----------------

            // loadManifestFromPlayerPrefs
            {

                if(!forcibly && this.m_manifestInfo.usePlayerPrefsToSaveAndLoadManifest)
                {

                    bool shouldBreak = false;

                    yield return this.loadManifestFromPlayerPrefs(() =>
                    {
                        shouldBreak = true;
                    });

                    if (shouldBreak)
                    {
                        yield break;
                    }

                }

            }

            // -----------------

            float noProgressTimer = 0.0f;
            float previousProgress = 0.0f;

            // -----------------

            using (WWW www = new WWW(this.m_manifestInfo.manifestFileUrl))
            {

#if !UNITY_WEBGL
                www.threadPriority = this.m_threadPriority;
#endif

                // set dummyStartup
                {
                    this.m_manifestInfo.dummyStartup.currentWorkingState = StartupContents.WorkingState.NowWorking; // (meaningless)
                    this.m_manifestInfo.dummyStartup.urlIfNeeded = www.url;
                }

                // wait www done
                {

                    while (!www.isDone)
                    {

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
                                    this.m_manifestInfo.dummyStartup.errorMessage = this.messageTimeout();
                                    break;
                                }

                            }

                        }

                        yield return null;

                    } // while (!www.isDone)

                    yield return null;

                }

                // success or fail
                {

                    // set errorMessage
                    {
                        if (string.IsNullOrEmpty(this.m_manifestInfo.dummyStartup.errorMessage))
                        {
                            this.m_manifestInfo.dummyStartup.errorMessage = www.error;
                        }
                    }

                    // success
                    if (string.IsNullOrEmpty(this.m_manifestInfo.dummyStartup.errorMessage))
                    {

                        yield return this.loadManifest(www.assetBundle);

                        if(this.m_manifestInfo.manifest)
                        {
                            this.m_manifestInfo.saveManifestToPlayerprefsIfNeeded(www.bytes);
                        }

                    }

                    // fail
                    else
                    {
                        this.updateError(this.m_manifestInfo.dummyStartup);
                    }

                }

                // set dummyStartup
                {
                    this.m_manifestInfo.dummyStartup.currentWorkingState = StartupContents.WorkingState.DoneSuccessOrError; // (meaningless)
                }

            } // using (WWW www = new WWW(this.m_manifestInfo.manifestFileUrl))

        }

    }

}
