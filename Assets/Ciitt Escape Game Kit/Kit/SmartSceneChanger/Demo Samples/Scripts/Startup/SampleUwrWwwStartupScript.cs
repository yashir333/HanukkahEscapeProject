using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace SSCSample
{

    [RequireComponent(typeof(MeshRenderer))]
    public class SampleUwrWwwStartupScript : SSC.UwrWwwStartupScript
    {

        [SerializeField]
        protected string m_url = "";

        MeshRenderer m_refRenderer = null;

        protected override void initOnStart()
        {
            this.m_refRenderer = this.GetComponent<MeshRenderer>();
        }

        protected override UnityWebRequest createUnityWebRequest()
        {
#if UNITY_2017_1_OR_NEWER
            return UnityWebRequestTexture.GetTexture(this.m_url);
#else
            return UnityWebRequest.GetTexture(this.m_url);
#endif

        }

        public override void success(UnityWebRequest uwr)
        {
            this.m_refRenderer.material.mainTexture = DownloadHandlerTexture.GetContent(uwr);
        }

        public override void failed(UnityWebRequest www)
        {
            Debug.LogError(www.error);
        }

        public override void progress(UnityWebRequest uwr)
        {

        }

        void OnDestroy()
        {
            if (this.m_refRenderer)
            {
                Destroy(this.m_refRenderer.material.mainTexture);
                Destroy(this.m_refRenderer.material);
            }
        }

    }

}
