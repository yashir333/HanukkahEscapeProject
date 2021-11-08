using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace SSC
{

    /// <summary>
    /// Class for UnityWebRequest WWW startup
    /// </summary>
    public abstract class UwrWwwStartupScript : MonoBehaviour
    {

        ///// <summary>
        ///// Dispose UnityWebRequest when it's finished
        ///// </summary>
        //[SerializeField]
        //[Tooltip("Dispose UnityWebRequest when it's finished")]
        //protected bool m_disposeUwrWhenFinished = true;

        /// <summary>
        /// Create UnityWebRequest
        /// </summary>
        protected abstract UnityWebRequest createUnityWebRequest();

        /// <summary>
        /// Called in Start()
        /// </summary>
        protected abstract void initOnStart();

        /// <summary>
        /// Success function
        /// </summary>
        /// <param name="uwr">UnityWebRequest</param>
        public abstract void success(UnityWebRequest uwr);

        /// <summary>
        /// Failed function
        /// </summary>
        /// <param name="uwr">UnityWebRequest</param>
        public abstract void failed(UnityWebRequest uwr);

        /// <summary>
        /// Progress function
        /// </summary>
        /// <param name="uwr">UnityWebRequest</param>
        public abstract void progress(UnityWebRequest uwr);

        /// <summary>
        /// Start()
        /// </summary>
        protected virtual void Start()
        {

            this.initOnStart();

            WwwStartupManager.Instance.addSceneStartupWwwUwr(
                this.createUnityWebRequest(),
                true,
                this.success,
                this.failed,
                this.progress
                );

        }

    }

}
