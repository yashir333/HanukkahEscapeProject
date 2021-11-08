using System;
using System.Collections.Generic;
using UnityEngine;

namespace ciitt.EscapeGameKit
{

    [Serializable]
    public class KeyAndData : IdentifierInterface
    {

        public string key = "";
        public string stringOrJson = "";

        public string getIdentifier()
        {
            return this.key;
        }

        public KeyAndData(string _key, string _stringOrJson)
        {
            this.key = _key;
            this.stringOrJson = _stringOrJson;
        }

    }

    [Serializable]
    public class UserProgressDataSO : ScriptableObject
    {

        /// <summary>
        /// Data version
        /// </summary>
        [Tooltip("Data version")]
        public int dataVersion = 1;

        /// <summary>
        /// Scene name
        /// </summary>
        [HideInInspector]
        public string sceneName = "";

        /// <summary>
        /// Date time string
        /// </summary>
        [HideInInspector]
        public string dateTimeStr = "";

        /// <summary>
        /// Data list
        /// </summary>
        [HideInInspector]
        public List<KeyAndData> dataList = new List<KeyAndData>();

        /// <summary>
        /// Data dictionary
        /// </summary>
        public Dictionary<string, KeyAndData> dataDict = new Dictionary<string, KeyAndData>();

        /// <summary>
        /// Datetime
        /// </summary>
        protected DateTime m_dateTime = DateTime.MinValue;

        /// <summary>
        /// Clear data
        /// </summary>
        // -----------------------------------------------------------------------------------------
        public virtual void clearData()
        {
            this.sceneName = "";
            this.dateTimeStr = "";
            this.dataList.Clear();
            this.dataDict.Clear();
            this.m_dateTime = DateTime.MinValue;
        }

        /// <summary>
        /// Is default data
        /// </summary>
        /// <returns>is default</returns>
        // -----------------------------------------------------------------------------------------
        public virtual bool isDefaultData()
        {
            return string.IsNullOrEmpty(dateTimeStr);
        }

        /// <summary>
        /// Datetime
        /// </summary>
        /// <returns>DateTime</returns>
        // -----------------------------------------------------------------------------------------
        public virtual DateTime dateTime()
        {

            if (this.m_dateTime == DateTime.MinValue && !string.IsNullOrEmpty(dateTimeStr))
            {

                this.m_dateTime = DateTime.ParseExact(
                    this.dateTimeStr,
                    SystemManager.DateTimeFormat,
                    System.Globalization.CultureInfo.InvariantCulture
                    );

            }

            return this.m_dateTime;

        }

        /// <summary>
        /// Init dictionary
        /// </summary>
        /// <param name="dataVersionFromSystemManager">data version from SystemManager</param>
        // -----------------------------------------------------------------------------------------
        public virtual void initDictionary(int dataVersionFromSystemManager)
        {

            if (this.dataVersion == dataVersionFromSystemManager)
            {
                Funcs.listToDictionary<KeyAndData>(this.dataList, this.dataDict);
            }

            else
            {

#if UNITY_EDITOR

                Debug.LogWarning("Saved data version is old : " + this.dataVersion + " / " + dataVersionFromSystemManager);
#endif

                this.clearData();

            }

#if MY_DEBUG
            
            Debug.Log("initDictionary");

            foreach(var kv in this.dataDict)
            {
                Debug.Log(kv.Value.key + " - " + kv.Value.stringOrJson);
            }

#endif

        }

    }

}
