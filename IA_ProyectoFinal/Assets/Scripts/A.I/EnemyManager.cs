using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NX
{
    public class EnemyManager : CharacterManager
    {
        EnemyLocomotion enemyLocomotion;

        public bool isPerformingAction;

        private void Awake()
        {
            enemyLocomotion = GetComponent<EnemyLocomotion>();
        }


        private void FixedUpdate()
        {
            HandleCurrentAction();
        }

        private void HandleCurrentAction()
        {
            if (enemyLocomotion.currentTarget != null)
            {
                enemyLocomotion.HandleMoveToTarget();
            }
            else if (enemyLocomotion.distanceFromTarget > enemyLocomotion.stoppingDistance)
            {
                enemyLocomotion.HandleMoveToTarget();
            }
            else if (enemyLocomotion.distanceFromTarget <= enemyLocomotion.stoppingDistance)
            {
                // Handle Attacks
            }
        }

        #region Attacks

        #endregion
    }
}
