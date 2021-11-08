using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ciitt.EscapeGameKit
{

    public class SharedMaterialAnimationScript : MonoBehaviour
    {

        /// <summary>
        /// Reference to Material
        /// </summary>
        [SerializeField]
        [Tooltip("Reference to Material")]
        Material m_refMaterial = null;

        /// <summary>
        /// Tile X count
        /// </summary>
        [SerializeField]
        [Tooltip("Tile X count")]
        [Range(1, 16)]
        int m_tileX = 4;

        /// <summary>
        /// Tile Y count
        /// </summary>
        [SerializeField]
        [Tooltip("Tile Y count")]
        [Range(1, 16)]
        int m_tileY = 4;

        /// <summary>
        /// Frames to sleep
        /// </summary>
        [SerializeField]
        [Tooltip("Frames to sleep")]
        [Range(1, 100)]
        int m_sleepFrame = 1;

        /// <summary>
        /// Index counter
        /// </summary>
        int m_indexCounter = 0;

        /// <summary>
        /// Sleep counter
        /// </summary>
        int m_sleepCounter = 0;

        /// <summary>
        /// Start
        /// </summary>
        // ---------------------------------------------------------------------------------------
        void Start()
        {

            this.m_tileX = Mathf.Max(1, this.m_tileX);
            this.m_tileY = Mathf.Max(1, this.m_tileY);

#if UNITY_EDITOR

            if (!this.m_refMaterial)
            {
                Debug.LogWarning("(#if UNITY_EDITOR) m_refMaterial is null : " + Funcs.createHierarchyPath(this.transform));
            }

#endif

            if (this.m_refMaterial)
            {
                this.m_refMaterial.mainTextureScale = new Vector2(1.0f / this.m_tileX, 1.0f / this.m_tileY);
            }

        }

        /// <summary>
        /// Update
        /// </summary>
        // ---------------------------------------------------------------------------------------
        void Update()
        {

            if(!this.m_refMaterial)
            {
                return;
            }

            // -------------

            this.m_sleepCounter++;

            if(this.m_sleepCounter < this.m_sleepFrame)
            {
                return;
            }

            this.m_sleepCounter = 0;

            int indexX = this.m_indexCounter % this.m_tileX;
            int indexY = this.m_indexCounter / this.m_tileX;

            this.m_refMaterial.mainTextureOffset =
                new Vector2(
                    indexX * this.m_refMaterial.mainTextureScale.x,
                    1.0f - ((indexY + 1) * this.m_refMaterial.mainTextureScale.y)
                    );

            this.m_indexCounter = (this.m_indexCounter + 1) % (this.m_tileX * this.m_tileY);

        }

    }

}
