using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ciitt.EscapeGameKit
{

    /// <summary>
    /// Open close anim
    /// </summary>
    public abstract class OpenCloseAnimHolder : MonoBehaviour
    {

        /// <summary>
        /// OpenCloseState
        /// </summary>
        public abstract OpenCloseState currentOpenCloseState();

        /// <summary>
        /// Play open anim
        /// </summary>
        /// <param name="immediately">immediately</param>
        public abstract void playOpenAnim(bool immediately);

        /// <summary>
        /// Play close anim
        /// </summary>
        /// <param name="immediately">immediately</param>
        public abstract void playCloseAnim(bool immediately);

        /// <summary>
        /// Delay seconds
        /// </summary>
        [SerializeField]
        [Tooltip("Delay seconds")]
        [Range(0.0f, 10.0f)]
        protected float m_delay = 0.0f;

    }

}
