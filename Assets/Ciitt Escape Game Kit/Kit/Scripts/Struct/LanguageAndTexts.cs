using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ciitt.EscapeGameKit
{

    /// <summary>
    /// SystemLanguage and text
    /// </summary>
    [Serializable]
    public class LanguageAndTexts
    {

        /// <summary>
        /// SystemLanguage and text dictionary
        /// </summary>
        public Dictionary<SystemLanguage, string> languageAndTextDictionary = new Dictionary<SystemLanguage, string>();

        /// <summary>
        /// Languaget and its text
        /// </summary>
        [SerializeField]
        [Tooltip("Languaget and its text")]
        protected List<LanguageAndTextStruct> languageAndTexts = null;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="baseString">base string</param>
        public LanguageAndTexts(string baseString)
        {
            this.languageAndTexts = LanguageAndTextStruct.createDefaultList(baseString);
        }

        /// <summary>
        /// Init dictionary
        /// </summary>
        public void initDictionary(Transform trans)
        {

            foreach (var val in this.languageAndTexts)
            {

                if (!this.languageAndTextDictionary.ContainsKey(val.systemLanguage))
                {
                    this.languageAndTextDictionary.Add(val.systemLanguage, val.text);
                }

                else
                {
                    Debug.LogWarning("Already contains key : " + val.systemLanguage.ToString());
                }

            }

#if UNITY_EDITOR

            string transName = trans ? Funcs.createHierarchyPath(trans) : "";

            if (this.languageAndTextDictionary.Count <= 0)
            {
                Debug.LogWarning("(#if UNITY_EDITOR) languageAndTextDictionary is empty : " + transName);
            }

            else
            {

                foreach (var val in SystemManager.Instance.availableLanguageList)
                {

                    if (!this.languageAndTextDictionary.ContainsKey(val))
                    {
                        Debug.LogError("(#if UNITY_EDITOR) languageAndTextDictionary does not contain " + val.ToString() + " : " + transName);
                    }

                }

            }

#endif

        }

        /// <summary>
        /// Get text
        /// </summary>
        public string getText(SystemLanguage sl)
        {

            if(this.languageAndTextDictionary.ContainsKey(sl))
            {
                return this.languageAndTextDictionary[sl];
            }

            return "";

        }

    }

    /// <summary>
    /// SystemLanguage and text
    /// </summary>
    [Serializable]
    public class LanguageAndTextStruct
    {

        /// <summary>
        /// SystemLanguage
        /// </summary>
        public SystemLanguage systemLanguage = SystemLanguage.Unknown;

        /// <summary>
        /// Text
        /// </summary>
        public string text = "";

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="_systemLanguage">systemLanguage</param>
        /// <param name="_text">text</param>
        public LanguageAndTextStruct(SystemLanguage _systemLanguage, string _text)
        {
            this.systemLanguage = _systemLanguage;
            this.text = _text;
        }

        /// <summary>
        /// Default list
        /// </summary>
        /// <returns>list</returns>
        public static List<LanguageAndTextStruct> createDefaultList(string str)
        {

            List<LanguageAndTextStruct> ret = new List<LanguageAndTextStruct>();

            SystemLanguage previous = SystemLanguage.Unknown;

            foreach (var val in Enum.GetValues(typeof(SystemLanguage)))
            {

                if (previous != (SystemLanguage)val)
                {
                    ret.Add(new LanguageAndTextStruct((SystemLanguage)val, str));
                }

                previous = (SystemLanguage)val;

            }

            return ret;

        }

    }

}
