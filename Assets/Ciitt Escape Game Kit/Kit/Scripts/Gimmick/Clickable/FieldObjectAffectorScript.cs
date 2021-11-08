using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ciitt.EscapeGameKit
{

    /// <summary>
    /// Affector
    /// </summary>
    public class FieldObjectAffectorScript : ClickableColliderScript
    {

        /// <summary>
        /// Target item
        /// </summary>
        [SerializeField]
        [Tooltip("Target item")]
        ItemWaitingRoomScript m_refTargetItem = null;

        /// <summary>
        /// Start
        /// </summary>
        // ----------------------------------------------------------------------------------
        protected override void Start()
        {

            base.Start();

#if UNITY_EDITOR

            if (!this.m_refTargetItem)
            {
                Debug.LogError("m_refTargetItem is null : " + Funcs.createHierarchyPath(this.transform));
            }

#endif

        }

        /// <summary>
        /// Function when unlocked
        /// </summary>
        // ----------------------------------------------------------------------------------
        protected override void actionWhenUnlocked()
        {

            if (
                this.m_refTargetItem &&
                this.m_refTargetItem == ItemManager.Instance.currentSelectedItem &&
                !this.m_refTargetItem.evolved
                )
            {
                this.m_refTargetItem.evolveByFieldObjectIfNeeded();
            }

        }

    }

}
