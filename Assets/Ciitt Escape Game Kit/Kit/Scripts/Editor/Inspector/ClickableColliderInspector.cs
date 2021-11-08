using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ciitt.EscapeGameKit
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(ClickableColliderScript), true)]
    public class ClickableColliderInspector : Editor
    {

        /// <summary>
        /// OnInspectorGUI
        /// </summary>
        // ----------------------------------------------------------------------------------------
        public override void OnInspectorGUI()
        {

            ClickableColliderScript script = target as ClickableColliderScript;

            DrawDefaultInspector();

            GUILayout.Space(20.0f);

            if (Selection.gameObjects.Length == 1)
            {

                if (GUILayout.Button("Calculate Bounds for BoxCollider", GUILayout.MinHeight(30.0f)))
                {
                    script.calculateBoundsForBoxCollider();
                }

            }

        }

    }

}
