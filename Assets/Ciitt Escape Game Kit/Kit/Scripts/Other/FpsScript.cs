using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ciitt.EscapeGameKit
{

    [RequireComponent(typeof(Text))]
    public class FpsScript : MonoBehaviour
    {

        Text m_refText = null;

        void Start()
        {
            this.m_refText = this.GetComponent<Text>();
        }

        void Update()
        {

            if(Time.frameCount % 60 == 0)
            {
                this.m_refText.text = (1.0f / Time.deltaTime).ToString();
            }
            
        }

    }

}