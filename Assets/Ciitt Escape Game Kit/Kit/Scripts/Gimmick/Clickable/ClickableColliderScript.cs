#pragma warning disable 0618

using System;
using System.Collections;
using System.Collections.Generic;
using SSC;
using UnityEngine;

namespace ciitt.EscapeGameKit
{

    /// <summary>
    /// Clickable
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class ClickableColliderScript : MonoBehaviour
    {

        public enum LockState
        {
            Locked,
            Unlocked
        }

        /// <summary>
        /// Lock state
        /// </summary>
        [SerializeField]
        [Tooltip("Lock state")]
        protected LockState m_lockState = LockState.Unlocked;

        /// <summary>
        /// Reference to ViewPoint
        /// </summary>
        [SerializeField]
        [Tooltip("Reference to ViewPoint")]
        protected ViewPoint m_refTargetViewPoint = null;

        /// <summary>
        /// Way point list
        /// </summary>
        [SerializeField]
        [Tooltip("Way point list")]
        protected List<Transform> m_wayPointList = new List<Transform>();

        /// <summary>
        /// Disable Collider at Start (Obsoleted)
        /// </summary>
        [SerializeField]
        [Tooltip("Disable Collider at Start (Obsoleted)")]
        [Obsolete("Use m_lockState")]
        protected bool m_disableColliderAtStart = false;

        /// <summary>
        /// Reference to StateWatcher<MainGameSceneState>
        /// </summary>
        protected StateWatcher<MainGameSceneState> m_refMainGameSceneStateWatcher = null;

        /// <summary>
        /// Reference to Collider
        /// </summary>
        protected Collider m_refCollider = null;

        // ----------------------------------------------------------------------------------

        /// <summary>
        /// Reference to ViewPoint
        /// </summary>
        public ViewPoint targetViewPoint { get { return this.m_refTargetViewPoint; } }

        /// <summary>
        /// Way point list
        /// </summary>
        public List<Transform> wayPointList { get { return this.m_wayPointList; } }

        /// <summary>
        /// Lock state
        /// </summary>
        public LockState lockState { get { return this.m_lockState; } }

        /// <summary>
        /// Reference to Collider
        /// </summary>
        public Collider refCollider { get { return this.m_refCollider; } }

        /// <summary>
        /// Disable Collider at Start
        /// </summary>
        [Obsolete("Use lockState")]
        public bool disableColliderAtStart { get { return this.m_disableColliderAtStart; } }

        /// <summary>
        /// Awake
        /// </summary>
        // ----------------------------------------------------------------------------------
        protected virtual void Awake()
        {

            this.m_refCollider = this.GetComponent<Collider>();

        }

        /// <summary>
        /// Start
        /// </summary>
        // ----------------------------------------------------------------------------------
        protected virtual void Start()
        {

#if UNITY_EDITOR

            // m_refTargetViewPoint
            {

                if (!this.m_refTargetViewPoint)
                {
                    Debug.LogError("m_refTargetViewPoint is null : " + Funcs.createHierarchyPath(this.transform));
                }

            }

            // m_wayPointList
            {

                for (int i = this.m_wayPointList.Count - 1; i >= 0; i--)
                {
                    if (!this.m_wayPointList[i])
                    {
                        Debug.LogError("m_wayPointList has empty element : " + Funcs.createHierarchyPath(this.transform));
                        this.m_wayPointList.RemoveAt(i);
                    }
                }

            }

            // m_refCollider
            {

                if (!this.m_refCollider.enabled)
                {
                    Debug.LogError("Use m_disableColliderAtStart : " + Funcs.createHierarchyPath(this.transform));
                }

            }

#endif

            // m_refMainGameSceneStateWatcher
            {
                this.m_refMainGameSceneStateWatcher = CustomReduxManager.CustomReduxManagerInstance.MainGameSceneStateWatcher;
            }

            // m_disableColliderAtStart
            {

                if (this.m_disableColliderAtStart)
                {
                    this.m_refCollider.enabled = false;


#if UNITY_EDITOR
                    Debug.LogWarningFormat(
                        "[m_disableColliderAtStart] is obsoledted, so please use [m_lockState]'s [Locked] enum state : {0}",
                        Funcs.createHierarchyPath(this.transform)
                        );
#endif

                }

                else
                {

                    if (this.m_lockState == LockState.Locked)
                    {
                        this.lockThis(true);
                    }

                    else
                    {
                        this.unlockThis();
                    }

                }

            }

        }

        /// <summary>
        /// Click
        /// </summary>
        // ----------------------------------------------------------------------------------
        public virtual void onClickInEscapeGame()
        {

            MainGameSceneState mgsState = this.m_refMainGameSceneStateWatcher.state();

            if (mgsState.stateEnum != MainGameSceneState.StateEnum.MainGameSceneMain)
            {
                return;
            }

            // -----------------------

            if (this.m_refTargetViewPoint != mgsState.changeCameraViewInfo.currentTargetViewPoint)
            {
                this.changeCameraView();
            }

            else
            {

                if (this.m_lockState == LockState.Locked)
                {
                    this.actionWhenLocked();
                }

                else
                {
                    this.actionWhenUnlocked();
                }

            }

        }

        /// <summary>
        /// Change camera view
        /// </summary>
        // ----------------------------------------------------------------------------------
        protected virtual void changeCameraView()
        {

            MainGameSceneState mgsState = this.m_refMainGameSceneStateWatcher.state();

            if (mgsState.stateEnum != MainGameSceneState.StateEnum.MainGameSceneMain)
            {
                return;
            }

            // -----------------------

            ViewPoint current = mgsState.changeCameraViewInfo.currentTargetViewPoint;

            // -----------------------

            if(this.m_refTargetViewPoint && this.m_refTargetViewPoint.isAbleToMoveCameraToThis(current))
            {

                mgsState.setChangeCameraViewState(
                    this.m_refTargetViewPoint,
                    this.m_wayPointList,
                    true
                    );

            }

        }

        /// <summary>
        /// Function when locked
        /// </summary>
        // ----------------------------------------------------------------------------------
        protected virtual void actionWhenLocked()
        {

        }

        /// <summary>
        /// Function when unlocked
        /// </summary>
        // ----------------------------------------------------------------------------------
        protected virtual void actionWhenUnlocked()
        {

        }

        /// <summary>
        /// Lock this
        /// </summary>
        // ----------------------------------------------------------------------------------
        public virtual void lockThis(bool disableCollider)
        {

            this.m_lockState = LockState.Locked;

            if(disableCollider)
            {
                this.m_refCollider.enabled = false;
            }

        }

        /// <summary>
        /// Unlock this
        /// </summary>
        // ----------------------------------------------------------------------------------
        public virtual void unlockThis()
        {

            this.m_lockState = LockState.Unlocked;

            if (!this.m_refCollider.enabled)
            {
                this.m_refCollider.enabled = true;
            }

        }

#if UNITY_EDITOR

        /// <summary>
        /// Calculate bounds for BoxCollider
        /// </summary>
        // ----------------------------------------------------------------------------------
        public void calculateBoundsForBoxCollider()
        {

            BoxCollider bc = this.GetComponent<BoxCollider>();

            MeshFilter[] mfs = this.GetComponentsInChildren<MeshFilter>(true);

            if (!bc)
            {
                Debug.LogError("Not found BoxCollider");
                return;
            }

            if (mfs.Length <= 0)
            {
                Debug.LogError("Not found MeshFilter");
                return;
            }

            UnityEditor.Undo.RecordObject(bc, "Changed Box Collider");

            // --------------

            Vector3 _min = Vector3.one * 1000000f;
            Vector3 _max = Vector3.one * -1000000f;

            Vector3 tempMin = Vector3.zero;
            Vector3 tempMax = Vector3.zero;

            Vector3 tempInverse = Vector3.zero;

            Vector3 center = Vector3.zero;

            foreach (MeshFilter rend in mfs)
            {

                tempInverse = this.transform.InverseTransformPoint(rend.transform.position);

                center = tempInverse + rend.sharedMesh.bounds.center;

                tempMin = center - rend.sharedMesh.bounds.extents;
                tempMax = center + rend.sharedMesh.bounds.extents;

                _min.x = Mathf.Min(_min.x, tempMin.x);
                _min.y = Mathf.Min(_min.y, tempMin.y);
                _min.z = Mathf.Min(_min.z, tempMin.z);

                _max.x = Mathf.Max(_max.x, tempMax.x);
                _max.y = Mathf.Max(_max.y, tempMax.y);
                _max.z = Mathf.Max(_max.z, tempMax.z);

            }

            // center
            {
                bc.center = (_max + _min) / 2.0f;
            }

            // size
            {

                Vector3 temp = Vector3.one;

                temp.x = Mathf.Abs(_max.x - _min.x);
                temp.y = Mathf.Abs(_max.y - _min.y);
                temp.z = Mathf.Abs(_max.z - _min.z);

                bc.size = temp;

            }

        }

#endif

    }

}
