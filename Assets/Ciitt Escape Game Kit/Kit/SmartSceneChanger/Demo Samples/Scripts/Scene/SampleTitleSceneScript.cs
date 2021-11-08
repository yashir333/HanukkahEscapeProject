using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SSC;

namespace SSCSample
{

    public class SampleTitleSceneScript : MonoBehaviour
    {

#if UNITY_EDITOR

        [SerializeField]
        UnityEngine.Object m_abScene;

        [SerializeField]
        UnityEngine.Object m_wwwScene;

        [SerializeField]
        UnityEngine.Object m_ieScene;

        [SerializeField]
        UnityEngine.Object m_simpleScene;

        [SerializeField]
        UnityEngine.Object m_errorScene;

        [SerializeField]
        UnityEngine.Object m_dialogScene;

#endif

        [HideInInspector]
        [SerializeField]
        string m_abSceneName = "";

        [HideInInspector]
        [SerializeField]
        string m_wwwSceneName = "";

        [HideInInspector]
        [SerializeField]
        string m_ieSceneName = "";

        [HideInInspector]
        [SerializeField]
        string m_simpleSceneName = "";

        [HideInInspector]
        [SerializeField]
        string m_errorSceneName = "";

        [HideInInspector]
        [SerializeField]
        string m_dialogSceneName = "";

        void loadScene(string sceneName)
        {
            SceneChangeManager.Instance.loadNextScene(sceneName, "NowLoading1", true);
        }

        public void loadSampleAbScene()
        {
            this.loadScene(this.m_abSceneName);
        }

        public void loadSampleWwwScene()
        {
            this.loadScene(this.m_wwwSceneName);
        }

        public void loadSampleIeScene()
        {
            this.loadScene(this.m_ieSceneName);
        }

        public void loadSampleSimpleScene()
        {
            this.loadScene(this.m_simpleSceneName);
        }

        public void loadSampleErrorScene()
        {
            this.loadScene(this.m_errorSceneName);
        }

        public void loadSampleDialogScene()
        {
            this.loadScene(this.m_dialogSceneName);
        }

        void OnValidate()
        {

#if UNITY_EDITOR

            this.m_abSceneName = (this.m_abScene && !string.IsNullOrEmpty(this.m_abScene.name)) ? this.m_abScene.name : "";
            this.m_wwwSceneName = (this.m_wwwScene && !string.IsNullOrEmpty(this.m_wwwScene.name)) ? this.m_wwwScene.name : "";
            this.m_ieSceneName = (this.m_ieScene && !string.IsNullOrEmpty(this.m_ieScene.name)) ? this.m_ieScene.name : "";
            this.m_simpleSceneName = (this.m_simpleScene && !string.IsNullOrEmpty(this.m_simpleScene.name)) ? this.m_simpleScene.name : "";
            this.m_errorSceneName = (this.m_errorScene && !string.IsNullOrEmpty(this.m_errorScene.name)) ? this.m_errorScene.name : "";
            this.m_dialogSceneName = (this.m_dialogScene && !string.IsNullOrEmpty(this.m_dialogScene.name)) ? this.m_dialogScene.name : "";

#endif

        }

    }

}
