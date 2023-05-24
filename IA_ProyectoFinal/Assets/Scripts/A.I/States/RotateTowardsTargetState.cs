using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NX
{
    // Este estado sirve para rotar al jefe de forma suave usando animaciones predefinidas
    // Este metodo se llama justo despues de terminar un ataque, en el que el jefe normalmente
    // habra acabado en una rotacion en la que el jugador no esta delante suyo.

    // Cuando se entra en este estado, se calcula el angulo entre el frente del jefe y el jugador,
    // dependiendo del valor, se elegira una de las tres animaciones de girar: 90 grados hacia izquierda/derecha o 180 grados
    // Cuando se ha ejecutado la animacion correcta, se pasara al estado de combate

    public class RotateTowardsTargetState : State
    {
        public CombatState combatStanceState;

        public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorHandler enemyAnimatorHandler)
        {
            // Inmovilizar a este enemigo al entrar en este estado
            enemyAnimatorHandler.anim.SetFloat("Vertical", 0);
            enemyAnimatorHandler.anim.SetFloat("Horizontal", 0);

            Vector3 targetDirection = enemyManager.currentTarget.transform.position - enemyManager.transform.position;
            float viewableAngle = Vector3.SignedAngle(targetDirection, enemyManager.transform.forward, Vector3.up);

            // Cuando entremos en este estado, el ultimo ataque seguira en marcha, por lo que tenemos
            // que esperar a que termine su animacion para poder empezar a girar
            if (enemyManager.isInteracting)
                return this;

            // Elegir la animacion mas aproximada para estar mirando directamente al jugador
            if (!enemyManager.isInteracting)
            {
                if (viewableAngle >= 100 && viewableAngle <= 180)
                {
                    enemyAnimatorHandler.PlayTargetAnimationWithRootRotation("BackTurn", true);
                    return combatStanceState;
                }
                else if (viewableAngle <= -101 && viewableAngle >= -180)
                {
                    enemyAnimatorHandler.PlayTargetAnimationWithRootRotation("BackTurn", true);
                    return combatStanceState;
                }
                else if (viewableAngle <= -45 && viewableAngle >= -100)
                {
                    enemyAnimatorHandler.PlayTargetAnimationWithRootRotation("RightTurn", true);
                    return combatStanceState;
                }
                else if (viewableAngle >= 45 && viewableAngle <= 100)
                {
                    enemyAnimatorHandler.PlayTargetAnimationWithRootRotation("LeftTurn", true);
                    return combatStanceState;
                }
            }

            return combatStanceState;
        }
    }
}