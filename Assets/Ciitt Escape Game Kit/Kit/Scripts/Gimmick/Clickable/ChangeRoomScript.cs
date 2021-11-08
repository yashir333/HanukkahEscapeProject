using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ciitt.EscapeGameKit
{

    /// <summary>
    /// Change room
    /// </summary>
    public class ChangeRoomScript : ClickableColliderScript
    {

        [Space(20.0f)]

        /// <summary>
        /// Reference to another ViewPoint
        /// </summary>
        [SerializeField]
        [Tooltip("Reference to another ViewPoint")]
        protected ViewPoint m_refAnotherTargetViewPoint = null;

        /// <summary>
        /// Another way point list
        /// </summary>
        [SerializeField]
        [Tooltip("Another way point list")]
        protected List<Transform> m_anotherWayPointList = new List<Transform>();

        /// <summary>
        /// Start
        /// </summary>
        // ----------------------------------------------------------------------------------
        protected override void Start()
        {

            base.Start();

#if UNITY_EDITOR

            // m_refAnotherTargetViewPoint
            {

                if (!this.m_refAnotherTargetViewPoint)
                {
                    Debug.LogError("m_refAnotherTargetViewPoint is null : " + Funcs.createHierarchyPath(this.transform));
                }

            }

#endif

        }

        /// <summary>
        /// Click
        /// </summary>
        // ----------------------------------------------------------------------------------
        public override void onClickInEscapeGame()
        {

            MainGameSceneState mgsState = this.m_refMainGameSceneStateWatcher.state();

            if (mgsState.stateEnum != MainGameSceneState.StateEnum.MainGameSceneMain)
            {
                return;
            }

            // -----------------------

            if (this.m_lockState == LockState.Locked)
            {
                this.actionWhenLocked();
            }

            else
            {
                this.changeCameraView();
            }

        }

        /// <summary>
        /// Change camera view
        /// </summary>
        // ----------------------------------------------------------------------------------
        protected override void changeCameraView()
        {

            MainGameSceneState mgsState = this.m_refMainGameSceneStateWatcher.state();

            if (mgsState.stateEnum != MainGameSceneState.StateEnum.MainGameSceneMain)
            {
                return;
            }

            // -----------------------

            if (
                this.m_refTargetViewPoint &&
                this.m_refTargetViewPoint.viewPointGroup == mgsState.changeCameraViewInfo.currentTargetViewPoint.viewPointGroup
                )
            {

                mgsState.setChangeCameraViewState(
                    this.m_refAnotherTargetViewPoint,
                    this.m_anotherWayPointList,
                    true
                    );

            }

            else if (
                this.m_refAnotherTargetViewPoint &&
                this.m_refAnotherTargetViewPoint.viewPointGroup == mgsState.changeCameraViewInfo.currentTargetViewPoint.viewPointGroup
                )
            {

                mgsState.setChangeCameraViewState(
                    this.m_refTargetViewPoint,
                    this.m_wayPointList,
                    true
                    );

            }

        }

    }

}
