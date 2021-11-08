using System;
using System.Collections;
using System.Collections.Generic;
using SSC;
using UnityEngine;

namespace ciitt.EscapeGameKit
{

    /// <summary>
    /// Slider input
    /// </summary>
    public class SliderInputScript : ClickableColliderScript
    {

        /// <summary>
        /// User progress data
        /// </summary>
        [Serializable]
        class UserProgressData
        {

            public int currentIndex = 0;

            public void clear()
            {
                this.currentIndex = 0;
            }

        }

        /// <summary>
        /// Reference to SubmitSliderScript
        /// </summary>
        [SerializeField]
        [Tooltip("Reference to SubmitSliderScript")]
        SubmitSliderScript m_refSubmitSliderScript = null;

        /// <summary>
        /// Reference to slider Transform
        /// </summary>
        [SerializeField]
        [Tooltip("Reference to slider Transform")]
        Transform m_refSliderTransform = null;

        /// <summary>
        /// Division
        /// </summary>
        [SerializeField]
        [Tooltip("Division")]
        [Range(2, 9)] // max = 9 from NumberCharacters
        int m_division = 5;

        /// <summary>
        /// From local position
        /// </summary>
        [SerializeField]
        [Tooltip("From local position")]
        Vector3 m_fromLocalPos = Vector3.zero;

        /// <summary>
        /// To local position
        /// </summary>
        [SerializeField]
        [Tooltip("To local position")]
        Vector3 m_toLocalPos = Vector3.zero;

        /// <summary>
        /// Answer character index
        /// </summary>
        [SerializeField]
        [Tooltip("Answer character index")]
        int m_answerCharacterIndex = 0;

        /// <summary>
        /// UserProgressData
        /// </summary>
        UserProgressData m_userProgressData = new UserProgressData();

        /// <summary>
        /// Start
        /// </summary>
        // --------------------------------------------------------------------------------------------
        protected override void Start()
        {

            base.Start();

#if UNITY_EDITOR

            if (!this.m_refSubmitSliderScript)
            {
                Debug.LogError("m_refSubmitSliderScript is null : " + Funcs.createHierarchyPath(this.transform));
            }

            if (!this.m_refSliderTransform)
            {
                Debug.LogError("m_refSliderTransform is null : " + Funcs.createHierarchyPath(this.transform));
            }

#endif

            // m_refSubmitSliderScript
            {

                if (this.m_refSubmitSliderScript)
                {
                    this.m_refSubmitSliderScript.addInputButtonReference(this);
                }

            }

            // m_refSliderTransform
            {

                if (this.m_refSliderTransform)
                {
                    this.m_refSliderTransform.localPosition = this.m_fromLocalPos;
                }

            }

            // CustomReduxManager
            {
                CustomReduxManager.CustomReduxManagerInstance.addSceneChangeStateReceiver(this.onSceneChangeStateReceiver);
                CustomReduxManager.CustomReduxManagerInstance.addUserProgressDataSignalReceiver(this.onUserProgressDataSignal);
            }

        }

        /// <summary>
        /// Function when unlocked
        /// </summary>
        // ----------------------------------------------------------------------------------
        protected override void actionWhenUnlocked()
        {

            this.moveSlider();

            // playSe
            {
                SoundManager.Instance.playSe(SoundManager.SeType.GimmickButton);
            }

        }

        /// <summary>
        /// Move slider
        /// </summary>
        // ------------------------------------------------------------------------------------------
        void moveSlider()
        {

            if (!this.m_refSliderTransform)
            {
                return;
            }

            // -----------------

            // setSlider
            {
                this.setSlider((this.m_userProgressData.currentIndex + 1) % this.m_division);
            }

        }

        /// <summary>
        /// Set slider
        /// </summary>
        // ------------------------------------------------------------------------------------------
        void setSlider(int index)
        {

            if (!this.m_refSliderTransform)
            {
                return;
            }

            // -----------------

            // currentIndex
            {
                this.m_userProgressData.currentIndex = index;
            }

            // move
            {

                this.m_refSliderTransform.localPosition = Vector3.Lerp(
                    this.m_fromLocalPos,
                    this.m_toLocalPos,
                    this.m_userProgressData.currentIndex / (float)(this.m_division - 1)
                    );

            }

            // m_refSubmitSliderScript
            {

                if (this.m_refSubmitSliderScript)
                {
                    this.m_refSubmitSliderScript.setUserInput(
                        this.m_answerCharacterIndex,
                        (NumberCharacters)this.m_userProgressData.currentIndex
                        );
                }

            }

        }

        /// <summary>
        /// UserProgressDataSignal receiver
        /// </summary>
        /// <param name="updSignal">UserProgressDataSignal</param>
        // ------------------------------------------------------------------------------------------
        void onUserProgressDataSignal(UserProgressDataSignal updSignal)
        {

            updSignal.addDataAction(
                SystemManager.Instance.createKeyPath(this.transform, this),
                JsonUtility.ToJson(this.m_userProgressData)
                );

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

                CustomSceneChangeManager cscManager = CustomSceneChangeManager.Instance as CustomSceneChangeManager;

                if (cscManager.isLoadingCurrentSceneWithUserProgressData())
                {

                    this.m_userProgressData = cscManager.getDataFromCurrentUserProgressData<UserProgressData>(this.transform, this);

                    this.setSlider(this.m_userProgressData.currentIndex);

                }

            }

        }

    }

}
