using System.Collections;
using System.Collections.Generic;
using SSC;
using UnityEngine;

namespace ciitt.EscapeGameKit
{

    /// <summary>
    /// Sound manager
    /// </summary>
    public class SoundManager : SingletonMonoBehaviour<SoundManager>
    {

        public enum SeType
        {
            ShowItem,
            CloseItem,
            InvalidAnswer,
            CorrectAnswer,
            GimmickButton,
            OpenFieldObject,
            CloseFieldObject,
            Evolve,
            MoveCamera,
            SelectItem,
            Save,
            StartAndContinueInTitle
        }

        /// <summary>
        /// AudioSource for SE
        /// </summary>
        [SerializeField]
        [Tooltip("AudioSource for SE")]
        AudioSource m_refAudioSourceForSe = null;


        /// <summary>
        /// Show item
        /// </summary>
        [SerializeField]
        [Tooltip("Show item")]
        AudioClip m_showItem = null;

        /// <summary>
        /// Close item
        /// </summary>
        [SerializeField]
        [Tooltip("Close item")]
        AudioClip m_closeItem = null;

        /// <summary>
        /// Invalid answer
        /// </summary>
        [SerializeField]
        [Tooltip("Invalid answer")]
        AudioClip m_invalidAnswer = null;

        /// <summary>
        /// Correct answer
        /// </summary>
        [SerializeField]
        [Tooltip("Correct answer")]
        AudioClip m_correctAnswer = null;

        /// <summary>
        /// Gimmick button
        /// </summary>
        [SerializeField]
        [Tooltip("Gimmick button")]
        AudioClip m_gimmickButton = null;

        /// <summary>
        /// Open field object
        /// </summary>
        [SerializeField]
        [Tooltip("Open field object")]
        AudioClip m_openFieldObject = null;

        /// <summary>
        /// Close field object
        /// </summary>
        [SerializeField]
        [Tooltip("Close field object")]
        AudioClip m_closeFieldObject = null;

        /// <summary>
        /// Evolve
        /// </summary>
        [SerializeField]
        [Tooltip("Evolve")]
        AudioClip m_evolve = null;

        /// <summary>
        /// Move camera
        /// </summary>
        [SerializeField]
        [Tooltip("Move camera")]
        AudioClip m_moveCamera = null;

        /// <summary>
        /// Select item
        /// </summary>
        [SerializeField]
        [Tooltip("Select item")]
        AudioClip m_selectItem = null;

        /// <summary>
        /// Save data
        /// </summary>
        [SerializeField]
        [Tooltip("Save data")]
        AudioClip m_saveData = null;

        /// <summary>
        /// Start and continue in title
        /// </summary>
        [SerializeField]
        [Tooltip("Start and continue in title")]
        AudioClip m_startAndContinueInTitle = null;

        /// <summary>
        /// Called in Awake
        /// </summary>
        // -------------------------------------------------------------------------------------
        protected override void initOnAwake()
        {

#if UNITY_EDITOR

            if (!this.m_refAudioSourceForSe)
            {
                Debug.LogError("m_refAudioSourceForSe is null : " + Funcs.createHierarchyPath(this.transform));
            }

#endif

        }

        /// <summary>
        /// Play SE
        /// </summary>
        /// <param name="clip">AudioClip</param>
        /// <param name="delay">delay</param>
        // -------------------------------------------------------------------------------------
        public void playSe(AudioClip clip, float delay = 0.0f)
        {

            if(!this.m_refAudioSourceForSe || !clip)
            {
                return;
            }

            // --------------

            if (delay > 0.0f)
            {
                this.m_refAudioSourceForSe.clip = clip;
                this.m_refAudioSourceForSe.PlayDelayed(delay);
            }

            else
            {
                this.m_refAudioSourceForSe.PlayOneShot(clip);
            }

        }

        /// <summary>
        /// Play SE
        /// </summary>
        /// <param name="seType">SeType</param>
        /// <param name="delay">delay</param>
        // -------------------------------------------------------------------------------------
        public void playSe(SeType seType, float delay = 0.0f)
        {

            if(!this.m_refAudioSourceForSe)
            {
                return;
            }

            // --------------

            if(seType == SeType.ShowItem)
            {
                this.playSe(this.m_showItem, delay);
            }

            else if (seType == SeType.CloseItem)
            {
                this.playSe(this.m_closeItem, delay);
            }

            else if (seType == SeType.InvalidAnswer)
            {
                this.playSe(this.m_invalidAnswer, delay);
            }

            else if (seType == SeType.CorrectAnswer)
            {
                this.playSe(this.m_correctAnswer, delay);
            }

            else if (seType == SeType.GimmickButton)
            {
                this.playSe(this.m_gimmickButton, delay);
            }

            else if (seType == SeType.OpenFieldObject)
            {
                this.playSe(this.m_openFieldObject, delay);
            }

            else if (seType == SeType.CloseFieldObject)
            {
                this.playSe(this.m_closeFieldObject, delay);
            }

            else if (seType == SeType.Evolve)
            {
                this.playSe(this.m_evolve, delay);
            }

            else if (seType == SeType.MoveCamera)
            {
                this.playSe(this.m_moveCamera, delay);
            }

            else if (seType == SeType.SelectItem)
            {
                this.playSe(this.m_selectItem, delay);
            }

            else if (seType == SeType.Save)
            {
                this.playSe(this.m_saveData, delay);
            }

            else if (seType == SeType.StartAndContinueInTitle)
            {
                this.playSe(this.m_startAndContinueInTitle, delay);
            }

        }

    }

}
