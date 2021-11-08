using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ciitt.EscapeGameKit
{

    [RequireComponent(typeof(Camera))]
    public class ItemCameraScript : MonoBehaviour
    {
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
        /// Reference to Camera
        /// </summary>
        Camera m_refItemCamera = null;

        /// <summary>
        /// Start
        /// </summary>
        // -----------------------------------------------------------------------
        void Start()
        {

            // m_refItemCamera
            {
                this.m_refItemCamera = this.GetComponent<Camera>();
            }

        }

        /// <summary>
        /// Update
        /// </summary>
        // -----------------------------------------------------------------------
        void Update()
        {

            // zoomCamera
            {
                this.zoomCamera();
            }

        }

        /// <summary>
        /// Zoom camera
        /// </summary>
        // -----------------------------------------------------------------------
        void zoomCamera()
        {

            if (!this.m_refItemCamera)
            {
                return;
            }

            MainGameSceneState mgsState = CustomReduxManager.CustomReduxManagerInstance.MainGameSceneStateWatcher.state();

            if (mgsState.stateEnum != MainGameSceneState.StateEnum.MainGameSceneItemShowroom)
            {
                return;
            }

            // ------------------------

            if (Input.GetAxis("Mouse ScrollWheel") > 0.0f)
            {
                this.m_refItemCamera.fieldOfView = Mathf.Max(this.m_minZoomFov, this.m_refItemCamera.fieldOfView - 2.0f);
            }

            else if (Input.GetAxis("Mouse ScrollWheel") < 0.0f)
            {
                this.m_refItemCamera.fieldOfView = Mathf.Min(this.m_maxZoomFov, this.m_refItemCamera.fieldOfView + 2.0f);
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

                this.m_refItemCamera.fieldOfView =
                    Mathf.Clamp(
                        this.m_refItemCamera.fieldOfView + (deltaMagnitudeDiff * 0.1f),
                        this.m_minZoomFov,
                        this.m_maxZoomFov
                        );

            }

        }

    }

}
