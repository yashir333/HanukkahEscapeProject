using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ciitt.EscapeGameKit
{

    [CustomEditor(typeof(SystemManager), true)]
    public class SystemManagerInspector : Editor
    {

        /// <summary>
        /// OnInspectorGUI
        /// </summary>
        // ----------------------------------------------------------------------------------------
        public override void OnInspectorGUI()
        {

            DrawDefaultInspector();

            GUILayout.Space(30.0f);

            GUILayout.Label("(You should delete old data if you changed the password settings.)");

            if (GUILayout.Button("Delete All PlayerPrefs", GUILayout.MinHeight(30.0f)))
            {

                if (EditorUtility.DisplayDialog("Confirmation", "Delete All PlayerPrefs ?", "Yes", "Cancel"))
                {

                    PlayerPrefs.DeleteAll();

                    EditorUtility.DisplayDialog("Confirmation", "Done.", "Ok");

                }

            }

        }

    }

}
