using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.UI;

namespace NX
{
    public class AnimatorHandler : MonoBehaviour
    {
        PlayerManager playerManager;
        public Animator anim;
        InputHandler inputHandler;
        PlayerLocomotion playerLocomotion;
        int horizontal;
        int vertical;
        public bool canRotate;

        DamageCollider damageCollider;

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
;           }

            Debug.Log("v = " + v);
            Debug.Log("h = " + h);
            Debug.Log("isSprinting = " + isSprinting);

            anim.SetFloat(vertical, v, 0.1f, Time.deltaTime);
            anim.SetFloat(horizontal, h, 0.1f, Time.deltaTime);
        }

        public void PlayTargetAnimation(string targetAnim, bool isInteracting)
        {
            anim.applyRootMotion = isInteracting;
            anim.SetBool("isInteracting", isInteracting);
            anim.CrossFade(targetAnim, .2f);
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
    }
}
