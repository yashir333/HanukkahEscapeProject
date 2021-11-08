#pragma warning disable 0618

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using SSC;
using UnityEngine;

namespace ciitt.EscapeGameKit
{

    /// <summary>
    /// System manager
    /// </summary>
    public partial class SystemManager : SingletonMonoBehaviour<SystemManager>
    {

        /// <summary>
        /// DateTime format
        /// </summary>
        public static string DateTimeFormat = "yyyy/MM/dd HH:mm:ss";

        /// <summary>
        /// Targe frame rate
        /// </summary>
        [SerializeField]
        [Tooltip("Targe frame rate")]
        protected int m_targetFrameRate = 60;

        /// <summary>
        /// Use password for data
        /// </summary>
        [SerializeField]
        [Tooltip("Use password for data")]
        protected bool m_usePasswordForData = true;

        /// <summary>
        /// Crypto version when m_usePasswordForData is true
        /// </summary>
        [SerializeField]
        [Tooltip("Crypto version when m_usePasswordForData is true")]
        protected CryptoVersion m_cryptoVersion = CryptoVersion.Ver1_deprecated;

        /// <summary>
        /// Password for data
        /// </summary>
        [SerializeField]
        [Tooltip("Password for data")]
        protected string m_passwordForData = "";

        /// <summary>
        /// User progress data file name prefix
        /// </summary>
        [SerializeField]
        [Tooltip("User progress data file name prefix")]
        protected string m_userProgressDataFileNamePrefix = "data_";

        /// <summary>
        /// Data file extension
        /// </summary>
        [SerializeField]
        [Tooltip("Data file extension")]
        protected string m_dataFileExtension = "dat";

        /// <summary>
        /// Default config data
        /// </summary>
        [SerializeField]
        [Tooltip("Default config data")]
        protected ConfigDataSO m_configDataSO;

        /// <summary>
        /// User progress data version
        /// </summary>
        [SerializeField]
        [Tooltip("User progress data version")]
        protected int m_userProgressDataVersion = 1;

        /// <summary>
        /// Available language list
        /// </summary>
        [SerializeField]
        [Tooltip("Available language list")]
        List<SystemLanguage> m_availableLanguageList = new List<SystemLanguage>();

        /// <summary>
        /// UserProgressDataSO
        /// </summary>
        protected UserProgressDataSO m_userProgressData = null;

        /// <summary>
        /// Startup done
        /// </summary>
        protected bool m_startupDone = false;

        /// <summary>
        /// Data id
        /// </summary>
        protected string m_userProgressDataId = "0";

        // -------------------------------------------------------------------------------------

        /// <summary>
        /// Config data
        /// </summary>
        public ConfigDataSO configDataSO { get { return this.m_configDataSO; } }

        /// <summary>
        /// Startup done
        /// </summary>
        public bool isStartupDone { get { return this.m_startupDone; } }

        /// <summary>
        /// Startup done
        /// </summary>
        public UserProgressDataSO userProgressData { get { return this.m_userProgressData; } }

        /// <summary>
        /// Available language list
        /// </summary>
        public List<SystemLanguage> availableLanguageList { get { return this.m_availableLanguageList; } }

        /// <summary>
        /// Called in Awake
        /// </summary>
        // -------------------------------------------------------------------------------------
        protected override void initOnAwake()
        {

#if UNITY_ANDROID || UNITY_IOS
            
            if(QualitySettings.vSyncCount == 0)
            {
                Application.targetFrameRate = this.m_targetFrameRate;
            }

#elif UNITY_WEBGL && UNITY_EDITOR

            if (QualitySettings.vSyncCount == 0)
            {
                Application.targetFrameRate = this.m_targetFrameRate;
            }

#elif UNITY_WEBGL
            
            //
            
#else
            
            if(QualitySettings.vSyncCount == 0)
            {
                Application.targetFrameRate = this.m_targetFrameRate;
            }

#endif

#if UNITY_EDITOR

            if (this.m_usePasswordForData)
            {

                if (this.m_cryptoVersion == CryptoVersion.Ver1_deprecated)
                {
                    Debug.LogWarning(
                        "(#if UNITY_EDITOR) : CryptoVersion.Ver1_deprecated uses old crypto function, please use newer instead : " +
                        SSC.Funcs.CreateHierarchyPath(this.transform));
                }

            }

#endif

        }

