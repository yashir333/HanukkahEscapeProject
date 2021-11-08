using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ciitt.EscapeGameKit
{

    public class SimpleBezierHandlePoint
    {

        public Vector3 worldStartTangentPoint = Vector3.zero;
        public Vector3 worldEndTangentPoint = Vector3.zero;
        public Vector3 worldPoint = Vector3.zero;
        public Quaternion rotation = Quaternion.identity;

        public SimpleBezierHandlePoint()
        {

        }

        public static SimpleBezierHandlePoint create(Transform previous, Transform mid, Transform next, float tangentStrength = 0.1f)
        {

            SimpleBezierHandlePoint ret = new SimpleBezierHandlePoint();

            if (!mid)
            {
                return ret;
            }

            // ----------------

            Vector3 tangent = Vector3.zero;

            if (next)
            {
                tangent = (next.position - mid.position) * tangentStrength;
            }

            ret.worldPoint = mid.position;
            ret.worldStartTangentPoint = mid.position + tangent;
            ret.worldEndTangentPoint = mid.position - tangent;
            ret.rotation = mid.rotation;

            return ret;

        }

        public static SimpleBezierHandlePoint create(Vector3 previous, Vector3 mid, Vector3 next, Quaternion lastRot, float tangentStrength = 0.1f)
        {

            SimpleBezierHandlePoint ret = new SimpleBezierHandlePoint();

            Vector3 tangent = (next - mid) * tangentStrength;

            ret.worldPoint = mid;
            ret.worldStartTangentPoint = mid + tangent;
            ret.worldEndTangentPoint = mid - tangent;

            Vector3 temp = next - mid;
            ret.rotation = (temp == Vector3.zero) ? lastRot : Quaternion.LookRotation(temp);

            return ret;

        }

    }

}
