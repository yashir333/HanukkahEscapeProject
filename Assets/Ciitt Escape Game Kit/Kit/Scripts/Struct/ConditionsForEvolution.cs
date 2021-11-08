using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ciitt.EscapeGameKit
{

    [Serializable]
    public class ConditionsForEvolution
    {

        /// <summary>
        /// Item
        /// </summary>
        [Tooltip("Item")]
        public ItemWaitingRoomScript item = null;

        /// <summary>
        /// Required number of items
        /// </summary>
        [Tooltip("Required number of items")]
        public int requiredNumberOfItems = 1;

        /// <summary>
        /// Required revolution
        /// </summary>
        [Tooltip("Required revolution")]
        public bool requiredEvolution = false;

        /// <summary>
        /// Match conditions
        /// </summary>
        /// <param name="item">ItemWaitingRoomScript</param>
        /// <returns>match</returns>
        // ----------------------------------------------------------------------------------------------
        public bool matchConditions(ItemWaitingRoomScript _item)
        {

            if (!item || !_item)
            {
                return false;
            }

            return (
                this.item == _item &&
                this.requiredEvolution == _item.evolved &&
                this.requiredNumberOfItems <= _item.currentItemCount()
                )
                ;

        }

    }

}
