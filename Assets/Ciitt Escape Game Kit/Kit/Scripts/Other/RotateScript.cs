using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ciitt.EscapeGameKit
{

    /// <summary>
    /// Rotate
    /// </summary>
    public class RotateScript : MonoBehaviour
    {

        [SerializeField]
        Vector3 m_speed = Vector3.zero;

        /// <summary>
        /// Update
        /// </summary>
        // ----------------------------------------------------------------------
        void Update()
        {
            this.transform.Rotate(this.m_speed);
        }

    }

}
