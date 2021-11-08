using System;
using System.Collections;
using System.Collections.Generic;
using SSC;
using UnityEngine;

namespace ciitt.EscapeGameKit
{

    /// <summary>
    /// Room door
    /// </summary>
    public class RoomDoorScript : ClickableColliderScript
    {

        /// <summary>
        /// User progress data
        /// </summary>
        [Serializable]
        class UserProgressData
        {

            public bool openTrueCloseFalse = false;

            public void clear()
            {

                this.openTrueCloseFalse = false;

            }

        }

        /// <summary>
        /// Reference to another ViewPoint
        /// </summary>
        [SerializeField]
        [Tooltip("Reference to another ViewPoint")]
        ViewPoint m_refAnotherTargetViewPoint = null;

        /// <summary>
        /// Another way point list
        /// </summary>
        [SerializeField]
        [Tooltip("Another way point list")]
        List<Transform> m_anotherWayPointList = new List<Transform>();

        /// <summary>
        /// Unlock target list
        /// </summary>
        [SerializeField]
        [Tooltip("Unlock target list")]
        List<ClickableColliderScript> m_refUnlockTargetList = new List<ClickableColliderScript>();

        /// <summary>
        /// OpenCloseAnimHolder list
        /// </summary>
        [SerializeField]
        [Tooltip("OpenCloseAnimHolder list")]
        List<OpenCloseAnimHolder> m_refAnimList = new List<OpenCloseAnimHolder>();

        /// <summary>
        /// UserProgressData
        /// </summary>
        UserProgressData m_userProgressData = new UserProgressData();

        /// <summary>
        /// Start
        /// </summary>
        // ----------------------------------------------------------------------------------
        protected override void Start()
        {

            base.Start();

#if UNITY_EDITOR

            // m_refAnotherTargetViewPoint
            {

                if (!this.m_refAnotherTargetViewPoint)
                {
                    Debug.LogError("m_refAnotherTargetViewPoint is null : " + Funcs.createHierarchyPath(this.transform));
                }

            }

            // m_refUnlockTargetList
            {

                if (this.m_refUnlockTargetList.Count <= 0)
                {
                    Debug.LogError("m_refUnlockTargetList.Count <= 0 : " + Funcs.createHierarchyPath(this.transform));
                }

                for (int i = this.m_refUnlockTargetList.Count - 1; i >= 0; i--)
                {

                    if (!this.m_refUnlockTargetList[i])
                    {
                        Debug.LogError("m_refUnlockTargetList contains null : " + Funcs.createHierarchyPath(this.transform));
                        this.m_refUnlockTargetList.RemoveAt(i);
                    }

                }

            }

            // m_refAnimList
            {

                if (this.m_refAnimList.Count <= 0)
                {
                    Debug.LogError("m_refAnimList.Count <= 0 : " + Funcs.createHierarchyPath(this.transform));
                }

                for (int i = this.m_refAnimList.Count - 1; i >= 0; i--)
                {

                    if (!this.m_refAnimList[i])
                    {
                        Debug.LogError("m_refAnimList contains null : " + Funcs.createHierarchyPath(this.transform));
                        this.m_refAnimList.RemoveAt(i);
                    }

                }

            }

#endif

            // CustomReduxManager
            {
                CustomReduxManager.CustomReduxManagerInstance.addSceneChangeStateReceiver(this.onSceneChangeStateReceiver);
                CustomReduxManager.CustomReduxManagerInstance.addUserProgressDataSignalReceiver(this.onUserProgressDataSignal);
            }

        }

        /// <summary>
        /// Click
        /// </summary>
        // ----------------------------------------------------------------------------------
        public override void onClickInEscapeGame()
        {

            MainGameSceneState mgsState = this.m_refMainGameSceneStateWatcher.state();

            if (mgsState.stateEnum != MainGameSceneState.StateEnum.MainGameSceneMain)
            {
                return;
            }

            // -----------------------

            if (
                this.m_refTargetViewPoint != mgsState.changeCameraViewInfo.currentTargetViewPoint &&
                this.m_refAnotherTargetViewPoint != mgsState.changeCameraViewInfo.currentTargetViewPoint
                )
            {
                this.changeCameraView();
            }

            else
            {

                if (this.m_lockState == LockState.Locked)
                {
                    this.actionWhenLocked();
                }

                else
                {
                    this.actionWhenUnlocked();
                }

            }

        }

