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
        /// Add startup (UnityWebRequest)
        /// </summary>
        /// <param name="nameDotVariant">nameDotVariant</param>
        /// <param name="abs">AbStartupContentsUwr</param>
        // -------------------------------------------------------------------------------------------------------
        protected void addNewRuntimeAbStartupContentsUwr(string nameDotVariant, AbStartupContentsUwr abs)
        {

            if (SimpleReduxManager.Instance.SceneChangeStateWatcher.state().stateEnum != SceneChangeState.StateEnum.ScenePlaying)
            {
                return;
            }

            // -----------------

            // Enqueue
            {

                AbStartupContentsGroupUwr group = new AbStartupContentsGroupUwr(nameDotVariant);

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
        /// Add AbStartupContents to runtime queue (UnityWebRequest)
        /// </summary>
        /// <param name="assetBundleName">assetBundleName</param>
        /// <param name="variant">variant</param>
        /// <param name="successAction">successAction</param>
        /// <param name="failedAction">failedAction</param>
        /// <param name="progressAction">progressAction</param>
        // -------------------------------------------------------------------------------------------------------
        public void loadAssetBundleInRuntimeUwr(
            string assetBundleName,
            string variant,
            Action<AssetBundle> successAction,
            Action<UnityWebRequest> failedAction,
            Action<UnityWebRequest> progressAction
            )
        {

            this.addNewRuntimeAbStartupContentsUwr(
                this.createNameDotVariantString(assetBundleName, variant),
                new AbStartupContentsUwr(successAction, failedAction, progressAction)
            );

        }

        /// <summary>
        /// Add AbStartupContents ro list (UnityWebRequest)
        /// </summary>
        /// <param name="assetBundleName">assetBundleName</param>
        /// <param name="variant">variant</param>
        /// <param name="successDetailAction">successDetailAction</param>
        /// <param name="failedAction">failedAction</param>
        /// <param name="progressAction">progressAction</param>
        // -------------------------------------------------------------------------------------------------------
        public void loadAssetBundleInRuntimeUwr(
            string assetBundleName,
            string variant,
            Action<AssetBundle, System.Object> successDetailAction,
            Action<UnityWebRequest> failedAction,
            Action<UnityWebRequest> progressAction,
            System.Object identifierForDetail
            )
        {

            this.addNewRuntimeAbStartupContentsUwr(
                this.createNameDotVariantString(assetBundleName, variant),
                new AbStartupContentsUwr(successDetailAction, failedAction, progressAction, identifierForDetail)
            );

        }

        /// <summary>
        /// Add AbStartupContents ro list (UnityWebRequest)
        /// </summary>
        /// <param name="assetBundleName">assetBundleName</param>
        /// <param name="variant">variant</param>
        /// <param name="successDetailActionForAsync">successDetailActionForAsync</param>
        /// <param name="failedAction">failedAction</param>
        /// <param name="progressAction">progressAction</param>
        /// <param name="identifierForDetail">identifierForDetail</param>
        // -------------------------------------------------------------------------------------------------------
        public void loadAssetBundleInRuntimeUwr(
            string assetBundleName,
            string variant,
            Action<AssetBundle, System.Object, Action> successDetailActionForAsync,
            Action<UnityWebRequest> failedAction,
            Action<UnityWebRequest> progressAction,
            System.Object identifierForDetail
            )
        {

            this.addNewRuntimeAbStartupContentsUwr(
                this.createNameDotVariantString(assetBundleName, variant),
                new AbStartupContentsUwr(successDetailActionForAsync, failedAction, progressAction, identifierForDetail)
            );

        }

    }

}
