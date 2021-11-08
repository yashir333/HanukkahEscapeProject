using System;
using System.Collections;
using System.Collections.Generic;
using SSC;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ciitt.EscapeGameKit
{

    /// <summary>
    /// Player camera
    /// </summary>
    public class PlayerCameraScript : MonoBehaviour
    {

        /// <summary>
        /// User progress data
        /// </summary>
        [Serializable]
        class UserProgressData
        {

            public Vector3 localPosition = Vector3.zero;
            public Quaternion localRotation = Quaternion.identity;

            public void clear()
            {
                this.localPosition = Vector3.zero;
                this.localRotation = Quaternion.identity;
            }

        }

        /// <summary>
        /// Reference to Camera
        /// </summary>
        [SerializeField]
        [Tooltip("Reference to Camera")]
        Camera m_refCamera = null;

        /// <summary>
        /// First ViewPoint
        /// </summary>
        [SerializeField]
        [Tooltip("First ViewPoint")]
        ViewPoint m_refFirstViewPoint = null;

        /// <summary>
        /// Raycast distance for click
        /// </summary>
        [SerializeField]
        [Tooltip("Raycast distance for click")]
        float m_clickRaycastDistance = 20.0f;

        /// <summary>
        /// Strength for yaw
        /// </summary>
        [SerializeField]
        [Tooltip("Strength for yaw")]
        float m_strengthYaw = 100.0f;

        /// <summary>
        /// Strength for pitch
        /// </summary>
        [SerializeField]
        [Tooltip("Strength for pitch")]
        float m_strengthPitch = 100.0f;

        /// <summary>
        /// Camera speed
        /// </summary>
        [SerializeField]
        [Tooltip("Camera speed")]
        [Range(0.1f, 10.0f)]
        float m_changeViewMeterPerSecond = 2.0f;

        /// <summary>
        /// Min zoom fov
        /// </summary>
        [SerializeField]
        [Tooltip("Min zoom fov")]
        float m_minZoomFov = 30.0f;

        /// <summary>
        /// Max zoom fov
        /// </summary>
        [SerializeField]
        [Tooltip("Max zoom fov")]
        float m_maxZoomFov = 60.0f;

        /// <summary>
        /// Previous
        /// </summary>
        Vector3 m_previousMousePosition = Vector3.zero;

        /// <summary>
        /// Current yaw degree
        /// </summary>
        float m_currentDegreeYaw = 0.0f;

        /// <summary>
        /// Current pitch degree
        /// </summary>
        float m_currentDegreePitch = 0.0f;

        /// <summary>
        /// Dummy
        /// </summary>
        List<Transform> m_dummyTransformList = new List<Transform>();

        /// <summary>
        /// Clicked position down
        /// </summary>
        Vector3 m_clickedPositionDown = Vector3.zero;

        /// <summary>
        /// Clicked position down time
        /// </summary>
        float m_clickedPositionDownTime = 0.0f;

        /// <summary>
        /// UserProgressData
        /// </summary>
        UserProgressData m_userProgressData = new UserProgressData();

        /// <summary>
        /// SimpleBezierPath
        /// </summary>
        SimpleBezierPath m_simpleBezierPath = new SimpleBezierPath();

        /// <summary>
        /// Delta position X
        /// </summary>
        float m_deltaPositionX = 0.0f;

        /// <summary>
        /// Delta position Y
        /// </summary>
        float m_deltaPositionY = 0.0f;

        /// <summary>
        /// Temp list
        /// </summary>
        List<RaycastResult> m_tempListForPointerOverUIObject = new List<RaycastResult>();

        /// <summary>
        /// Start
        /// </summary>
        // -----------------------------------------------------------------------
        void Start()
        {

#if UNITY_EDITOR

            if (!this.m_refCamera)
            {
                Debug.LogError("m_refCamera == null : " + Funcs.createHierarchyPath(this.transform));
            }

            if (!this.m_refFirstViewPoint)
            {
                Debug.LogError("m_refFirstViewPoint == null : " + Funcs.createHierarchyPath(this.transform));
            }

#endif

            // m_refFirstPosition
            {

                if (this.m_refFirstViewPoint)
                {

                    this.transform.SetPositionAndRotation(
                        this.m_refFirstViewPoint.worldPosition,
                        Quaternion.Euler(this.m_refFirstViewPoint.worldRotation)
                        );

                    CustomReduxManager.CustomReduxManagerInstance.MainGameSceneStateWatcher.state().
                        changeCameraViewInfo.currentTargetViewPoint = this.m_refFirstViewPoint;

                }

            }

            // CustomReduxManager
            {
                CustomReduxManager.CustomReduxManagerInstance.addMainGameSceneStateReceiver(this.onMainGameSceneStateReceiver);
                CustomReduxManager.CustomReduxManagerInstance.addSceneChangeStateReceiver(this.onSceneChangeStateReceiver);
                CustomReduxManager.CustomReduxManagerInstance.addUserProgressDataSignalReceiver(this.onUserProgressDataSignal);
            }

        }

        /// <summary>
        /// Update
        /// </summary>
        // -----------------------------------------------------------------------
        void Update()
        {

            // input
            {

                if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
                {
                    this.m_clickedPositionDown = Input.mousePosition;
                    this.m_clickedPositionDownTime = Time.realtimeSinceStartup;
                }

            }

            // calcDeltaPosition
            {
                this.calcDeltaPosition();
            }

            // zoomCamera
            {
                this.zoomCamera();
            }

            // rotateCamera
            {
                this.rotateCamera();
            }

            // rayCast
            {
                this.rayCast();
            }

        }

        /// <summary>
        /// Calculate delta position
        /// </summary>
        // -----------------------------------------------------------------------
        void calcDeltaPosition()
        {

            Vector3 mousePosition = Input.mousePosition;

            if (Input.touchCount > 0)
            {
                mousePosition = Input.GetTouch(0).position;
            }

            // -------------------------

            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.touchCount >= 2)
            {
                this.m_deltaPositionX = 0.0f;
                this.m_deltaPositionY = 0.0f;
            }

            else if(Input.touchCount == 1 && Input.GetTouch(0).fingerId != 0)
            {
                this.m_deltaPositionX = 0.0f;
                this.m_deltaPositionY = 0.0f;
            }

            else
            {
                this.m_deltaPositionX = (this.m_previousMousePosition.x - mousePosition.x) / (float)Screen.width;
                this.m_deltaPositionY = (this.m_previousMousePosition.y - mousePosition.y) / (float)Screen.height;

            }

            // m_previousClickedPositionDown
            {
                this.m_previousMousePosition = mousePosition;
            }

        }

        /// <summary>
        /// Set current degrees
        /// </summary>
        // -----------------------------------------------------------------------
        void setCurrentDegreeValues()
        {

            this.m_currentDegreePitch = this.transform.localEulerAngles.x;
            this.m_currentDegreeYaw = this.transform.localEulerAngles.y;

            if (this.m_currentDegreePitch > 180.0f)
            {
                this.m_currentDegreePitch = this.m_currentDegreePitch - 360.0f;
            }

            else if (this.m_currentDegreePitch < -180.0f)
            {
                this.m_currentDegreePitch = this.m_currentDegreePitch + 360.0f;
            }

        }

        /// <summary>
        /// Raycast
        /// </summary>
        // -----------------------------------------------------------------------
        void rayCast()
        {

            MainGameSceneState mgsState = CustomReduxManager.CustomReduxManagerInstance.MainGameSceneStateWatcher.state();

            if (mgsState.stateEnum != MainGameSceneState.StateEnum.MainGameSceneMain)
            {
                return;
            }

            // -----------------------


#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
            
            if(Input.touchCount != 1)
            {
                return;
            }

            //
            {

                Touch touch = Input.GetTouch(0);

                if (touch.phase != TouchPhase.Ended || this.isPointerOverUIObject())
                {
                    return;
                }

            }

#else

            if (!Input.GetMouseButtonUp(0) || EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

#endif

            if (Time.realtimeSinceStartup - this.m_clickedPositionDownTime > 0.5f)
            {
                return;
            }

            if (Vector3.Distance(this.m_clickedPositionDown, Input.mousePosition) > 10.0f)
            {
                return;
            }

            // -----------------------

            // Raycast
            {

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, this.m_clickRaycastDistance))
                {

                    ClickableColliderScript ccs = hit.transform.GetComponent<ClickableColliderScript>();

                    if (ccs)
                    {
                        ccs.onClickInEscapeGame();
                    }

                }

            }

        }

        /// <summary>
        /// Rotate camera
        /// </summary>
        // -----------------------------------------------------------------------
        void rotateCamera()
        {

            if (Input.touchCount >= 2)
            {
                return;
            }

            MainGameSceneState mgsState = CustomReduxManager.CustomReduxManagerInstance.MainGameSceneStateWatcher.state();

            if (mgsState.stateEnum != MainGameSceneState.StateEnum.MainGameSceneMain)
            {
                return;
            }

            // -------------

            //if (EventSystem.current.IsPointerOverGameObject())
            //{
            //    return;
            //}

            if (Vector3.Distance(this.m_clickedPositionDown, Input.mousePosition) < 10.0f)
            {
                return;
            }

            if (Input.touchCount > 0)
            {

                if (Input.GetTouch(0).phase != TouchPhase.Moved)
                {
                    return;
                }

            }

            // -------------

            if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
            {

                this.setCurrentDegreeValues();

                float flipSign = SystemManager.Instance.configDataSO.rotationFlip ? -1 : 1;

                float movePitch = this.m_deltaPositionY * this.m_strengthPitch * flipSign;
                float moveYaw = -this.m_deltaPositionX * this.m_strengthYaw * flipSign;

                this.m_currentDegreePitch += movePitch;
                this.m_currentDegreeYaw += moveYaw;

                this.m_currentDegreePitch = Mathf.Clamp(this.m_currentDegreePitch, -85f, 85f);
                this.m_currentDegreeYaw = this.m_currentDegreeYaw % 360f;

                this.transform.localRotation = Quaternion.Euler(this.m_currentDegreePitch, this.m_currentDegreeYaw, 0.0f);

            }

        }

        /// <summary>
        /// Zoom camera
        /// </summary>
        // -----------------------------------------------------------------------
        void zoomCamera()
        {

            if(!this.m_refCamera)
            {
                return;
            }

            MainGameSceneState mgsState = CustomReduxManager.CustomReduxManagerInstance.MainGameSceneStateWatcher.state();

            if (mgsState.stateEnum != MainGameSceneState.StateEnum.MainGameSceneMain)
            {
                return;
            }

            // ------------------------

            if(Input.GetAxis("Mouse ScrollWheel") > 0.0f)
            {
                this.m_refCamera.fieldOfView = Mathf.Max(this.m_minZoomFov, this.m_refCamera.fieldOfView - 2.0f);
            }

            else if (Input.GetAxis("Mouse ScrollWheel") < 0.0f)
            {
                this.m_refCamera.fieldOfView = Mathf.Min(this.m_maxZoomFov, this.m_refCamera.fieldOfView + 2.0f);
            }

            else if (Input.touchCount == 2)
            {

                Touch touchZero = Input.GetTouch(0);
                Touch touchOne = Input.GetTouch(1);

                Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
                Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

                float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
                float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

                float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

                this.m_refCamera.fieldOfView =
                    Mathf.Clamp(
                        this.m_refCamera.fieldOfView + (deltaMagnitudeDiff * 0.1f),
                        this.m_minZoomFov,
                        this.m_maxZoomFov
                        );

            }

        }

        /// <summary>
        /// MainGameSceneState receiver
        /// </summary>
        /// <param name="mgsState">MainGameSceneState</param>
        // -----------------------------------------------------------------------
        void onMainGameSceneStateReceiver(MainGameSceneState mgsState)
        {

            if (mgsState.stateEnum == MainGameSceneState.StateEnum.MainGameSceneChangeCameraView)
            {

                StopAllCoroutines();

                StartCoroutine(this.moveCamera(
                    mgsState.changeCameraViewInfo.currentTargetViewPoint,
                    mgsState.changeCameraViewInfo.wayPointList,
                    0.0f,
                    mgsState.changeCameraViewInfo.rotateForcibly,
                    true
                    ));

            }

            else if (mgsState.stateEnum == MainGameSceneState.StateEnum.MainGameSceneFieldObjectEvolution)
            {

                if (
                    mgsState.currentFieldObjectEvolutionInfo.evolvableFieldObject &&
                    mgsState.currentFieldObjectEvolutionInfo.evolvableFieldObject.viewPointWhenEvolution.viewPoint
                    )
                {

                    StopAllCoroutines();

                    StartCoroutine(this.moveCamera(
                        mgsState.changeCameraViewInfo.currentTargetViewPoint,
                        mgsState.changeCameraViewInfo.wayPointList,
                        mgsState.currentFieldObjectEvolutionInfo.evolvableFieldObject.viewPointWhenEvolution.delay,
                        mgsState.changeCameraViewInfo.rotateForcibly,
                        false
                        ));

                }

            }

        }

        /// <summary>
        /// Move camera
        /// </summary>
        /// <param name="targetViewPoint">target ViewPoint</param>
        /// <param name="wayPointList">way point list</param>
        /// <param name="waitSeconds">Seconds to wait</param>
        /// <param name="rotateForcibly">Forcibly rotate camera</param>
        /// <param name="sendEscapeGameSignal">send EscapeGame signal</param>
        /// <returns>IEnumerator</returns>
        // -----------------------------------------------------------------------
        IEnumerator moveCamera(
            ViewPoint targetViewPoint,
            List<Transform> wayPointList,
            float waitSeconds,
            bool rotateForcibly,
            bool sendEscapeGameSignal
            )
        {

            if (!targetViewPoint)
            {
                yield break;
            }

            // ----------------

            float currentDistance = 0.0f;

            Vector3 position = Vector3.zero;

            Quaternion rotation = Quaternion.identity;

            this.m_simpleBezierPath.setValues(
                this.transform.position,
                targetViewPoint.worldPosition,
                Quaternion.Euler(targetViewPoint.worldRotation),
                wayPointList
                );

            // ----------------

            if (waitSeconds > 0.0f)
            {
                yield return new WaitForSeconds(waitSeconds);
            }

            // playSe
            {
                SoundManager.Instance.playSe(SoundManager.SeType.MoveCamera);
            }

            {

                BezierPoint lastPoint = this.m_simpleBezierPath.pointAtBezierCurveDetail(this.m_simpleBezierPath.totalMeterLength, true);

                Quaternion fromRotation = this.transform.rotation;
                Quaternion toRotation = Quaternion.Euler(targetViewPoint.worldRotation);

                // at least 0.5s
                float speed =
                    (this.m_changeViewMeterPerSecond < this.m_simpleBezierPath.totalMeterLength * 2f) ?
                    this.m_changeViewMeterPerSecond :
                    this.m_simpleBezierPath.totalMeterLength * 2f
                    ;
                
                // less than 1 second && more than 120 angle
                if (!rotateForcibly && speed >= this.m_simpleBezierPath.totalMeterLength && Quaternion.Angle(fromRotation, toRotation) > 120f)
                {
                    Vector3 temp = fromRotation.eulerAngles;
                    temp.x = 0.0f;
                    toRotation = Quaternion.Euler(temp);
                }

                while (currentDistance < this.m_simpleBezierPath.totalMeterLength)
                {

                    currentDistance += speed * Time.deltaTime;

                    position = this.m_simpleBezierPath.pointAtBezierCurve(currentDistance, false);
                    rotation = Quaternion.Slerp(fromRotation, toRotation, currentDistance / this.m_simpleBezierPath.totalMeterLength);

                    this.transform.SetPositionAndRotation(position, rotation);

                    yield return null;

                }

                // finish
                {
                    this.transform.SetPositionAndRotation(lastPoint.point, toRotation);
                }

            }

            yield return null;

            // EscapeGame
            {

                if (sendEscapeGameSignal)
                {

                    MainGameSceneState mgsState = CustomReduxManager.CustomReduxManagerInstance.MainGameSceneStateWatcher.state();

                    mgsState.setState(
                        MainGameSceneState.StateEnum.MainGameSceneMain
                        );

                }

            }

        }

        /// <summary>
        /// Back view
        /// </summary>
        // -----------------------------------------------------------------------
        public void onClickBackViewButton()
        {

            MainGameSceneState mgsState = CustomReduxManager.CustomReduxManagerInstance.MainGameSceneStateWatcher.state();

            if (mgsState.stateEnum != MainGameSceneState.StateEnum.MainGameSceneMain)
            {
                return;
            }

            // ----------------------

            if (
                mgsState.changeCameraViewInfo.currentTargetViewPoint &&
                mgsState.changeCameraViewInfo.currentTargetViewPoint.viewPointParent
                )
            {

                mgsState.setChangeCameraViewState(
                    mgsState.changeCameraViewInfo.currentTargetViewPoint.viewPointParent,
                    this.m_dummyTransformList,
                    false
                    );

            }

        }

        /// <summary>
        /// UserProgressDataSignal receiver
        /// </summary>
        /// <param name="updSignal">UserProgressDataSignal</param>
        // ------------------------------------------------------------------------------------------
        void onUserProgressDataSignal(UserProgressDataSignal updSignal)
        {

            this.m_userProgressData.localPosition = this.transform.localPosition;
            this.m_userProgressData.localRotation= this.transform.localRotation;

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

                    this.transform.localPosition = this.m_userProgressData.localPosition;
                    this.transform.localRotation = this.m_userProgressData.localRotation;

                }

            }

        }

        /// <summary>
        /// Is pointer over ui
        /// </summary>
        /// <returns>over</returns>
        // ------------------------------------------------------------------------------------------
        bool isPointerOverUIObject()
        {

            if(!EventSystem.current)
            {
                return false;
            }

            // ----------------

            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            this.m_tempListForPointerOverUIObject.Clear();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, this.m_tempListForPointerOverUIObject);
            return this.m_tempListForPointerOverUIObject.Count > 0;

        }

    }

}
