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

        float rotationSpeed;

        // Velocidad a la que rota normalmente
        float normalRotationSpeed = 5;
        // Velocidad a la que rota mientras ataca
        float attackingRotationSpeed = 3;

        public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorHandler enemyAnimatorHandler)
        {
            float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);


            // Caminar alrededor del jugador

            if (enemyManager.isPerformingAction)
            {
                enemyAnimatorHandler.anim.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
            }
            else HandleRotateTowardsTarget(enemyManager);


            // Comprobar si se esta a rango de ataque
            if (enemyManager.currentRecoveryTime <= 0 && distanceFromTarget <= enemyManager.maximumAttackRange)
            {
                return attackState;
            }
            else if (distanceFromTarget > enemyManager.maximumAttackRange)
            {
                enemyManager.navMeshAgent.transform.rotation = enemyManager.transform.rotation;
                return pursueTargetState;
            }
            else
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
    }
}
