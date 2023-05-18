using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace NX
{
    public class InputHandler : MonoBehaviour
    {
        PlayerManager playerManager;

        public float horizontal;
        public float vertical;
        public float moveAmount;
        public float mouseX;
        public float mouseY;

        public bool b_input;
        public bool s_input;
        public bool rb_input;
        public bool rt_input;

        public bool rollFlag;
        public bool sprintFlag;
        public float rollInputTimer;

        PlayerControls inputActions;
        PlayerAttacker playerAttacker;

        Vector2 movementInput;
        Vector2 cameraInput;

        private void Awake()
        {
            playerManager = GetComponent<PlayerManager>();
            playerAttacker = GetComponent<PlayerAttacker>();
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
            HandleAttackInput(delta);
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

        private void HandleAttackInput(float delta)
        {
            inputActions.PlayerActions.RB.performed += i => rb_input = true;
            inputActions.PlayerActions.RT.performed += i => rt_input = true;

            if (!playerManager.isInteracting)
            {
                if (rb_input)
                    playerAttacker.HandleLightAttack();
                else if (rt_input)
                    playerAttacker.HandleHeavyAttack();
            }
        }
    }
}