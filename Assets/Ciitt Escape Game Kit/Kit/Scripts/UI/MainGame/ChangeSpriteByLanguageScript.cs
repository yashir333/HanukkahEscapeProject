using System.Collections;
using System.Collections.Generic;
using SSC;
using UnityEngine;
using UnityEngine.UI;

namespace ciitt.EscapeGameKit
{

    /// <summary>
    /// Change Sprite by language
    /// </summary>
    [RequireComponent(typeof(Image))]
    public class ChangeSpriteByLanguageScript : MonoBehaviour
    {

        /// <summary>
        /// LanguageAndSprites
        /// </summary>
        [SerializeField]
        [Tooltip("LanguageAndSprites")]
        LanguageAndSprites m_languageAndSprites = new LanguageAndSprites(null);

        /// <summary>
        /// Reference to Image
        /// </summary>
        Image m_refImage = null;

        /// <summary>
        /// Start
        /// </summary>
        // ----------------------------------------------------------------------------------------------
        void Start()
        {

            // m_refImage
            {
                this.m_refImage = this.GetComponent<Image>();
            }

            // CustomReduxManager
            {
                CustomReduxManager.CustomReduxManagerInstance.addSceneChangeStateReceiver(this.onSceneChangeStateReceiver);
                CustomReduxManager.CustomReduxManagerInstance.addChangeLanguageSignalReceiver(this.onChangeLanguageSignalReceiver);
            }

            // m_languageAndSprites
            {
                this.m_languageAndSprites.initDictionary(this.transform);
            }

        }

        /// <summary>
        /// Set Sprite
        /// </summary>
        // ------------------------------------------------------------------------------------------
        void setSpriteByLanguage()
        {

            if (this.m_languageAndSprites.languageAndValueDictionary.ContainsKey(SystemManager.Instance.configDataSO.systemLanguage))
            {
                this.m_refImage.sprite = this.m_languageAndSprites.languageAndValueDictionary[SystemManager.Instance.configDataSO.systemLanguage];
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
                this.setSpriteByLanguage();
            }

        }

        /// <summary>
        /// ChangeLanguageSignal receiver
        /// </summary>
        /// <param name="clSignal">ChangeLanguageSignal</param>
        // ------------------------------------------------------------------------------------------
        void onChangeLanguageSignalReceiver(ChangeLanguageSignal clSignal)
        {
            this.setSpriteByLanguage();
        }

    }

}
