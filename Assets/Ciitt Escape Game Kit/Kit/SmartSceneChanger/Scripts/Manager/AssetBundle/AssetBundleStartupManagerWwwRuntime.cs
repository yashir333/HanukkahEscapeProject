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
        /// Add startup
        /// </summary>
        /// <param name="nameDotVariant">nameDotVariant</param>
        /// <param name="abs">AbStartupContents</param>
        [Obsolete("Use addNewRuntimeAbStartupContentsUwr", false)]
        // -------------------------------------------------------------------------------------------------------
        protected void addNewRuntimeAbStartupContents(string nameDotVariant, AbStartupContentsWww abs)
        {

#if UNITY_EDITOR

            // warning
            {
                Debug.LogWarning("(#if UNITY_EDITOR) Use loadAssetBundleInRuntimeUwr : " + nameDotVariant);
            }
#endif

            if (SimpleReduxManager.Instance.SceneChangeStateWatcher.state().stateEnum != SceneChangeState.StateEnum.ScenePlaying)
            {
                return;
            }

            // -----------------

            // Enqueue
            {

                AbStartupContentsGroupWww group = new AbStartupContentsGroupWww(nameDotVariant);

                group.absList.Add(abs);

                this.m_runtimeQueue.Enqueue(group);

            }

            // StartCoroutine
            {
                if (this.m_runtimeLoading == null)
                {
                    StartCoroutine(this.m_runtimeLoading = this.loadAssetBundleInRuntimeIE());
                }
            }

        }

        /// <summary>
        /// Add AbStartupContents to runtime queue
        /// </summary>
        /// <param name="assetBundleName">assetBundleName</param>
        /// <param name="variant">variant</param>
        /// <param name="successAction">successAction</param>
        /// <param name="failedAction">failedAction</param>
        /// <param name="progressAction">progressAction</param>
        [Obsolete("Use loadAssetBundleInRuntimeUwr", false)]
        // -------------------------------------------------------------------------------------------------------
        public void loadAssetBundleInRuntime(
            string assetBundleName,
            string variant,
            Action<AssetBundle> successAction,
            Action<WWW> failedAction,
            Action<WWW> progressAction
            )
        {

            this.addNewRuntimeAbStartupContents(
                this.createNameDotVariantString(assetBundleName, variant),
                new AbStartupContentsWww(successAction, failedAction, progressAction)
            );

        }

        /// <summary>
        /// Add AbStartupContents ro list
        /// </summary>
        /// <param name="assetBundleName">assetBundleName</param>
        /// <param name="variant">variant</param>
        /// <param name="successDetailAction">successDetailAction</param>
        /// <param name="failedAction">failedAction</param>
        /// <param name="progressAction">progressAction</param>
        [Obsolete("Use loadAssetBundleInRuntimeUwr", false)]
        // -------------------------------------------------------------------------------------------------------
        public void loadAssetBundleInRuntime(
            string assetBundleName,
            string variant,
            Action<AssetBundle, System.Object> successDetailAction,
            Action<WWW> failedAction,
            Action<WWW> progressAction,
            System.Object identifierForDetail
            )
        {

            this.addNewRuntimeAbStartupContents(
                this.createNameDotVariantString(assetBundleName, variant),
                new AbStartupContentsWww(successDetailAction, failedAction, progressAction, identifierForDetail)
            );

        }

        /// <summary>
        /// Add AbStartupContents ro list
        /// </summary>
        /// <param name="assetBundleName">assetBundleName</param>
        /// <param name="variant">variant</param>
        /// <param name="successDetailActionForAsync">successDetailActionForAsync</param>
        /// <param name="failedAction">failedAction</param>
        /// <param name="progressAction">progressAction</param>
        /// <param name="identifierForDetail">identifierForDetail</param>
        [Obsolete("Use loadAssetBundleInRuntimeUwr", false)]
        // -------------------------------------------------------------------------------------------------------
        public void loadAssetBundleInRuntime(
            string assetBundleName,
            string variant,
            Action<AssetBundle, System.Object, Action> successDetailActionForAsync,
            Action<WWW> failedAction,
            Action<WWW> progressAction,
            System.Object identifierForDetail
            )
        {

            this.addNewRuntimeAbStartupContents(
                this.createNameDotVariantString(assetBundleName, variant),
                new AbStartupContentsWww(successDetailActionForAsync, failedAction, progressAction, identifierForDetail)
            );

        }

    }

}
