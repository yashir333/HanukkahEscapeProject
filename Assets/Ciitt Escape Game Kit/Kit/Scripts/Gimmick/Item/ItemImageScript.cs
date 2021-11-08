using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ciitt.EscapeGameKit
{

    /// <summary>
    /// Item Image
    /// </summary>
    public class ItemImageScript : MonoBehaviour
    {

        /// <summary>
        /// Reference to Item Image
        /// </summary>
        [SerializeField]
        [Tooltip("Reference to Item Image")]
        Image m_refItemImage = null;

        /// <summary>
        /// Reference to Checkmark Image
        /// </summary>
        [SerializeField]
        [Tooltip("Reference to Checkmark Image")]
        Image m_refCheckmarkImage = null;

        /// <summary>
        /// Reference to Toggle
        /// </summary>
        [SerializeField]
        [Tooltip("Reference to Toggle")]
        Toggle m_refToggle = null;

        /// <summary>
        /// Reference to ItemWaitingRoomScript
        /// </summary>
        ItemWaitingRoomScript m_refWaitingRoom = null;

        // --------------------------------------------------------------------------

        /// <summary>
        /// Reference to ItemWaitingRoomScript
        /// </summary>
        public ItemWaitingRoomScript itemObjectScript { get { return this.m_refWaitingRoom; } }


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
        /// Start
        /// </summary>
        // --------------------------------------------------------------------------
        void Start()
        {

#if UNITY_EDITOR


            if (!this.m_refItemImage)
            {
                Debug.LogError("m_refItemImage is null : " + Funcs.createHierarchyPath(this.transform));
            }

            if (!this.m_refCheckmarkImage)
            {
                Debug.LogError("m_refCheckmarkImage is null : " + Funcs.createHierarchyPath(this.transform));
            }

            if (!this.m_refToggle)
            {
                Debug.LogError("m_refToggle is null : " + Funcs.createHierarchyPath(this.transform));
            }

            // checkItemWaitingroomReference
            {
                Invoke("checkItemWaitingroomReference", 0.1f);
            }

#endif

            // m_refCheckmarkImage
            {

                if (this.m_refCheckmarkImage)
                {
                    this.m_refCheckmarkImage.enabled = false;
                }

            }

            // enableItemImage
            {
                this.enableItemImage(false);
            }

        }

        /// <summary>
        /// Enable image
        /// </summary>
        /// <param name="enable">enable</param>
        // --------------------------------------------------------------------------
        public void enableItemImage(bool enable)
        {

            if(this.m_refItemImage)
            {
                this.m_refItemImage.enabled = enable;
            }

            if (this.m_refToggle)
            {
                this.m_refToggle.interactable = enable;
            }

        }

        /// <summary>
        /// Set Sprite
        /// </summary>
        /// <param name="sprite">Sprite</param>
        // --------------------------------------------------------------------------
        public void setItemImage(Sprite sprite)
        {

            if (this.m_refItemImage)
            {
                this.m_refItemImage.sprite = sprite;
            }

        }

        /// <summary>
        /// Set used
        /// </summary>
        // --------------------------------------------------------------------------
        public void setAlreadyUsed()
        {

            if (this.m_refToggle)
            {
                this.m_refToggle.interactable = false;
            }

            if(ItemManager.Instance.currentSelectedItem == this.m_refWaitingRoom)
            {
                ItemManager.Instance.currentSelectedItem = null;
            }

            this.showSelectedCheckmarkImage(false);

        }

        /// <summary>
        /// Show selected checkmark image
        /// </summary>
        /// <param name="selected">selected</param>
        // --------------------------------------------------------------------------
        void showSelectedCheckmarkImage(bool selected)
        {

            if (!this.m_refCheckmarkImage)
            {
                return;
            }

            // ----------------------------

            this.m_refCheckmarkImage.enabled = selected;

        }

        /// <summary>
        /// Select item
        /// </summary>
        // --------------------------------------------------------------------------
        public void selectOrDeselectItem()
        {

            SoundManager.Instance.playSe(SoundManager.SeType.SelectItem);

            if (this.m_refWaitingRoom == ItemManager.Instance.currentSelectedItem)
            {
                ItemManager.Instance.currentSelectedItem = null;
                this.showSelectedCheckmarkImage(false);
            }

            else
            {
                ItemManager.Instance.currentSelectedItem = this.m_refWaitingRoom;
                this.showSelectedCheckmarkImage(true);
            }

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

    }

}
