using System;
using System.Collections;
using System.Collections.Generic;
using SSC;
using UnityEngine;

namespace ciitt.EscapeGameKit
{

    /// <summary>
    /// Item waiting room
    /// </summary>
    public class ItemWaitingRoomScript : MonoBehaviour
    {

        /// <summary>
        /// User progress data
        /// </summary>
        [Serializable]
        class UserProgressData
        {

            public bool evolved = false;
            public int usedCount = 0;

            public void clear()
            {
                this.evolved = false;
                this.usedCount = 0;
            }

        }

        /// <summary>
        /// Usable count
        /// </summary>
        [SerializeField]
        [Tooltip("Usable count")]
        int m_usableCount = 1;

        /// <summary>
        /// ConditionsForEvolution
        /// </summary>
        [SerializeField]
        [Tooltip("ConditionsForEvolution")]
        [UnityEngine.Serialization.FormerlySerializedAs("m_condition")]
        ConditionsForEvolution m_conditionsForEvolution = new ConditionsForEvolution();

        /// <summary>
        /// Main room
        /// </summary>
        [SerializeField]
        [Tooltip("Main room")]
        Transform m_refMainRoom = null;

        /// <summary>
        /// Sub room
        /// </summary>
        [SerializeField]
        [Tooltip("Sub room")]
        Transform m_refSubRoom = null;

        /// <summary>
        /// Reference to ItemImageScript
        /// </summary>
        [SerializeField]
        [Tooltip("Reference to ItemImageScript")]
        ItemImageScript m_refItemImage = null;

        /// <summary>
        /// Sprite before evolution
        /// </summary>
        [SerializeField]
        [Tooltip("Sprite before evolution")]
        Sprite m_beforeSprite = null;

        /// <summary>
        /// Sprite after evolution
        /// </summary>
        [SerializeField]
        [Tooltip("Sprite after evolution")]
        Sprite m_afterSprite = null;

        /// <summary>
        /// Reference to Items
        /// </summary>
        [SerializeField]
        [Tooltip("Reference to Items")]
        List<ItemObjectScript> m_refItems = new List<ItemObjectScript>();

        /// <summary>
        /// LanguageAndTexts
        /// </summary>
        [SerializeField]
        [Tooltip("LanguageAndTexts")]
        LanguageAndTexts m_beforeEvolutionItemText = new LanguageAndTexts("");

        /// <summary>
        /// LanguageAndTexts
        /// </summary>
        [SerializeField]
        [Tooltip("LanguageAndTexts")]
        LanguageAndTexts m_afterEvolutionItemText = new LanguageAndTexts("");

        /// <summary>
        /// Current possession items
        /// </summary>
        HashSet<ItemObjectScript> m_currentPossessionItems = new HashSet<ItemObjectScript>();

        /// <summary>
        /// UserProgressData
        /// </summary>
        UserProgressData m_userProgressData = new UserProgressData();

        // ----------------------------------------------------------------------------------------------

        /// <summary>
        /// Evolved
        /// </summary>
        public bool evolved { get { return this.m_userProgressData.evolved; } }

        /// <summary>
        /// Max item count
        /// </summary>
        public int maxItemCount { get { return this.m_refItems.Count; } }

        /// <summary>
        /// Start
        /// </summary>
        // ----------------------------------------------------------------------------------------------
        void Start()
        {

#if UNITY_EDITOR

            if (!this.m_refMainRoom)
            {
                Debug.LogError("m_refMainRoom is null : " + Funcs.createHierarchyPath(this.transform));
            }

            if (!this.m_refSubRoom)
            {
                Debug.LogError("m_refSubRoom is null : " + Funcs.createHierarchyPath(this.transform));
            }

            if (!this.m_refItemImage)
            {
                Debug.LogError("m_refItemImage is null : " + Funcs.createHierarchyPath(this.transform));
            }

            if(this.m_refItems.Count <= 0)
            {
                Debug.LogError("m_refItems is empty : " + Funcs.createHierarchyPath(this.transform));
            }

            for (int i = this.m_refItems.Count - 1; i >= 0; i--)
            {

                if(!this.m_refItems[i])
                {
                    Debug.LogError("m_refItems contains null : " + Funcs.createHierarchyPath(this.transform));
                    this.m_refItems.RemoveAt(i);
                }

            }

            if(!this.m_beforeSprite)
            {
                Debug.LogError("m_beforeSprite is null : " + Funcs.createHierarchyPath(this.transform));
            }

            if(this.m_conditionsForEvolution.item && !this.m_afterSprite)
            {
                Debug.LogError("m_afterSprite is null : " + Funcs.createHierarchyPath(this.transform));
            }

#endif

            // m_refItems
            {
                foreach (var val in this.m_refItems)
                {
                    val.setWaitingRoomReference(this);
                }
            }

            // m_refItemImage
            {
                if(this.m_refItemImage)
                {
                    this.m_refItemImage.setWaitingRoomReference(this);
                }
            }

            // CustomReduxManager
            {
                CustomReduxManager.CustomReduxManagerInstance.addSceneChangeStateReceiver(this.onSceneChangeStateReceiver);
                CustomReduxManager.CustomReduxManagerInstance.addMainGameSceneStateReceiver(this.onMainGameSceneStateReceiver);
                CustomReduxManager.CustomReduxManagerInstance.addUserProgressDataSignalReceiver(this.onUserProgressDataSignal);
            }

            //m_beforeEvolutionItemText m_afterEvolutionItemText
            {
                this.m_beforeEvolutionItemText.initDictionary(this.transform);
                this.m_afterEvolutionItemText.initDictionary(this.transform);
            }

        }

