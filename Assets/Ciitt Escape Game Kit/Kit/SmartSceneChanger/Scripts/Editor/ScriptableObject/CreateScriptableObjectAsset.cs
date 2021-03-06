using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace SSC
{

    /// <summary>
    /// Create ScriptableObject asset
    /// </summary>
    public class CreateScriptableObjectAsset
    {

        /// <summary>
        /// Check selection count
        /// </summary>
        /// <returns>ok</returns>
        [MenuItem("Assets/SSC/Create ScriptableObject Asset", true)]
        private static bool ValidateCreateScriptableObjectAsset()
        {
            return Selection.objects.Length == 1;
        }

        /// <summary>
        /// Create ScriptableObject asset
        /// </summary>
        [MenuItem("Assets/SSC/Create ScriptableObject Asset")]
        public static void createScriptableObjectAsset()
        {

            var selected = Selection.objects[0];

            string assetPath = AssetDatabase.GetAssetPath(selected);
            string fileName = Path.GetFileNameWithoutExtension(assetPath);

            ScriptableObject asset = ScriptableObject.CreateInstance(fileName);

            if (asset)
            {

                string path = EditorUtility.SaveFilePanelInProject(
                    "Save",
                    fileName,
                    "asset",
                    "Please enter a file name to save the ScriptableObject to",
                    Path.GetDirectoryName(assetPath)
                    );

                if (!string.IsNullOrEmpty(path))
                {

                    AssetDatabase.CreateAsset(asset, path);
                    AssetDatabase.SaveAssets();

                    EditorUtility.FocusProjectWindow();

                    Selection.activeObject = asset;

                }

            }

        }

    }

}
