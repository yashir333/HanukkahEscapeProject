using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace ciitt.EscapeGameKit
{

    /// <summary>
    /// Identifier interface
    /// </summary>
    public interface IdentifierInterface
    {
        /// <summary>
        /// Get identifier
        /// </summary>
        /// <returns>identifier</returns>
        string getIdentifier();
    }

    /// <summary>
    /// Any functions
    /// </summary>
    public class Funcs
    {

        /// <summary>
        /// Temp StringBuilder
        /// </summary>
        protected static StringBuilder tempStringBuilder = new StringBuilder();

        /// <summary>
        /// Resize Texture
        /// </summary>
        /// <param name="source">source texture</param>
        /// <param name="newWidth">new texture width</param>
        /// <param name="newHeight">new texture height</param>
        /// <returns>Texture2D</returns>
        // -------------------------------------------------------------------------------------------
        public static Texture2D resizeTexture(Texture2D source, int newWidth, int newHeight, FilterMode filterMode)
        {

            RenderTexture rt = RenderTexture.GetTemporary(newWidth, newHeight);
            rt.filterMode = filterMode;
            RenderTexture.active = rt;

            Graphics.Blit(source, rt);

            Texture2D nTex = new Texture2D(newWidth, newHeight);
            nTex.ReadPixels(new Rect(0, 0, newWidth, newWidth), 0, 0);
            nTex.Apply();

            RenderTexture.active = null;
            rt.DiscardContents();

            return nTex;

        }

        /// <summary>
        /// Convert list to dictionary
        /// </summary>
        /// <typeparam name="T">IdentifierInterface</typeparam>
        /// <param name="list">IdentifierInterface list</param>
        /// <returns>Dictionary<string, T></returns>
        // -------------------------------------------------------------------------------------------
        public static void listToDictionary<T>(List<T> source, Dictionary<string, T> dist) where T : IdentifierInterface
        {

            dist.Clear();

            string id = "";

            foreach (IdentifierInterface val in source)
            {

                id = val.getIdentifier();

                if (!dist.ContainsKey(id))
                {
                    dist.Add(id, (T)val);
                }

#if UNITY_EDITOR

                else
                {
                    Debug.LogWarning("(#if UNITY_EDITOR) Dictionary already contains a key : " + id);
                }

#endif

            }

        }

        /// <summary>
        /// Convert json to T 
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="json">json</param>
        /// <returns>T or new T</returns>
        // -------------------------------------------------------------------------------------------
        public static T convertJson<T>(string json, Action error) where T : new()
        {

            T ret = new T();

            if (!string.IsNullOrEmpty(json))
            {

                ret = JsonUtility.FromJson<T>(json);

                if (ret == null)
                {

                    ret = new T();

                    if (error != null)
                    {
                        error();
                    }

                }

            }

            return ret;

        }

        /// <summary>
        /// Create hierachy path
        /// </summary>
        /// <param name="trans">Transform</param>
        /// <returns>path</returns>
        // -------------------------------------------------------------------------------------------
        public static string createHierarchyPath(Transform trans)
        {

            if (!trans)
            {
                return "";
            }

            // --------------------------

            tempStringBuilder.Length = 0;

            tempStringBuilder.Append("/" + trans.name);

            Transform temp = trans.parent;

            while (temp != null)
            {

                tempStringBuilder.Insert(0, "/" + temp.name);

                temp = temp.parent;
            }

            return tempStringBuilder.ToString();

        }

    }

}
