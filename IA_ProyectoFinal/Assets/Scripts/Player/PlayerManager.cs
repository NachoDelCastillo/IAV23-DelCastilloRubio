using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace NX
{
    public class PlayerManager : CharacterManager
    {
        InputHandler inputHandler;
        Animator anim;
        CameraHandler cameraHandler;
        PlayerLocomotion playerLocomotion;

        public bool isInteracting;

        [Header("Player Flags")]
        public bool isSprinting;
        public bool isInvulnerable;

        private void Awake()
        {
            cameraHandler = FindObjectOfType<CameraHandler>();
        }

        private void Start()
        {
            inputHandler = GetComponent<InputHandler>();
            anim = GetComponentInChildren<Animator>();
            playerLocomotion = GetComponent<PlayerLocomotion>();
        } 

        private void Update()
        {
            isInteracting = anim.GetBool("isInteracting");
            isInvulnerable = anim.GetBool("isInvulnerable");

            float delta = Time.deltaTime;
            inputHandler.TickInput(delta);
            playerLocomotion.HandleMovement(delta);
            playerLocomotion.HandleRollAndSprint(delta);
        }


        private void FixedUpdate()
        {
            float delta = Time.deltaTime;

            if (cameraHandler != null)
            {
                cameraHandler.FollowTarget(delta);
                cameraHandler.HandleCameraRotation(delta, inputHandler.mouseX, inputHandler.mouseY);
            }
        }


        private void LateUpdate()
        {
            inputHandler.rollFlag = false;
            inputHandler.sprintFlag = false;

            inputHandler.rb_input = false;
            inputHandler.rt_input = false;
        }
    }
}
