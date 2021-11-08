using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;

namespace ciitt.EscapeGameKit
{

    [CanEditMultipleObjects]
    [CustomEditor(typeof(CustomToggleButton))]
    public class CustomToggleButtonInspector : ToggleEditor
    {

        SerializedProperty onClick;

        /// <summary>
        /// OnEnable
        /// </summary>
        // ----------------------------------------------------------------------------------------
        protected override void OnEnable()
        {

            base.OnEnable();

            this.onClick = serializedObject.FindProperty("onClick");

        }


        /// <summary>
        /// OnInspectorGUI
        /// </summary>
        // ----------------------------------------------------------------------------------------
        public override void OnInspectorGUI()
        {

            CustomToggleButton script = (CustomToggleButton)target;

            GUI.enabled = false;
            EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour(script), typeof(MonoScript), false);
            GUI.enabled = true;

            base.OnInspectorGUI();

            serializedObject.Update();

            Color temp = GUI.backgroundColor;

            GUI.backgroundColor = Color.yellow;
            EditorGUILayout.PropertyField(this.onClick);
            GUI.backgroundColor = temp;

            serializedObject.ApplyModifiedProperties();

        }

    }

}
