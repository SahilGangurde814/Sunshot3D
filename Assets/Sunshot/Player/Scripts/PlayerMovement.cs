using Sunshot.SolarEnergySystem.UI;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace Sunshot.PlayerSystem
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] Camera mainCamera;

        [Space]

        [SerializeField] Transform playerMesh;
        [SerializeField] float forwardMovementSpeed = 3f;
        [SerializeField] float horizontalMoveSpeed = 10f;
        [SerializeField] float thrusterSpeed = 2f;
        [SerializeField] float touchDragSensitivity = 5f;

        private bool isThrusterInUse = false;

        private Vector2 startTouchPos;
        private bool isDragging = false;

        Vector3 startPos;
        Vector3 topPos = new Vector3(0, 4.5f, 0);

        // Dependecies 
        HUDmanager hudManager;

        private void Start()
        {
            hudManager = HUDmanager.instance;

            startPos = playerMesh.position;
        }

        private void Update()
        {
            PlayerMovementControl();
            
            CameraFollow();

            //if(Input.GetKey(KeyCode.W))
            //    isThrusterInUse = true;
            //else
            //    isThrusterInUse = false;
                
        }

        void PlayerMovementControl()
        {
            ForwardMove();

            //HorizontalMovement();
            
            Thruster();

            TouchInputs();
        }

        void CameraFollow()
        {
            Vector3 camPos = mainCamera.transform.position;
            camPos.z = transform.position.z;
            mainCamera.transform.position = camPos + new Vector3(0, 0, -4.5f);
        }

        void ForwardMove()
        {
            MoveByDir(transform, transform.forward, forwardMovementSpeed);
        }

        #region For Keyboard (old input system)
        //void HorizontalMovement()
        //{
        //    Quaternion rotation = Quaternion.identity;

        //    if (Input.GetKey(KeyCode.D))
        //    {
        //        rotation = Quaternion.Euler(0, 0, -30);
        //        MoveByDir(playerMesh, Vector3.right, horizontalMoveSpeed);
        //    }
        //    else if (Input.GetKey(KeyCode.A))
        //    {
        //        rotation = Quaternion.Euler(0, 0, 30);
        //        MoveByDir(playerMesh, Vector3.left, horizontalMoveSpeed);
        //    }

        //    ControlRotation(playerMesh, rotation);
        //}
        #endregion

        void Thruster()
        {
            if(isThrusterInUse == true && hudManager.CanUseThruster())
            {
                float distance = playerMesh.position.y - topPos.y;
                if (distance > 0.01f) return;

                playerMesh.Translate(playerMesh.up * thrusterSpeed * Time.deltaTime);
            }
            else
            {
                float distance = playerMesh.position.y - startPos.y;
                if(distance < 0.01f) return;

                playerMesh.Translate(-playerMesh.up * thrusterSpeed * Time.deltaTime);
            }
        }

        void MoveByDir(Transform trasformToMove, Vector3 dir, float speed)
        {
            trasformToMove.position += dir * speed * Time.deltaTime;
        }

        void ControlRotation(Transform trasformToRotate, Quaternion rotation)
        {
            trasformToRotate.rotation = Quaternion.Slerp(trasformToRotate.rotation, rotation, 5 * Time.deltaTime);
        }

        void TouchInputs()
        {
            Quaternion targetRoatation = Quaternion.identity;   
            if (Touchscreen.current == null || Touchscreen.current.touches.Count == 0)
                return;

            foreach (var touchControl in Touchscreen.current.touches)
            {
                if (!touchControl.press.isPressed && !touchControl.press.wasPressedThisFrame && !touchControl.press.wasReleasedThisFrame)
                    continue;

                var touchPos = touchControl.position.ReadValue();
                bool isLeftTouch = touchPos.x < Screen.width / 2;
                bool isRightTouch = touchPos.x > Screen.width / 2;

                if (touchControl.press.wasPressedThisFrame)
                {
                    if (isLeftTouch) isDragging = true;
                    if (isRightTouch) isThrusterInUse = true;
                    startTouchPos = touchPos;
                }
                else if (touchControl.press.isPressed && touchControl.delta.ReadValue() != Vector2.zero)
                {
                    if (isDragging)
                    {
                        Vector2 delta = touchPos - startTouchPos;

                        if (delta.x > touchDragSensitivity)
                        {
                            MoveByDir(playerMesh, transform.right, horizontalMoveSpeed);
                            targetRoatation = Quaternion.Euler(0, 0, -30);
                        }
                        else if (delta.x < -touchDragSensitivity)
                        {
                            MoveByDir(playerMesh, -transform.right, horizontalMoveSpeed);
                            targetRoatation = Quaternion.Euler(0, 0, 30);
                        }

                        startTouchPos = touchPos;
                    }

                    if (isRightTouch)
                        isThrusterInUse = true;
                }
                else if (touchControl.press.wasReleasedThisFrame)
                {
                    isDragging = false;
                    isThrusterInUse = false;
                }
            }

            ControlRotation(playerMesh, targetRoatation);
        }

        public bool GetIsThrusterOn()
        {
            return isThrusterInUse;
        }
    }

}
