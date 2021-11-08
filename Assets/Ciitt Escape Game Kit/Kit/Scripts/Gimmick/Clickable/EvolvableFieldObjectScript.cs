using System;
using System.Collections;
using System.Collections.Generic;
using SSC;
using UnityEngine;

namespace ciitt.EscapeGameKit
{

    /// <summary>
    /// Evolvable
    /// </summary>
    public class EvolvableFieldObjectScript : ClickableColliderScript
    {

        /// <summary>
        /// User progress data
        /// </summary>
        [Serializable]
        class UserProgressData
        {

            public bool evolved = false;

            public void clear()
            {

                this.evolved = false;

            }

        }

        /// <summary>
        /// ViewPoint when evolution
        /// </summary>
        [Serializable]
        public class ViewPointWhenEvolution
        {
            public ViewPoint viewPoint = null;
            public float delay = 0.5f;
        }

        /// <summary>
        /// Wait for UI
        /// </summary>
        [SerializeField]
        [Tooltip("Wait for UI")]
        float m_waitForUi = 0.5f;

        /// <summary>
        /// Unlock target list
        /// </summary>
        [SerializeField]
        [Tooltip("Unlock target list")]
        List<ClickableColliderScript> m_refUnlockTargetList = new List<ClickableColliderScript>();

        /// <summary>
        /// EvolveAnimHolder list
        /// </summary>
        [SerializeField]
        [Tooltip("EvolveAnimHolder list")]
        List<EvolveAnimHolder> m_refAnimList = new List<EvolveAnimHolder>();

        /// <summary>
        /// ConditionsForEvolution
        /// </summary>
        [SerializeField]
        [Tooltip("ConditionsForEvolution")]
        [UnityEngine.Serialization.FormerlySerializedAs("m_condition")]
        ConditionsForEvolution m_conditionsForEvolution = new ConditionsForEvolution();

        /// <summary>
        /// ViewPointWhenEvolution
        /// </summary>
        [SerializeField]
        [Tooltip("ViewPointWhenEvolution")]
        ViewPointWhenEvolution m_viewPointWhenEvolution = new ViewPointWhenEvolution();

        /// <summary>
        /// Back to parent ViewPoint after evolution
        /// </summary>
        [SerializeField]
        [Tooltip("Back to parent ViewPoint after evolution")]
        bool m_backToParentViewPointAfterEvolution = false;

        /// <summary>
        /// Activate before evolution list
        /// </summary>
        [SerializeField]
        [Tooltip("Activate before evolution list")]
        [UnityEngine.Serialization.FormerlySerializedAs("m_activateList")]
        List<Transform> m_activateBeforeEvolutionList = new List<Transform>();

        /// <summary>
        /// Deactivate after evolution list
        /// </summary>
        [SerializeField]
        [Tooltip("Deactivate after evolution list")]
        [UnityEngine.Serialization.FormerlySerializedAs("m_deactivateList")]
        List<Transform> m_deactivateAfterEvolutionList = new List<Transform>();

        /// <summary>
        /// UserProgressData
        /// </summary>
        UserProgressData m_userProgressData = new UserProgressData();

        /// <summary>
        /// IEnumerator for evolveIE
        /// </summary>
        IEnumerator m_evolveIE = null;

        // ----------------------------------------------------------------------------------

        /// <summary>
        /// Evolved
        /// </summary>
        public bool evolved { get { return this.m_userProgressData.evolved; } }

        /// <summary>
        /// ViewPointWhenEvolution
        /// </summary>
        public ViewPointWhenEvolution viewPointWhenEvolution { get { return this.m_viewPointWhenEvolution; } }

        /// <summary>
        /// Start
        /// </summary>
        // ----------------------------------------------------------------------------------
        protected override void Start()
        {

            base.Start();

#if UNITY_EDITOR

            for (int i = this.m_refUnlockTargetList.Count - 1; i >= 0; i--)
            {

                if (!this.m_refUnlockTargetList[i])
                {
                    Debug.LogError("m_refUnlockTargetList contains null : " + Funcs.createHierarchyPath(this.transform));
                    this.m_refUnlockTargetList.RemoveAt(i);
                }

            }

            for (int i = this.m_refAnimList.Count - 1; i >= 0; i--)
            {

                if (!this.m_refAnimList[i])
                {
                    Debug.LogError("m_refAnimList contains null : " + Funcs.createHierarchyPath(this.transform));
                    this.m_refAnimList.RemoveAt(i);
                }

            }

            if (!this.m_conditionsForEvolution.item)
            {
                Debug.LogError("m_conditionsForEvolution.item is null : " + Funcs.createHierarchyPath(this.transform));
            }

#endif

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

            if (
                !this.m_userProgressData.evolved &&
                this.m_conditionsForEvolution.matchConditions(ItemManager.Instance.currentSelectedItem)
                )
            {

                if (this.m_evolveIE != null)
                {
                    StopCoroutine(this.m_evolveIE);
                    this.m_evolveIE = null;
                }

                StartCoroutine(this.m_evolveIE = this.evolveIE(false));

            }

        }

