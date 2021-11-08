using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SSC;

namespace SSCSample
{

    public class SampleSceneCommonScript : MonoBehaviour
    {

        public string m_nowLoadingUiIdentifier = "NowLoading1";

        public void reloadCurrentScene()
        {
            SceneChangeManager.Instance.loadNextScene(SceneChangeManager.Instance.nowLoadingSceneName, this.m_nowLoadingUiIdentifier, true);
        }
        
        public void backToTitleScene()
        {
            SceneChangeManager.Instance.loadNextScene(SceneChangeManager.Instance.titleSceneName, this.m_nowLoadingUiIdentifier, true);
        }

    }

}
