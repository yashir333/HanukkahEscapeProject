using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ciitt.EscapeGameKit
{

    /// <summary>
    /// Color input
    /// </summary>
    public class ColorInputScript : ButtonInputScript
    {

        /// <summary>
        /// Reference to SubmitColorScript
        /// </summary>
        [SerializeField]
        [Tooltip("Reference to SubmitColorScript")]
        SubmitColorScript m_refSubmitColorScript = null;

        /// <summary>
        /// ColorEnum
        /// </summary>
        [SerializeField]
        [Tooltip("ColorEnum")]
        ColorEnum m_color = ColorEnum.White;

        /// <summary>
        /// Start
        /// </summary>
        // --------------------------------------------------------------------------------------------
        protected override void Start()
        {

            base.Start();

#if UNITY_EDITOR

            if (!this.m_refSubmitColorScript)
            {
                Debug.LogError("m_refSubmitColorScript is null : " + Funcs.createHierarchyPath(this.transform));
            }

#endif

            // m_refSubmitDirectionScript
            {

                if (this.m_refSubmitColorScript)
                {
                    this.m_refSubmitColorScript.addInputButtonReference(this);
                }

            }

        }

        /// <summary>
        /// Function when unlocked
        /// </summary>
        // ----------------------------------------------------------------------------------
        protected override void actionWhenUnlocked()
        {

            if (this.m_refSubmitColorScript)
            {
                this.m_refSubmitColorScript.addUserInput(this.m_color);
            }

            // changeAndResumeColor
            {
                this.changeAndResumeColor();
            }

            // playSe
            {
                SoundManager.Instance.playSe(SoundManager.SeType.GimmickButton);
            }

        }

    }

}
