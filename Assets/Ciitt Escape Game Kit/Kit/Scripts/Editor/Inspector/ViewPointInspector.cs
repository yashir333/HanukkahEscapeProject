using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace ciitt.EscapeGameKit
{

    [CustomEditor(typeof(ViewPoint))]
    public class ViewPointInspector : Editor
    {

        bool m_sync = false;

        /// <summary>
        /// OnInspectorGUI
        /// </summary>
        // ----------------------------------------------------------------------------------------
        public override void OnInspectorGUI()
        {

            ViewPoint script = target as ViewPoint;

            DrawDefaultInspector();

            GUILayout.Space(20.0f);

            //serializedObject.Update();

            if (!this.m_sync)
            {
                if (GUILayout.Button("Start Sync with Scene Camera", GUILayout.MinHeight(30.0f)))
                {
                    this.m_sync = true;
                    script.startSync();
                }
            }

            else
            {
                if (GUILayout.Button("Stop", GUILayout.MinHeight(30.0f)))
                {
                    this.m_sync = false;
                    script.stopSync();
                    EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
                }
            }

            if (GUILayout.Button("Move Scene Camera and Main Camera", GUILayout.MinHeight(30.0f)))
            {
                Undo.RecordObject(Camera.main.transform, "Moved Main Camera");
                script.moveSceneAndMainCamera();
            }

            //serializedObject.ApplyModifiedProperties();

        }

    }

}