        /// <summary>
        /// Start
        /// </summary>
        // -------------------------------------------------------------------------------------
        protected virtual IEnumerator Start()
        {

#if UNITY_EDITOR

            if (this.m_usePasswordForData)
            {
                if (this.m_passwordForData.Length != 16 && this.m_passwordForData.Length != 24 && this.m_passwordForData.Length != 32)
                {
                    Debug.LogError("(#if UNITY_EDITOR) m_passwordForData length must be 16, 24, or 32");
                }
            }

            // m_availableLanguageList
            {

                if (this.m_availableLanguageList.Count <= 0)
                {
                    Debug.LogWarning("(#if UNITY_EDITOR) m_availableLanguageList is empty : " + Funcs.createHierarchyPath(this.transform));
                }

            }

#endif

            // m_configDataSO
            {
                this.readConfigDataFromPlayerPrefs();
            }

            // setSystemLanguage
            {
                this.setSystemLanguage(this.m_configDataSO.systemLanguage);
            }

            // master volume
            {
                AudioListener.volume = this.m_configDataSO.masterVolume01;
            }

            // m_userProgressData
            {

                this.readDataFromPlayerPrefs<UserProgressDataSO>(this.dataPath(this.m_userProgressDataId), false, (result, error) =>
                {
                    this.m_userProgressData = result;
                });

                if (this.m_userProgressData)
                {
                    this.m_userProgressData.initDictionary(this.m_userProgressDataVersion);
                }

            }

            yield return null;

            this.m_startupDone = true;

        }

        /// <summary>
        /// Is continue data available
        /// </summary>
        /// <returns></returns>
        // -------------------------------------------------------------------------------------
        public bool isContinueDataAvailable()
        {
            return
                this.m_userProgressData != null &&
                !this.m_userProgressData.isDefaultData()
                ;
        }

        /// <summary>
        /// User progress data path
        /// </summary>
        /// <param name="dataNumber">data number</param>
        /// <returns>path</returns>
        // -------------------------------------------------------------------------------------
        protected virtual string dataPath(string dataId)
        {

            return
                this.rootDataPath() +
                "/" +
                this.m_userProgressDataFileNamePrefix +
                dataId +
                "." +
                this.m_dataFileExtension
                ;

        }

        /// <summary>
        /// Convert json to data
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="json">json</param>
        /// <returns>data</returns>
        // -------------------------------------------------------------------------------------
        public T convertJson<T>(string json) where T : new()
        {
            return Funcs.convertJson<T>(json, null);
        }

        /// <summary>
        /// Create key path
        /// </summary>
        /// <param name="trans">Transform</param>
        /// <param name="mb"></param>
        /// <returns>key path</returns>
        // -------------------------------------------------------------------------------------
        public string createKeyPath(Transform trans, MonoBehaviour mb)
        {

            if (!trans || !mb)
            {
                return "";
            }

            // ---------------------------------

            return Funcs.createHierarchyPath(trans) + "/" + mb.GetType().Name;

        }

        /// <summary>
        /// Root data path
        /// </summary>
        /// <returns>path</returns>
        // -------------------------------------------------------------------------------------
        public virtual string rootDataPath()
        {
            return Application.persistentDataPath;
        }

        /// <summary>
        /// Config data file path
        /// </summary>
        /// <returns>config data file path</returns>
        // -------------------------------------------------------------------------------------
        protected virtual string configFilePath()
        {
            return this.rootDataPath() + "/config." + this.m_dataFileExtension;
        }

