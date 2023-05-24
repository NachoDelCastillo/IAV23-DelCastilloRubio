using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NX
{
    public class EnemyAnimatorHandler : AnimatorManager
    {
        EnemyManager enemyManager;

        DamageCollider damageCollider;

        private void Awake()
        {
            anim = GetComponent<Animator>();
            enemyManager = GetComponentInParent<EnemyManager>();

            damageCollider = transform.parent.GetComponentInChildren<DamageCollider>();
        }

        private void OnAnimatorMove()
        {
            float delta = Time.deltaTime;
            enemyManager.enemyRigidbody.drag = 0;
            Vector3 deltaPosition = anim.deltaPosition;
            deltaPosition.y = 0;
            Vector3 velocity = deltaPosition / delta;
            enemyManager.enemyRigidbody.velocity = velocity;

            if (enemyManager.canRotate)
            {
                CombatState.HandleRotateTowardsTarget(enemyManager);
            }
            {
                if (enemyManager.isRotatingWithRootMotion)
                    enemyManager.transform.rotation *= anim.deltaRotation;
            }
        }


        public void EnableDamageCollider()
        {
            damageCollider.EnableDamageCollider();
        }

        public void DisableDamageCollider()
        {
            damageCollider.DisableDamageCollider();
        }


        public void EnableCanRotate()
        {
            anim.SetBool("canRotate", true);
        }

        public void DisableCanRotate()
        {
            anim.SetBool("canRotate", false);
        }
    }
}
