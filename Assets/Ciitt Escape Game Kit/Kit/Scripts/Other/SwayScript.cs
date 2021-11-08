using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ciitt.EscapeGameKit
{

    public class SwayScript : MonoBehaviour
    {

        [SerializeField]
        Vector3 m_from = Vector3.zero;

        [SerializeField]
        Vector3 m_to = Vector3.zero;

        [SerializeField]
        [Range(0.1f, 10.0f)]
        float m_seconds = 1.0f;

        float timer = 0.0f;

        /// <summary>
        /// Update
        /// </summary>
        // ----------------------------------------------------------------------
        void Update()
        {

            this.timer = (this.timer + Time.deltaTime) % this.m_seconds;

            Vector3 rot = this.transform.localEulerAngles;

            float normalized = Mathf.Sin(Mathf.PI * (this.timer / this.m_seconds));

            normalized *= normalized;

            rot.x = Mathf.Lerp(this.m_from.x, this.m_to.x, normalized);
            rot.y = Mathf.Lerp(this.m_from.y, this.m_to.y, normalized);
            rot.z = Mathf.Lerp(this.m_from.z, this.m_to.z, normalized);

            this.transform.localEulerAngles = rot;

        }

    }

}