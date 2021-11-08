using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ciitt.EscapeGameKit
{

    /// <summary>
    /// Evolve anim
    /// </summary>
    public class EvolveIeHolder : EvolveAnimHolder
    {

        /// <summary>
        /// Rotate type
        /// </summary>
        [SerializeField]
        [Tooltip("Rotate type")]
        protected RotateType m_rotateType = RotateType.QuaternionSlerp;

        /// <summary>
        /// Seconds
        /// </summary>
        [SerializeField]
        [Tooltip("Seconds")]
        [Range(0.0f, 10.0f)]
        protected float m_seconds = 0.5f;

        /// <summary>
        /// Local trans for evolved
        /// </summary>
        [SerializeField]
        [Tooltip("Local trans for evolved")]
        TransformInfo m_evolvedLocalInfo = new TransformInfo();

        /// <summary>
        /// Is anim finished
        /// </summary>
        EvolveAnimState m_currentEvolveAnimState = EvolveAnimState.NotEvolved;

        /// <summary>
        /// Current EvolveAnimState
        /// </summary>
        // ----------------------------------------------------------------------------------
        public override EvolveAnimState currentEvolveAnimState()
        {
            return this.m_currentEvolveAnimState;
        }

        /// <summary>
        /// Evaluate normalized value
        /// </summary>
        /// <param name="t">t</param>
        /// <returns>evaluated</returns>
        // ----------------------------------------------------------------------------------
        float evaluate(float t)
        {
            return 1.0f - ((t - 1.0f) * (t - 1.0f));
        }

        /// <summary>
        /// Play evolve anim
        /// </summary>
        /// <param name="immediately">immediately</param>
        // ----------------------------------------------------------------------------------
        public override void playEvolveAnim(bool immediately)
        {
            StopAllCoroutines();
            StartCoroutine(this.evolveAnimIE(immediately));
        }

        /// <summary>
        /// Evolve anim IEnumerator
        /// </summary>
        /// <param name="immediately">immediately</param>
        /// <returns>IEnumerator</returns>
        // ----------------------------------------------------------------------------------
        IEnumerator evolveAnimIE(bool immediately)
        {

            float timer = 0.0f;
            float t = 0.0f;

            Vector3 fromPosition = this.transform.localPosition;
            Vector3 toPosition = this.m_evolvedLocalInfo.position;

            Vector3 fromRotation = this.transform.localRotation.eulerAngles;
            Vector3 toRotation = this.m_evolvedLocalInfo.rotate;

            Quaternion fromQuaternion = this.transform.localRotation;
            Quaternion toQuaternion = Quaternion.Euler(this.m_evolvedLocalInfo.rotate);

            Vector3 fromScale = this.transform.localScale;
            Vector3 toScale = this.m_evolvedLocalInfo.scale;

            if (!immediately)
            {

                if (this.m_delay > 0.0f)
                {
                    yield return new WaitForSeconds(this.m_delay);
                }

                if (this.m_rotateType == RotateType.QuaternionSlerp)
                {

                    while (timer < this.m_seconds)
                    {

                        timer += Time.deltaTime;
                        t = this.evaluate(timer / this.m_seconds);

                        this.transform.localPosition = Vector3.Lerp(fromPosition, toPosition, t);
                        this.transform.localRotation = Quaternion.Slerp(fromQuaternion, toQuaternion, t);
                        this.transform.localScale = Vector3.Lerp(fromScale, toScale, t);

                        yield return null;

                    }

                }

                else
                {

                    while (timer < this.m_seconds)
                    {

                        timer += Time.deltaTime;
                        t = this.evaluate(timer / this.m_seconds);

                        this.transform.localPosition = Vector3.Lerp(fromPosition, toPosition, t);
                        this.transform.localRotation = Quaternion.Euler(Vector3.Lerp(fromRotation, toRotation, t));
                        this.transform.localScale = Vector3.Lerp(fromScale, toScale, t);

                        yield return null;

                    }

                }

            }

            // finish
            {

                this.transform.localPosition = toPosition;
                this.transform.localRotation = toQuaternion;
                this.transform.localScale = toScale;

                this.m_currentEvolveAnimState = EvolveAnimState.Evolved;

            }

        }

    }

}
