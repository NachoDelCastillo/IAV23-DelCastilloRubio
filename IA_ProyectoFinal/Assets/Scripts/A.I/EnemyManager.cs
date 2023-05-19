using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NX
{
    public class EnemyManager : CharacterManager
    {
        EnemyLocomotion enemyLocomotion;
        EnemyAnimatorHandler enemyAnimatorHandler;

        public bool isPerformingAction;

        public EnemyAttackAction[] enemyAttacks;
        public EnemyAttackAction currentAttack;

        public float currentRecoveryTime = 0;

        private void Awake()
        {
            enemyLocomotion = GetComponent<EnemyLocomotion>();
            enemyAnimatorHandler = GetComponentInChildren<EnemyAnimatorHandler>();
        }

        private void Update()
        {
            HandleRecoveryTimer();
        }

        private void FixedUpdate()
        {
            HandleCurrentAction();
        }

        private void HandleCurrentAction()
        {
            if (enemyLocomotion.currentTarget != null)
            {
                enemyLocomotion.distanceFromTarget = Vector3.Distance
                    (enemyLocomotion.currentTarget.transform.position, transform.position);
            }

            if (enemyLocomotion.currentTarget == null)
            {
                // Buscar al jugador
            }
            else if (enemyLocomotion.distanceFromTarget > enemyLocomotion.stoppingDistance)
            {
                enemyLocomotion.HandleMoveToTarget();
            }
            else if (enemyLocomotion.distanceFromTarget <= enemyLocomotion.stoppingDistance)
            {
                AttackTarget();
            }
        }

        private void HandleRecoveryTimer()
        {
            if (currentRecoveryTime > 0)
                currentRecoveryTime -= Time.deltaTime;

            if (isPerformingAction)
                if (currentRecoveryTime <= 0)
                    isPerformingAction = false;
        }

        #region Attacks

        private void AttackTarget()
        {
            if (isPerformingAction)
                return;

            if (currentAttack == null)
            {
                GetNewAttack();
            }
            else
            {
                isPerformingAction = true;
                currentRecoveryTime = currentAttack.recoveryTime;
                enemyAnimatorHandler.PlayTargetAnimation(currentAttack.actionAnimation, true);
                currentAttack = null;
            }
        }

        private void GetNewAttack()
        {
            Vector3 targetsDirection = enemyLocomotion.currentTarget.transform.position - transform.position;
            float viewableAngle = Vector3.Angle(targetsDirection, transform.forward);
            enemyLocomotion.distanceFromTarget = Vector3.Distance
                (enemyLocomotion.currentTarget.transform.position, transform.position);

            int maxScore = 0;

            for (int i = 0; i < enemyAttacks.Length; i++)
            {
                EnemyAttackAction enemyAttackAction = enemyAttacks[i];

                if (enemyLocomotion.distanceFromTarget <= enemyAttackAction.maximumDistanceNeededToAttack
                    && enemyLocomotion.distanceFromTarget >= enemyAttackAction.minimumDistanceNeededToAttack)
                {
                    if (viewableAngle <= enemyAttackAction.maximunAttackAngle
                        && viewableAngle >= enemyAttackAction.minimumAttackAngle)
                    {
                        maxScore += enemyAttackAction.attackScore;
                    }
                }
            }

            int randomValue = Random.Range(0, maxScore);
            int temporaryScore = 0;

            for (int i = 0; i < enemyAttacks.Length; i++)
            {
                EnemyAttackAction enemyAttackAction = enemyAttacks[i];

                if (enemyLocomotion.distanceFromTarget <= enemyAttackAction.maximumDistanceNeededToAttack
                    && enemyLocomotion.distanceFromTarget >= enemyAttackAction.minimumDistanceNeededToAttack)
                {
                    if (viewableAngle <= enemyAttackAction.maximunAttackAngle
                        && viewableAngle >= enemyAttackAction.minimumAttackAngle)
                    {
                        if (currentAttack != null)
                            return;

                        temporaryScore += enemyAttackAction.attackScore;

                        if (temporaryScore > randomValue)
                        {
                            currentAttack = enemyAttackAction;
                        }
                    }
                }
            }
        }

        #endregion
    }
}
