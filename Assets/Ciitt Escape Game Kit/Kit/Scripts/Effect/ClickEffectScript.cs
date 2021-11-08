using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ciitt.EscapeGameKit
{

    /// <summary>
    /// Click effect
    /// </summary>
    [RequireComponent(typeof(Image))]
    public class ClickEffectScript : MonoBehaviour
    {

        /// <summary>
        /// Reference to Image
        /// </summary>
        Image m_refImage = null;

        /// <summary>
        /// Seconds
        /// </summary>
        [SerializeField]
        [Tooltip("Seconds")]
        float m_seconds = 0.5f;

        /// <summary>
        /// Seconds
        /// </summary>
        [SerializeField]
        [Tooltip("Seconds")]
        float m_scaleFrom = 5.0f;

        /// <summary>
        /// Start
        /// </summary>
        // -----------------------------------------------------------------------
        void Start()
        {

            this.m_refImage = this.GetComponent<Image>();

            this.setMaterialValues(this.m_scaleFrom, 0.0f);

#if UNITY_EDITOR

            if (!this.m_refImage.material)
            {
                Debug.LogError("m_refImage.material is null : " + Funcs.createHierarchyPath(this.transform));
                this.enabled = false;
            }

#endif

        }

        /// <summary>
        /// Update
        /// </summary>
        // -----------------------------------------------------------------------
        void Update()
        {

            if(Input.GetMouseButtonDown(0))
            {
                StopAllCoroutines();
                StartCoroutine(this.startEffect(Input.mousePosition));
            }

        }

        /// <summary>
        /// Set material values
        /// </summary>
        /// <param name="scale"></param>
        /// <param name="alpha"></param>
        // -----------------------------------------------------------------------
        void setMaterialValues(float scale, float alpha)
        {

            if (this.m_refImage.material)
            {
                this.m_refImage.material.SetFloat("_Scale", scale);
                this.m_refImage.material.SetFloat("_Alpha", alpha);
            }

        }

        /// <summary>
        /// Evaluate
        /// </summary>
        /// <param name="t">t</param>
        /// <returns>new t</returns>
        // -----------------------------------------------------------------------
        float evaluate(float t)
        {
            return 1.0f - ((t - 1.0f) * (t - 1.0f));
        }

        /// <summary>
        /// Effect
        /// </summary>
        /// <param name="mousePosition">mousePosition</param>
        /// <returns>startEffect</returns>
        // -----------------------------------------------------------------------
        IEnumerator startEffect(Vector3 mousePosition)
        {

            // init
            {
                this.transform.position = mousePosition;
                this.setMaterialValues(this.m_scaleFrom, 0.0f);
            }

            yield return null;

            // main
            {

                float timer = 0.0f;

                float t = 0.0f;

                while (timer < this.m_seconds)
                {

                    timer += Time.deltaTime;

                    t = this.evaluate(timer / this.m_seconds);

                    this.setMaterialValues(Mathf.Lerp(this.m_scaleFrom, 1.0f, t), 1.0f - t);

                    yield return null;

                }

            }

            yield return null;

            // finish
            {
                this.setMaterialValues(10.0f, 0.0f);
            }

        }

    }

}
