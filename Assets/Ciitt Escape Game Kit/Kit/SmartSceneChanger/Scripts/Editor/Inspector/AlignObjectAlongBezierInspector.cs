using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace SSC
{

    /// <summary>
    /// AlignObjectAlongBezierScript inspector
    /// </summary> 
    [CustomEditor(typeof(AlignObjectAlongBezierScript))]
    public class AlignObjectAlongBezierInspector : Editor
    {

        protected SerializedProperty m_refBezierCurveScript;
        protected SerializedProperty m_startOffsetMeter;
        protected SerializedProperty m_betweenOffsetMeter;
        protected SerializedProperty m_lookAtType;
        protected SerializedProperty m_refLookAt;
        protected SerializedProperty m_onValidateSyncEditorOnly;

        GUIStyle m_labelGuiStyle = new GUIStyle();
        GUIStyleState m_labelGuiStyleState = new GUIStyleState();

        /// <summary>
        /// OnEnable
        /// </summary>
        // ----------------------------------------------------------------------------------------
        protected virtual void OnEnable()
        {

            if (!target)
            {
                return;
            }

            this.m_refBezierCurveScript = serializedObject.FindProperty("m_refBezierCurveScript");
            this.m_startOffsetMeter = serializedObject.FindProperty("m_startOffsetMeter");
            this.m_betweenOffsetMeter = serializedObject.FindProperty("m_betweenOffsetMeter");
            this.m_lookAtType = serializedObject.FindProperty("m_lookAtType");
            this.m_refLookAt = serializedObject.FindProperty("m_refLookAt");
            this.m_onValidateSyncEditorOnly = serializedObject.FindProperty("m_onValidateSyncEditorOnly");

            this.m_labelGuiStyleState.background = Texture2D.whiteTexture;
            this.m_labelGuiStyle.normal = this.m_labelGuiStyleState;

        }

        /// <summary>
        /// OnInspectorGUI
        /// </summary>
        // ----------------------------------------------------------------------------------------
        public override void OnInspectorGUI()
        {

            serializedObject.Update();

            AlignObjectAlongBezierScript script = (AlignObjectAlongBezierScript)target;

            // script
            {
                GUI.enabled = false;
                EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour(script), typeof(MonoScript), false);
                GUI.enabled = true;
            }

            // field
            {

                EditorGUILayout.PropertyField(this.m_refBezierCurveScript);
                EditorGUILayout.PropertyField(this.m_startOffsetMeter);
                EditorGUILayout.PropertyField(this.m_betweenOffsetMeter);
                EditorGUILayout.PropertyField(this.m_lookAtType);

                if (((AlignObjectAlongBezierScript.LookAtType)this.m_lookAtType.enumValueIndex) == AlignObjectAlongBezierScript.LookAtType.LookAtObject)
                {
                    EditorGUILayout.PropertyField(this.m_refLookAt);
                }

                GUILayout.Space(30.0f);

                EditorGUILayout.PropertyField(this.m_onValidateSyncEditorOnly);

            }

            // message
            {

                if (!Application.isPlaying && this.m_onValidateSyncEditorOnly.boolValue)
                {

                    Color defaultBackgroundColor = GUI.backgroundColor;
                    GUI.backgroundColor = Color.green;
                    EditorGUILayout.LabelField("Syncing", this.m_labelGuiStyle);
                    GUI.backgroundColor = defaultBackgroundColor;

                }

            }

            serializedObject.ApplyModifiedProperties();

        }

    }

}
