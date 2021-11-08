using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ciitt.EscapeGameKit
{

    public class CopyAndTransformWindow : EditorWindow
    {

        /// <summary>
        /// Scroll pos
        /// </summary>
        Vector2 m_scrollPos = Vector2.zero;

        /// <summary>
        /// GameObject to copy
        /// </summary>
        GameObject m_refOriginalGameObject = null;

        /// <summary>
        /// Count to copy
        /// </summary>
        int m_copyCount = 1;

        /// <summary>
        /// Relative translate
        /// </summary>
        Vector3 m_translate = Vector3.zero;

        /// <summary>
        /// Relative rotate
        /// </summary>
        Vector3 m_rotate = Vector3.zero;

        /// <summary>
        /// Bounds info
        /// </summary>
        Vector3 m_boundsInfo = Vector3.zero;

        /// <summary>
        /// OnGUI
        /// </summary>
        // -----------------------------------------------------------------------------------------
        void OnGUI()
        {

            EditorGUIUtility.labelWidth = 200f;

            this.m_scrollPos = EditorGUILayout.BeginScrollView(this.m_scrollPos);

            // HelpBox
            {
                EditorGUILayout.HelpBox(
                    "Copy and Transform",
                    MessageType.Info
                    );
            }

            GUILayout.Space(30.0f);

            // m_refOriginalGameObject
            {

                GameObject previous = this.m_refOriginalGameObject;

                this.m_refOriginalGameObject =
                    EditorGUILayout.ObjectField("Select GameObject in scene", this.m_refOriginalGameObject, typeof(GameObject), true) as GameObject;

                if(this.m_refOriginalGameObject && EditorUtility.IsPersistent(this.m_refOriginalGameObject))
                {
                    EditorUtility.DisplayDialog("Error", "Select GameObject in scene", "OK");
                    this.m_refOriginalGameObject = null;
                }

                if(this.m_refOriginalGameObject && previous != this.m_refOriginalGameObject)
                {

                    MeshFilter[] mfs = this.m_refOriginalGameObject.GetComponentsInChildren<MeshFilter>(true);
                    
                    Vector3 boundsMin = Vector3.one * 1000000f;
                    Vector3 boundsMax = -Vector3.one * 1000000f;

                    foreach (var mf in mfs)
                    {
                        
                        boundsMin.x = Mathf.Min(boundsMin.x, mf.sharedMesh.bounds.min.x);
                        boundsMin.y = Mathf.Min(boundsMin.y, mf.sharedMesh.bounds.min.y);
                        boundsMin.z = Mathf.Min(boundsMin.z, mf.sharedMesh.bounds.min.z);

                        boundsMax.x = Mathf.Max(boundsMax.x, mf.sharedMesh.bounds.max.x);
                        boundsMax.y = Mathf.Max(boundsMax.y, mf.sharedMesh.bounds.max.y);
                        boundsMax.z = Mathf.Max(boundsMax.z, mf.sharedMesh.bounds.max.z);

                    }

                    this.m_boundsInfo.x = Mathf.Abs(boundsMax.x - boundsMin.x);
                    this.m_boundsInfo.y = Mathf.Abs(boundsMax.y - boundsMin.y);
                    this.m_boundsInfo.z = Mathf.Abs(boundsMax.z - boundsMin.z);

                }

                GUILayout.Space(10.0f);

                GUI.enabled = false;
                EditorGUILayout.Vector3Field("Bounds size", this.m_boundsInfo);
                GUI.enabled = true;

            }

            GUILayout.Space(30.0f);

            // m_copyCount
            {
                this.m_copyCount = EditorGUILayout.IntSlider("Copy Count", this.m_copyCount, 1, 100);
            }

            GUILayout.Space(30.0f);

            //
            {

                EditorGUILayout.LabelField("Relative Local Transform", EditorStyles.boldLabel);

                this.m_translate = EditorGUILayout.Vector3Field("Translate", this.m_translate);
                this.m_rotate = EditorGUILayout.Vector3Field("Rotate", this.m_rotate);

            }

            GUILayout.Space(30.0f);

            //
            {

                GUI.enabled = this.m_refOriginalGameObject;

                if (GUILayout.Button("Copy", GUILayout.MinHeight(30)))
                {

                    if(EditorUtility.DisplayDialog("Confirmation", "Copy?", "Yes", "Cancel"))
                    {

                        GameObject newObj = null;

                        for(int i = 1; i <= this.m_copyCount; i++)
                        {

                            newObj = Instantiate(this.m_refOriginalGameObject, this.m_refOriginalGameObject.transform.parent);

                            newObj.transform.localPosition = this.m_refOriginalGameObject.transform.localPosition + (this.m_translate * i);
                            newObj.transform.localRotation = this.m_refOriginalGameObject.transform.localRotation * Quaternion.Euler(this.m_rotate * i);

                            newObj.transform.name = newObj.transform.name.Replace("(Clone)", string.Format(" ({0})", i));

                            Undo.RegisterCreatedObjectUndo(newObj, "Create New Object");

                        }

                        EditorUtility.DisplayDialog("Confirmation", "Done.", "OK");

                    }

                }

                GUI.enabled = true;

            }

            EditorGUILayout.EndScrollView();

        }

    }

}
