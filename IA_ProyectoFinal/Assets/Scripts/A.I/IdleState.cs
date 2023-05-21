using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace NX
{
    public class IdleState : State
    {
        public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorHandler enemyAnimatorHandler)
        {
            // Buscar un target

            // Cambiar al estado de Perseguir

            return this;
        }
    }
}
