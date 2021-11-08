using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace SSC
{

    /// <summary>
    /// Class for AssetBundle startup (UnityWebRequest)
    /// </summary>
    public abstract class UwrAssetBundleStartupScript : MonoBehaviour
    {

        /// <summary>
        /// AssetBundle name
        /// </summary>
        [SerializeField]
        [Tooltip("AssetBundle name")]
        protected string m_assetBundleName = "";

        /// <summary>
        /// AssetBundle variant
        /// </summary>
        [SerializeField]
        [Tooltip("AssetBundle variant")]
        protected string m_variant = "";

        /// <summary>
        /// Called in Start()
        /// </summary>
        protected abstract void initOnStart();

        /// <summary>
        /// Success function
        /// </summary>
        /// <param name="ab">Result AssetBundle</param>
        public abstract void success(AssetBundle ab);

        /// <summary>
        /// Failed function
        /// </summary>
        /// <param name="www">failed UnityWebRequest</param>
        public abstract void failed(UnityWebRequest uwr);

        /// <summary>
        /// Progress function
        /// </summary>
        /// <param name="www">progress UnityWebRequest</param>
        public abstract void progress(UnityWebRequest uwr);

        /// <summary>
        /// Start()
        /// </summary>
        protected virtual void Start()
        {

            this.initOnStart();

            AssetBundleStartupManager.Instance.addSceneStartupAssetBundleUwr(this.m_assetBundleName, this.m_variant, this.success, this.failed, this.progress);

        }

    }

}
