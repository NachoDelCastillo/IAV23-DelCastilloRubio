using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NX {
    public class AttackState : State
    {
        public CombatState combatStanceState;
        public RotateTowardsTargetState rotateTowardsTargetState;
        public PursueState pursueTargetState;
        public EnemyAttackAction currentAttack;


        bool willDoComboNextAttack = false;
        public bool hasPerformedAttack = false;

        public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorHandler enemyAnimatorHandler)
        {
            // Elegir un ataque

            // Volver al estado de combate
            float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);

            // Cambia al estado de persecucion si el jugador se aleja demasiado
            if (distanceFromTarget > enemyManager.maximumAggroRadius)
                return pursueTargetState;

            // Si todavia no se a realizado el ataque elegido en el estado de combate, ejecutarlo
            if (!hasPerformedAttack)
                AttackTarget(enemyAnimatorHandler, enemyManager);


            // Cuando se haya realizado el ataque, pasar al estado de rotacion hacia el jugador
            return rotateTowardsTargetState;
        }

        // Se encarga de ejecutar las animaciones y actualizar los valores de ataque
        private void AttackTarget(EnemyAnimatorHandler enemyAnimatorHandler, EnemyManager enemyManager)
        {
            enemyAnimatorHandler.PlayTargetAnimation(currentAttack.actionAnimation, true);
            // Activar el recovery timer para dejar al jugador una oportunidad de atacar despues del ataque
            enemyManager.currentRecoveryTime = currentAttack.recoveryTime;
            // Actualizar las variables
            hasPerformedAttack = true;
            currentAttack = null; //////
        }
    }
}
