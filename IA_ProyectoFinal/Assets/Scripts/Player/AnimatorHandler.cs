using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.UI;

namespace NX
{
    public class AnimatorHandler : AnimatorManager
    {
        PlayerManager playerManager;
        InputHandler inputHandler;
        PlayerLocomotion playerLocomotion;
        int horizontal;
        int vertical;
        public bool canRotate;

        DamageCollider damageCollider;

        [SerializeField]
        TrailRenderer[] swordTr;

        [SerializeField]
        TrailRenderer[] footsTr;

        private void Awake()
        {
            foreach (TrailRenderer trail in swordTr)
                trail.emitting = false;

            foreach (TrailRenderer trail in footsTr)
                trail.emitting = false;

            initialSize = swordTr[0].startWidth;
        }

        public void Initialize()
        {
            playerManager = GetComponentInParent<PlayerManager>();
            anim = GetComponent<Animator>();
            inputHandler = GetComponentInParent<InputHandler>();
            playerLocomotion = GetComponentInParent<PlayerLocomotion>();
            vertical = Animator.StringToHash("Vertical");
            horizontal = Animator.StringToHash("Horizontal");

            damageCollider = transform.parent.GetComponentInChildren<DamageCollider>();
        }

        public void UpdateAnimatorValues(float verticalMovement, float horizontalMovement, bool isSprinting)
        {
            float v = verticalMovement;
            float h = horizontalMovement;

            //#region Vertical
            //if (verticalMovement > 0 && verticalMovement < 0.55f)
            //    v = .5f;

            //else if (verticalMovement > .55f)
            //    v = 1;

            //else if (verticalMovement < 0 && verticalMovement > -0.55f)
            //    v = -.5f;

            //else if (verticalMovement < -.55f)
            //    v = -1;
            //else
            //    v = 0;

            //#endregion

            //#region Horizontal
            //if (horizontalMovement > 0 && horizontalMovement < 0.55f)
            //    h = .5f;

            //else if (horizontalMovement > .55f)
            //    h = 1;

            //else if (horizontalMovement < 0 && horizontalMovement > -0.55f)
            //    h = -.5f;

            //else if (horizontalMovement < -.55f)
            //    h = -1;
            //else
            //    h = 0;

            //#endregion

            if (isSprinting)
            {
                v = 2;
                h = horizontalMovement;
                ;
            }

            anim.SetFloat(vertical, v, 0.1f, Time.deltaTime);
            anim.SetFloat(horizontal, h, 0.1f, Time.deltaTime);
        }

        public void CanRotate()
        {
            canRotate = true;
        }

        public void StopRotation()
        {
            canRotate = false;
        }

        private void OnAnimatorMove()
        {
            if (playerManager.isInteracting == false)
                return;

            float delta = Time.deltaTime;
            playerLocomotion.rigidbody.drag = 0;
            Vector3 deltaPosition = anim.deltaPosition;
            deltaPosition.y = 0;
            Vector3 velocity = deltaPosition / delta;
            playerLocomotion.rigidbody.velocity = velocity;

        }



        public void EnableDamageCollider()
        {
            damageCollider.EnableDamageCollider();
        }

        public void DisableDamageCollider()
        {
            damageCollider.DisableDamageCollider();
        }

        public void EnableIsInvurenable()
        {
            anim.SetBool("isInvulnerable", true);
        }

        public void DisableIsInvurenable()
        {
            anim.SetBool("isInvulnerable", false);
        }


        #region VFX

        float initialSize;
        public void EnableTrailOnSword(float size = 0)
        {
            foreach (TrailRenderer trail in swordTr)
            {
                trail.emitting = true;
                if (size > 0)
                    trail.startWidth = size;
                else
                    trail.startWidth = initialSize;
            }
        }

        public void DisableTrailOnSword()
        {
            foreach (TrailRenderer trail in swordTr)
                trail.emitting = false;
        }


        public void EnableTrailOnFoot()
        {
            foreach (TrailRenderer trail in footsTr)
                trail.emitting = true;
        }

        public void DisableTrailOnFoot()
        {
            foreach (TrailRenderer trail in footsTr)
                trail.emitting = false;
        }

        #endregion
    }
}
