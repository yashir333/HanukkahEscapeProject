using System;
using System.Collections;
using System.Collections.Generic;
using SSC;
using UnityEngine;
using UnityEngine.UI;

namespace ciitt.EscapeGameKit
{

    /// <summary>
    /// Item object script
    /// </summary>
    public class ItemObjectScript : ClickableColliderScript
    {

        /// <summary>
        /// User progress data
        /// </summary>
        [Serializable]
        class UserProgressData
        {

            public bool possession = false;
            public bool evolved = false;

            public void clear()
            {

                this.possession = false;
                this.evolved = false;

            }

        }

        /// <summary>
        /// EvolveAnimHolder list
        /// </summary>
        [SerializeField]
        [Tooltip("EvolveAnimHolder list")]
        List<EvolveAnimHolder> m_refAnimList = new List<EvolveAnimHolder>();

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
        /// Reference to ItemWaitingRoomScript
        /// </summary>
        ItemWaitingRoomScript m_refWaitingRoom = null;

        /// <summary>
        /// UserProgressData
        /// </summary>
        UserProgressData m_userProgressData = new UserProgressData();

        /// <summary>
        /// User data key
        /// </summary>
        string m_userDataKey = "";

        // ----------------------------------------------------------------------------------

        /// <summary>
        /// Evolved
        /// </summary>
        public bool evolved { get { return this.m_userProgressData.evolved; } }

#if UNITY_EDITOR

        void checkItemWaitingroomReference()
        {
            if (!this.m_refWaitingRoom)
            {
                Debug.LogError("m_refWaitingRoom is null : " + Funcs.createHierarchyPath(this.transform));
            }
        }

#endif

        /// <summary>
        /// Awake
        /// </summary>
        // ----------------------------------------------------------------------------------
        protected override void Awake()
        {

            base.Awake();

#if UNITY_EDITOR

            // LayerMask
            {

                Transform[] transArray = this.GetComponentsInChildren<Transform>(true);

                foreach(var trans in transArray)
                {

                    if (LayerMask.NameToLayer("Item") != trans.gameObject.layer)
                    {
                        Debug.LogError("layer is not [Item] : " + Funcs.createHierarchyPath(trans));
                    }

                }

            }

            // m_refAnimList
            {

                for (int i = this.m_refAnimList.Count - 1; i >= 0; i--)
                {

                    if (!this.m_refAnimList[i])
                    {
                        Debug.LogError("m_refAnimList contains null : " + Funcs.createHierarchyPath(this.transform));
                        this.m_refAnimList.RemoveAt(i);
                    }

                }

            }

            // checkItemWaitingroomReference
            {
                Invoke("checkItemWaitingroomReference", 0.1f);
            }

#endif

            // m_userDataKey
            {
                this.m_userDataKey = SystemManager.Instance.createKeyPath(this.transform, this);
            }

        }

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

        }

        /// <summary>
        /// Function when unlocked
        /// </summary>
        // ----------------------------------------------------------------------------------
        protected override void actionWhenUnlocked()
        {

            this.getItem();

        }

        /// <summary>
        /// Get item
        /// </summary>
        // ----------------------------------------------------------------------------------
        void getItem()
        {

            MainGameSceneState mgsState = this.m_refMainGameSceneStateWatcher.state();

            if (mgsState.stateEnum != MainGameSceneState.StateEnum.MainGameSceneMain)
            {
                return;
            }

            // ----------------

            if(this.m_refWaitingRoom)
            {
                this.m_refWaitingRoom.getItemIfNotHave(this, true);
            }

            // possession
            {
                this.m_userProgressData.possession = true;
            }

        }

        /// <summary>
        /// Play all evolve anims
        /// </summary>
        /// <param name="immediately">immediately</param>
        // ----------------------------------------------------------------------------------
        public void playAllEvolveAnims(bool immediately)
        {

            foreach (var val in this.m_refAnimList)
            {
                val.playEvolveAnim(immediately);
            }

            this.m_userProgressData.evolved = true;

        }

        /// <summary>
        /// Is all evolve anim finished
        /// </summary>
        /// <returns>finished</returns>
        // ----------------------------------------------------------------------------------
        public bool isAllEvolveAnimFinished()
        {

            bool ret = true;

            foreach (var val in this.m_refAnimList)
            {
                ret = ret && (val.currentEvolveAnimState() == EvolveAnimState.Evolved);
            }

            return ret;

        }

        /// <summary>
        /// Set waiting room reference
        /// </summary>
        /// <param name="waitingRoom">ItemWaitingRoomScript</param>
        // ----------------------------------------------------------------------------------
        public void setWaitingRoomReference(ItemWaitingRoomScript waitingRoom)
        {
            this.m_refWaitingRoom = waitingRoom;
        }

        /// <summary>
        /// Activate
        /// </summary>
        // ----------------------------------------------------------------------------------
        public void activateBeforeEvolution()
        {

            foreach (var val in this.m_activateBeforeEvolutionList)
            {
                val.gameObject.SetActive(true);
            }

        }

        /// <summary>
        /// Deactivate
        /// </summary>
        // ----------------------------------------------------------------------------------
        public void deactivateAfterEvolution()
        {

            foreach (var val in this.m_deactivateAfterEvolutionList)
            {
                val.gameObject.SetActive(false);
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
                this.m_userDataKey,
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

                    if (this.m_userProgressData.possession)
                    {

                        if (this.m_userProgressData.evolved)
                        {

                            // activateBeforeEvolution
                            {
                                this.activateBeforeEvolution();
                            }

                            // playAllEvolveAnims
                            {
                                this.playAllEvolveAnims(true);
                            }

                            // deactivateAfterEvolution
                            {
                                this.deactivateAfterEvolution();
                            }

                        }

                        if (this.m_refWaitingRoom)
                        {
                            this.m_refWaitingRoom.getItemIfNotHave(this, false);
                        }
                        
                    }

                }

            }

        }

    }

}