        /// <summary>
        /// Read config data from PlayerPrefs
        /// </summary>
        // -------------------------------------------------------------------------------------
        protected virtual void readConfigDataFromPlayerPrefs()
        {

            this.readDataFromPlayerPrefs<ConfigDataSO>(this.configFilePath(), true, (result, error) =>
            {

                if(result)
                {
                    this.m_configDataSO = result;
                }

                else
                {

                    this.m_configDataSO =
                    (this.m_configDataSO) ?
                    Instantiate(this.m_configDataSO) :
                    ScriptableObject.CreateInstance<ConfigDataSO>();

                }

            });

        }

        /// <summary>
        /// Read ScriptableObject data from PlayerPrefs
        /// </summary>
        /// <typeparam name="T">ScriptableObject</typeparam>
        /// <param name="filePath">data file path</param>
        /// <param name="callback">result callback</param>
        // -------------------------------------------------------------------------------------
        protected virtual void readDataFromPlayerPrefs<T>(string filePath, bool returnNullIfError, Action<T, ErrorCode> callback) where T : ScriptableObject
        {

            ErrorCode ec = ErrorCode.Unknown;

            T result = null;

            string key = Path.GetFileNameWithoutExtension(filePath);

            string prefString = PlayerPrefs.GetString(key);

            if (string.IsNullOrEmpty(prefString))
            {

                if (callback != null)
                {
                    callback(returnNullIfError ? null : ScriptableObject.CreateInstance<T>(), ErrorCode.Success);
                }

                return;

            }

            // --------------------

            try
            {

                if (this.m_usePasswordForData)
                {

                    byte[] temp = System.Convert.FromBase64String(prefString);

                    ec = ErrorCode.FailedDecryptTextData;

                    string json = this.decryptBinaryData(temp, this.m_passwordForData);

                    result = ScriptableObject.CreateInstance<T>();

                    JsonUtility.FromJsonOverwrite(json, result);

                }

                else
                {
                    string json = UTF8Encoding.UTF8.GetString(Convert.FromBase64String(prefString));
                    result = ScriptableObject.CreateInstance<T>();
                    JsonUtility.FromJsonOverwrite(json, result);
                }

                ec = ErrorCode.Success;

            }

            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }

            if (!result)
            {
                result = returnNullIfError ? null : ScriptableObject.CreateInstance<T>();
            }

            if (callback != null)
            {
                callback(result, ec);
            }

        }

        /// <summary>
        /// Decrypt binary data
        /// </summary>
        /// <param name="data">binary data</param>
        /// <param name="pass">pass</param>
        /// <returns>text data</returns>
        // -------------------------------------------------------------------------------------
        protected virtual string decryptBinaryData(byte[] data, string pass)
        {

            if (this.m_cryptoVersion == SSC.CryptoVersion.Ver1_deprecated)
            {
                return SSC.Funcs.DecryptBinaryDataToText(data, pass);
            }

            else if (this.m_cryptoVersion == SSC.CryptoVersion.Ver2)
            {
                return SSC.Funcs.DecryptBinaryDataToText2(data, pass);
            }

            return SSC.Funcs.DecryptBinaryDataToText(data, pass);

        }

        /// <summary>
        /// Set system language
        /// </summary>
        /// <param name="sl">SystemLanguage</param>
        // -------------------------------------------------------------------------------------
        public void setSystemLanguage(SystemLanguage sl)
        {

            this.m_configDataSO.systemLanguage = sl;

            CustomReduxManager.CustomReduxManagerInstance.ChangeLanguageSignalWatcher.state().sendSignal();

        }

        /// <summary>
        /// Switch system language
        /// </summary>
        // -------------------------------------------------------------------------------------
        public void switchSystemLanguage()
        {

            int listCount = this.m_availableLanguageList.Count;

            if(listCount <= 1)
            {
                return;
            }

            // ----------------------

            for (int i = 0; i < listCount; i++)
            {

                if(this.m_availableLanguageList[i] == this.m_configDataSO.systemLanguage)
                {
                    this.setSystemLanguage(this.m_availableLanguageList[(i + 1) % listCount]);
                    return;
                }

            }

        }

    }

}
