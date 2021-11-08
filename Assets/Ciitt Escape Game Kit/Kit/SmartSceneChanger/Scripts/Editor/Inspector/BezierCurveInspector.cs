using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace SSC
{

    /// <summary>
    /// BezierCurveScript inspector
    /// </summary> 
    [CustomEditor(typeof(BezierCurveScript))]
    public class BezierCurveInspector : Editor
    {

        /// <summary>
        /// previous file path
        /// </summary>
        static string previousFilePath = "";

        /// <summary>
        /// OnInspectorGUI
        /// </summary>
        // ----------------------------------------------------------------------------------------
        public override void OnInspectorGUI()
        {

            DrawDefaultInspector();

            GUILayout.Space(30.0f);

            BezierCurveScript script = (BezierCurveScript)target;

            if (GUILayout.Button("Import Bezier Json", GUILayout.MinHeight(30)))
            {

                string path = EditorUtility.OpenFilePanel("Import Bezier Json", previousFilePath, "json");

                if (!string.IsNullOrEmpty(path))
                {

                    previousFilePath = path;

                    try
                    {
                        using (StreamReader sr = new StreamReader(path))
                        {
                            this.importBezier(sr.ReadToEnd());
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.LogError(e.Message);
                    }

                }

            }

        }

        /// <summary>
        /// Import bezier json
        /// </summary>
        /// <param name="bezierJson">bezier json</param>
        // ----------------------------------------------------------------------------------------
        void importBezier(string bezierJson)
        {

            if(string.IsNullOrEmpty(bezierJson))
            {
                return;
            }

            if (!EditorUtility.DisplayDialog("Confirmation", "This action will destroy all child GameObjects.\n\nContinue?", "OK", "Cancel"))
            {
                return;
            }

            // -----------------------

            BezierCurveScript script = (BezierCurveScript)target;

            BezierImportFormat bif = new BezierImportFormat();

            GameObject tempGameObject = null;

            BezierHandleScript tempBezierHandleScript = null;

            // -----------------------

            // clearBezierHandleListEditorOnly
            {
                script.clearBezierHandleListEditorOnly();
            }

            // destroy
            {

                Transform child = null;

                for(int i = script.transform.childCount - 1; i >= 0; i--)
                {

                    child = script.transform.GetChild(i);

                    if (child.GetComponent<BezierHandleScript>())
                    {
                        DestroyImmediate(child.gameObject);
                    }

                }

            }

            // bif
            {

                JsonUtility.FromJsonOverwrite(bezierJson, bif);

                if(bif == null)
                {
                    return;
                }

            }

            // create
            {

                for (int i = 0; i < bif.bezierImportInfoList.Length; i++)
                {

                    tempGameObject = new GameObject(string.Format("Bezier Handle ({0})", i+1));

                    // SetParent
                    {
                        tempGameObject.transform.SetParent(script.transform);
                    }

                    // BezierHandleScript
                    {
                        tempBezierHandleScript = tempGameObject.AddComponent<BezierHandleScript>();
                        tempBezierHandleScript.importEditorOnly(bif, bif.bezierImportInfoList[i]);
                    }

                    // addBezierHandleScriptEditorOnly
                    {
                        script.addBezierHandleScriptEditorOnly(tempBezierHandleScript);
                    }

                }

            }

            // initEditorOnly
            {
                script.initEditorOnly();
            }

            // finish
            {
                EditorUtility.DisplayDialog("Confirmation", "Import finished.", "OK");
            }

        }

    }

}
