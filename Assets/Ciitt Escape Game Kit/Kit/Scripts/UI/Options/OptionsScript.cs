using System.Collections;
using System.Collections.Generic;
using SSC;
using UnityEngine;
using UnityEngine.UI;

namespace ciitt.EscapeGameKit
{

    /// <summary>
    /// Options
    /// </summary>
    public class OptionsScript : MonoBehaviour
    {

        /// <summary>
        /// Reference to sound Slider
        /// </summary>
        [SerializeField]
        [Tooltip("Reference to sound Slider")]
        Slider m_refSlider = null;

        /// <summary>
        /// Reference to rotation flip Image
        /// </summary>
        [SerializeField]
        [Tooltip("Reference to rotation flip Image")]
        Image m_refRotationFlipImage = null;

        /// <summary>
        /// On Sprite
        /// </summary>
        [SerializeField]
        [Tooltip("On Sprite")]
        Sprite m_onSprite = null;

        /// <summary>
        /// Off Sprite
        /// </summary>
        [SerializeField]
        [Tooltip("Off Sprite")]
        Sprite m_offSprite = null;

        /// <summary>
        /// Start
        /// </summary>
        // -----------------------------------------------------------------------
        IEnumerator Start()
        {

#if UNITY_EDITOR

            if (!this.m_refSlider)
            {
                Debug.LogError("m_refSlider is null : " + this.gameObject.name);
            }

            if (!this.m_refRotationFlipImage)
            {
                Debug.LogError("m_refRotationFlipImage is null : " + this.gameObject.name);
            }

#endif

            while(!SystemManager.Instance.isStartupDone)
            {
                yield return null;
            }

            this.setSoundVolume(SystemManager.Instance.configDataSO.masterVolume01);
            this.setRotationFlipImage();

        }

        /// <summary>
        /// Set sound volume
        /// </summary>
        /// <param name="vol">volume</param>
        // -----------------------------------------------------------------------
        public void setSoundVolume(float vol)
        {

            if(!this.m_refSlider)
            {
                return;
            }

            // ------------

            AudioListener.volume = vol;

            SystemManager.Instance.configDataSO.masterVolume01 = vol;

            if (!Mathf.Approximately(this.m_refSlider.value, vol))
            {
                this.m_refSlider.value = vol;
            }

        }

        /// <summary>
        /// Switch system language
        /// </summary>
        // -------------------------------------------------------------------------------------
        public void switchSystemLanguage()
        {
            SystemManager.Instance.switchSystemLanguage();
        }

        /// <summary>
        /// Switch rotation flip
        /// </summary>
        // -------------------------------------------------------------------------------------
        public void setRotationFlipImage()
        {

            if (this.m_refRotationFlipImage)
            {

                this.m_refRotationFlipImage.sprite =
                    (SystemManager.Instance.configDataSO.rotationFlip) ?
                    this.m_onSprite :
                    this.m_offSprite
                    ;

            }

        }
        /// <summary>
        /// Switch rotation flip
        /// </summary>
        // -------------------------------------------------------------------------------------
        public void switchRotationFlip()
        {

            SystemManager.Instance.configDataSO.rotationFlip = !SystemManager.Instance.configDataSO.rotationFlip;

            this.setRotationFlipImage();

        }

    }

}
