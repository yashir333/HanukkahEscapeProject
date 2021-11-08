using System;
using System.Collections.Generic;
using UnityEngine;

namespace ciitt.EscapeGameKit
{

    /// <summary>
    /// MainGameSceneState class
    /// </summary>
    public class MainGameSceneState : SSC.SReduxStateBase<MainGameSceneState>
    {

        /// <summary>
        /// enum state
        /// </summary>
        public enum StateEnum
        {
            Neutral,
            MainGameSceneMain,
            MainGameSceneItemShowroom,
            MainGameSceneChangeCameraView,
            MainGameSceneFieldObjectEvolution,
            MainGameSceneItemEvolution,
            MainGameSceneOptions,
            MainGameSceneHowToPlay,
            TitleSceneMain,
            TitleSceneOptions,
            EndingSceneMain,
        }

        public class ChangeCameraViewInfo
        {

            public ViewPoint currentTargetViewPoint = null;
            public List<Transform> wayPointList = new List<Transform>();
            public bool rotateForcibly = false;

            public void reset()
            {
                this.currentTargetViewPoint = null;
                this.wayPointList.Clear();
                this.rotateForcibly = false;
            }

        }

        public class CurrentSelectedItemInfo
        {

            public ItemWaitingRoomScript currentShowroomItem = null;

            public void reset()
            {
                this.currentShowroomItem = null;
            }

        }

        public class FieldObjectEvolutionInfo
        {

            public EvolvableFieldObjectScript evolvableFieldObject = null;

            public void reset()
            {
                this.evolvableFieldObject = null;
            }

        }

        /// <summary>
        /// Current state
        /// </summary>
        public StateEnum stateEnum = StateEnum.Neutral;

        /// <summary>
        /// Previous state
        /// </summary>
        public StateEnum previousStateEnum = StateEnum.Neutral;

        /// <summary>
        /// ChangeCameraViewInfo
        /// </summary>
        public ChangeCameraViewInfo changeCameraViewInfo = new ChangeCameraViewInfo();

        /// <summary>
        /// CurrentSelectedItemInfo
        /// </summary>
        public CurrentSelectedItemInfo currentSelectedItemInfo = new CurrentSelectedItemInfo();

        /// <summary>
        /// FieldObjectEvolutionInfo
        /// </summary>
        public FieldObjectEvolutionInfo currentFieldObjectEvolutionInfo = new FieldObjectEvolutionInfo();

        /// <summary>
        /// Set state
        /// </summary>
        /// <param name="_stateEnum">stateEnum</param>
        // ----------------------------------------------------------------------------------------------
        public void setState(StateEnum _stateEnum)
        {
            this.previousStateEnum = this.stateEnum;
            this.stateEnum = _stateEnum;
            this.m_refWatcher.sendState();
        }

        /// <summary>
        /// Set ChangeCameraView state
        /// </summary>
        /// <param name="_targetViewPoint">ViewPoint</param>
        // ----------------------------------------------------------------------------------------------
        public void setChangeCameraViewState(ViewPoint _targetViewPoint)
        {

            this.previousStateEnum = this.stateEnum;

            this.stateEnum = StateEnum.MainGameSceneChangeCameraView;

            this.changeCameraViewInfo.currentTargetViewPoint = _targetViewPoint;
            this.changeCameraViewInfo.wayPointList.Clear();

            this.m_refWatcher.sendState();

        }

        /// <summary>
        /// Set ChangeCameraView state
        /// </summary>
        /// <param name="_targetViewPoint">ViewPoint</param>
        /// <param name="_wayPointList">Transform list</param>
        /// <param name="_rotateForcibly">Forcibly rotate camera</param>
        // ----------------------------------------------------------------------------------------------
        public void setChangeCameraViewState(
            ViewPoint _targetViewPoint,
            List<Transform> _wayPointList,
            bool _rotateForcibly
            )
        {

            this.previousStateEnum = this.stateEnum;

            this.stateEnum = StateEnum.MainGameSceneChangeCameraView;

            this.changeCameraViewInfo.currentTargetViewPoint = _targetViewPoint;
            this.changeCameraViewInfo.wayPointList.Clear();
            this.changeCameraViewInfo.wayPointList.AddRange(_wayPointList);

            this.changeCameraViewInfo.rotateForcibly = _rotateForcibly;

            this.m_refWatcher.sendState();

        }

        /// <summary>
        /// Set ItemShowroom state
        /// </summary>
        /// <param name="showroomItem">ItemObjectScript</param>
        // ----------------------------------------------------------------------------------------------
        public void setItemShowroomState(ItemWaitingRoomScript showroomItem)
        {

            this.previousStateEnum = this.stateEnum;

            this.stateEnum = StateEnum.MainGameSceneItemShowroom;

            this.currentSelectedItemInfo.currentShowroomItem = showroomItem;

            this.m_refWatcher.sendState();

        }

