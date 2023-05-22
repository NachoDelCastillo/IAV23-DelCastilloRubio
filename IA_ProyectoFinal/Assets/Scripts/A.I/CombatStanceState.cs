using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

namespace NX
{
    public class CombatStanceState : State
    {
        public AttackState attackState;
        public PursueTargetState pursueTargetState;

        public EnemyAttackAction[] enemyAttacks;

        float rotationSpeed;

        // Velocidad a la que rota normalmente
        float normalRotationSpeed = 5;
        // Velocidad a la que rota mientras ataca
        float attackingRotationSpeed = 3;

        bool randomDestinationSet = false;
        float verticalMovementValue = 0;
        float horizontalMovementValue = 0;

        public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorHandler enemyAnimatorHandler)
        {
            float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);

            enemyAnimatorHandler.anim.SetFloat("Vertical", verticalMovementValue, 0.2f, Time.deltaTime);
            enemyAnimatorHandler.anim.SetFloat("Horizontal", horizontalMovementValue, 0.2f, Time.deltaTime);

            attackState.hasPerformedAttack = false;

            if (enemyManager.isInteracting)
            {
                enemyAnimatorHandler.anim.SetFloat("Vertical", 0);
                enemyAnimatorHandler.anim.SetFloat("Horizontal", 0);
                return this;
            }
            else if (distanceFromTarget > enemyManager.maximumAggroRadius)
            {
                //enemyManager.navMeshAgent.transform.rotation = enemyManager.transform.rotation;
                return pursueTargetState;
            }


            // Caminar alrededor del jugador
            if (!randomDestinationSet)
            {
                randomDestinationSet = true;
                DecideCirclingAction(enemyAnimatorHandler);
                // Decicir el destino aleatoriamente
            }


            HandleRotateTowardsTarget(enemyManager);


            // Comprobar si se esta a rango de ataque
            if (enemyManager.currentRecoveryTime <= 0 && attackState.currentAttack != null)
            {
                randomDestinationSet = false;
                return attackState;
            }
            else
            {
                GetNewAttack(enemyManager);
            }

            return this;


            // Si esta en rango de ataque, cambiar a estado de ataque

            // Si se esta en un cool down despues de un ataque, seguir en este modo

            // Si el jugador se va fuera del rango de combate, cambiar al estado de persecucion
        }

        public void HandleRotateTowardsTarget(EnemyManager enemyManager)
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


        private void DecideCirclingAction(EnemyAnimatorHandler enemyAnimatorHandler)
        {
            WalkAroundTarget(enemyAnimatorHandler);
        }

        private void WalkAroundTarget(EnemyAnimatorHandler enemyAnimatorHandler)
        {
            verticalMovementValue = 0.1f;

            horizontalMovementValue = Random.Range(-1, 1f);

            if (horizontalMovementValue <= 1 && horizontalMovementValue >= 0)
            {
                horizontalMovementValue = .5f;
            }
            else if (horizontalMovementValue >= -1 && horizontalMovementValue < 0)
            {
                horizontalMovementValue = -.5f;
            }
        }

        private void GetNewAttack(EnemyManager enemyManager)
        {
            Vector3 targetsDirection = enemyManager.currentTarget.transform.position - transform.position;
            float viewableAngle = Vector3.Angle(targetsDirection, transform.forward);
            float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);

            int maxScore = 0;

            for (int i = 0; i < enemyAttacks.Length; i++)
            {
                EnemyAttackAction enemyAttackAction = enemyAttacks[i];

                if (distanceFromTarget <= enemyAttackAction.maximumDistanceNeededToAttack
                    && distanceFromTarget >= enemyAttackAction.minimumDistanceNeededToAttack)
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

                if (distanceFromTarget <= enemyAttackAction.maximumDistanceNeededToAttack
                    && distanceFromTarget >= enemyAttackAction.minimumDistanceNeededToAttack)
                {
                    if (viewableAngle <= enemyAttackAction.maximunAttackAngle
                        && viewableAngle >= enemyAttackAction.minimumAttackAngle)
                    {
                        if (attackState.currentAttack != null)
                            return;

                        temporaryScore += enemyAttackAction.attackScore;

                        if (temporaryScore > randomValue)
                        {
                            attackState.currentAttack = enemyAttackAction;
                        }
                    }
                }
            }
        }
    }
}
