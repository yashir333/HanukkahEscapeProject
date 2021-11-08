using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ciitt.EscapeGameKit
{

    /// <summary>
    /// Config data SO
    /// </summary>
    [Serializable]
    public class ConfigDataSO : ScriptableObject
    {

        /// <summary>
        /// Data version
        /// </summary>
        [Tooltip("Data version")]
        public int dataVersion = 1;

        /// <summary>
        /// Target SystemLanguage
        /// </summary>
        [Tooltip("Target SystemLanguage")]
        public SystemLanguage systemLanguage = SystemLanguage.Japanese;

        /// <summary>
        /// Master volume
        /// </summary>
        [Range(0.0f, 1.0f)]
        [Tooltip("Master volume")]
        public float masterVolume01 = 1.0f;

        /// <summary>
        /// Rotation flip
        /// </summary>
        [Tooltip("Rotation flip")]
        public bool rotationFlip = false;

    }

}
