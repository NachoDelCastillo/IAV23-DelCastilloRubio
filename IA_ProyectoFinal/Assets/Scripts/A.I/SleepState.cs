using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NX
{
    public class SleepState : State
    {
        public PursueTargetState pursueTargetState;

        public bool isSleeping;
        public float detectionRadius = 2;
        public string sleepAnimation;
        public string wakeAnimation;


        public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorHandler enemyAnimatorHandler)
        {
            if (isSleeping && !enemyManager.isInteracting)
            {
                enemyAnimatorHandler.PlayTargetAnimation(sleepAnimation, true);
            }

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


            #region Cambiar de estado

            //if (enemyManager.currentTarget != null)
            //    return pursueTargetState;
            //else
            //    return this;

            return this;

            #endregion
        }
    }
}