        /// <summary>
        /// Evolve by item if needed
        /// </summary>
        // ----------------------------------------------------------------------------------------------
        public void evolveByFieldObjectIfNeeded()
        {

            if (this.m_userProgressData.evolved || this.currentItemCount() <= 0)
            {
                return;
            }

            // evolved
            {
                this.m_userProgressData.evolved = true;
            }

            // --------------

            // activateBeforeEvolution
            {
                this.activateBeforeEvolution();
            }

            // m_refItemImage
            {
                if (this.m_refItemImage)
                {
                    this.m_refItemImage.setItemImage(this.m_afterSprite);
                }
            }

            // playAllEvolveAnims
            {

                foreach (var val in this.m_refItems)
                {
                    val.playAllEvolveAnims(true);
                }

            }

            // deactivateAfterEvolution
            {
                this.funcsAfterEvolution();
            }

            // MainGameSceneState
            {

                StateWatcher<MainGameSceneState> mgssWatcher =
                    CustomReduxManager.CustomReduxManagerInstance.MainGameSceneStateWatcher;

                mgssWatcher.state().setItemShowroomState(ItemManager.Instance.currentSelectedItem);

            }

        }

        /// <summary>
        /// Add used count
        /// </summary>
        // ----------------------------------------------------------------------------------------------
        public void addUsedCount()
        {
            this.m_userProgressData.usedCount++;
            this.setAlreadyUsedIfNeeded();
        }

        /// <summary>
        /// Set already used
        /// </summary>
        // ----------------------------------------------------------------------------------------------
        void setAlreadyUsedIfNeeded()
        {

            if (this.m_userProgressData.usedCount >= this.m_usableCount)
            {

                if (this.m_refItemImage)
                {
                    this.m_refItemImage.setAlreadyUsed();
                }

            }

        }

        /// <summary>
        /// Evolve by item if needed
        /// </summary>
        // ----------------------------------------------------------------------------------------------
        public void evolveByItemIfNeeded()
        {

            if(this.m_userProgressData.evolved || this.currentItemCount() <= 0 || !ItemManager.Instance.currentSelectedItem)
            {
                return;
            }

            if(!this.m_conditionsForEvolution.matchConditions(ItemManager.Instance.currentSelectedItem))
            {
                return;
            }

            // evolved
            {
                this.m_userProgressData.evolved = true;
            }

            // activateBeforeEvolution
            {
                this.activateBeforeEvolution();
            }

            // playAllEvolveAnims
            {

                foreach (var val in this.m_refItems)
                {
                    val.playAllEvolveAnims(false);
                }

            }

            // playSe
            {
                SoundManager.Instance.playSe(SoundManager.SeType.Evolve);
            }

            // MainGameSceneState
            {

                StateWatcher<MainGameSceneState> mgssWatcher =
                    CustomReduxManager.CustomReduxManagerInstance.MainGameSceneStateWatcher;

                mgssWatcher.state().setState(MainGameSceneState.StateEnum.MainGameSceneItemEvolution);

            }

            // funcsAfterEvolution
            {
                StartCoroutine(this.funcsAfterEvolution());
            }

            // addUsedCount
            {
                ItemManager.Instance.currentSelectedItem.addUsedCount();
            }

        }

        /// <summary>
        /// Set active true
        /// </summary>
        // ----------------------------------------------------------------------------------
        void activateBeforeEvolution()
        {

            // activateBeforeEvolution
            {

                foreach (var val in this.m_refItems)
                {
                    val.activateBeforeEvolution();
                }

            }

        }

        /// <summary>
        /// After evolution
        /// </summary>
        // ----------------------------------------------------------------------------------
        IEnumerator funcsAfterEvolution()
        {

            // wait
            {

                bool isAllAnimFinished = false;

                while (!isAllAnimFinished)
                {

                    isAllAnimFinished = true;

                    foreach (var val in this.m_refItems)
                    {

                        if (!val.isAllEvolveAnimFinished())
                        {
                            isAllAnimFinished = false;
                            yield return null;
                            break;
                        }

                    }

                    yield return null;

                }

            }

            // deactivateAfterEvolution
            {

                foreach (var val in this.m_refItems)
                {
                    val.deactivateAfterEvolution();
                }

            }

            // MainGameSceneState
            {

                StateWatcher<MainGameSceneState> mgssWatcher =
                    CustomReduxManager.CustomReduxManagerInstance.MainGameSceneStateWatcher;

                mgssWatcher.state().setItemShowroomState(this);

            }

            // m_refItemImage
            {
                if (this.m_refItemImage)
                {
                    this.m_refItemImage.setItemImage(this.m_afterSprite);
                }
            }

        }

