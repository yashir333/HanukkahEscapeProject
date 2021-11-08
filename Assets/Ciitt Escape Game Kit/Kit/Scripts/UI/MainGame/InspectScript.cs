using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ciitt.EscapeGameKit
{

    [RequireComponent(typeof(Button))]
    public class InspectScript : MonoBehaviour
    {

        /// <summary>
        /// Reference to Button
        /// </summary>
        Button m_refButton = null;

        /// <summary>
        /// Reference to ItemManager
        /// </summary>
        ItemManager m_refItemManager = null;

        /// <summary>
        /// Start
        /// </summary>
        // -----------------------------------------------------------------------
        void Start()
        {

            // m_refButton
            {
                this.m_refButton = this.GetComponent<Button>();
            }

            // m_refItemManager
            {
                this.m_refItemManager = ItemManager.Instance;
            }

        }

        /// <summary>
        /// Update
        /// </summary>
        // -----------------------------------------------------------------------
        void Update()
        {

            if (this.m_refButton.interactable != this.m_refItemManager.currentSelectedItem)
            {
                this.m_refButton.interactable = this.m_refItemManager.currentSelectedItem;
            }

        }

    }

}
