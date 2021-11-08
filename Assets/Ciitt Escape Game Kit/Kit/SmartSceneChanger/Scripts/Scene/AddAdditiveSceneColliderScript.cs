using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SSC
{

    /// <summary>
    /// Add additive scene by collider
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class AddAdditiveSceneColliderScript : AddAdditiveSceneScript
    {

        /// <summary>
        /// Seconds for invoke in OnTriggerEnter
        /// </summary>
        [SerializeField]
        [Tooltip("Seconds for invoke in OnTriggerEnter")]
        protected float m_enterInvokeSeconds = 0.1f;

        /// <summary>
        /// Seconds for invoke in OnCollisionExit
        /// </summary>
        [SerializeField]
        [Tooltip("Seconds for invoke in OnCollisionExit")]
        protected float m_exitInvokeSeconds = 0.1f;

        /// <summary>
        /// Start
        /// </summary>
        // -----------------------------------------------------------------------------------------------
        protected virtual void Start()
        {

#if UNITY_EDITOR

            Collider collider = this.GetComponent<Collider>();

            if(!collider.isTrigger)
            {
                Debug.LogWarning("collider.isTrigger should be true : " + Funcs.CreateHierarchyPath(this.transform));
            }

#endif

        }

        /// <summary>
        /// OnTriggerEnter
        /// </summary>
        /// <param name="other">Collider</param>
        // -----------------------------------------------------------------------------------------------
        protected virtual void OnTriggerEnter(Collider other)
        {

            CancelInvoke();
            Invoke("addScene", this.m_enterInvokeSeconds);

        }

        /// <summary>
        /// OnTriggerExit
        /// </summary>
        /// <param name="other">Collider</param>
        // -----------------------------------------------------------------------------------------------
        protected virtual void OnTriggerExit(Collider other)
        {

            CancelInvoke();
            Invoke("unloadScene", this.m_exitInvokeSeconds);

        }

    }

}
