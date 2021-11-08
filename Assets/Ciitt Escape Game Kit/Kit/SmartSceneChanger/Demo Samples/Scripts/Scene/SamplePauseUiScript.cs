using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SSCSample
{

    public class SamplePauseUiScript : MonoBehaviour
    {

        public string m_pauseUi = "Pause";

        public string m_unpauseUi = "Sample";

        SSC.PauseState m_refPauseState = null;

        void Start()
        {

            this.m_refPauseState = SSC.SimpleReduxManager.Instance.PauseStateWatcher.state();

        }

        void Update()
        {

            if (Input.GetMouseButtonDown(0))
            {

                if(this.m_refPauseState.pause)
                {
                    this.unpause();
                }

            }

        }

        public void pause()
        {

            print("pause");

            SSC.UiManager.Instance.showUi(this.m_pauseUi, true, false);

        }

        public void unpause()
        {

            print("unpause");

            SSC.UiManager.Instance.showUi(this.m_unpauseUi, true, false);

        }

    }

}
