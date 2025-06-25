using Sunshot.SolarEnergySystem.UI;
using UnityEngine;
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

            if(Input.GetKey(KeyCode.W))
                isThrusterInUse = true;
            else
                isThrusterInUse = false;
                
        }

        void PlayerMovementControl()
        {
            ForwardMove();

            HorizontalMovement();
            
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

        void HorizontalMovement()
        {
            Quaternion rotation = Quaternion.identity;

            if (Input.GetKey(KeyCode.D))
            {
                rotation = Quaternion.Euler(0, 0, -30);
                MoveByDir(playerMesh, Vector3.right, horizontalMoveSpeed);
            }
            else if(Input.GetKey(KeyCode.A))
            {
                rotation = Quaternion.Euler(0, 0, 30);
                MoveByDir(playerMesh, Vector3.left, horizontalMoveSpeed);
            }

            ControlRotation(playerMesh, rotation);
        }

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
            //trasformToRotate.rotation = rotation;
        }

        void TouchInputs()
        {
            isThrusterInUse = false;
            isDragging = false;

            if (Input.touchCount <= 0) return;

            for(int i = 0; i < Input.touchCount; i++)
            {
                Touch touch = Input.GetTouch(i);
                Vector2 touchPos = touch.position;
                bool isLeftTouch = touchPos.x < Screen.width / 2;
                bool isRightTouch = touchPos.x > Screen.width / 2;

                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        if (isLeftTouch)
                            isDragging = true;

                        if(isRightTouch)
                            isThrusterInUse = true;

                        startTouchPos = touch.position;
                        break;

                    case TouchPhase.Moved:
                        if (isDragging == false) break;

                        Vector2 delta = touch.position - startTouchPos;
                        Quaternion rotation = Quaternion.identity;

                        if (delta.x > touchDragSensitivity)
                        {
                            MoveByDir(playerMesh, transform.right, horizontalMoveSpeed);
                            rotation = Quaternion.Euler(0, 0, -30);
                        }
                        else if (delta.x < -touchDragSensitivity)
                        {
                            MoveByDir(playerMesh, -transform.right, horizontalMoveSpeed);
                            rotation = Quaternion.Euler(0, 0, 30);
                        }
                        ControlRotation(playerMesh, rotation);

                        startTouchPos = touch.position;
                    
                        goto case TouchPhase.Stationary;

                    case TouchPhase.Stationary:
                        if(isRightTouch == true)
                            isThrusterInUse = true;
                        break;

                    case TouchPhase.Ended:
                    case TouchPhase.Canceled:
                        if(isLeftTouch == true)
                            isDragging = false;

                        if (isRightTouch == true)
                            isThrusterInUse = false;
                        break;
                }

            }
        }

        public bool GetIsThrusterOn()
        {
            return isThrusterInUse;
        }
    }

}
