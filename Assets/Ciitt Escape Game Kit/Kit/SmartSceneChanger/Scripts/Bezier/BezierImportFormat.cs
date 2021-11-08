using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SSC
{

    [Serializable]
    public class BezierImportFormat
    {

        [Serializable]
        public class BezierImportInfo
        {

            public Vector3 point = Vector3.zero;

            public Vector3 startTangent = Vector3.zero;

            public Vector3 endTangent = Vector3.zero;

        }

        public string version = "";

        public bool relativeTangentPos = false;

        public BezierImportInfo[] bezierImportInfoList = null;

    }

}
