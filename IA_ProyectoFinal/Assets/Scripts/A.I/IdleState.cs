using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace NX
{
    // Estado cuyo unico proposito es que el enemigo no se mueva,
    // Este estado se llama cuando el enemigo muere, junto a la animacion de muerte del mismo
    // Este estado tambien se llama cuando el jugador muere, donde el enemigo realiza en bucle
    // la animacion de victoria hasta que se reinicie la escena

    public class IdleState : State
    {
        public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorHandler enemyAnimatorHandler)
        {
            return this;
        }
    }
}
