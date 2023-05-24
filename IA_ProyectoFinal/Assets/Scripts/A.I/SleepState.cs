using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NX
{
    // Estado en el que comienza el enemigo
    // En este estado, el enemigo se esta quieto y repite en loop la animacion asignada
    // con el parametro "sleepAnimation", cuando el jugador se acerca a menos de "detectionRadius"
    // de distancia, el estado cambia al estado "PursueTargetState" en el que se perseguira al jugador

    public class SleepState : State
    {
        public PursueTargetState pursueTargetState;

        public bool isSleeping;
        public float detectionRadius = 2;
        public string sleepAnimation;
        public string wakeAnimation;

        public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorHandler enemyAnimatorHandler)
        {
            // Si esta durmiendo, poner la animacion de dormir
            if (isSleeping && !enemyManager.isInteracting)
                enemyAnimatorHandler.PlayTargetAnimation(sleepAnimation, true);

            // Comprueba si el jugador esta a menos de la distancia parametrizada
            #region Detectar al jugador

            Collider[] colliders = Physics.OverlapSphere(enemyManager.transform.position, detectionRadius);

            for (int i = 0; i < colliders.Length; i++)
            {
                PlayerStats playerStats = colliders[i].transform.GetComponent<PlayerStats>();

                if (playerStats != null)
                {
                    Vector3 targetDirection = playerStats.transform.position - enemyManager.transform.position;

                    //enemyManager.currentTarget = playerStats;
                    isSleeping = false;
                    enemyAnimatorHandler.PlayTargetAnimation(wakeAnimation, true);

                    return pursueTargetState;
                }
            }

            #endregion

            // Si no se ha detectado al jugador, seguir en este estado
            return this;
        }
    }
}
