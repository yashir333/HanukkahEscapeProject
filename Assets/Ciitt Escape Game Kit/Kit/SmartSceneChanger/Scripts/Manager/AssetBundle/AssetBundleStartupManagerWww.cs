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
        /// Class for AssetBundle startup (WWW)
        /// </summary>
        [Obsolete("Use AbStartupContentsUwr", false)]
        protected class AbStartupContentsWww : AbStartupContentsBase
        {

            /// <summary>
            /// Failed action
            /// </summary>
            public Action<WWW> failedAction = null;

            /// <summary>
            /// Progress action
            /// </summary>
            public Action<WWW> progressAction = null;

            /// <summary>
            /// Constructor
            /// </summary>
            public AbStartupContentsWww() : base()
            {

            }

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="_successAction">successAction</param>
            /// <param name="_failedAction">failedAction</param>
            /// <param name="_progressAction">progressAction</param>
            public AbStartupContentsWww(
                Action<AssetBundle> _successAction,
                Action<WWW> _failedAction,
                Action<WWW> _progressAction
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
            public AbStartupContentsWww(
                Action<AssetBundle, System.Object> _successDetailAction,
                Action<WWW> _failedAction,
                Action<WWW> _progressAction,
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
            public AbStartupContentsWww(
                Action<AssetBundle, System.Object, Action> _successDetailActionForAsync,
                Action<WWW> _failedAction,
                Action<WWW> _progressAction,
                System.Object _identifierForDetail
                ) : base(_successDetailActionForAsync, _identifierForDetail)
            {
                this.failedAction = _failedAction;
                this.progressAction = _progressAction;
            }

        }

        /// <summary>
        /// Class for AssetBundle startup (WWW)
        /// </summary>
        [Obsolete("Use AbStartupContentsGroupUwr", false)]
        protected class AbStartupContentsGroupWww : AbStartupContentsGroupBase
        {

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="_nameDotVariant">nameDotVariant</param>
            public AbStartupContentsGroupWww(string _nameDotVariant) : base(_nameDotVariant)
            {
                
            }

        }

        /// <summary>
        /// Add startup data (WWW)
        /// </summary>
        /// <param name="assetBundleName">assetBundleName</param>
        /// <param name="variant">variant</param>
        /// <param name="successAction">success function</param>
        /// <param name="failedAction">failed function</param>
        /// <param name="progressAction">progress function</param>
        [Obsolete("Use addSceneStartupAssetBundleUwr", false)]
        // -------------------------------------------------------------------------------------------------------
        public void addSceneStartupAssetBundle(
            string assetBundleName,
            string variant,
            Action<AssetBundle> successAction,
            Action<WWW> failedAction,
            Action<WWW> progressAction
        )
        {

            this.addNewDetectedAbStartupContentsWww(
                this.createNameDotVariantString(assetBundleName, variant),
                new AbStartupContentsWww(successAction, failedAction, progressAction)
            );

        }

        /// <summary>
        /// Add startup data (WWW)
        /// </summary>
        /// <param name="assetBundleName">assetBundleName</param>
        /// <param name="variant">variant</param>
        /// <param name="successAction">success function</param>
        /// <param name="failedAction">failed function</param>
        /// <param name="progressAction">progress function</param>
        [Obsolete("Use addSceneStartupAssetBundleUwr", false)]
        // -------------------------------------------------------------------------------------------------------
        public void addSceneStartupAssetBundle(
            string assetBundleName,
            string variant,
            Action<AssetBundle, System.Object> successAction,
            Action<WWW> failedAction,
            Action<WWW> progressAction,
            System.Object identifierForDetail
            )
        {

            this.addNewDetectedAbStartupContentsWww(
                this.createNameDotVariantString(assetBundleName, variant),
                new AbStartupContentsWww(successAction, failedAction, progressAction, identifierForDetail)
            );

        }

        /// <summary>
        /// Add startup data (WWW)
        /// </summary>
        /// <param name="assetBundleName">assetBundleName</param>
        /// <param name="variant">variant</param>
        /// <param name="successAction">success function</param>
        /// <param name="failedAction">failed function</param>
        /// <param name="progressAction">progress function</param>
        [Obsolete("Use addSceneStartupAssetBundleUwr", false)]
        // -------------------------------------------------------------------------------------------------------
        public void addSceneStartupAssetBundle(
            string assetBundleName,
            string variant,
            Action<AssetBundle, System.Object, Action> successAction,
            Action<WWW> failedAction,
            Action<WWW> progressAction,
            System.Object identifierForDetail
            )
        {

            this.addNewDetectedAbStartupContentsWww(
                this.createNameDotVariantString(assetBundleName, variant),
                new AbStartupContentsWww(successAction, failedAction, progressAction, identifierForDetail)
            );

        }

        /// <summary>
        /// Add startup (WWW)
        /// </summary>
        /// <param name="nameDotVariant">nameDotVariant</param>
        /// <param name="abs">AbStartupContents</param>
        [Obsolete("Use addNewDetectedAbStartupContentsUwr", false)]
        // -------------------------------------------------------------------------------------------------------
        protected void addNewDetectedAbStartupContentsWww(string nameDotVariant, AbStartupContentsWww abs)
        {

#if UNITY_EDITOR

            // warning
            {
                Debug.LogWarning("(#if UNITY_EDITOR) Use addSceneStartupAssetBundleUwr : " + nameDotVariant);
            }
#endif

            if (SimpleReduxManager.Instance.SceneChangeStateWatcher.state().stateEnum == SceneChangeState.StateEnum.NowLoadingMain)
            {

                if (!this.m_newDetected.ContainsKey(nameDotVariant))
                {
                    this.m_newDetected.Add(nameDotVariant, new AbStartupContentsGroupWww(nameDotVariant));
                }

                this.m_newDetected[nameDotVariant].absList.Add(abs);

            }

            else
            {
                this.addNewRuntimeAbStartupContents(nameDotVariant, abs);
            }

        }

        /// <summary>
        /// Load AbStartupContentsGroup (WWW)
        /// </summary>
        /// <param name="absGroup">AbStartupContentsGroup</param>
        /// <returns>IEnumerator</returns>
        [Obsolete("Use loadAbStartupContentsUwr", false)]
        // -------------------------------------------------------------------------------------------------------
        protected IEnumerator loadAbStartupContentsWww(AbStartupContentsGroupWww absGroup)
        {

            if (absGroup == null)
            {
                yield break;
            }

            if (absGroup.absList.Count <= 0)
            {
#if UNITY_EDITOR

                Debug.LogError("(#if UNITY_EDITOR) : Implementation Error in loadAbStartupContentsWww");
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

                foreach (var abs in absGroup.absList)
                {

                    if (abs.currentWorkingState == StartupContents.WorkingState.DoneSuccessOrError)
                    {
                        continue;
                    }

                    // --------------

                    // TODO?
                    yield return this.loadAdditiveSceneIfNeeded(absGroup.assetBundle);

                    if (abs.successAction != null)
                    {
                        abs.successAction(absGroup.assetBundle);
                    }

                }

            }

            else
            {

                float noProgressTimer = 0.0f;
                float previousProgress = 0.0f;

                using (WWW www =
                    WWW.LoadFromCacheOrDownload(
                        this.createAssetBundleUrl(absGroup.nameDotVariant),
                        this.m_manifestInfo.manifest.GetAssetBundleHash(absGroup.nameDotVariant)
                        ))
                {

#if !UNITY_WEBGL
                    www.threadPriority = this.m_threadPriority;
#endif

                    // set progressValueFunc
                    {
                        foreach (var abs in absGroup.absList)
                        {
                            abs.progressValueFunc = () =>
                            {
                                return (www != null) ? www.progress : 0.0f;
                            };
                        }
                    }

                    // set urlIfNeeded
                    {
                        foreach (var abs in absGroup.absList)
                        {
                            abs.urlIfNeeded = www.url;
                        }
                    }

                    // wait www done
                    {

                        while (!www.isDone)
                        {

                            foreach (var abs in absGroup.absList)
                            {
                                if ((abs as AbStartupContentsWww).progressAction != null)
                                {
                                    (abs as AbStartupContentsWww).progressAction(www);
                                }
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

                                        foreach (var abs in absGroup.absList)
                                        {
                                            abs.errorMessage = this.messageTimeout();
                                        }

                                        break;

                                    }

                                }

                            }

                            yield return null;

                        } // while (!www.isDone)

                        foreach (var abs in absGroup.absList)
                        {
                            if ((abs as AbStartupContentsWww).progressAction != null)
                            {
                                (abs as AbStartupContentsWww).progressAction(www);
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
                                    abs.errorMessage = www.error;
                                }
                            }

                        }

                        // success
                        if (string.IsNullOrEmpty(this.m_manifestInfo.dummyStartup.errorMessage))
                        {

                            if (www.assetBundle)
                            {

                                yield return this.decryptAssetBundleIfNeeded(www.assetBundle, (ab) =>
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

                            } // if (www.assetBundle)

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
                                if ((abs as AbStartupContentsWww).failedAction != null)
                                {
                                    (abs as AbStartupContentsWww).failedAction(www);
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
