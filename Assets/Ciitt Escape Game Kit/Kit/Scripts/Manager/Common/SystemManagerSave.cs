#pragma warning disable 0618

using System;
using System.Collections;
using System.Collections.Generic;
using SSC;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Text;

#if UNITY_EDITOR

using System.Linq;

#endif

namespace ciitt.EscapeGameKit
{

    /// <summary>
    /// SystemManager for save
    /// </summary>
    public partial class SystemManager : SingletonMonoBehaviour<SystemManager>
    {

        /// <summary>
        /// Save config data
        /// </summary>
        // -------------------------------------------------------------------------------------
        public void saveConfigData()
        {
            this.saveDataToPlayerPrefs(this.configFilePath(), this.m_configDataSO);
        }

        /// <summary>
        /// Save current user progress data
        /// </summary>
        // -------------------------------------------------------------------------------------
        public void saveCurrentUserProgressData()
        {

            // saveConfigData
            {
                this.saveConfigData();
            }

            // clear
            {
                //this.m_userProgressData = ScriptableObject.CreateInstance<UserProgressDataSO>();
                this.m_userProgressData.clearData();
            }

            // UserProgressDataSignalWatcher
            {

                CustomReduxManager.CustomReduxManagerInstance.UserProgressDataSignalWatcher.state().sendCreateSignal(
                    CustomReduxManager.CustomReduxManagerInstance.UserProgressDataSignalWatcher,
                    this.addDataToCurrentUserProgressData
                    );

            }

            // set
            {

                this.m_userProgressData.dataVersion = this.m_userProgressDataVersion;
                this.m_userProgressData.sceneName = SceneManager.GetActiveScene().name;
                this.m_userProgressData.dateTimeStr = DateTime.Now.ToString(DateTimeFormat);

                this.m_userProgressData.initDictionary(this.m_userProgressData.dataVersion);

            }

            // saveDataToPlayerPrefs
            {

                ErrorCode ec = this.saveDataToPlayerPrefs(this.dataPath(this.m_userProgressDataId), this.m_userProgressData);

                if (ec == ErrorCode.Success)
                {
                    ec = this.saveDataToPlayerPrefs(this.configFilePath(), this.m_configDataSO);
                }

                if (ec == ErrorCode.Success)
                {
                    CustomUiManager.CustomUiManagerInstance.showTempMessageUi("רמשנ");
                }

                else
                {
                    CustomUiManager.CustomUiManagerInstance.showTempMessageUi(ec.ToString()+" :האיגש");
                }

            }

        }

        /// <summary>
        /// Save data to PlayerPrefs
        /// </summary>
        /// <param name="filePath">file path</param>
        /// <param name="data">data</param>
        // -------------------------------------------------------------------------------------
        protected virtual ErrorCode saveDataToPlayerPrefs(string filePath, System.Object data)
        {

            if (string.IsNullOrEmpty(filePath))
            {
                return ErrorCode.InvalidFilePath;
            }

            if (data == null)
            {
                return ErrorCode.EmptyData;
            }

            // -----------------

            ErrorCode ret = ErrorCode.Unknown;

            string key = Path.GetFileNameWithoutExtension(filePath);
            string json = JsonUtility.ToJson(data);

            try
            {

                if (this.m_usePasswordForData)
                {

                    ret = ErrorCode.FailedEncryptTextData;

                    byte[] bytes = this.encryptTextData(json, this.m_passwordForData);

                    string str = Convert.ToBase64String(bytes);

                    ret = ErrorCode.FailedPlayerPrefs;

                    PlayerPrefs.SetString(key, str);

                }

                else
                {
                    ret = ErrorCode.FailedPlayerPrefs;
                    PlayerPrefs.SetString(key, Convert.ToBase64String(UTF8Encoding.UTF8.GetBytes(json)));
                }

                ret = ErrorCode.Success;

            }

            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }

            return ret;

        }

        /// <summary>
        /// Encrypt text data
        /// </summary>
        /// <param name="data">text data</param>
        /// <param name="pass">pass</param>
        /// <returns>encrypted data</returns>
        // -------------------------------------------------------------------------------------
        protected virtual byte[] encryptTextData(string data, string pass)
        {

            if (this.m_cryptoVersion == SSC.CryptoVersion.Ver1_deprecated)
            {
                return SSC.Funcs.EncryptTextData(data, pass);
            }

            else if (this.m_cryptoVersion == SSC.CryptoVersion.Ver2)
            {
                return SSC.Funcs.EncryptTextData2(data, pass);
            }

            return SSC.Funcs.EncryptTextData(data, pass);

        }

        /// <summary>
        /// Add data to current user progress data
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="json">data</param>
        // -------------------------------------------------------------------------------------
        public void addDataToCurrentUserProgressData(string key, string json)
        {

            if (!string.IsNullOrEmpty(key))
            {

#if UNITY_EDITOR

                if (this.m_userProgressData.dataList.Any(val => val.key == key))
                {
                    Debug.LogError("(#if UNITY_EDITOR) dataList already contains a key : " + key);
                }

#endif

                this.m_userProgressData.dataList.Add(new KeyAndData(key, json));

            }

        }

    }

}
