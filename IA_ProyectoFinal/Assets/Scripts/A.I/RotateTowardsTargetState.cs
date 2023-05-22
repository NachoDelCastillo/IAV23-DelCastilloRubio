using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NX
{
    public class RotateTowardsTargetState : State
    {
        public CombatStanceState combatStanceState;

        public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorHandler enemyAnimatorHandler)
        {
            enemyAnimatorHandler.anim.SetFloat("Vertical", 0);
            enemyAnimatorHandler.anim.SetFloat("Horizontal", 0);

            Vector3 targetDirection = enemyManager.currentTarget.transform.position - enemyManager.transform.position;

            float viewableAngle = Vector3.SignedAngle(targetDirection, enemyManager.transform.forward, Vector3.up);

            // Cuando entremos en este estado, el ultimo ataque seguira en marcha, por lo que tenemos
            // que esperar a que termine su animacion para poder empezar a girar
            if (enemyManager.isInteracting)
                return this;

            if (!enemyManager.isInteracting)
            {
                if (viewableAngle >= 100 && viewableAngle <= 180)
                {
                    enemyAnimatorHandler.PlayTargetAnimationWithRootRotation("BackTurn", true);
                    return combatStanceState;
                }
                else if (viewableAngle <= -101 && viewableAngle >= -180)
                {
                    enemyAnimatorHandler.PlayTargetAnimationWithRootRotation("BackTurn", true);
                    return combatStanceState;
                }
                else if (viewableAngle <= -45 && viewableAngle >= -100)
                {
                    enemyAnimatorHandler.PlayTargetAnimationWithRootRotation("RightTurn", true);
                    return combatStanceState;
                }
                else if (viewableAngle >= 45 && viewableAngle <= 100)
                {
                    enemyAnimatorHandler.PlayTargetAnimationWithRootRotation("LeftTurn", true);
                    return combatStanceState;
                }
            }

            return combatStanceState;
        }
    }
}