using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace SSCSample
{

    public class SampleUwrAssetBundleStartupScript : SSC.UwrAssetBundleStartupScript
    {

        protected override void initOnStart()
        {

        }

        public override void success(AssetBundle assetBundle)
        {

            // do not Unload here

            if (assetBundle.isStreamedSceneAssetBundle)
            {
                return;
            }

            foreach (string name in assetBundle.GetAllAssetNames())
            {
                Object obj = assetBundle.LoadAsset(name);
                GameObject gobj = Instantiate(obj) as GameObject;
                gobj.transform.SetParent(this.transform, false);
            }

        }

        public override void failed(UnityWebRequest uwr)
        {
            Debug.LogError(uwr.url + " " + uwr.error);
        }

        public override void progress(UnityWebRequest uwr)
        {

        }

    }

}
