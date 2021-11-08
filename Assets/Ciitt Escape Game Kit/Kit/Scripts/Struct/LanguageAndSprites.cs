using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ciitt.EscapeGameKit
{

    /// <summary>
    /// SystemLanguage and Sprite
    /// </summary>
    [Serializable]
    public class LanguageAndSprites
    {

        /// <summary>
        /// SystemLanguage and value dictionary
        /// </summary>
        public Dictionary<SystemLanguage, Sprite> languageAndValueDictionary = new Dictionary<SystemLanguage, Sprite>();

        /// <summary>
        /// Languaget and its text
        /// </summary>
        [SerializeField]
        [Tooltip("Languaget and its value")]
        protected List<LanguageAndSpriteStruct> languageAndValues = null;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="baseValue">base value</param>
        public LanguageAndSprites(Sprite baseValue)
        {
            this.languageAndValues = LanguageAndSpriteStruct.createDefaultList(baseValue);
        }

        /// <summary>
        /// Init dictionary
        /// </summary>
        public void initDictionary(Transform trans)
        {

            foreach (var val in this.languageAndValues)
            {

                if (!this.languageAndValueDictionary.ContainsKey(val.systemLanguage))
                {
                    this.languageAndValueDictionary.Add(val.systemLanguage, val.value);
                }

                else
                {
                    Debug.LogWarning("Already contains key : " + val.systemLanguage.ToString());
                }

            }

#if UNITY_EDITOR

            string transName = trans ? Funcs.createHierarchyPath(trans) : "";

            if (this.languageAndValueDictionary.Count <= 0)
            {
                Debug.LogWarning("(#if UNITY_EDITOR) languageAndValueDictionary is empty : " + transName);
            }

            else
            {

                foreach (var val in SystemManager.Instance.availableLanguageList)
                {

                    if (!this.languageAndValueDictionary.ContainsKey(val))
                    {
                        Debug.LogError("(#if UNITY_EDITOR) languageAndValueDictionary does not contain " + val.ToString() + " : " + transName);
                    }

                }

            }

#endif

        }

        /// <summary>
        /// Get value
        /// </summary>
        public Sprite getValue(SystemLanguage sl)
        {

            if (this.languageAndValueDictionary.ContainsKey(sl))
            {
                return this.languageAndValueDictionary[sl];
            }

            return null;

        }

    }

    /// <summary>
    /// SystemLanguage and Sprite
    /// </summary>
    [Serializable]
    public class LanguageAndSpriteStruct
    {

        /// <summary>
        /// SystemLanguage
        /// </summary>
        public SystemLanguage systemLanguage = SystemLanguage.Unknown;

        /// <summary>
        /// Sprite
        /// </summary>
        public Sprite value = null;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="_systemLanguage">systemLanguage</param>
        /// <param name="_value">value</param>
        public LanguageAndSpriteStruct(SystemLanguage _systemLanguage, Sprite _value)
        {
            this.systemLanguage = _systemLanguage;
            this.value = _value;
        }

        /// <summary>
        /// Default list
        /// </summary>
        /// <returns>list</returns>
        public static List<LanguageAndSpriteStruct> createDefaultList(Sprite value)
        {

            List<LanguageAndSpriteStruct> ret = new List<LanguageAndSpriteStruct>();

            SystemLanguage previous = SystemLanguage.Unknown;

            foreach (var val in Enum.GetValues(typeof(SystemLanguage)))
            {

                if (previous != (SystemLanguage)val)
                {
                    ret.Add(new LanguageAndSpriteStruct((SystemLanguage)val, value));
                }

                previous = (SystemLanguage)val;

            }

            return ret;

        }

    }

}
