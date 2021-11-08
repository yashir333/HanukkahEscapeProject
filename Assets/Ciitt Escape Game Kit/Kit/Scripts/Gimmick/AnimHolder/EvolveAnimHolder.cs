using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ciitt.EscapeGameKit
{

    /// <summary>
    /// Evolve anim
    /// </summary>
    public abstract class EvolveAnimHolder : MonoBehaviour
    {

        /// <summary>
        /// Current EvolveAnimState
        /// </summary>
        public abstract EvolveAnimState currentEvolveAnimState();

        /// <summary>
        /// Play evolve anim
        /// </summary>
        /// <param name="immediately">immediately</param>
        public abstract void playEvolveAnim(bool immediately);

        /// <summary>
        /// Seconds
        /// </summary>
        [SerializeField]
        [Tooltip("Seconds")]
        [Range(0.0f, 10.0f)]
        protected float m_delay = 0.0f;

    }

}