        /// <summary>
        /// Change camera view
        /// </summary>
        // ----------------------------------------------------------------------------------
        protected override void changeCameraView()
        {

            MainGameSceneState mgsState = this.m_refMainGameSceneStateWatcher.state();

            if (mgsState.stateEnum != MainGameSceneState.StateEnum.MainGameSceneMain)
            {
                return;
            }

            // -----------------------

            ViewPoint current = mgsState.changeCameraViewInfo.currentTargetViewPoint;

            // -----------------------

            if (
                this.m_refTargetViewPoint &&
                this.m_refTargetViewPoint.viewPointGroup == current.viewPointGroup
                )
            {

                if (!(current.disableMoveToParentByClick && current.viewPointParent == this.m_refTargetViewPoint))
                {

                    mgsState.setChangeCameraViewState(
                        this.m_refTargetViewPoint,
                        this.m_wayPointList,
                        true
                        );

                }

            }

            else if (
                this.m_refAnotherTargetViewPoint &&
                this.m_refAnotherTargetViewPoint.viewPointGroup == current.viewPointGroup
                )
            {

                if (!(current.disableMoveToParentByClick && current.viewPointParent == this.m_refAnotherTargetViewPoint))
                {

                    mgsState.setChangeCameraViewState(
                        this.m_refAnotherTargetViewPoint,
                        this.m_anotherWayPointList,
                        true
                        );

                }

            }

        }

        /// <summary>
        /// Check OpenCloseState
        /// </summary>
        /// <param name="checkState">OpenCloseState</param>
        /// <returns>all ok</returns>
        // ----------------------------------------------------------------------------------
        bool checkOpenCloseState(OpenCloseState checkState)
        {

            foreach (var val in this.m_refAnimList)
            {
                if (val.currentOpenCloseState() != checkState)
                {
                    return false;
                }
            }

            return true;

        }

        /// <summary>
        /// Play all open anims
        /// </summary>
        /// <param name="immediately">immediately</param>
        // ----------------------------------------------------------------------------------
        protected void playAllOpenAnims(bool immediately)
        {

            // open
            {

                foreach (var val in this.m_refAnimList)
                {
                    val.playOpenAnim(immediately);
                }

            }

            // unlockAll
            {

                if (!immediately)
                {
                    StartCoroutine(this.waitForAllAnims(OpenCloseState.Open, this.unlockAll));
                }

                else
                {
                    this.unlockAll();
                }

            }

            // openTrueCloseFalse
            {
                this.m_userProgressData.openTrueCloseFalse = true;
            }

        }

        /// <summary>
        /// Play all open anims
        /// </summary>
        /// <param name="immediately">immediately</param>
        // ----------------------------------------------------------------------------------
        protected void playAllCloseAnims(bool immediately)
        {

            // close
            {

                foreach (var val in this.m_refAnimList)
                {
                    val.playCloseAnim(immediately);
                }

            }

            // lockAll
            {

                if (!immediately)
                {
                    StartCoroutine(this.waitForAllAnims(OpenCloseState.Close, this.lockAll));
                }

                else
                {
                    this.lockAll();
                }

            }

            // openTrueCloseFalse
            {
                this.m_userProgressData.openTrueCloseFalse = false;
            }

        }

        /// <summary>
        /// Wait for anims
        /// </summary>
        /// <param name="targetState">targetState</param>
        /// <param name="actionAfterAnims">actionAfterAnims</param>
        /// <returns>IEnumerator</returns>
        // ----------------------------------------------------------------------------------
        IEnumerator waitForAllAnims(OpenCloseState targetState, Action actionAfterAnims)
        {

            yield return null;

            // wait
            {

                bool temp = this.checkOpenCloseState(targetState);

                while(!temp)
                {
                    yield return null;
                    temp = this.checkOpenCloseState(targetState);
                }

            }

            // actionAfterAnims
            {

                if (actionAfterAnims != null)
                {
                    actionAfterAnims();
                }

            }

        }

        /// <summary>
        /// Function when unlocked
        /// </summary>
        // ----------------------------------------------------------------------------------
        protected override void actionWhenUnlocked()
        {

            if (this.checkOpenCloseState(OpenCloseState.Open))
            {
                this.playAllCloseAnims(false);
                SoundManager.Instance.playSe(SoundManager.SeType.CloseFieldObject);
            }

            else if (this.checkOpenCloseState(OpenCloseState.Close))
            {
                this.playAllOpenAnims(false);
                SoundManager.Instance.playSe(SoundManager.SeType.OpenFieldObject);
            }

        }

        /// <summary>
        /// Lock all
        /// </summary>
        // ----------------------------------------------------------------------------------
        protected virtual void lockAll()
        {

            foreach (var val in this.m_refUnlockTargetList)
            {
                val.lockThis(true);
            }

        }

        /// <summary>
        /// Unlock all
        /// </summary>
        // ----------------------------------------------------------------------------------
        protected virtual void unlockAll()
        {

            foreach (var val in this.m_refUnlockTargetList)
            {
                val.unlockThis();
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

                    if (this.m_userProgressData.openTrueCloseFalse)
                    {
                        this.playAllOpenAnims(true);
                    }

                }

            }

        }

    }

}
