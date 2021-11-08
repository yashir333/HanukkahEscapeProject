using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace ciitt.EscapeGameKit
{

    /// <summary>
    /// Change vertex color
    /// </summary>
    public class ChangeVertexColorWindow : EditorWindow
    {

        enum PaintType
        {
            Multiply,
            Replace
        }

        /// <summary>
        /// Scroll pos
        /// </summary>
        Vector2 m_scrollPos = Vector2.zero;

        /// <summary>
        /// Reference to Mesh
        /// </summary>
        Mesh m_refMesh = null;

        /// <summary>
        /// Color
        /// </summary>
        Color m_color = Color.white;

        /// <summary>
        /// PaintType
        /// </summary>
        PaintType m_paintType = PaintType.Multiply;

        /// <summary>
        /// Previous folder path to save
        /// </summary>
        string m_previousSaveFolderPath = "";

        /// <summary>
        /// OnGUI
        /// </summary>
        // -----------------------------------------------------------------------------------------
        void OnGUI()
        {

            this.m_scrollPos = EditorGUILayout.BeginScrollView(this.m_scrollPos);

            // HelpBox
            {
                EditorGUILayout.HelpBox(
                    "Change vertex color",
                    MessageType.Info
                    );
            }

            GUILayout.Space(30.0f);

            // m_refMesh
            {
                this.m_refMesh = EditorGUILayout.ObjectField("Select Mesh", this.m_refMesh, typeof(Mesh), false) as Mesh;
            }

            GUILayout.Space(30.0f);

            // m_paintType
            {
                this.m_paintType = (PaintType)EditorGUILayout.EnumPopup("Paint Type", this.m_paintType);
            }

            // m_color
            {
                this.m_color = EditorGUILayout.ColorField("New Vertex Color", this.m_color);
            }

            GUILayout.Space(30.0f);

            //
            {

                GUI.enabled = this.m_refMesh;

                if (GUILayout.Button("Create New Mesh File", GUILayout.MinHeight(30)))
                {

                    string assetPath = AssetDatabase.GetAssetPath(this.m_refMesh);
                    string defaultFileName = this.m_refMesh.name;

                    if(string.IsNullOrEmpty(this.m_previousSaveFolderPath))
                    {
                        this.m_previousSaveFolderPath = Path.GetDirectoryName(assetPath);
                    }

                    string savePath = EditorUtility.SaveFilePanelInProject("Save Mesh", defaultFileName, "asset", "save mesh", this.m_previousSaveFolderPath);

                    if (!string.IsNullOrEmpty(savePath))
                    {

                        this.m_previousSaveFolderPath = Path.GetDirectoryName(savePath);

                        Color[] colors = this.m_refMesh.colors;

                        if(colors.Length <= 0)
                        {
                            EditorUtility.DisplayDialog("Error", "Not Found Vertex Color Array", "OK");
                        }

                        else
                        {

                            for (int i = colors.Length - 1; i >= 0; i--)
                            {

                                if (this.m_paintType == PaintType.Multiply)
                                {
                                    colors[i] = colors[i] * this.m_color;
                                }

                                else
                                {
                                    colors[i] = this.m_color;
                                }

                            }

                            // CreateAsset
                            {

                                Mesh newMesh = Instantiate(this.m_refMesh);

                                newMesh.colors = colors;

                                AssetDatabase.CreateAsset(newMesh, savePath);

                            }

                            AssetDatabase.Refresh();

                            EditorUtility.DisplayDialog("Confirmation", "Done.", "OK");

                            Selection.activeObject = AssetDatabase.LoadMainAssetAtPath(savePath);

                        }

                    }

                }

                GUI.enabled = true;

            }

            EditorGUILayout.EndScrollView();

        }

    }

}
