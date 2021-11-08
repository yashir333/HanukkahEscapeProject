using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ciitt.EscapeGameKit
{

    public class DebugSceneBackScript : MonoBehaviour
    {

        public void backScene()
        {
            SSC.SceneChangeManager.Instance.loadNextScene(SSC.SceneChangeManager.Instance.previousSceneName);
        }

    }

}
