using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace SSC
{

    /// <summary>
    /// Class for AssetBundle startup
    /// </summary>
    public partial class AssetBundleStartupManager : SingletonMonoBehaviour<AssetBundleStartupManager>
    {

        /// <summary>
        /// Class for AssetBundle startup (UnityWebRequest)
        /// </summary>
        protected class AbStartupContentsUwr : AbStartupContentsBase
        {

            /// <summary>
            /// Failed action
            /// </summary>
            public Action<UnityWebRequest> failedAction = null;

            /// <summary>
            /// Progress action
            /// </summary>
            public Action<UnityWebRequest> progressAction = null;

            /// <summary>
            /// Constructor
            /// </summary>
            public AbStartupContentsUwr() : base()
            {

            }

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="_successAction">successAction</param>
            /// <param name="_failedAction">failedAction</param>
            /// <param name="_progressAction">progressAction</param>
            public AbStartupContentsUwr(
                Action<AssetBundle> _successAction,
                Action<UnityWebRequest> _failedAction,
                Action<UnityWebRequest> _progressAction
                ) : base(_successAction)
            {
                this.failedAction = _failedAction;
                this.progressAction = _progressAction;
            }

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="_successDetailAction">successDetailAction</param>
            /// <param name="_failedAction">failedAction</param>
            /// <param name="_progressAction">progressAction</param>
            /// <param name="_identifierForDetail">identifierForDetail</param>
            public AbStartupContentsUwr(
                Action<AssetBundle, System.Object> _successDetailAction,
                Action<UnityWebRequest> _failedAction,
                Action<UnityWebRequest> _progressAction,
                System.Object _identifierForDetail
                ) : base(_successDetailAction, _identifierForDetail)
            {
                this.failedAction = _failedAction;
                this.progressAction = _progressAction;
            }

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="_successDetailActionForAsync">successDetailActionForAsync</param>
            /// <param name="_failedAction">failedAction</param>
            /// <param name="_progressAction">progressAction</param>
            /// <param name="_identifierForDetail">identifierForDetail</param>
            public AbStartupContentsUwr(
                Action<AssetBundle, System.Object, Action> _successDetailActionForAsync,
                Action<UnityWebRequest> _failedAction,
                Action<UnityWebRequest> _progressAction,
                System.Object _identifierForDetail
                ) : base(_successDetailActionForAsync, _identifierForDetail)
            {
                this.failedAction = _failedAction;
                this.progressAction = _progressAction;
            }

        }

        /// <summary>
        /// Class for AssetBundle startup (UnityWebRequest)
        /// </summary>
        protected class AbStartupContentsGroupUwr : AbStartupContentsGroupBase
        {

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="_nameDotVariant">nameDotVariant</param>
            public AbStartupContentsGroupUwr(string _nameDotVariant) : base(_nameDotVariant)
            {
                
            }

        }

        /// <summary>
        /// Add startup data (UnityWebRequest)
        /// </summary>
        /// <param name="assetBundleName">assetBundleName</param>
        /// <param name="variant">variant</param>
        /// <param name="successAction">success function</param>
        /// <param name="failedAction">failed function</param>
        /// <param name="progressAction">progress function</param>
        // -------------------------------------------------------------------------------------------------------
        public void addSceneStartupAssetBundleUwr(
            string assetBundleName,
            string variant,
            Action<AssetBundle> successAction,
            Action<UnityWebRequest> failedAction,
            Action<UnityWebRequest> progressAction
        )
        {

            this.addNewDetectedAbStartupContentsUwr(
                this.createNameDotVariantString(assetBundleName, variant),
                new AbStartupContentsUwr(successAction, failedAction, progressAction)
            );

        }

        /// <summary>
        /// Add startup data (UnityWebRequest)
        /// </summary>
        /// <param name="assetBundleName">assetBundleName</param>
        /// <param name="variant">variant</param>
        /// <param name="successAction">success function</param>
        /// <param name="failedAction">failed function</param>
        /// <param name="progressAction">progress function</param>
        // -------------------------------------------------------------------------------------------------------
        public void addSceneStartupAssetBundleUwr(
            string assetBundleName,
            string variant,
            Action<AssetBundle, System.Object> successAction,
            Action<UnityWebRequest> failedAction,
            Action<UnityWebRequest> progressAction,
            System.Object identifierForDetail
            )
        {

            this.addNewDetectedAbStartupContentsUwr(
                this.createNameDotVariantString(assetBundleName, variant),
                new AbStartupContentsUwr(successAction, failedAction, progressAction, identifierForDetail)
            );

        }

        /// <summary>
        /// Add startup data (UnityWebRequest)
        /// </summary>
        /// <param name="assetBundleName">assetBundleName</param>
        /// <param name="variant">variant</param>
        /// <param name="successAction">success function</param>
        /// <param name="failedAction">failed function</param>
        /// <param name="progressAction">progress function</param>
        // -------------------------------------------------------------------------------------------------------
        public void addSceneStartupAssetBundleUwr(
            string assetBundleName,
            string variant,
            Action<AssetBundle, System.Object, Action> successAction,
            Action<UnityWebRequest> failedAction,
            Action<UnityWebRequest> progressAction,
            System.Object identifierForDetail
            )
        {

            this.addNewDetectedAbStartupContentsUwr(
                this.createNameDotVariantString(assetBundleName, variant),
                new AbStartupContentsUwr(successAction, failedAction, progressAction, identifierForDetail)
            );

        }

        /// <summary>
        /// Add startup (UnityWebRequest)
        /// </summary>
        /// <param name="nameDotVariant">nameDotVariant</param>
        /// <param name="abs">AbStartupContentsUwr</param>
        // -------------------------------------------------------------------------------------------------------
        protected void addNewDetectedAbStartupContentsUwr(string nameDotVariant, AbStartupContentsUwr abs)
        {

            if (SimpleReduxManager.Instance.SceneChangeStateWatcher.state().stateEnum == SceneChangeState.StateEnum.NowLoadingMain)
            {

                if (!this.m_newDetected.ContainsKey(nameDotVariant))
                {
                    this.m_newDetected.Add(nameDotVariant, new AbStartupContentsGroupUwr(nameDotVariant));
                }

                this.m_newDetected[nameDotVariant].absList.Add(abs);

            }

            else
            {
                this.addNewRuntimeAbStartupContentsUwr(nameDotVariant, abs);
            }

        }

        /// <summary>
        /// Load AbStartupContentsGroupUwr
        /// </summary>
        /// <param name="absGroup">AbStartupContentsGroupUwr</param>
        /// <returns>IEnumerator</returns>
        // -------------------------------------------------------------------------------------------------------
        protected IEnumerator loadAbStartupContentsUwr(AbStartupContentsGroupUwr absGroup)
        {

            if (absGroup == null)
            {
                yield break;
            }

            if (absGroup.absList.Count <= 0)
            {
#if UNITY_EDITOR

                Debug.LogError("(#if UNITY_EDITOR) : Implementation Error in loadAbStartupContentsUwr");
#endif
                yield break;
            }

            // ---------------------

            // Caching
            {
                while (!Caching.ready)
                {
                    yield return null;
                }
            }

            // ---------------------

            if (!absGroup.assetBundle)
            {
                absGroup.assetBundle = this.findAlreadyLoadedAssetBundle(absGroup.nameDotVariant);
            }

            if (absGroup.assetBundle)
            {

                foreach (var absUwr in absGroup.absList)
                {

                    if (absUwr.currentWorkingState == StartupContents.WorkingState.DoneSuccessOrError)
                    {
                        continue;
                    }

                    // --------------

                    // TODO?
                    yield return this.loadAdditiveSceneIfNeeded(absGroup.assetBundle);

                    if (absUwr.successAction != null)
                    {
                        absUwr.successAction(absGroup.assetBundle);
                    }

                }

            }

            else
            {
                
                float noProgressTimer = 0.0f;
                float previousProgress = 0.0f;

#if UNITY_2018_1_OR_NEWER
                using (UnityWebRequest uwr =
                    UnityWebRequestAssetBundle.GetAssetBundle(
                        this.createAssetBundleUrl(absGroup.nameDotVariant),
                        this.m_manifestInfo.manifest.GetAssetBundleHash(absGroup.nameDotVariant),
                        0
                        ))
#else
                using (UnityWebRequest uwr =
                    UnityWebRequest.GetAssetBundle(
                        this.createAssetBundleUrl(absGroup.nameDotVariant),
                        this.m_manifestInfo.manifest.GetAssetBundleHash(absGroup.nameDotVariant),
                        0
                        ))
#endif
                {

                    // -------------------

#if UNITY_2017_2_OR_NEWER
                    UnityWebRequestAsyncOperation ao = uwr.SendWebRequest();
#else
                    AsyncOperation ao = uwr.Send();
#endif

                    // -------------------

                    if (ao == null)
                    {
                        yield break;
                    }

                    // -------------------

                    // set progressValueFunc
                    {
                        foreach (var abs in absGroup.absList)
                        {
                            abs.progressValueFunc = () =>
                            {
                                return ao.progress;
                            };
                        }
                    }

                    // set urlIfNeeded
                    {
                        foreach (var abs in absGroup.absList)
                        {
                            abs.urlIfNeeded = uwr.url;
                        }
                    }

                    // wait ao done
                    {

                        while (!ao.isDone)
                        {

                            foreach (var abs in absGroup.absList)
                            {
                                if ((abs as AbStartupContentsUwr).progressAction != null)
                                {
                                    (abs as AbStartupContentsUwr).progressAction(uwr);
                                }
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

                                        foreach (var abs in absGroup.absList)
                                        {
                                            abs.errorMessage = this.messageTimeout();
                                        }

                                        break;

                                    }

                                }

                            }

                            yield return null;

                        } // while (!ao.isDone)

                        foreach (var abs in absGroup.absList)
                        {
                            if ((abs as AbStartupContentsUwr).progressAction != null)
                            {
                                (abs as AbStartupContentsUwr).progressAction(uwr);
                            }
                        }

                        yield return null;

                    }

                    // success or fail
                    {

                        // set errorMessage
                        {

                            foreach (var abs in absGroup.absList)
                            {
                                if (string.IsNullOrEmpty(abs.errorMessage))
                                {
                                    abs.errorMessage = uwr.error;
                                }
                            }

                        }

                        // success
                        if (string.IsNullOrEmpty(this.m_manifestInfo.dummyStartup.errorMessage))
                        {

                            AssetBundle assetBundle = DownloadHandlerAssetBundle.GetContent(uwr);

                            if (assetBundle)
                            {

                                yield return this.decryptAssetBundleIfNeeded(assetBundle, (ab) =>
                                {
                                    absGroup.assetBundle = ab;
                                });

                                if (absGroup.assetBundle)
                                {

                                    yield return this.loadAdditiveSceneIfNeeded(absGroup.assetBundle);

                                    // success action
                                    {

                                        foreach (var abs in absGroup.absList)
                                        {

                                            if (abs.successAction != null)
                                            {
                                                abs.successAction(absGroup.assetBundle);
                                            }

                                            else if (abs.successDetailAction != null)
                                            {
                                                abs.successDetailAction(absGroup.assetBundle, abs.identifierForDetail);
                                            }

                                            else if (abs.successDetailActionForAsync != null)
                                            {

                                                bool finished = false;

                                                abs.successDetailActionForAsync(absGroup.assetBundle, abs.identifierForDetail, () =>
                                                {
                                                    finished = true;
                                                });

                                                while (!finished)
                                                {
                                                    yield return null;
                                                }

                                            }

                                        }

                                    }

                                    // Don't do here
                                    {
                                        // absGroup.unloadAssetBundle(false);
                                    }

                                } // if (this.m_manifestInfo.manifestAssetBundle)

                                else
                                {
                                    absGroup.absList[0].errorMessage = this.messageFailedToDecryptAssetBundle();
                                    this.updateError(absGroup.absList[0]);
                                }

                            } // if (assetBundle)

                            else
                            {
                                absGroup.absList[0].errorMessage = this.messageAssetBundleNotFound();
                                this.updateError(absGroup.absList[0]);
                            }

                        } // if (string.IsNullOrEmpty(this.m_manifestInfo.dummyStartup.errorMessage))

                        // fail
                        else
                        {

                            foreach (var abs in absGroup.absList)
                            {
                                if ((abs as AbStartupContentsUwr).failedAction != null)
                                {
                                    (abs as AbStartupContentsUwr).failedAction(uwr);
                                }
                            }

                            // updateError
                            {
                                this.updateError(absGroup.absList[0]);
                            }

                        }

                    } // success or fail

                    // set progressValueFunc to null
                    {
                        foreach (var abs in absGroup.absList)
                        {
                            abs.progressValueFunc = null;
                        }
                    }

                } // using

            }

        }

    }

}
