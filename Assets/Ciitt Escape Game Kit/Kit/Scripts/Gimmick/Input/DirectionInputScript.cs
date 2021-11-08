using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ciitt.EscapeGameKit
{

    /// <summary>
    /// Direction input
    /// </summary>
    public class DirectionInputScript : ButtonInputScript
    {

        /// <summary>
        /// Reference to SubmitDirectionScript
        /// </summary>
        [SerializeField]
        [Tooltip("Reference to SubmitDirectionScript")]
        SubmitDirectionScript m_refSubmitDirectionScript = null;

        /// <summary>
        /// DirectionEnum
        /// </summary>
        [SerializeField]
        [Tooltip("DirectionEnum")]
        DirectionEnum m_direction = DirectionEnum.Up;

        /// <summary>
        /// Start
        /// </summary>
        // --------------------------------------------------------------------------------------------
        protected override void Start()
        {

            base.Start();

#if UNITY_EDITOR

            if (!this.m_refSubmitDirectionScript)
            {
                Debug.LogError("m_refSubmitDirectionScript is null : " + Funcs.createHierarchyPath(this.transform));
            }

#endif

            // m_refSubmitDirectionScript
            {

                if (this.m_refSubmitDirectionScript)
                {
                    this.m_refSubmitDirectionScript.addInputButtonReference(this);
                }

            }

        }

        /// <summary>
        /// Function when unlocked
        /// </summary>
        // ----------------------------------------------------------------------------------
        protected override void actionWhenUnlocked()
        {
            
            if(this.m_refSubmitDirectionScript)
            {
                this.m_refSubmitDirectionScript.addUserInput(this.m_direction);
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
