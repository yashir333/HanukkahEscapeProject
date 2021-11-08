#pragma warning disable 0618

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SSC
{

    /// <summary>
    /// Class for AssetBundle startup
    /// </summary>
    public partial class AssetBundleStartupManager : SingletonMonoBehaviour<AssetBundleStartupManager>
    {

        /// <summary>
        /// Class for AssetBundle startup base
        /// </summary>
        protected abstract class AbStartupContentsBase : StartupContents
        {

            /// <summary>
            /// Success action 
            /// </summary>
            public Action<AssetBundle> successAction = null;

            /// <summary>
            /// Success detail action 
            /// </summary>
            public Action<AssetBundle, System.Object> successDetailAction = null;

            /// <summary>
            /// Success detail action for asunc
            /// </summary>
            public Action<AssetBundle, System.Object, Action> successDetailActionForAsync = null;

            /// <summary>
            /// Identifier for detail
            /// </summary>
            public System.Object identifierForDetail = null;

            /// <summary>
            /// Progress value function
            /// </summary>
            public Func<float> progressValueFunc = null;

            /// <summary>
            /// Constructor
            /// </summary>
            public AbStartupContentsBase()
            {

            }

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="_successAction">successAction</param>
            public AbStartupContentsBase(
                Action<AssetBundle> _successAction
                )
            {
                this.successAction = _successAction;
            }

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="_successDetailAction">successDetailAction</param>
            /// <param name="_identifierForDetail">identifierForDetail</param>
            public AbStartupContentsBase(
                Action<AssetBundle, System.Object> _successDetailAction,
                System.Object _identifierForDetail
                )
            {
                this.successDetailAction = _successDetailAction;
                this.identifierForDetail = _identifierForDetail;
            }

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="_successDetailActionForAsync">successDetailActionForAsync</param>
            /// <param name="_identifierForDetail">identifierForDetail</param>
            public AbStartupContentsBase(
                Action<AssetBundle, System.Object, Action> _successDetailActionForAsync,
                System.Object _identifierForDetail
                )
            {
                this.successDetailActionForAsync = _successDetailActionForAsync;
                this.identifierForDetail = _identifierForDetail;
            }

        }

        /// <summary>
        /// Class for AssetBundle startup base
        /// </summary>
        protected abstract class AbStartupContentsGroupBase
        {

            /// <summary>
            /// AssetBundle name
            /// </summary>
            public string nameDotVariant = "";

            /// <summary>
            /// AssetBundle
            /// </summary>
            public AssetBundle assetBundle = null;

            /// <summary>
            /// AbStartupContentsBase list
            /// </summary>
            public List<AbStartupContentsBase> absList = new List<AbStartupContentsBase>();

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="_nameDotVariant">nameDotVariant</param>
            public AbStartupContentsGroupBase(string _nameDotVariant)
            {
                this.nameDotVariant = _nameDotVariant;
            }

            /// <summary>
            /// Unload AssetBundle
            /// </summary>
            /// <param name="unloadAllLoadedObjects">unloadAllLoadedObjects</param>
            public void unloadAssetBundle(bool unloadAllLoadedObjects)
            {

                if (this.assetBundle)
                {
                    this.assetBundle.Unload(unloadAllLoadedObjects);
                }

                this.assetBundle = null;

            }

        }

        /// <summary>
        /// The number of parallel loading coroutines
        /// </summary>
        [SerializeField]
        [Tooltip("The number of parallel loading coroutines")]
        protected int m_numberOfCo = 4;

        /// <summary>
        /// Ignore error except manifest
        /// </summary>
        [SerializeField]
        [Tooltip("Ignore error except manifest")]
        protected bool m_ignoreErrorExceptManifest = false;

        /// <summary>
        /// After loading done, redownload manifest and compare new and old.
        /// if deference detected, reload unity scene
        /// </summary>
        [SerializeField]
        [Tooltip("After loading done, redownload a manifest and compare new one with old one, if some differences detected, reload unity scene silently")]
        protected bool m_checkManifestAfterLoading = false;

        /// <summary>
        /// Use decryption
        /// </summary>
        [SerializeField]
        [Tooltip("Use decryption. If you changed this, don't forget to clear cache.")]
        protected bool m_useDecryption = false;

        /// <summary>
        /// Crypto version when m_useDecryption is true
        /// </summary>
        [SerializeField]
        [Tooltip("Crypto version when m_useDecryption is true")]
        protected CryptoVersion m_cryptoVersion = CryptoVersion.Ver1_deprecated;

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
        /// Additive scene progress
        /// </summary>
        protected Dictionary<int, float> m_additiveSceneProgressDict = new Dictionary<int, float>();

        /// <summary>
        /// WaitForSeconds for loading
        /// </summary>
        protected WaitForSeconds m_defaultWaitForSeconds = new WaitForSeconds(0.5f);

        /// <summary>
        /// AbStartupContentsBase dictionary
        /// </summary>
        protected Dictionary<string, AbStartupContentsGroupBase> m_absList = new Dictionary<string, AbStartupContentsGroupBase>();

        /// <summary>
        /// New detected AbStartupContentsBase dictionary
        /// </summary>
        protected Dictionary<string, AbStartupContentsGroupBase> m_newDetected = new Dictionary<string, AbStartupContentsGroupBase>();

        /// <summary>
        ///Dependencies AbStartupContentsBase dictionary
        /// </summary>
        protected Dictionary<string, AbStartupContentsGroupBase> m_dependencies = new Dictionary<string, AbStartupContentsGroupBase>();

        /// <summary>
        /// ThreadPriority
        /// </summary>
        [SerializeField]
        [Tooltip("ThreadPriority")]
        [Obsolete("Obsoleted", false)]
        protected UnityEngine.ThreadPriority m_threadPriority = UnityEngine.ThreadPriority.Low;

        /// <summary>
        /// AbStartupContentsBase list for runtime
        /// </summary>
        protected List<AbStartupContentsBase> m_absListRuntime = new List<AbStartupContentsBase>();

        /// <summary>
        /// override
        /// </summary>
        // -------------------------------------------------------------------------------------------------------
        protected override void initOnAwake()
        {

            this.m_numberOfCo = Math.Max(1, this.m_numberOfCo);

#if UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS)

            if(!SystemInfo.graphicsDeviceType.ToString().ToLower().Contains("opengl"))
            {
                Debug.LogWarning("(#if UNITY_EDITOR) Use OpenGLES, or you will see pink shader, perhaps.");
            }

#endif

#if UNITY_EDITOR

            if (this.m_useDecryption)
            {

                if (this.m_cryptoVersion == CryptoVersion.Ver1_deprecated)
                {
                    Debug.LogWarning(
                        "(#if UNITY_EDITOR) : CryptoVersion.Ver1_deprecated uses old crypto function, please use newer instead : " +
                        Funcs.CreateHierarchyPath(this.transform));
                }

                else
                {
                    Debug.Log("(#if UNITY_EDITOR) : AssetBundleStartupManager's CryptoVersion == " + this.m_cryptoVersion.ToString());
                }

            }

#endif

        }

        /// <summary>
        /// Create name.variant string
        /// </summary>
        /// <returns>name.variant</returns>
        /// <param name="assetBundleName">AssetBundle name</param>
        /// <param name="variant">variant</param>
        // -------------------------------------------------------------------------------------------------------
        protected string createNameDotVariantString(string assetBundleName, string variant)
        {
            return string.IsNullOrEmpty(variant) ? assetBundleName : assetBundleName + "." + variant;
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
        /// Message for failed to get manifest
        /// </summary>
        /// <returns>message</returns>
        // -------------------------------------------------------------------------------------------------------
        protected virtual string messageFailedToGetAssetBundleManifest()
        {
            return "Failed to get AssetBundleManifest";
        }

        /// <summary>
        /// Message for failed to decrypt
        /// </summary>
        /// <returns>message</returns>
        // -------------------------------------------------------------------------------------------------------
        protected virtual string messageFailedToDecryptAssetBundle()
        {
            return "Failed to load AssetBundle";
        }

        /// <summary>
        /// Message for AssetBundle not found
        /// </summary>
        /// <returns>message</returns>
        // -------------------------------------------------------------------------------------------------------
        protected virtual string messageAssetBundleNotFound()
        {
            return "AssetBundle not found";
        }

        /// <summary>
        /// Denominator of progress
        /// </summary>
        /// <returns>value</returns>
        // -------------------------------------------------------------------------------------------------------
        public int progressDenominator()
        {

            int ret = 0;

            // m_absList
            {
                foreach (var group in this.m_absList)
                {
                    ret += group.Value.absList.Count;
                }
            }

            // m_dependencies
            {
                ret += this.m_dependencies.Count;
            }

            // m_additiveSceneProgressDict
            {
                ret += this.m_additiveSceneProgressDict.Count;
            }

            return ret;

        }

        /// <summary>
        /// Numerator of progress
        /// </summary>
        /// <returns>value</returns>
        // -------------------------------------------------------------------------------------------------------
        public float progressNumerator()
        {

            float ret = 0.0f;

            // old
            {

                foreach (var group in this.m_absList)
                {

                    foreach (var abs in group.Value.absList)
                    {

                        if (abs.currentWorkingState == StartupContents.WorkingState.DoneSuccessOrError)
                        {
                            ret += 1.0f;
                        }

                        else if (abs.progressValueFunc != null)
                        {
                            ret += abs.progressValueFunc();
                        }

                    }

                }

                foreach (var group in this.m_dependencies)
                {

                    foreach (var abs in group.Value.absList)
                    {

                        if (abs.currentWorkingState == StartupContents.WorkingState.DoneSuccessOrError)
                        {
                            ret += 1.0f;
                        }

                        else if (abs.progressValueFunc != null)
                        {
                            ret += abs.progressValueFunc();
                        }

                    }

                }

            }

            // m_additiveSceneProgressDict
            {

                foreach (var val in this.m_additiveSceneProgressDict)
                {
                    ret += val.Value;
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

            if (this.m_newDetected.Count > 0)
            {
                return true;
            }

            foreach (var val in this.m_absList)
            {
                if (val.Value.absList.Find(x => x.currentWorkingState == StartupContents.WorkingState.NotYet) != null)
                {
                    return true;
                }
            }

            return false;

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
                this.m_messages.mainMessage = lm.getFormattedString(LanguageManager.SSCLanguageKeys.Error_AssetBundle_Startup, errorMessage);
                this.m_messages.subMessage = lm.getFormattedString(LanguageManager.SSCLanguageKeys.Dialog_Sub_Retry);
                this.m_messages.urlIfNeeded = errorUrl;

            }

            else
            {
                this.m_messages.title = "AssetBundle Error";
                this.m_messages.mainMessage = errorMessage;
                this.m_messages.subMessage = "Retry ?";
                this.m_messages.urlIfNeeded = errorUrl;
            }

            return this.m_messages;

        }

        /// <summary>
        /// Clear contents
        /// </summary>
        /// <param name="unloadManifest">unload manifest</param>
        // -------------------------------------------------------------------------------------------------------
        public void clearContents(bool unloadManifest)
        {

            // m_absList
            {

                foreach (var val in this.m_absList)
                {
                    val.Value.unloadAssetBundle(false);
                }

                this.m_absList.Clear();

            }

            // m_dependencies
            {

                foreach (var val in this.m_dependencies)
                {
                    val.Value.unloadAssetBundle(false);
                }

                this.m_dependencies.Clear();

            }

            // m_runtimeQueue
            {

                //foreach (var val in this.m_runtimeQueue)
                //{
                //    val.unloadAssetBundle(false);
                //}

                //this.m_runtimeQueue.Clear();

            }

            // m_manifestInfo
            {
                this.m_manifestInfo.clear(unloadManifest);
            }

            // m_additiveSceneProgressDict
            {
                this.m_additiveSceneProgressDict.Clear();
            }

            // clearErrorForRestart
            {
                this.clearErrorForRestart();
            }

        }

        /// <summary>
        /// Check if need to reload scene
        /// </summary>
        /// <returns>yes</returns>
        // -------------------------------------------------------------------------------------------------------
        public bool checkIfNeedToReloadScene()
        {

            if (!this.m_checkManifestAfterLoading)
            {
                return false;
            }

            // ------------------

            return this.m_manifestInfo.hasDifferenceBetweenNewAndOld();

        }

        /// <summary>
        /// Check if new manifest detected
        /// </summary>
        /// <returns>IEnumerator</returns>
        // -------------------------------------------------------------------------------------------------------
        public IEnumerator checkNewManifestIfNeeded()
        {

            if (!this.m_checkManifestAfterLoading)
            {
                yield break;
            }

            // ---------------

            // m_oldManifestKeyHashSet
            {
                this.setManifestKeyHashSet(this.m_manifestInfo.oldManifestKeyHashSet);
            }

            // clear
            {
                this.m_manifestInfo.clear(true);
            }

            // downloadManifestUwr
            {
                yield return this.downloadManifestUwr(true);
            }

            // newManifestKeyHashSet
            {
                this.setManifestKeyHashSet(this.m_manifestInfo.newManifestKeyHashSet);
            }

        }

        /// <summary>
        /// Create AssetBundle url
        /// </summary>
        /// <param name="nameDotVariant">AssetBundle name</param>
        /// <returns>url</returns>
        // -------------------------------------------------------------------------------------------------------
        protected virtual string createAssetBundleUrl(string nameDotVariant)
        {
            return this.m_manifestInfo.manifestFolderUrl + nameDotVariant;
        }

        /// <summary>
        /// Decrypt binary data
        /// </summary>
        /// <param name="textAsset">binary data</param>
        /// <returns>decrypted binary data</returns>
        // -------------------------------------------------------------------------------------------------------
        protected virtual byte[] decryptBinaryData(TextAsset textAsset)
        {

#if UNITY_EDITOR

            Debug.LogWarning("(#if UNITY_EDITOR) : You must override [decryptBinaryData] function.");

#endif

            if (!textAsset)
            {
                return new byte[] { };
            }

            if (this.m_cryptoVersion == CryptoVersion.Ver1_deprecated)
            {
                return Funcs.DecryptBinaryData(textAsset.bytes, "PassworDPassworD");
            }

            else if (this.m_cryptoVersion == CryptoVersion.Ver2)
            {
                return Funcs.DecryptBinaryData2(textAsset.bytes, "PassworDPassworD_123");
            }

#if UNITY_EDITOR
            Debug.LogError("Developer Implementation Error in AssetBundleStartupManager.decryptBinaryData");
#endif

            return Funcs.DecryptBinaryData(textAsset.bytes, "PassworDPassworD");

        }

        /// <summary>
        /// Decrypt AssetBundle if needed
        /// </summary>
        /// <param name="assetBundle">AssetBundle</param>
        /// <param name="ret">return function</param>
        /// <returns>IEnumerator</returns>
        // -------------------------------------------------------------------------------------------------------
        protected IEnumerator decryptAssetBundleIfNeeded(AssetBundle assetBundle, Action<AssetBundle> ret)
        {

            yield return null;

            if (!assetBundle)
            {
                yield break;
            }

            // ------------------

            if (this.m_useDecryption)
            {

                string[] assetNames = assetBundle.GetAllAssetNames();
                string assetName = (assetNames.Length > 0) ? assetNames[0] : "";

                if (string.IsNullOrEmpty(assetName))
                {
                    yield break;
                }

                byte[] decrypted = this.decryptBinaryData(assetBundle.LoadAsset<TextAsset>(assetName));

                yield return null;

                if (decrypted != null && decrypted.Length > 0)
                {

                    AssetBundleCreateRequest abcr = AssetBundle.LoadFromMemoryAsync(decrypted);

                    if (abcr != null)
                    {
                        yield return abcr;
                        ret(abcr.assetBundle);
                        assetBundle.Unload(true);
                    }

                }

                else
                {
                    assetBundle.Unload(true);
                }

            }

            else
            {
                ret(assetBundle);
            }

        }

        /// <summary>
        /// Load additive scene
        /// </summary>
        /// <param name="assetBundle">AssetBundle</param>
        /// <returns>IEnumerator</returns>
        // -------------------------------------------------------------------------------------------------------
        protected IEnumerator loadAdditiveSceneIfNeeded(AssetBundle assetBundle)
        {

            if (!assetBundle)
            {
                yield break;
            }

            // ----------------

            int progressId = 0;

            foreach (string str in assetBundle.GetAllScenePaths())
            {

                var aoForAdditive = SceneManager.LoadSceneAsync(Path.GetFileNameWithoutExtension(str), LoadSceneMode.Additive);

                progressId = this.m_additiveSceneProgressDict.Count;

                if (!this.m_additiveSceneProgressDict.ContainsKey(progressId))
                {
                    this.m_additiveSceneProgressDict.Add(progressId, 0.0f);
                }

                if (aoForAdditive != null)
                {

                    while (!aoForAdditive.isDone)
                    {
                        this.m_additiveSceneProgressDict[progressId] = aoForAdditive.progress;
                        yield return null;
                    }

                    this.m_additiveSceneProgressDict[progressId] = 1.0f;

                }

            }

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

            // m_manifestInfo
            {
                this.m_manifestInfo.clear(false);
            }

            // m_absList
            {

                foreach (var absGroup in this.m_absList)
                {

                    foreach (var abs in absGroup.Value.absList)
                    {

                        if (!string.IsNullOrEmpty(abs.errorMessage))
                        {
                            abs.currentWorkingState = StartupContents.WorkingState.NotYet;
                        }

                        abs.errorMessage = "";

                    }

                }

            }

            // m_dependencies
            {

                foreach (var absGroup in this.m_dependencies)
                {

                    foreach (var abs in absGroup.Value.absList)
                    {

                        if (!string.IsNullOrEmpty(abs.errorMessage))
                        {
                            abs.currentWorkingState = StartupContents.WorkingState.NotYet;
                        }

                        abs.errorMessage = "";

                    }

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

            if (startupContents != this.m_manifestInfo.dummyStartup && this.m_ignoreErrorExceptManifest)
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
        /// Merge dictionaries
        /// </summary>
        /// <param name="into">into</param>
        /// <param name="merge">maege this into</param>
        [Obsolete("Use mergeDictionariesUwr", false)]
        // -------------------------------------------------------------------------------------------------------
        protected void mergeDictionaries(Dictionary<string, AbStartupContentsGroupBase> into, Dictionary<string, AbStartupContentsGroupBase> merge)
        {

            foreach (var mergeKv in merge)
            {

                if (into.ContainsKey(mergeKv.Key))
                {
                    into[mergeKv.Key].absList.AddRange(mergeKv.Value.absList);
                }

                else
                {
                    into.Add(mergeKv.Key, mergeKv.Value);
                }

            }

        }

        /// <summary>
        /// Start AB loadings
        /// </summary>
        /// <returns>IEnumerator</returns>
        // -------------------------------------------------------------------------------------------------------
        public IEnumerator startAbStartup()
        {

            if (!this.hasNotYetContent())
            {
                yield break;
            }

            // ----------------

            // clearErrorForRestart
            {
                this.clearErrorForRestart();
            }

            // mergeDictionaries
            {
                this.mergeDictionaries(this.m_absList, this.m_newDetected);
                this.m_newDetected.Clear();
            }

            // setManifestFileAndFolderUrl
            {
                this.setManifestFileAndFolderUrl();
            }

            // downloadManifest
            {

                if (!this.m_manifestInfo.manifest)
                {
                    yield return this.downloadManifest(false);
                }

                if (this.hasError())
                {
                    yield break;
                }

            }

            // addAllDependencies
            {
                yield return this.addAllDependencies();
            }

            // -----------------

            // m_dependencies
            {

                for (int i = 0; i < this.m_numberOfCo; i++)
                {
                    StartCoroutine(this.startAbStartupSub(this.m_dependencies));
                }

                // wait 1 frame
                {
                    yield return null;
                }

                // wait coroutines
                {

                    foreach (var key in this.m_dependencies.Keys)
                    {
                        while (this.m_dependencies[key].absList.Find(x => x.currentWorkingState == StartupContents.WorkingState.NowWorking) != null)
                        {
                            yield return this.m_defaultWaitForSeconds;
                        }
                    }

                }

                if (this.hasError())
                {
                    yield break;
                }

            }

            // m_absList
            {

                for (int i = 0; i < this.m_numberOfCo; i++)
                {
                    StartCoroutine(this.startAbStartupSub(this.m_absList));
                }

                // wait 1 frame
                {
                    yield return null;
                }

                // wait coroutines
                {

                    foreach (var key in this.m_absList.Keys)
                    {
                        while (this.m_absList[key].absList.Find(x => x.currentWorkingState == StartupContents.WorkingState.NowWorking) != null)
                        {
                            yield return this.m_defaultWaitForSeconds;
                        }
                    }

                }

                if (this.hasError())
                {
                    yield break;
                }

            }

        }

        /// <summary>
        /// Start AB loadings sub
        /// </summary>
        /// <param name="groupDict">group dictionary</param>
        /// <returns>IEnumerator</returns>
        // -------------------------------------------------------------------------------------------------------
        protected IEnumerator startAbStartupSub(Dictionary<string, AbStartupContentsGroupBase> groupDict)
        {

            foreach (var key in groupDict.Keys)
            {

                // hasError break
                {
                    if (this.hasError())
                    {
                        break;
                    }
                }

                // ----------------------

                // continue if not NotYet
                {

                    if (groupDict[key].absList.Find(x => x.currentWorkingState == StartupContents.WorkingState.NotYet) == null)
                    {
                        continue;
                    }

                }

                // NowWorking
                {
                    foreach (var abs in groupDict[key].absList)
                    {
                        if (abs.currentWorkingState == StartupContents.WorkingState.NotYet)
                        {
                            abs.currentWorkingState = StartupContents.WorkingState.NowWorking;
                        }
                    }
                }

                // loadAbStartupContentsWww, loadAbStartupContentsUwr
                {

                    if (groupDict[key] is AbStartupContentsGroupWww)
                    {
                        yield return this.loadAbStartupContentsWww(groupDict[key] as AbStartupContentsGroupWww);
                    }

                    else if (groupDict[key] is AbStartupContentsGroupUwr)
                    {
                        yield return this.loadAbStartupContentsUwr(groupDict[key] as AbStartupContentsGroupUwr);
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
                    foreach (var abs in groupDict[key].absList)
                    {
                        abs.currentWorkingState = StartupContents.WorkingState.DoneSuccessOrError;
                    }
                }

            }

        }

        /// <summary>
        /// Find already loaded AssetBundle
        /// </summary>
        /// <param name="nameDotVariant">AssetBundle name</param>
        /// <returns>AssetBundle</returns>
        // -------------------------------------------------------------------------------------------------------
        protected AssetBundle findAlreadyLoadedAssetBundle(string nameDotVariant)
        {

            if (this.m_absList.ContainsKey(nameDotVariant) && this.m_absList[nameDotVariant].assetBundle)
            {
                return this.m_absList[nameDotVariant].assetBundle;
            }

            else if (this.m_dependencies.ContainsKey(nameDotVariant) && this.m_dependencies[nameDotVariant].assetBundle)
            {
                return this.m_dependencies[nameDotVariant].assetBundle;
            }

            return null;

        }

    }

}
