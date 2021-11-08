using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SSC;

namespace SSCSample
{

    public class SimpleMoveScript : MonoBehaviour
    {

        public float m_walkSpeed = 0.04f;
        public float m_rotateSpeed = 1f;

        void Update()
        {

            if(Input.GetKey(KeyCode.W))
            {
                this.transform.position += this.transform.forward * this.m_walkSpeed;
            }

            else if (Input.GetKey(KeyCode.S))
            {
                this.transform.position += this.transform.forward * -this.m_walkSpeed;
            }

            if (Input.GetKey(KeyCode.A))
            {
                this.transform.Rotate(new Vector3(0, -this.m_rotateSpeed, 0));
            }

            else if (Input.GetKey(KeyCode.D))
            {
                this.transform.Rotate(new Vector3(0, this.m_rotateSpeed, 0));
            }

        }

    }

}
