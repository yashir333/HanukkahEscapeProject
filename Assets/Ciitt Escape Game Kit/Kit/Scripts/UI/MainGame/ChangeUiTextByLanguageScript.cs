using System.Collections;
using System.Collections.Generic;
using SSC;
using UnityEngine;
using UnityEngine.UI;

namespace ciitt.EscapeGameKit
{

    /// <summary>
    /// Change UI text by language
    /// </summary>
    [RequireComponent(typeof(Text))]
    public class ChangeUiTextByLanguageScript : MonoBehaviour
    {

        /// <summary>
        /// LanguageAndTexts
        /// </summary>
        [SerializeField]
        [Tooltip("LanguageAndTexts")]
        LanguageAndTexts m_languageAndTexts = new LanguageAndTexts("");

        /// <summary>
        /// Reference to Text
        /// </summary>
        Text m_refText = null;

        /// <summary>
        /// Start
        /// </summary>
        // ----------------------------------------------------------------------------------------------
        void Start()
        {

            // m_refText
            {
                this.m_refText = this.GetComponent<Text>();
            }

            // CustomReduxManager
            {
                CustomReduxManager.CustomReduxManagerInstance.addSceneChangeStateReceiver(this.onSceneChangeStateReceiver);
                CustomReduxManager.CustomReduxManagerInstance.addChangeLanguageSignalReceiver(this.onChangeLanguageSignalReceiver);
            }

            // m_languageAndTexts
            {
                this.m_languageAndTexts.initDictionary(this.transform);
            }

        }

        /// <summary>
        /// Set text
        /// </summary>
        // ------------------------------------------------------------------------------------------
        void setTextByLanguage()
        {

            if (this.m_languageAndTexts.languageAndTextDictionary.ContainsKey(SystemManager.Instance.configDataSO.systemLanguage))
            {
                this.m_refText.text = this.m_languageAndTexts.languageAndTextDictionary[SystemManager.Instance.configDataSO.systemLanguage];
            }

        }

        /// <summary>
        /// SceneChangeState receiver
        /// </summary>
        /// <param name="scState">SceneChangeState</param>
        // ------------------------------------------------------------------------------------------
        void onSceneChangeStateReceiver(SceneChangeState scState)
        {

            if (scState.stateEnum == SceneChangeState.StateEnum.AllStartupsDone)
            {
                this.setTextByLanguage();
            }

        }

        /// <summary>
        /// ChangeLanguageSignal receiver
        /// </summary>
        /// <param name="clSignal">ChangeLanguageSignal</param>
        // ------------------------------------------------------------------------------------------
        void onChangeLanguageSignalReceiver(ChangeLanguageSignal clSignal)
        {
            this.setTextByLanguage();
        }


    }

}
