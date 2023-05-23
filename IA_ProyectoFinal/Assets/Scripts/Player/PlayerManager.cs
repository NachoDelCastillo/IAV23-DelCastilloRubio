using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace NX
{
    public class PlayerManager : CharacterManager
    {
        InputHandler inputHandler;
        AnimatorHandler animatorHandler;
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
            animatorHandler = GetComponentInChildren<AnimatorHandler>();
            inputHandler = GetComponent<InputHandler>();
            anim = GetComponentInChildren<Animator>();
            playerLocomotion = GetComponent<PlayerLocomotion>();
        }

        private void Update()
        {
            isInteracting = anim.GetBool("isInteracting");
            isInvulnerable = anim.GetBool("isInvulnerable");

            // Si no esta realizando ninguna accion
            if (!isInteracting)
            {
                // Activar el trail solo cuando este esprintando
                if (isSprinting)
                {
                    animatorHandler.EnableTrailOnFoot();
                    //animatorHandler.EnableTrailOnSword();
                }
                else
                {
                    animatorHandler.DisableTrailOnFoot();
                    //animatorHandler.DisableTrailOnSword();
                }
            }

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
