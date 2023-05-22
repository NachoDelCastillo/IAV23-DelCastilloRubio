using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace NX
{
    public class PursueTargetState : State
    {
        public CombatStanceState combatStanceState;

        public RotateTowardsTargetState rotateTowardsTargetState;

        public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorHandler enemyAnimatorHandler)
        {
            // Seguir al jugador

            // Si esta en rango de combate, cambiar a estado de combate

            // Si no esta en rango de combate, seguir persiguiendo

            Vector3 targetDirection = enemyManager.currentTarget.transform.position - enemyManager.transform.position;
            float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);
            float viewableAngle = Vector3.SignedAngle(targetDirection, enemyManager.transform.forward, Vector3.up);


            HandleRotateTowardsTarget(enemyManager);

            if (viewableAngle > 65 || viewableAngle < -65)
                return rotateTowardsTargetState;


            if (enemyManager.isPerformingAction)
            {
                enemyAnimatorHandler.anim.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);

                // Si se esta en medio de un ataque y se permite rotar en el ataque
                if (enemyManager.canRotate)
                {
                    Debug.Log("CANROTATE");
                    combatStanceState.HandleRotateTowardsTarget(enemyManager);
                }

                return this;
            }


            if (distanceFromTarget > enemyManager.maximumAggroRadius)
            {
                enemyAnimatorHandler.anim.SetFloat("Vertical", 1, 0.1f, Time.deltaTime);
            }

            enemyManager.navMeshAgent.transform.localPosition = Vector3.zero;
            enemyManager.navMeshAgent.transform.localRotation = Quaternion.identity;


            if (distanceFromTarget <= enemyManager.maximumAggroRadius)
            {
                return combatStanceState;
            }
            else
            {
                return this;
            }
        }

        public void HandleRotateTowardsTarget(EnemyManager enemyManager)
        {
            // Rotar manualmente
            if (enemyManager.isPerformingAction)
            {
                Vector3 direction = enemyManager.currentTarget.transform.position - enemyManager.transform.position;
                direction.y = 0;
                direction.Normalize();

                if (direction == Vector3.zero)
                    direction = enemyManager.transform.forward;

                Quaternion targetRotation = Quaternion.LookRotation(direction);
                enemyManager.transform.rotation = Quaternion.Slerp(enemyManager.transform.rotation, targetRotation, enemyManager.rotationSpeed / Time.deltaTime);
            }
            // Rotar usando pathfinding
            else
            {
                Vector3 relativeDirection = enemyManager.transform.InverseTransformDirection(enemyManager.navMeshAgent.desiredVelocity);
                Vector3 targetVelocity = enemyManager.enemyRigidbody.velocity;
                enemyManager.navMeshAgent.enabled = true;
                enemyManager.navMeshAgent.SetDestination(enemyManager.currentTarget.transform.position);
                enemyManager.enemyRigidbody.velocity = targetVelocity;
                enemyManager.transform.rotation = Quaternion.Slerp(enemyManager.transform.rotation, enemyManager.navMeshAgent.transform.rotation, enemyManager.rotationSpeed / Time.deltaTime);
            }
        }

        //public void HandleRotateTowardsTarget(EnemyManager enemyManager)
        //{
        //    // Rotar manualmente
        //    if (enemyManager.isPerformingAction)
        //    {
        //        Vector3 direction = enemyManager.currentTarget.transform.position - transform.position;
        //        direction.y = 0;
        //        direction.Normalize();

        //        if (direction == Vector3.zero)
        //            direction = transform.forward;

        //        Quaternion targetRotation = Quaternion.LookRotation(direction);
        //        enemyManager.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, enemyManager.rotationSpeed);
        //    }
        //    // Rotar usando pathfinding
        //    else
        //    {
        //        Vector3 relativeDirection = transform.InverseTransformDirection(enemyManager.navMeshAgent.desiredVelocity);
        //        Vector3 targetVelocity = enemyManager.enemyRigidbody.velocity;
        //        enemyManager.navMeshAgent.enabled = true;
        //        enemyManager.navMeshAgent.SetDestination(enemyManager.currentTarget.transform.position);
        //        enemyManager.enemyRigidbody.velocity = targetVelocity;
        //        enemyManager.transform.rotation = Quaternion.Slerp(enemyManager.transform.rotation, enemyManager.navMeshAgent.transform.rotation, enemyManager.rotationSpeed / Time.deltaTime);
        //    }
        //}
    }
}
