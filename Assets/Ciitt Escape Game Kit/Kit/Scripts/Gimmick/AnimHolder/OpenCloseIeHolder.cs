using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ciitt.EscapeGameKit
{

    /// <summary>
    /// Open and close
    /// </summary>
    public class OpenCloseIeHolder : OpenCloseAnimHolder
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
        /// OpenCloseState
        /// </summary>
        [SerializeField]
        [Tooltip("OpenCloseState")]
        OpenCloseState m_openCloseState = OpenCloseState.Close;

        /// <summary>
        /// Local trans for open
        /// </summary>
        [SerializeField]
        [Tooltip("Local trans for open")]
        TransformInfo m_openLocalInfo = new TransformInfo();

        /// <summary>
        /// Local trans for close
        /// </summary>
        [SerializeField]
        [Tooltip("Local trans for close")]
        TransformInfo m_closeLocalInfo = new TransformInfo();

        /// <summary>
        /// OpenCloseState
        /// </summary>
        // ------------------------------------------------------------------------------
        public override OpenCloseState currentOpenCloseState()
        {
            return this.m_openCloseState;
        }

        /// <summary>
        /// Start
        /// </summary>
        // ------------------------------------------------------------------------------
        void Start()
        {

#if UNITY_EDITOR

            if (this.m_openCloseState == OpenCloseState.Transition)
            {
                Debug.LogError("m_openCloseState should not be Transition : " + Funcs.createHierarchyPath(this.transform));
            }

#endif

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
        /// Play open anim
        /// </summary>
        /// <param name="immediately">immediately</param>
        // ----------------------------------------------------------------------------------
        public override void playOpenAnim(bool immediately)
        {
            StopAllCoroutines();
            StartCoroutine(this.openAnimIE(immediately));
        }

        /// <summary>
        /// Play close anim
        /// </summary>
        /// <param name="immediately">immediately</param>
        // ----------------------------------------------------------------------------------
        public override void playCloseAnim(bool immediately)
        {
            StopAllCoroutines();
            StartCoroutine(this.closeAnimIE(immediately));
        }

        /// <summary>
        /// Open anim IEnumerator
        /// </summary>
        /// <param name="immediately">immediately</param>
        /// <returns>IEnumerator</returns>
        // ----------------------------------------------------------------------------------
        IEnumerator openAnimIE(bool immediately)
        {

            this.m_openCloseState = OpenCloseState.Transition;

            float timer = 0.0f;
            float t = 0.0f;

            Vector3 fromPosition = this.transform.localPosition;
            Vector3 toPosition = this.m_openLocalInfo.position;

            Vector3 fromRotation = this.transform.localRotation.eulerAngles;
            Vector3 toRotation = this.m_openLocalInfo.rotate;

            Quaternion fromQuaternion = this.transform.localRotation;
            Quaternion toQuaternion = Quaternion.Euler(this.m_openLocalInfo.rotate);

            Vector3 fromScale = this.transform.localScale;
            Vector3 toScale = this.m_openLocalInfo.scale;

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
            }

            // m_openCloseState
            {
                this.m_openCloseState = OpenCloseState.Open;
            }

        }

        /// <summary>
        /// Close anim IEnumerator
        /// </summary>
        /// <param name="immediately">immediately</param>
        /// <returns>IEnumerator</returns>
        // ----------------------------------------------------------------------------------
        IEnumerator closeAnimIE(bool immediately)
        {

            this.m_openCloseState = OpenCloseState.Transition;

            float timer = 0.0f;
            float t = 0.0f;

            Vector3 fromPosition = this.transform.localPosition;
            Vector3 toPosition = this.m_closeLocalInfo.position;

            Vector3 fromRotation = this.transform.localRotation.eulerAngles;
            Vector3 toRotation = this.m_closeLocalInfo.rotate;

            Quaternion fromQuaternion = this.transform.localRotation;
            Quaternion toQuaternion = Quaternion.Euler(this.m_closeLocalInfo.rotate);

            Vector3 fromScale = this.transform.localScale;
            Vector3 toScale = this.m_closeLocalInfo.scale;

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
            }

            // m_openCloseState
            {
                this.m_openCloseState = OpenCloseState.Close;
            }

        }

    }

}
