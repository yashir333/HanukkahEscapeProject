using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SSC
{

    [CanEditMultipleObjects]
    [CustomEditor(typeof(BezierHandleScript))]
    public class BezierHandleInspector : Editor
    {

        void OnSceneGUI()
        {

            BezierHandleScript script = target as BezierHandleScript;

            Vector3 startTangentHandlePos = script.startTangentHandlePoint();
            Vector3 endTangentHandlePos = script.endTangentHandlePoint();

            Handles.color = Color.blue;

            Handles.DrawLine(startTangentHandlePos, script.point());
            Handles.DrawLine(script.point(), endTangentHandlePos);

            if (Event.current.alt)
            {
                return;
            }
            
            //
            {

                EditorGUI.BeginChangeCheck();

                Quaternion quat = (Tools.pivotRotation == PivotRotation.Global) ? Quaternion.identity : script.transform.rotation;

                startTangentHandlePos = Handles.PositionHandle(startTangentHandlePos, quat);
                endTangentHandlePos = Handles.PositionHandle(endTangentHandlePos, quat);

                if (EditorGUI.EndChangeCheck())
                {

                    Undo.RecordObject(script, "Change Bezier Handle");

                    if (script.handleType == BezierHandleScript.HandleType.SemiAbsolute)
                    {
                        script.startTangent = Quaternion.Inverse(script.transform.parent.rotation) * (startTangentHandlePos - script.transform.position);
                        script.endTangent = Quaternion.Inverse(script.transform.parent.rotation) * (endTangentHandlePos - script.transform.position);
                    }

                    else
                    {
                        script.startTangent = script.transform.InverseTransformPoint(startTangentHandlePos);
                        script.endTangent = script.transform.InverseTransformPoint(endTangentHandlePos);
                    }

                }

            }

        }

    }

}
