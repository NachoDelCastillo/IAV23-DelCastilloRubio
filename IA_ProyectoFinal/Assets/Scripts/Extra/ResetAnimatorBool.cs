using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NX
{
    public class ResetAnimatorBool : StateMachineBehaviour
    {
        public string isInteracting = "isInteracting";
        public bool isInteractingStatus = false;

        public string isRotatingWithRootMotion = "isRotatingWithRootMotion";
        public bool isRotatingWithRootMotionStatus = false;

        public string canRotate = "canRotate";
        public bool canRotateStatus = false;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.SetBool(isInteracting, isInteractingStatus);
            animator.SetBool(isRotatingWithRootMotion, isRotatingWithRootMotionStatus);
            animator.SetBool(canRotate, canRotateStatus);

            GolemAnimatorHandler animatorHandler = animator.transform.GetComponent<GolemAnimatorHandler>();
            if (animatorHandler != null)
            {
                animatorHandler.DisableTrailOnFoot();
                animatorHandler.DisableTrailOnPunch();
            }
        }
    }

}