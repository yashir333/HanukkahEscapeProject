using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SSCSample
{

    public class SampleMoveAlongBezierScript : MonoBehaviour
    {

        public SSC.BezierCurveScript m_refBezierCurveScript = null;

        [Range(0.0f, 1.0f)]
        public float m_speed = 0.01f;

        float m_counter = 0.0f;

        protected SSC.BezierCurveScript.SamplingInfo m_samplingInfo = new SSC.BezierCurveScript.SamplingInfo();

        void Update()
        {

            if(this.m_refBezierCurveScript)
            {

                this.m_refBezierCurveScript.findSampledPointByNormalizedValue(this.m_counter, this.m_samplingInfo);

                this.transform.position = this.m_samplingInfo.position;
                this.transform.rotation = this.m_samplingInfo.rotation;

                this.m_counter = (this.m_counter + this.m_speed) % 1.0f;

            }

        }

    }

}
