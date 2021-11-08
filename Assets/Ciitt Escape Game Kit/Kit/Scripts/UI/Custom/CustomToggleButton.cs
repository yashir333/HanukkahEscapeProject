using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ciitt.EscapeGameKit
{

    /// <summary>
    /// Custom Toggle
    /// </summary>
    public class CustomToggleButton : Toggle
    {

        public Button.ButtonClickedEvent onClick;

        /// <summary>
        /// OnPointerClick
        /// </summary>
        /// <param name="eventData">PointerEventData</param>
        // -------------------------------------------------------------------------------------------
        public override void OnPointerClick(PointerEventData eventData)
        {

            base.OnPointerClick(eventData);
            
            if(this.interactable)
            {
                this.onClick.Invoke();
            }

        }

        /// <summary>
        /// OnSubmit
        /// </summary>
        /// <param name="eventData">BaseEventData</param>
        // -------------------------------------------------------------------------------------------
        public override void OnSubmit(BaseEventData eventData)
        {

            base.OnSubmit(eventData);

            if (this.interactable)
            {
                this.onClick.Invoke();
            }

        }

        /// <summary>
        /// onClick
        /// </summary>
        // -------------------------------------------------------------------------------------------
        public void invokeOnClick()
        {

            if (this.interactable)
            {
                this.onClick.Invoke();
            }

        }

        /// <summary>
        /// Set toggle and click
        /// </summary>
        // -------------------------------------------------------------------------------------------
        public void setOnAndInvokeClick()
        {

            this.isOn = true;

            if (this.interactable)
            {
                this.onClick.Invoke();
            }

        }

    }

}
