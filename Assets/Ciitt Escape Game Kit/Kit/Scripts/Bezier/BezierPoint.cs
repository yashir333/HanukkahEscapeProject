using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ciitt.EscapeGameKit
{

    /// <summary>
    /// Bezier point
    /// </summary>
    [Serializable]
    public class BezierPoint
    {

        public Vector3 point = Vector3.zero;
        public Vector3 tangent = Vector3.forward;
        public Quaternion rotation = Quaternion.identity;

    }

}
