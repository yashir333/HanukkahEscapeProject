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
        /// Queue for loading AssetBundle in runtime
        /// </summary>
        protected Queue<AbStartupContentsGroupBase> m_runtimeQueue = new Queue<AbStartupContentsGroupBase>();

        /// <summary>
        /// IEnumerator for runtime loading
        /// </summary>
        protected IEnumerator m_runtimeLoading = null;

        /// <summary>
        /// Retry runtime
        /// </summary>
        [Obsolete("Use retryRuntimeUwr", false)]
        // -------------------------------------------------------------------------------------------------------
        protected void retryRuntime()
        {

            // m_runtimeLoading
            {

                if (this.m_runtimeLoading != null)
                {
                    StopCoroutine(this.m_runtimeLoading);
                }

                StartCoroutine(this.m_runtimeLoading = this.loadAssetBundleInRuntimeIE());

            }

        }

        /// <summary>
        /// Back to title because of error
        /// </summary>
        [Obsolete("Use backToTileBecauseOfRuntimeUwrError", false)]
        // -------------------------------------------------------------------------------------------------------
        protected void backToTileBecauseOfRuntimeError()
        {

            // removeLockFromBefore
            {
                SceneChangeManager.Instance.removeLockFromBefore(this);
            }

            // m_runtimeLoading
            {

                if (this.m_runtimeLoading != null)
                {
                    StopCoroutine(this.m_runtimeLoading);
                }

                this.m_runtimeLoading = null;

            }

            // backToTitleSceneWithOkDialog
            {
                SceneChangeManager.Instance.backToTitleSceneWithOkDialog();
            }

        }

        /// <summary>
        /// Load AssetBundle in runtime
        /// </summary>
        /// <returns>IEnumerator</returns>
        // -------------------------------------------------------------------------------------------------------
        protected IEnumerator loadAssetBundleInRuntimeIE()
        {

            // addLockToBefore
            {
                SceneChangeManager.Instance.addLockToBefore(this);
            }

            // wait 1 frame
            {
                yield return null;
            }

            // setManifestFileAndFolderUrl
            {
                this.setManifestFileAndFolderUrl();
            }

            // loop
            {

                AbStartupContentsGroupBase group = null;

                while (this.m_runtimeQueue.Count > 0)
                {

                    // Peek
                    {
                        group = this.m_runtimeQueue.Peek();
                    }

                    // clearContents
                    {
                        this.clearContents(false);
                    }

                    // downloadManifest
                    {

                        if (!this.m_manifestInfo.manifest)
                        {
                            yield return this.downloadManifest(false);
                        }

                        if (this.hasError())
                        {
                            break;
                        }

                    }

                    // addAllDependencies
                    {
                        yield return this.addAllDependencies(group.GetType(), group.nameDotVariant);
                    }

                    // m_dependencies
                    {

                        // loadAbStartupContentsWww, loadAbStartupContentsUwr
                        {

                            foreach (var depend in this.m_dependencies)
                            {

                                if (depend.Value is AbStartupContentsGroupWww)
                                {
                                    yield return this.loadAbStartupContentsWww(depend.Value as AbStartupContentsGroupWww);
                                }

                                else if (depend.Value is AbStartupContentsGroupUwr)
                                {
                                    yield return this.loadAbStartupContentsUwr(depend.Value as AbStartupContentsGroupUwr);
                                }

#if UNITY_EDITOR
                                else
                                {
                                    Debug.LogWarning("TODO");
                                }
#endif

                            }

                        }

                        if (this.hasError())
                        {
                            break;
                        }

                    }

                    // group
                    {

                        // loadAbStartupContentsWww, loadAbStartupContentsUwr
                        {

                            if (group is AbStartupContentsGroupWww)
                            {
                                yield return this.loadAbStartupContentsWww(group as AbStartupContentsGroupWww);
                            }

                            else if (group is AbStartupContentsGroupUwr)
                            {
                                yield return this.loadAbStartupContentsUwr(group as AbStartupContentsGroupUwr);
                            }

#if UNITY_EDITOR
                            else
                            {
                                Debug.LogWarning("TODO");
                            }
#endif

                            // unloadAssetBundle
                            {
                                group.unloadAssetBundle(false);
                            }
                            
                        }

                        if (this.hasError())
                        {
                            break;
                        }

                    }

                    // Dequeue
                    {
                        if (!this.hasError())
                        {
                            this.m_runtimeQueue.Dequeue();
                        }
                    }

                } // while (this.m_runtimeQueue.Count > 0)

            }

            // finish
            {

                if (this.hasError())
                {
                    DialogManager.Instance.showYesNoDialog(
                        this.createErrorMessage(),
                        this.retryRuntime,
                        this.backToTileBecauseOfRuntimeError
                    );
                }

                else
                {

                    // removeLockFromBefore
                    {
                        SceneChangeManager.Instance.removeLockFromBefore(this);
                    }

                    // m_runtimeLoading
                    {
                        this.m_runtimeLoading = null;
                    }

                }

            }

        }

    }

}
