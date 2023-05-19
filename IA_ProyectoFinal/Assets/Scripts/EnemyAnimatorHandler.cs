using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NX
{
    public class EnemyAnimatorHandler : AnimatorManager
    {

        EnemyLocomotion enemyLocomotion;

        private void Awake()
        {
            anim = GetComponent<Animator>();
            enemyLocomotion= GetComponentInParent<EnemyLocomotion>();
        }

        private void OnAnimatorMove()
        {
            float delta = Time.deltaTime;
            enemyLocomotion.enemyRigidbody.drag = 0;
            Vector3 deltaPosition = anim.deltaPosition;
            deltaPosition.y = 0;
            Vector3 velocity = deltaPosition / delta;
            enemyLocomotion.enemyRigidbody.velocity = velocity;
        }
    }
}