        /// <summary>
        /// Evolve IEnumerator
        /// </summary>
        /// <param name="immediately">immediately</param>
        /// <returns>IEnumerator</returns>
        // ----------------------------------------------------------------------------------
        IEnumerator evolveIE(bool immediately)
        {

            // m_evolved
            {
                this.m_userProgressData.evolved = true;
            }

            if (!immediately)
            {

                StateWatcher<MainGameSceneState> mgssWatcher =
                    CustomReduxManager.CustomReduxManagerInstance.MainGameSceneStateWatcher;

                mgssWatcher.state().setFieldObjectEvolution(this);

            }

            // playSe
            if (!immediately)
            {
                SoundManager.Instance.playSe(SoundManager.SeType.Evolve);
            }

            if (!immediately && this.m_waitForUi > 0.0f)
            {
                yield return new WaitForSeconds(this.m_waitForUi);
            }

            // activateBeforeEvolution
            {
                this.activateBeforeEvolution();
            }

            // evolveAnim
            {
                this.playAllEvolveAnims(immediately);
            }

            if (!immediately)
            {

                bool isAllAnimFinished = false;

                while(!isAllAnimFinished)
                {

                    isAllAnimFinished = true;

                    foreach (var val in this.m_refAnimList)
                    {

                        if (val.currentEvolveAnimState() != EvolveAnimState.Evolved)
                        {
                            isAllAnimFinished = false;
                            yield return null;
                            break;
                        }

                    }

                    yield return null;

                }

            }

            // unlockAll
            {
                this.unlockAll();
            }

            // deactivateAfterEvolution
            {
                this.deactivateAfterEvolution();
            }

            // m_deactivateAfterEvolution
            {
                this.m_evolveIE = null;
            }

            if (!immediately)
            {

                if (this.m_backToParentViewPointAfterEvolution)
                {

                    StateWatcher<MainGameSceneState> mgssWatcher =
                        CustomReduxManager.CustomReduxManagerInstance.MainGameSceneStateWatcher;

                    if (
                        mgssWatcher.state().stateEnum == MainGameSceneState.StateEnum.MainGameSceneFieldObjectEvolution &&
                        this.m_refTargetViewPoint &&
                        this.m_refTargetViewPoint.viewPointParent
                        )
                    {
                        mgssWatcher.state().setChangeCameraViewState(this.m_refTargetViewPoint.viewPointParent);
                    }

                }

                else
                {

                    StateWatcher<MainGameSceneState> mgssWatcher =
                        CustomReduxManager.CustomReduxManagerInstance.MainGameSceneStateWatcher;

                    if (mgssWatcher.state().stateEnum == MainGameSceneState.StateEnum.MainGameSceneFieldObjectEvolution)
                    {
                        mgssWatcher.state().setState(MainGameSceneState.StateEnum.MainGameSceneMain);
                    }

                }

            }

            if(!immediately && ItemManager.Instance.currentSelectedItem)
            {
                ItemManager.Instance.currentSelectedItem.addUsedCount();
            }

        }

        /// <summary>
        /// Activate
        /// </summary>
        // ----------------------------------------------------------------------------------
        void activateBeforeEvolution()
        {

            foreach (var val in this.m_activateBeforeEvolutionList)
            {
                val.gameObject.SetActive(true);
            }

        }

        /// <summary>
        /// Dectivate
        /// </summary>
        // ----------------------------------------------------------------------------------
        void deactivateAfterEvolution()
        {

            foreach (var val in this.m_deactivateAfterEvolutionList)
            {
                val.gameObject.SetActive(false);
            }

        }

        /// <summary>
        /// Play all evolve anims
        /// </summary>
        /// <param name="immediately">immediately</param>
        // ----------------------------------------------------------------------------------
        void playAllEvolveAnims(bool immediately)
        {

            foreach (var val in this.m_refAnimList)
            {
                val.playEvolveAnim(immediately);
            }

        }

        /// <summary>
        /// Unlock all
        /// </summary>
        // ----------------------------------------------------------------------------------
        void unlockAll()
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

                    {

                        if (this.m_userProgressData.evolved)
                        {
                            // evolve
                            {
                                StartCoroutine(this.evolveIE(true));
                            }

                        }

                    }

                }

            }

        }

    }

}
