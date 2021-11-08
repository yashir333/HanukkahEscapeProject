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
        /// Manifest info
        /// </summary>
        [Serializable]
        protected class ManifestInfo
        {

            /// <summary>
            /// Url for manifest file
            /// </summary>
            [NonSerialized]
            public string manifestFileUrl = "";

            /// <summary>
            /// Url for base url
            /// </summary>
            [NonSerialized]
            public string manifestFolderUrl = "";

            /// <summary>
            /// AssetBundle that has manifest
            /// </summary>
            [NonSerialized]
            public AssetBundle manifestAssetBundle = null;

            /// <summary>
            /// Current AssetBundleManifest
            /// </summary>
            [NonSerialized]
            public AssetBundleManifest manifest = null;

            /// <summary>
            /// Dummy startup for error
            /// </summary>
            [NonSerialized]
            public StartupContents dummyStartup = new StartupContents();

            /// <summary>
            /// New manifest hash dictionary
            /// </summary>
            [NonSerialized]
            public Dictionary<string, Hash128> newManifestKeyHashSet = new Dictionary<string, Hash128>();

            /// <summary>
            /// Old manifest hash dictionary
            /// </summary>
            [NonSerialized]
            public Dictionary<string, Hash128> oldManifestKeyHashSet = new Dictionary<string, Hash128>();

            /// <summary>
            /// Datetime manifest saved
            /// </summary>
            [NonSerialized]
            public string datetimeManifestSaved = "";

            /// <summary>
            /// Save manifest file to PlayerPrefs
            /// </summary>
            [Tooltip("Use PlayerPrefs to save and load manifest file")]
            public bool usePlayerPrefsToSaveAndLoadManifest = false;

            /// <summary>
            /// PlayerPrefs key for manifest
            /// </summary>
            [Tooltip("PlayerPrefs key for manifest")]
            public string manifestPlayerPrefsKey = "ABManifest";

            /// <summary>
            /// PlayerPrefs key for datetime
            /// </summary>
            [Tooltip("PlayerPrefs key for datetime")]
            public string datetimePlayerPrefsKey = "ABManifestDatetime";

            /// <summary>
            /// Clear
            /// </summary>
            /// <param name="unloadManifest">unload manifest</param>
            // -------------------------------------------------------------------------------------------------------
            public void clear(bool unloadManifest)
            {

                if (unloadManifest && this.manifestAssetBundle)
                {
                    this.manifestAssetBundle.Unload(true);
                    this.manifestAssetBundle = null;
                    this.manifest = null;
                }

                this.dummyStartup.currentWorkingState = StartupContents.WorkingState.NotYet;
                this.dummyStartup.errorMessage = "";
                this.dummyStartup.urlIfNeeded = "";

            }

            /// <summary>
            /// Has difference
            /// </summary>
            /// <returns>difference detected</returns>
            // -------------------------------------------------------------------------------------------------------
            public bool hasDifferenceBetweenNewAndOld()
            {

                if (this.newManifestKeyHashSet.Count != this.oldManifestKeyHashSet.Count)
                {
                    return false;
                }

                foreach (var newVal in this.newManifestKeyHashSet)
                {

                    if (!this.oldManifestKeyHashSet.ContainsKey(newVal.Key))
                    {
                        return true;
                    }

                    else if (this.oldManifestKeyHashSet[newVal.Key] != newVal.Value)
                    {
                        return true;
                    }

                }

                return false;

            }

            /// <summary>
            /// Load datetime from PlayerPrefs
            /// </summary>
            // -------------------------------------------------------------------------------------------------------
            public void loadDatetimeManifestSavedFromPlayerPrefs()
            {

                if (this.usePlayerPrefsToSaveAndLoadManifest && PlayerPrefs.HasKey(this.datetimePlayerPrefsKey))
                {
                    this.datetimeManifestSaved = PlayerPrefs.GetString(this.datetimePlayerPrefsKey);
                }

            }

            /// <summary>
            /// Save manifest to Playerprefs if needed
            /// </summary>
            /// <param name="wwwBytes">www bytes</param>
            // -------------------------------------------------------------------------------------------------------
            public void saveManifestToPlayerprefsIfNeeded(byte[] wwwBytes)
            {

                if (!this.usePlayerPrefsToSaveAndLoadManifest || wwwBytes == null || wwwBytes.Length <= 0)
                {
                    return;
                }

                // --------------------

                // PlayerPrefs
                {
                    PlayerPrefs.SetString(this.datetimePlayerPrefsKey, DateTime.Now.ToString());
                    PlayerPrefs.SetString(this.manifestPlayerPrefsKey, Convert.ToBase64String(wwwBytes));
                    PlayerPrefs.Save();
                }

            }

            /// <summary>
            /// Load manifest from Playerprefs
            /// </summary>
            /// <returns>www bytes</returns>
            // -------------------------------------------------------------------------------------------------------
            public byte[] loadManifestFromPlayerprefs()
            {

                if (!this.usePlayerPrefsToSaveAndLoadManifest || !PlayerPrefs.HasKey(this.manifestPlayerPrefsKey))
                {
                    return null;
                }

                // -------------------

                string val = PlayerPrefs.GetString(this.manifestPlayerPrefsKey);

                if (!string.IsNullOrEmpty(val))
                {
                    return Convert.FromBase64String(val);
                }

                return null;

            }

        }

        /// <summary>
        /// Manifest info
        /// </summary>
        [SerializeField]
        protected ManifestInfo m_manifestInfo = new ManifestInfo();

        /// <summary>
        /// Set manifest file and base folder url
        /// </summary>
        // -------------------------------------------------------------------------------------------------------
        protected virtual void setManifestFileAndFolderUrl()
        {

            this.setManifestFileAndFolderUrl(
                ref this.m_manifestInfo.manifestFolderUrl,
                ref this.m_manifestInfo.manifestFileUrl
                );

        }

        /// <summary>
        /// Set manifest file and base folder url
        /// </summary>
        /// <param name="manifestFolderUrl">manifest folder url</param>
        /// <param name="manifestFileUrl">manifest file url</param>
        // -------------------------------------------------------------------------------------------------------
        protected virtual void setManifestFileAndFolderUrl(ref string manifestFolderUrl, ref string manifestFileUrl)
        {

#if UNITY_EDITOR

            Debug.LogWarning("(#if UNITY_EDITOR) : You must override [setManifestFileAndFolderUrl(ref, ref)] function.");

#endif
            // sample
            {

                string manifestName = "";

#if UNITY_ANDROID
                
                manifestName = (this.m_useDecryption) ? "android.encrypted.unity3d" : "android.unity3d";

#elif UNITY_IOS
                
                manifestName = (this.m_useDecryption) ? "ios.encrypted.unity3d" : "ios.unity3d";
                
#else

                manifestName = (this.m_useDecryption) ? "windows.encrypted.unity3d" : "windows.unity3d";

#endif

                // endsWith slash
                manifestFolderUrl =
                    "http://localhost:50002/" +
                    manifestName +
                    ((this.m_useDecryption) ? "/encrypted/" : "/")
                    ;

                manifestFileUrl = manifestFolderUrl + manifestName;

            }

        }

        /// <summary>
        /// Set manifest hash
        /// </summary>
        /// <param name="target">target</param>
        // -------------------------------------------------------------------------------------------------------
        protected void setManifestKeyHashSet(Dictionary<string, Hash128> target)
        {

            target.Clear();

            if (this.m_manifestInfo.manifest)
            {
                foreach (string str in this.m_manifestInfo.manifest.GetAllAssetBundles())
                {
                    if (!target.ContainsKey(str))
                    {
                        target.Add(str, this.m_manifestInfo.manifest.GetAssetBundleHash(str));
                    }
                }
            }

        }

        /// <summary>
        /// Should download new manifest
        /// </summary>
        /// <param name="shouldDownload">should download</param>
        /// <returns>IEnumerator</returns>
        // -------------------------------------------------------------------------------------------------------
        protected virtual IEnumerator shouldDownloadNewManifest(Action shouldDownload)
        {

            yield return null;

            if (shouldDownload == null || !this.m_manifestInfo.usePlayerPrefsToSaveAndLoadManifest)
            {
                yield break;
            }

            // -----------------

#if UNITY_EDITOR
            Debug.LogWarning("(#if UNITY_EDITOR) : You should override AssetBundleStartupManager.shouldDownloadNewManifest and use encrypted AssetBundles");
#endif

            // Call [shouldDownload] if you want users to download new manifest file from your server

            // The purpose of this feature is to cache a manifest file to decrease users' download count.

            // -----------------

            DateTime now = DateTime.Now;
            DateTime manifestSaved;

            // -----------------

            if (DateTime.TryParse(this.m_manifestInfo.datetimeManifestSaved, out manifestSaved))
            {

                if ((now - manifestSaved).TotalHours >= 1.0)
                {
                    shouldDownload();
                }

            }

        }

        /// <summary>
        /// Load manifest
        /// </summary>
        /// <returns>IEnumerator</returns>
        /// <param name="assetBundle">AssetBundle</param>
        // -------------------------------------------------------------------------------------------------------
        protected IEnumerator loadManifest(AssetBundle assetBundle)
        {

            if (assetBundle)
            {

                yield return this.decryptAssetBundleIfNeeded(assetBundle, (ab) =>
                {
                    this.m_manifestInfo.manifestAssetBundle = ab;
                });

                if (this.m_manifestInfo.manifestAssetBundle)
                {

                    AssetBundleRequest request =
                        this.m_manifestInfo.manifestAssetBundle.LoadAssetAsync("AssetBundleManifest", typeof(AssetBundleManifest));

                    if (request != null)
                    {

                        yield return request;

                        this.m_manifestInfo.manifest = request.asset as AssetBundleManifest;

                        if (!this.m_manifestInfo.manifest)
                        {
                            this.m_manifestInfo.dummyStartup.errorMessage = "AssetBundleRequest.asset isn't AssetBundleManifest.";
                            this.updateError(this.m_manifestInfo.dummyStartup);
                        }

                    }

                    else
                    {
                        this.m_manifestInfo.dummyStartup.errorMessage = this.messageFailedToGetAssetBundleManifest();
                        this.updateError(this.m_manifestInfo.dummyStartup);
                    }

                } // if (this.m_manifestInfo.manifestAssetBundle)

                else
                {
                    this.m_manifestInfo.dummyStartup.errorMessage = this.messageFailedToDecryptAssetBundle();
                    this.updateError(this.m_manifestInfo.dummyStartup);
                }

            } // if (assetBundle)

            else
            {
                this.m_manifestInfo.dummyStartup.errorMessage = this.messageAssetBundleNotFound();
                this.updateError(this.m_manifestInfo.dummyStartup);
            }

        }

        /// <summary>
        /// Load manifest from PlayerPrefs
        /// </summary>
        /// <param name="doYieldBreak">do yield break</param>
        /// <returns>IEnumerator</returns>
        // -------------------------------------------------------------------------------------------------------
        protected IEnumerator loadManifestFromPlayerPrefs(Action doYieldBreak)
        {

            // loadDatetimeManifestSavedFromPlayerPrefs
            {
                this.m_manifestInfo.loadDatetimeManifestSavedFromPlayerPrefs();
            }

            // ------------------

            bool shouldDownload = false;

            // ------------------

            // shouldDownloadNewManifest
            {
                yield return this.shouldDownloadNewManifest(() =>
                {
                    shouldDownload = true;
                });
            }

            // break
            {
                if (shouldDownload)
                {
                    yield break;
                }
            }

            // loadManifestFromPlayerprefs
            {

                byte[] wwwBytes = this.m_manifestInfo.loadManifestFromPlayerprefs();

                if (wwwBytes != null && wwwBytes.Length > 0)
                {

                    AssetBundleCreateRequest crequest = AssetBundle.LoadFromMemoryAsync(wwwBytes);

                    if (crequest != null)
                    {

                        yield return crequest;

                        if (crequest.assetBundle)
                        {
                            yield return this.loadManifest(crequest.assetBundle);
                        }

                    }

                }

                if (this.m_manifestInfo.manifest && doYieldBreak != null)
                {
#if UNITY_EDITOR
                    Debug.Log("(#if UNITY_EDITOR) : Loaded a manifest file from PlayerPrefs");
#endif
                    doYieldBreak();
                }

            }

        }

        /// <summary>
        /// Download manifest
        /// </summary>
        /// <param name="forcibly">forcibly</param>
        /// <returns>IEnumerator</returns>
        // -------------------------------------------------------------------------------------------------------
        protected IEnumerator downloadManifestUwr(bool forcibly)
        {

            if (string.IsNullOrEmpty(this.m_manifestInfo.manifestFileUrl))
            {
#if UNITY_EDITOR
                Debug.LogWarning("(#if UNITY_EDITOR) : manifestFileUrl is empty");
#endif
                yield break;
            }

            // -----------------

            // loadManifestFromPlayerPrefs
            {

                if (!forcibly && this.m_manifestInfo.usePlayerPrefsToSaveAndLoadManifest)
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

            using (UnityWebRequest uwr = UnityWebRequest.Get(this.m_manifestInfo.manifestFileUrl))
            {

                // -------------------------

#if UNITY_2017_2_OR_NEWER
                UnityWebRequestAsyncOperation ao = uwr.SendWebRequest();
#else
                AsyncOperation ao = uwr.Send();
#endif

                // -------------------------

                // set dummyStartup
                {
                    this.m_manifestInfo.dummyStartup.currentWorkingState = StartupContents.WorkingState.NowWorking; // (meaningless)
                    this.m_manifestInfo.dummyStartup.urlIfNeeded = uwr.url;
                }

                // wait ao done
                {

                    while (!ao.isDone)
                    {

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
                                    this.m_manifestInfo.dummyStartup.errorMessage = this.messageTimeout();
                                    break;
                                }

                            }

                        }

                        yield return null;

                    } // while (!ao.isDone)

                    yield return null;

                }

                // success or fail
                {

                    // set errorMessage
                    {
                        if (string.IsNullOrEmpty(this.m_manifestInfo.dummyStartup.errorMessage))
                        {
                            this.m_manifestInfo.dummyStartup.errorMessage = uwr.error;
                        }
                    }

                    // success
                    if (string.IsNullOrEmpty(this.m_manifestInfo.dummyStartup.errorMessage))
                    {

                        AssetBundleCreateRequest crequest = AssetBundle.LoadFromMemoryAsync(uwr.downloadHandler.data);

                        if (crequest != null)
                        {

                            yield return crequest;

                            if (crequest.assetBundle)
                            {

                                yield return this.loadManifest(crequest.assetBundle);

                                if (this.m_manifestInfo.manifest && uwr.downloadHandler != null)
                                {
                                    this.m_manifestInfo.saveManifestToPlayerprefsIfNeeded(uwr.downloadHandler.data);
                                }

                            }

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

            } // using (UnityWebRequest uwr = UnityWebRequest.Get(this.m_manifestInfo.manifestFileUrl))

        }

    }

}
