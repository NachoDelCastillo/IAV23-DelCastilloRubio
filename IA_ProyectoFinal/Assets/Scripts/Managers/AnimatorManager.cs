using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NX
{
    public class AnimatorManager : MonoBehaviour
    {
        public Animator anim;

        public void PlayTargetAnimation(string targetAnim, bool isInteracting, bool canRotate = false)
        {
            anim.applyRootMotion = isInteracting;
            anim.SetBool("isInteracting", isInteracting);
            //anim.SetBool("canRotate", isInteracting);
            anim.CrossFade(targetAnim, .2f);
        }

        public void PlayTargetAnimationWithRootRotation(string targetAnim, bool isInteracting)
        {
            anim.applyRootMotion = isInteracting;
            anim.SetBool("isInteracting", isInteracting);
            anim.SetBool("isRotatingWithRootMotion", isInteracting);
            //anim.SetBool("canRotate", isInteracting);
            anim.CrossFade(targetAnim, .2f);
        }
    }
}
