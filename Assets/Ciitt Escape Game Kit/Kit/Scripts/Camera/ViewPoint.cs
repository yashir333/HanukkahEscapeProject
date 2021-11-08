using System;
using System.Collections;
using System.Collections.Generic;
using SSC;
using UnityEngine;

namespace ciitt.EscapeGameKit
{

    /// <summary>
    /// View point
    /// </summary>
    [ExecuteInEditMode]
    public class ViewPoint : MonoBehaviour
    {

        /// <summary>
        /// User progress data
        /// </summary>
        [Serializable]
        class UserProgressData
        {

            public bool current = false;

            public void clear()
            {
                this.current = false;
            }

        }

        /// <summary>
        /// Position
        /// </summary>
        [SerializeField]
        [Tooltip("Position")]
        Vector3 m_position = Vector3.zero;

        /// <summary>
        /// Rotation
        /// </summary>
        [SerializeField]
        [Tooltip("Rotation")]
        Vector3 m_rotation = Vector3.zero;

        /// <summary>
        /// Disable move to parent by click
        /// </summary>
        [SerializeField]
        [Tooltip("Disable move to parent by click")]
        bool m_disableMoveToParentByClick = false;

        /// <summary>
        /// ViewPoint parent
        /// </summary>
        ViewPoint m_reViewPointParent = null;

        /// <summary>
        /// Reference to ViewPointGroup
        /// </summary>
        ViewPointGroup m_refViewPointGroup = null;

        /// <summary>
        /// UserProgressData
        /// </summary>
        UserProgressData m_userProgressData = new UserProgressData();

        // -----------------------------------------------------------------------------------

        /// <summary>
        /// Position
        /// </summary>
        public Vector3 worldPosition { get { return this.m_position; } }

        /// <summary>
        /// Rotation
        /// </summary>
        public Vector3 worldRotation { get { return this.m_rotation; } }

        /// <summary>
        /// Disable move to parent by click
        /// </summary>
        public bool disableMoveToParentByClick { get { return this.m_disableMoveToParentByClick; } }

        /// <summary>
        /// Reference to ViewPointGroup
        /// </summary>
        public ViewPointGroup viewPointGroup { get { return this.m_refViewPointGroup; } }

        /// <summary>
        /// ViewPoint parent
        /// </summary>
        public ViewPoint viewPointParent { get { return this.m_reViewPointParent; } }

        /// <summary>
        /// Awake
        /// </summary>
        // -----------------------------------------------------------------------------------
        void Awake()
        {

            // Avoid ExecuteInEditMode
            if (!Application.isPlaying)
            {
                return;
            }

            // m_refViewPointGroup
            {

                this.m_refViewPointGroup = this.GetComponentInParent<ViewPointGroup>();

#if UNITY_EDITOR

                if (!this.m_refViewPointGroup)
                {
                    Debug.LogError("m_refViewPointGroup is null : " + Funcs.createHierarchyPath(this.transform));
                }

#endif

            }

            // m_reViewPointfParent
            {

                if (this.transform.parent)
                {
                    this.m_reViewPointParent = this.transform.parent.GetComponent<ViewPoint>();
                }

            }

        }

        /// <summary>
        /// Start
        /// </summary>
        // -----------------------------------------------------------------------------------
        void Start()
        {

            // Avoid ExecuteInEditMode
            if (!Application.isPlaying)
            {
                return;
            }

            // CustomReduxManager
            {
                CustomReduxManager.CustomReduxManagerInstance.addSceneChangeStateReceiver(this.onSceneChangeStateReceiver);
                CustomReduxManager.CustomReduxManagerInstance.addUserProgressDataSignalReceiver(this.onUserProgressDataSignal);
            }

        }

        /// <summary>
        /// Is able to move camera
        /// </summary>
        /// <returns>able</returns>
        // -----------------------------------------------------------------------------------
        public virtual bool isAbleToMoveCameraToThis(ViewPoint from)
        {

            if (!from || !from.viewPointGroup)
            {
                return false;
            }

            // ------------------

            if (this.m_refViewPointGroup == from.viewPointGroup)
            {

                if (from.disableMoveToParentByClick)
                {

                    ViewPoint parent = from.viewPointParent;

                    while(parent)
                    {

                        if(parent == this)
                        {
                            return false;
                        }

                        parent = parent.viewPointParent;

                    }

                    return true;

                }

                return true;

            }

            return false;

        }

        /// <summary>
        /// UserProgressDataSignal receiver
        /// </summary>
        /// <param name="updSignal">UserProgressDataSignal</param>
        // ------------------------------------------------------------------------------------------
        void onUserProgressDataSignal(UserProgressDataSignal updSignal)
        {

            this.m_userProgressData.current =
                CustomReduxManager.CustomReduxManagerInstance.MainGameSceneStateWatcher.state().changeCameraViewInfo.currentTargetViewPoint == this;

            updSignal.addDataAction(
                SystemManager.Instance.createKeyPath(this.transform, this),
                JsonUtility.ToJson(this.m_userProgressData)
                );

        }

        /// <summary>
        /// SceneChangeState receiver
        /// </summary>
        /// <param name="scState">SceneChangeState</param>
        // ------------------------------------------------------------------------------------------
        void onSceneChangeStateReceiver(SceneChangeState scState)
        {

            if (scState.stateEnum == SceneChangeState.StateEnum.AllStartupsDone)
            {

                CustomSceneChangeManager cscManager = CustomSceneChangeManager.Instance as CustomSceneChangeManager;

                if (cscManager.isLoadingCurrentSceneWithUserProgressData())
                {

                    this.m_userProgressData = cscManager.getDataFromCurrentUserProgressData<UserProgressData>(this.transform, this);

                    if (this.m_userProgressData.current)
                    {
                        CustomReduxManager.CustomReduxManagerInstance.MainGameSceneStateWatcher.state().changeCameraViewInfo.currentTargetViewPoint = this;
                    }

                }

            }

        }

        // -----------------------------------------------------------------------------------

#if UNITY_EDITOR

        public void moveSceneAndMainCamera()
        {

            Camera.main.transform.SetPositionAndRotation(
                this.m_position,
                Quaternion.Euler(this.m_rotation)
                );

            UnityEditor.SceneView.lastActiveSceneView.LookAtDirect(this.m_position, Quaternion.Euler(this.m_rotation), 0.001f);

        }

        public void startSync()
        {
            UnityEditor.EditorApplication.update -= this.SceneUpdate;
            UnityEditor.EditorApplication.update += this.SceneUpdate;
        }

        public void stopSync()
        {
            UnityEditor.EditorApplication.update -= this.SceneUpdate;
        }

        void SceneUpdate()
        {

            if (UnityEditor.Selection.activeTransform != this.transform)
            {
                this.stopSync();
                return;
            }

            if (!UnityEditor.SceneView.lastActiveSceneView)
            {
                return;
            }

            {

                Camera sceneCamera = UnityEditor.SceneView.lastActiveSceneView.camera;

                if (sceneCamera)
                {
                    this.m_position = sceneCamera.transform.position;
                    this.m_rotation = sceneCamera.transform.rotation.eulerAngles;
                }

            }

            if (Camera.main)
            {
                Camera.main.transform.SetPositionAndRotation(
                    this.m_position,
                    Quaternion.Euler(this.m_rotation)
                    );
            }

        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(this.m_position, 0.1f);
        }

#endif

    }

}
