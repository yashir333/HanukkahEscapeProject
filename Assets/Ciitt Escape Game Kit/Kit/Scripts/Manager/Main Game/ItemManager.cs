using System.Collections;
using System.Collections.Generic;
using SSC;
using UnityEngine;

namespace ciitt.EscapeGameKit
{

    /// <summary>
    /// Item manager
    /// </summary>
    public class ItemManager : SingletonMonoBehaviour<ItemManager>
    {

        /// <summary>
        /// Strength for yaw
        /// </summary>
        [SerializeField]
        [Tooltip("Strength for yaw")]
        float m_strengthYaw = 1.0f;

        /// <summary>
        /// Strength for pitch
        /// </summary>
        [SerializeField]
        [Tooltip("Strength for pitch")]
        float m_strengthPitch = 1.0f;

        /// <summary>
        /// Current selected item
        /// </summary>
        ItemWaitingRoomScript m_currentSelectedItem = null;

        /// <summary>
        /// Current showing item
        /// </summary>
        ItemWaitingRoomScript m_currentShowingItem = null;

        /// <summary>
        /// Reference to StateWatcher<MainGameSceneState>
        /// </summary>
        StateWatcher<MainGameSceneState> m_refMainGameSceneStateWatcher = null;

        /// <summary>
        /// Previous
        /// </summary>
        Vector3 m_previousMousePosition = Vector3.zero;

        /// <summary>
        /// Delta position X
        /// </summary>
        float m_deltaPositionX = 0.0f;

        /// <summary>
        /// Delta position Y
        /// </summary>
        float m_deltaPositionY = 0.0f;

        // ----------------------------------------------------------------------------------------------

        /// <summary>
        /// Current selected item
        /// </summary>
        public ItemWaitingRoomScript currentSelectedItem { get { return this.m_currentSelectedItem; } set { this.m_currentSelectedItem = value; } }

        /// <summary>
        /// Called in Awake
        /// </summary>
        // ----------------------------------------------------------------------------------------------
        protected override void initOnAwake()
        {

            this.m_refMainGameSceneStateWatcher = CustomReduxManager.CustomReduxManagerInstance.MainGameSceneStateWatcher;

        }

        /// <summary>
        /// Start
        /// </summary>
        // ----------------------------------------------------------------------------------------------
        void Start()
        {

            // CustomReduxManager
            {
                CustomReduxManager.CustomReduxManagerInstance.addMainGameSceneStateReceiver(this.onMainGameSceneStateReceiver);
            }

        }

        /// <summary>
        /// Update
        /// </summary>
        // ----------------------------------------------------------------------------------------------
        void Update()
        {

            this.calcDeltaPosition();

            this.rotateShowroomItem();

        }

        /// <summary>
        /// Calculate delta position
        /// </summary>
        // -----------------------------------------------------------------------
        void calcDeltaPosition()
        {

            Vector3 mousePosition = Input.mousePosition;

            if (Input.touchCount > 0)
            {
                mousePosition = Input.GetTouch(0).position;
            }

            // -------------------------

            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.touchCount >= 2)
            {
                this.m_deltaPositionX = 0.0f;
                this.m_deltaPositionY = 0.0f;
            }

            else if (Input.touchCount == 1 && Input.GetTouch(0).fingerId != 0)
            {
                this.m_deltaPositionX = 0.0f;
                this.m_deltaPositionY = 0.0f;
            }

            else
            {
                this.m_deltaPositionX = (this.m_previousMousePosition.x - mousePosition.x) / (float)Screen.width;
                this.m_deltaPositionY = (this.m_previousMousePosition.y - mousePosition.y) / (float)Screen.height;

            }

            // m_previousClickedPositionDown
            {
                this.m_previousMousePosition = mousePosition;
            }

        }

        /// <summary>
        /// onClick for item showroom
        /// </summary>
        // ----------------------------------------------------------------------------------------------
        public void onClickItemShowroomPanel()
        {

            if(this.m_currentShowingItem)
            {
                this.m_currentShowingItem.evolveByItemIfNeeded();
            }

        }

        /// <summary>
        /// Rotate
        /// </summary>
        // ----------------------------------------------------------------------------------------------
        void rotateShowroomItem()
        {

            MainGameSceneState mgsState = this.m_refMainGameSceneStateWatcher.state();

            if (
                mgsState.stateEnum != MainGameSceneState.StateEnum.MainGameSceneItemShowroom ||
                !mgsState.currentSelectedItemInfo.currentShowroomItem
                )
            {
                return;
            }

            // --------------------

            if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
            {

                if (Input.touchCount > 0)
                {

                    if (Input.touches[0].phase != TouchPhase.Moved)
                    {
                        return;
                    }

                }

                float moveVertical = -this.m_deltaPositionY * this.m_strengthPitch;
                float moveHorizontal = this.m_deltaPositionX * this.m_strengthYaw;

                mgsState.currentSelectedItemInfo.currentShowroomItem.transform.Rotate(
                    new Vector3(moveVertical, moveHorizontal, 0.0f), Space.World);

            }

        }

        /// <summary>
        /// Inspecting current selected item
        /// </summary>
        // ----------------------------------------------------------------------------------------------
        public void inspectCurrentSelectedItem()
        {

            if(this.m_currentSelectedItem)
            {

                this.m_refMainGameSceneStateWatcher = CustomReduxManager.CustomReduxManagerInstance.MainGameSceneStateWatcher;

                this.m_refMainGameSceneStateWatcher.state().setItemShowroomState(this.m_currentSelectedItem);

            }

        }

        /// <summary>
        /// Quit inspecting current selected item
        /// </summary>
        // ----------------------------------------------------------------------------------------------
        public void quitInspecting()
        {

            this.m_refMainGameSceneStateWatcher = CustomReduxManager.CustomReduxManagerInstance.MainGameSceneStateWatcher;

            this.m_refMainGameSceneStateWatcher.state().setState(MainGameSceneState.StateEnum.MainGameSceneMain);

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
                this.m_currentShowingItem = mgsState.currentSelectedItemInfo.currentShowroomItem;
            }

        }

    }

}
