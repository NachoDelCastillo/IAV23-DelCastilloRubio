using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace NX
{
    // En este estado, la IA hace uso del NavMesh para acercarse al jugador
    // Una vez que el enemigo se ha acercado lo suficiente al jugador teniendo en cuenta 
    // el parametro "enemyManager.maximumAggroRadius", pasa al estado de combate


    public class PursueState : State
    {
        public CombatState combatStanceState;
        public RotateTowardsTargetState rotateTowardsTargetState;

        public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorHandler enemyAnimatorHandler)
        {
            Vector3 targetDirection = enemyManager.currentTarget.transform.position - enemyManager.transform.position;
            float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);
            float viewableAngle = Vector3.SignedAngle(targetDirection, enemyManager.transform.forward, Vector3.up);

            // Se encarga de mover al enemigo hacia el jugador, ya sea usando Navmesh o rotacion manual usando matematicas
            HandleRotateTowardsTarget(enemyManager);

            // Si no esta mirando al jugador, pasar al estado de Rotate para tenerle de frente
            if (viewableAngle > 65 || viewableAngle < -65)
                return rotateTowardsTargetState;


            // Si se esta realizando una accion en mitad del proceso de persecucion
            if (enemyManager.isPerformingAction)
            {
                // No dejar moverse
                enemyAnimatorHandler.anim.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);

                // Si se esta en medio de un ataque y se permite rotar en el ataque
                if (enemyManager.canRotate)
                    CombatState.HandleRotateTowardsTarget(enemyManager);

                // Devolver el mismo estado, para que en la siguiente iteracion, vuelva a ejecutar este estado
                return this;
            }

            // Si todavia no se esta lo suficientemente cerca como para entrar en el estado de combate
            // Hacer que el modelo del personaje camine directamente hacia adelante
            if (distanceFromTarget > enemyManager.maximumAggroRadius)
                enemyAnimatorHandler.anim.SetFloat("Vertical", 1, 0.1f, Time.deltaTime);

            // Mantener el objeto que contiene al navmesh en el punto de origen en todo momento
            enemyManager.navMeshAgent.transform.localPosition = Vector3.zero;
            enemyManager.navMeshAgent.transform.localRotation = Quaternion.identity;


            // Si esta en rango de combate, cambiar a estado de combate
            if (distanceFromTarget <= enemyManager.maximumAggroRadius)
                return combatStanceState;
            // Si no esta en rango de combate, seguir persiguiendo
            else
                return this;
        }

        // Rotar usando navmesh, o manualmente
        public void HandleRotateTowardsTarget(EnemyManager enemyManager)
        {
            // Seguir al jugador usando navmesh
            Vector3 relativeDirection = enemyManager.transform.InverseTransformDirection(enemyManager.navMeshAgent.desiredVelocity);
            Vector3 targetVelocity = enemyManager.enemyRigidbody.velocity;
            enemyManager.navMeshAgent.enabled = true;
            enemyManager.navMeshAgent.SetDestination(enemyManager.currentTarget.transform.position);
            enemyManager.enemyRigidbody.velocity = targetVelocity;
            enemyManager.transform.rotation = Quaternion.Slerp(enemyManager.transform.rotation, enemyManager.navMeshAgent.transform.rotation, enemyManager.rotationSpeed / Time.deltaTime);
        }
    }
}
