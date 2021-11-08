using System;
using System.Collections;
using System.Collections.Generic;
using SSC;
using UnityEngine;

namespace ciitt.EscapeGameKit
{

    /// <summary>
    /// Alphabet input
    /// </summary>
    public class AlphabetInputScript : ButtonInputScript
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
        /// Reference to SubmitAlphabetScript
        /// </summary>
        [SerializeField]
        [Tooltip("Reference to SubmitAlphabetScript")]
        SubmitAlphabetScript m_refSubmitAlphabetScript = null;

        /// <summary>
        /// Answer character index
        /// </summary>
        [SerializeField]
        [Tooltip("Answer character index")]
        int m_answerCharacterIndex = 0;

        /// <summary>
        /// First uv array
        /// </summary>
        Vector2[] m_unitUvs;

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

            if (!this.m_refSubmitAlphabetScript)
            {
                Debug.LogError("m_refSubmitAlphabetScript is null : " + Funcs.createHierarchyPath(this.transform));
            }

#endif

            // m_refSubmitAlphabetScript
            {

                if (this.m_refSubmitAlphabetScript)
                {
                    this.m_refSubmitAlphabetScript.addInputButtonReference(this);
                }

            }

            // m_unitUvs
            {

                Vector2[] uvs = this.m_refMeshFilter.mesh.uv;

                for (int i = uvs.Length - 1; i >= 0; i--)
                {
                    uvs[i] = uvs[i] / 6f;
                }

                this.m_unitUvs = uvs;

            }

            // setUvOffset
            {
                this.setUvOffset(AlphabetCharacters._A);
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

            this.showNextCharacter();

            // playSe
            {
                SoundManager.Instance.playSe(SoundManager.SeType.GimmickButton);
            }

        }

        /// <summary>
        /// Show next character
        /// </summary>
        // --------------------------------------------------------------------------------------------
        void showNextCharacter()
        {

            if (this.m_userProgressData.currentIndex >= (int)AlphabetCharacters._Z)
            {
                this.setUvOffset(AlphabetCharacters._A);
            }

            else
            {
                this.setUvOffset((AlphabetCharacters)(this.m_userProgressData.currentIndex + 1));
            }

            // changeAndResumeColor
            {
                this.changeAndResumeColor();
            }

        }

        /// <summary>
        /// Set offset
        /// </summary>
        /// <param name="index">AlphabetCharacters</param>
        // --------------------------------------------------------------------------------------------
        void setUvOffset(AlphabetCharacters index)
        {

            int indexX = (int)index % 6;
            int indexY = (int)index / 6;

            float unit = 1.0f / 6f;

            // currentIndex
            {
                this.m_userProgressData.currentIndex = (int)index;
            }

            // uv
            {

                Vector2[] newUvs = this.m_refMeshFilter.mesh.uv;

                for (int i = newUvs.Length - 1; i >= 0; i--)
                {
                    newUvs[i].x = this.m_unitUvs[i].x + (unit * indexX);
                    newUvs[i].y = this.m_unitUvs[i].y + (unit * indexY);
                }

                this.m_refMeshFilter.mesh.uv = newUvs;

            }

            // m_refSubmitAlphabetScript
            {

                if (this.m_refSubmitAlphabetScript)
                {
                    this.m_refSubmitAlphabetScript.setUserInput(this.m_answerCharacterIndex, index);
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

                    this.setUvOffset((AlphabetCharacters)this.m_userProgressData.currentIndex);

                }

            }

        }

    }

}