        /// <summary>
        /// Set FieldObjectEvolution state
        /// </summary>
        /// <param name="evolvableFieldObject">EvolvableFieldObjectScript</param>
        // ----------------------------------------------------------------------------------------------
        public void setFieldObjectEvolution(EvolvableFieldObjectScript evolvableFieldObject)
        {

            this.previousStateEnum = this.stateEnum;

            this.stateEnum = StateEnum.MainGameSceneFieldObjectEvolution;

            this.currentFieldObjectEvolutionInfo.evolvableFieldObject = evolvableFieldObject;

            if(evolvableFieldObject && evolvableFieldObject.viewPointWhenEvolution.viewPoint)
            {
                this.changeCameraViewInfo.currentTargetViewPoint = evolvableFieldObject.viewPointWhenEvolution.viewPoint;
                this.changeCameraViewInfo.wayPointList.Clear();
            }

            this.m_refWatcher.sendState();

        }

        /// <summary>
        /// Reset params on scene loaded
        /// </summary>
        // ----------------------------------------------------------------------------------------------
        public override void resetOnSceneLevelLoaded()
        {

            this.previousStateEnum = StateEnum.Neutral;
            this.stateEnum = StateEnum.Neutral;

            this.changeCameraViewInfo.reset();
            this.currentSelectedItemInfo.reset();
            this.currentFieldObjectEvolutionInfo.reset();

        }

        // ----------------------------------------------------------------------------------------------
        // ----------------------------------------------------------------------------------------------
        // ----------------------------------------------------------------------------------------------

        // Obsolete

        /// <summary>
        /// Set state
        /// </summary>
        /// <param name="watcher">watcher</param>
        /// <param name="_stateEnum">stateEnum</param>
        [Obsolete("Use setState(StateEnum)", false)]
        // ----------------------------------------------------------------------------------------------
        public void setState(SSC.StateWatcher<MainGameSceneState> watcher, StateEnum _stateEnum)
        {
            this.setState(_stateEnum);
        }

        /// <summary>
        /// Set ChangeCameraView state
        /// </summary>
        /// <param name="watcher">watcher</param>
        /// <param name="_targetViewPoint">ViewPoint</param>
        [Obsolete("Use setChangeCameraViewState(ViewPoint)", false)]
        // ----------------------------------------------------------------------------------------------
        public void setChangeCameraViewState(
            SSC.StateWatcher<MainGameSceneState> watcher,
            ViewPoint _targetViewPoint
            )
        {
            this.setChangeCameraViewState(_targetViewPoint);
        }

        /// <summary>
        /// Set ChangeCameraView state
        /// </summary>
        /// <param name="watcher">watcher</param>
        /// <param name="_targetViewPoint">ViewPoint</param>
        /// <param name="_wayPointList">Transform list</param>
        /// <param name="_rotateForcibly">Forcibly rotate camera</param>
        [Obsolete("Use setChangeCameraViewState(ViewPoint, List<Transform>, bool)", false)]
        // ----------------------------------------------------------------------------------------------
        public void setChangeCameraViewState(
            SSC.StateWatcher<MainGameSceneState> watcher,
            ViewPoint _targetViewPoint,
            List<Transform> _wayPointList,
            bool _rotateForcibly
            )
        {
            this.setChangeCameraViewState(_targetViewPoint, _wayPointList, _rotateForcibly);
        }

        /// <summary>
        /// Set ItemShowroom state
        /// </summary>
        /// <param name="watcher">watcher</param>
        /// <param name="showroomItem">ItemObjectScript</param>
        [Obsolete("Use setItemShowroomState(ItemWaitingRoomScript)", false)]
        // ----------------------------------------------------------------------------------------------
        public void setItemShowroomState(
            SSC.StateWatcher<MainGameSceneState> watcher,
            ItemWaitingRoomScript showroomItem
            )
        {
            this.setItemShowroomState(showroomItem);
        }

        /// <summary>
        /// Set FieldObjectEvolution state
        /// </summary>
        /// <param name="watcher">watcher</param>
        /// <param name="evolvableFieldObject">EvolvableFieldObjectScript</param>
        [Obsolete("Use setFieldObjectEvolution(EvolvableFieldObjectScript)", false)]
        // ----------------------------------------------------------------------------------------------
        public void setFieldObjectEvolution(
            SSC.StateWatcher<MainGameSceneState> watcher,
            EvolvableFieldObjectScript evolvableFieldObject
            )
        {
            this.setFieldObjectEvolution(evolvableFieldObject);
        }

    }

}