        /// <summary>
        /// Current item count
        /// </summary>
        /// <returns>count</returns>
        // ----------------------------------------------------------------------------------------------
        public int currentItemCount()
        {

            int ret = 0;

            if (this.m_refMainRoom)
            {
                ret += this.m_refMainRoom.childCount;
            }

            if (this.m_refSubRoom)
            {
                ret += this.m_refSubRoom.childCount;
            }

            return ret;

        }

        /// <summary>
        /// MainGameSceneState receiver
        /// </summary>
        /// <param name="mgsState">MainGameSceneState</param>
        // -----------------------------------------------------------------------
        void onMainGameSceneStateReceiver(MainGameSceneState mgsState)
        {

            if (mgsState.stateEnum == MainGameSceneState.StateEnum.MainGameSceneItemShowroom)
            {

                if (mgsState.currentSelectedItemInfo.currentShowroomItem == this)
                {
                    this.gameObject.SetActive(true);
                }

                else
                {
                    this.gameObject.SetActive(false);
                }

            }

        }

        /// <summary>
        /// Has item
        /// </summary>
        /// <param name="item">ItemObjectScript</param>
        /// <returns>has</returns>
        // ----------------------------------------------------------------------------------------------
        public bool hasItem(ItemObjectScript item)
        {

            if (!item || !this.m_refMainRoom || !this.m_refSubRoom)
            {
                return false;
            }

            // -----------------------

            return this.m_currentPossessionItems.Contains(item);

        }

        /// <summary>
        /// Get item
        /// </summary>
        /// <param name="item">ItemObjectScript</param>
        /// <param name="showItemShowroom">show item showroom</param>
        // ----------------------------------------------------------------------------------------------
        public void getItemIfNotHave(ItemObjectScript item, bool showItemShowroom)
        {
         
            if(!item || !this.m_refMainRoom || !this.m_refSubRoom)
            {
                return;
            }

            if(this.hasItem(item))
            {
                return;
            }

            // -----------------------

            // item
            {

                if (this.m_refMainRoom.childCount >= 1)
                {
                    item.transform.SetParent(this.m_refSubRoom);
                }

                else
                {
                    item.transform.SetParent(this.m_refMainRoom);
                }

                item.transform.localPosition = Vector3.zero;
                item.transform.localRotation = Quaternion.identity;
                item.transform.localScale = Vector3.one;

            }

            // m_refItemImage
            {

                if (this.m_refItemImage)
                {
                    this.m_refItemImage.setItemImage((item.evolved) ? this.m_afterSprite : this.m_beforeSprite);
                    this.m_refItemImage.enableItemImage(true);
                }

            }

            // m_currentPossessionItems
            {
                this.m_currentPossessionItems.Add(item);
            }

            // setItemShowroomState
            {

                if (showItemShowroom)
                {

                    StateWatcher<MainGameSceneState> watcher = CustomReduxManager.CustomReduxManagerInstance.MainGameSceneStateWatcher;

                    watcher.state().setItemShowroomState(this);

                }

                else
                {
                    this.gameObject.SetActive(false);
                }

            }

        }

        /// <summary>
        /// Get description text
        /// </summary>
        /// <returns>description text</returns>
        // ----------------------------------------------------------------------------------------------
        public string descriptionText()
        {

            if(this.m_userProgressData.evolved)
            {
                return this.m_afterEvolutionItemText.getText(SystemManager.Instance.configDataSO.systemLanguage);
            }

            else
            {
                return this.m_beforeEvolutionItemText.getText(SystemManager.Instance.configDataSO.systemLanguage);
            }

        }

        /// <summary>
        /// Set ItemImageScript reference
        /// </summary>
        // ----------------------------------------------------------------------------------------------
        public void setItemImageScriptReference(ItemImageScript iis)
        {
            this.m_refItemImage = iis;
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

            if (scState.stateEnum == SceneChangeState.StateEnum.NowLoadingOutro)
            {

                CustomSceneChangeManager cscManager = CustomSceneChangeManager.Instance as CustomSceneChangeManager;

                if (cscManager.isLoadingCurrentSceneWithUserProgressData())
                {

                    this.m_userProgressData = cscManager.getDataFromCurrentUserProgressData<UserProgressData>(this.transform, this);

                    this.setAlreadyUsedIfNeeded();

                }

            }

        }

    }

}
