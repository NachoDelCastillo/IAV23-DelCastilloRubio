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
        public PursueState pursueTargetState;

        // Devuelve true si todavia el jugador ni se ha acercado
        public bool isSleeping;
        // Rango necesario al que tiene que estar el jugador para que se cambie de estado
        public float detectionRadius = 2;
        // Nombre de la animacion de dormir en el animator del enemigo
        public string sleepAnimation;
        // Nombre de la animacion de despertarse en el animator del enemigo
        public string wakeAnimation;

        public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorHandler enemyAnimatorHandler)
        {
            // Si esta durmiendo, poner la animacion de dormir
            if (isSleeping && !enemyManager.isInteracting)
                enemyAnimatorHandler.PlayTargetAnimation(sleepAnimation, true);

            // Comprueba si el jugador esta a menos de la distancia parametrizada
            #region Detectar al jugador

            // Detecta a todos los colliders a menos del rango deseado
            Collider[] colliders = Physics.OverlapSphere(enemyManager.transform.position, detectionRadius);

            for (int i = 0; i < colliders.Length; i++)
            {
                PlayerStats playerStats = colliders[i].transform.GetComponent<PlayerStats>();

                // Comprueba si alguno de esos colliders pertenecen al jugador
                if (playerStats != null)
                {
                    // Direccion desde el enemigo hasta el jugador
                    Vector3 targetDirection = playerStats.transform.position - enemyManager.transform.position;

                    // Dejar de estar dormido
                    isSleeping = false;
                    // Activar la animacion de despertarse
                    enemyAnimatorHandler.PlayTargetAnimation(wakeAnimation, true);

                    // Cambiar al estado de perseguir
                    return pursueTargetState;
                }
            }

            #endregion

            // Si no se ha detectado al jugador, seguir en este estado
            return this;
        }
    }
}
