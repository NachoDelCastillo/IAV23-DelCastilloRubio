using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace NX
{
    public class InputHandler : MonoBehaviour
    {
        public float horizontal;
        public float vertical;
        public float moveAmount;
        public float mouseX;
        public float mouseY;

        public bool b_input;
        public bool s_input;
        public bool rollFlag;
        public bool sprintFlag;
        public float rollInputTimer;
        public bool isInteracting;

        PlayerControls inputActions;
        CameraHandler cameraHandler;

        Vector2 movementInput;
        Vector2 cameraInput;

        private void Start()
        {
            cameraHandler = CameraHandler.singleton;
        }

        private void FixedUpdate()
        {
            float delta = Time.deltaTime;

            if (cameraHandler != null) {

                cameraHandler.FollowTarget(delta);
                cameraHandler.HandleCameraRotation(delta, mouseX, mouseY);
            }
        }

        public void OnEnable()
        {
            if (inputActions == null)
            {
                inputActions = new PlayerControls();
                inputActions.PlayerMovement.Movement.performed += inputActions => movementInput = inputActions.ReadValue<Vector2>();
                inputActions.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();
            }

            inputActions.Enable();
        }

        private void OnDisable()
        {
            inputActions.Disable();
        }

        public void TickInput(float delta)
        {
            MoveInput(delta);

            HandleRollInput(delta);
        }

        private void MoveInput(float delta)
        {
            horizontal = movementInput.x;
            vertical = movementInput.y;
            moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
            mouseX = cameraInput.x;
            mouseY = cameraInput.y;
        }


        private void HandleRollInput(float delta)
        {
            b_input = inputActions.PlayerActions.Roll.WasPressedThisFrame();
            s_input = inputActions.PlayerActions.Sprint.IsPressed();

            //inputActions.PlayerActions.Roll.started += ctx => b_input = true;
            //inputActions.PlayerActions.Roll.canceled += ctx => b_input = false;


            if (b_input)
            {
                rollFlag = true;
                sprintFlag = false;
            }

            else if (s_input)
            {
                //rollFlag = false;
                sprintFlag = true;
            }

            //if (b_input)
            //{
            //    rollInputTimer += delta;
            //    sprintFlag = true;
            //}
            //else
            //{
            //    if (rollInputTimer > 0 && rollInputTimer < .5f)
            //    {
            //        sprintFlag = false;
            //        rollFlag = true;
            //    }

            //    rollInputTimer= 0; 
            //}
        }
    }
}