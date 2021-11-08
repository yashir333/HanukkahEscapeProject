using System;
using System.Collections;
using System.Collections.Generic;
using SSC;
using UnityEngine;

namespace ciitt.EscapeGameKit
{

    /// <summary>
    /// Submit
    /// </summary>
    [RequireComponent(typeof(Renderer))]
    public abstract class SubmitScript : ButtonInputScript
    {

        /// <summary>
        /// User progress data
        /// </summary>
        [Serializable]
        protected class UserProgressData
        {

            public bool unlocked = false;

            public void clear()
            {
                this.unlocked = false;
            }

        }

        /// <summary>
        /// Unlock target list
        /// </summary>
        [SerializeField]
        [Tooltip("Unlock target list")]
        List<ClickableColliderScript> m_refUnlockTargetList = new List<ClickableColliderScript>();

        /// <summary>
        /// Additional disable target list
        /// </summary>
        [SerializeField]
        [Tooltip("Additional disable target list")]
        List<ClickableColliderScript> m_refAdditionalDisableTargetList = new List<ClickableColliderScript>();

        /// <summary>
        /// Unlocked Material
        /// </summary>
        [SerializeField]
        [Tooltip("Unlocked Material")]
        protected Material m_unlockedMaterial = null;

        /// <summary>
        /// Reference to input buttons
        /// </summary>
        protected HashSet<ClickableColliderScript> m_inputButtons = new HashSet<ClickableColliderScript>();

        /// <summary>
        /// UserProgressData
        /// </summary>
        protected UserProgressData m_userProgressData = new UserProgressData();

        /// <summary>
        /// Check answer
        /// </summary>
        // ----------------------------------------------------------------------------------
        protected abstract void checkAnswer();

        /// <summary>
        /// Start
        /// </summary>
        // ----------------------------------------------------------------------------------
        protected override void Start()
        {

            base.Start();

            // CustomReduxManager
            {
                CustomReduxManager.CustomReduxManagerInstance.addSceneChangeStateReceiver(this.onSceneChangeStateReceiver);
                CustomReduxManager.CustomReduxManagerInstance.addUserProgressDataSignalReceiver(this.onUserProgressDataSignal);
            }

#if UNITY_EDITOR

            if (this.m_refUnlockTargetList.Count <= 0)
            {
                Debug.LogError("m_refUnlockTarget == null : " + Funcs.createHierarchyPath(this.transform));
            }

            if(!this.m_unlockedMaterial)
            {
                Debug.LogError("m_unlockedMaterial == null : " + Funcs.createHierarchyPath(this.transform));
            }

            for (int i = this.m_refUnlockTargetList.Count - 1; i >= 0; i--)
            {
                if (!this.m_refUnlockTargetList[i])
                {
                    Debug.LogError("m_refUnlockTargetList has empty element : " + Funcs.createHierarchyPath(this.transform));
                    this.m_refUnlockTargetList.RemoveAt(i);
                }
            }

#endif

        }

        /// <summary>
        /// OnDestroy
        /// </summary>
        // ----------------------------------------------------------------------------------
        protected override void OnDestroy()
        {

            this.m_unlockedMaterial = null;

            base.OnDestroy();

        }

        /// <summary>
        /// Function when unlocked
        /// </summary>
        // ----------------------------------------------------------------------------------
        protected override void actionWhenUnlocked()
        {
            this.checkAnswer();
        }

        /// <summary>
        /// Change and resume color
        /// </summary>
        /// <param name="resumeInvokeTime">time to resume</param>
        // ------------------------------------------------------------------------------------------
        protected override void changeAndResumeColor(float resumeInvokeTime = 0.1f)
        {

            // changeColor
            {
                this.changeColor();
            }

            // resumeColor
            {
                CancelInvoke("resumeColor");
                Invoke("resumeColor", resumeInvokeTime);
            }

        }

        /// <summary>
        /// Change and resume color with playing SE
        /// </summary>
        /// <param name="resumeInvokeTime">time to resume</param>
        // ------------------------------------------------------------------------------------------
        protected virtual void changeAndResumeColorWithInvalidAnswerSe(float resumeInvokeTime = 0.1f)
        {

            // changeAndResumeColor
            {
                this.changeAndResumeColor(resumeInvokeTime);
            }

            // playSe
            {
                SoundManager.Instance.playSe(SoundManager.SeType.InvalidAnswer);
            }

        }

        /// <summary>
        /// Add button reference
        /// </summary>
        /// <param name="ccs">ClickableColliderScript</param>
        // ----------------------------------------------------------------------------------
        public void addInputButtonReference(ClickableColliderScript ccs)
        {
            this.m_inputButtons.Add(ccs);
        }

        /// <summary>
        /// Unlock
        /// </summary>
        // ----------------------------------------------------------------------------------
        protected virtual void unlockByCorrectAnswer(bool playSe)
        {

            // CorrectAnswer
            {
                if(playSe)
                {
                    SoundManager.Instance.playSe(SoundManager.SeType.CorrectAnswer);
                }
            }

            // m_refUnlockTargetList
            {

                foreach (var val in this.m_refUnlockTargetList)
                {
                    if (val)
                    {
                        val.unlockThis();
                    }
                }

            }

            // lockThis
            {
                this.lockThis(true);
            }

            // m_inputButtons lockState refCollider
            {

                foreach (var val in this.m_inputButtons)
                {
                    if (val)
                    {
                        val.lockThis(true);
                    }
                }

            }

            // m_refAdditionalDisableTargetList
            {
                foreach (var val in this.m_refAdditionalDisableTargetList)
                {
                    if (val)
                    {
                        val.lockThis(true);
                    }
                }
            }

            // m_unlockedMaterial
            {

                Renderer rend = this.GetComponent<Renderer>();

                if(rend && this.m_unlockedMaterial)
                {
                    rend.sharedMaterial = this.m_unlockedMaterial;
                }

            }

            // m_userProgressData
            {
                this.m_userProgressData.unlocked = true;
            }

        }

        /// <summary>
        /// UserProgressDataSignal receiver
        /// </summary>
        /// <param name="updSignal">UserProgressDataSignal</param>
        // ------------------------------------------------------------------------------------------
        protected virtual void onUserProgressDataSignal(UserProgressDataSignal updSignal)
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
        protected virtual void onSceneChangeStateReceiver(SceneChangeState scState)
        {

            if (scState.stateEnum == SceneChangeState.StateEnum.AllStartupsDone)
            {

                CustomSceneChangeManager cscManager = CustomSceneChangeManager.Instance as CustomSceneChangeManager;

                if (cscManager.isLoadingCurrentSceneWithUserProgressData())
                {

                    this.m_userProgressData = cscManager.getDataFromCurrentUserProgressData<UserProgressData>(this.transform, this);

                    if(this.m_userProgressData.unlocked)
                    {
                        this.unlockByCorrectAnswer(false);
                    }

                }

            }

        }

    }

}
