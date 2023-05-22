using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

namespace NX {
    public class AttackState : State
    {
        public CombatStanceState combatStanceState;
        public RotateTowardsTargetState rotateTowardsTargetState;
        public PursueTargetState pursueTargetState;
        public EnemyAttackAction currentAttack;


        bool willDoComboNextAttack = false;
        public bool hasPerformedAttack = false;

        public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorHandler enemyAnimatorHandler)
        {
            // Elegir un ataque

            // Si el ataque seleccionado no puede usarse (angulo no apropiado o distancia)
            // Si es valido, parar el movimiento y atacar el target (jugador)

            // Activar el recovery timer para dejar al jugador una oportunidad de atacar despues del ataque

            // Volver al estado de combate

            float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);

            RotateTowardsTargetWhileAttacking(enemyManager);

            if (distanceFromTarget > enemyManager.maximumAggroRadius)
            {
                return pursueTargetState;
            }

            // Sistema de combos

            //if (willDoComboNextAttack && enemyManager.canDoCombo)
            //{
            //      AttackTargetWithCombo(enemyAnimatorHandler, enemyManager);
            //}

            if (!hasPerformedAttack)
            {
                AttackTarget(enemyAnimatorHandler, enemyManager);
                //RollForComboChance();
            }

            //if (willDoComboNextAttack && hasPerformedAttack)
            //{
            //    return this;
            //}


            return rotateTowardsTargetState;
        }

        private void AttackTarget(EnemyAnimatorHandler enemyAnimatorHandler, EnemyManager enemyManager)
        {
            enemyAnimatorHandler.PlayTargetAnimation(currentAttack.actionAnimation, true);
            enemyManager.currentRecoveryTime = currentAttack.recoveryTime;
            hasPerformedAttack = true;
        }

        private void AttackTargetWithCombo(EnemyAnimatorHandler enemyAnimatorHandler, EnemyManager enemyManager)
        {
            willDoComboNextAttack = false;
            enemyAnimatorHandler.PlayTargetAnimation(currentAttack.actionAnimation, true);
            enemyManager.currentRecoveryTime = currentAttack.recoveryTime;
            currentAttack = null;
        }

        private void RollForComboChance(EnemyManager enemyManager)
        {
            //float comboChance = Random.Range(0, 100);

            //if (enemyManager.allowAIToPerformCombos && comboChance <= enemyManager.comboLikelyHood)
            //{
            //    if (currentAttack.comboAction != null)
            //    {
            //        willDoComboNextAttack = true;
            //        currentAttack = currentAttack.comboAction;
            //    }
            //    else
            //    {
            //        willDoComboOnNextAttack = false;
            //        currentAttack = null;
            //    }
            //}
        }

        public void RotateTowardsTargetWhileAttacking(EnemyManager enemyManager)
        {
            if (enemyManager.canRotate && enemyManager.isInteracting)
            {
                Vector3 direction = enemyManager.currentTarget.transform.position - enemyManager.transform.position;
                direction.y = 0;
                direction.Normalize();

                if (direction == Vector3.zero)
                    direction = enemyManager.transform.forward;

                Quaternion targetRotation = Quaternion.LookRotation(direction);
                //enemyManager.transform.rotation = Quaternion.Slerp(enemyManager.transform.rotation, targetRotation, (enemyManager.rotationSpeed * .005f) / Time.deltaTime);
                enemyManager.transform.rotation = Quaternion.Slerp
                    (enemyManager.transform.rotation, targetRotation, 5 * Time.deltaTime);
            }
        }

    }
}
