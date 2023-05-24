using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

namespace NX
{
    // El estado mas complicado de todos

    // Comprueba si el jugador ha muerto, en dicho caso, asigna la animacion correspondiente y pasa al estado Idle
    // Se encarga de tener al jugador de frente constantemente, calculando y modificando su siguiente rotacion

    // ELECCION DE ATAQUES
    // Tambien se encarga de calcular que ataque deberia ejecutarse en cada momento
    // Cada ataque tiene una variable que determina la probabilidad de que sea elegido 
    // sobre el resto (AttackScore), se suman todos los numeros y se elige uno aleatorio
    // Esta programado de forma que no se pueda eleguir el mismo ataque dos veces seguidas

    // Cada ataque tiene su propio tiempo de recovery, que es el numero de segundos que tiene que pasar antes de 
    // realizar otro ataque

    // FORMA EN LA QUE ACERCARSE AL JUGADOR
    // En este estado, el enemigo decide de que forma acercarse al jugador dependiendo
    // de la variable "combatWalkingTypes", facilmente modificable



    public class CombatState : State
    {
        public IdleState idleState;
        public AttackState attackState;
        public PursueState pursueTargetState;

        public EnemyAttackAction[] enemyAttacks;

        float rotationSpeed;

        // Velocidad a la que rota normalmente
        float normalRotationSpeed = 5;
        // Velocidad a la que rota mientras ataca
        float attackingRotationSpeed = 3;

        // Caminar de una de las formas designadas en el modo combate
        bool randomDestinationSet = false;
        float verticalMovementValue = 0;
        float horizontalMovementValue = 0;

        float randomMovementTimer = 0;
        float maxMovementTime = 6;
        float minMovementTime = 3;

        string lastAttackName = "";

        public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorHandler enemyAnimatorHandler)
        {
            // Comprobar si el jugador ha muerto, en dicho caso, asigna la animacion correspondiente y pasa al estado Idle
            if (enemyManager.currentTarget.GetComponent<PlayerStats>().isDead)
            {
                enemyAnimatorHandler.PlayTargetAnimation("EnemyVictory", true);
                return idleState;
            }

            // Distancia hasta el jugador
            float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);

            // Mover el enemigo mediante los parametros de las animaciones
            enemyAnimatorHandler.anim.SetFloat("Vertical", verticalMovementValue, 0.2f, Time.deltaTime);
            enemyAnimatorHandler.anim.SetFloat("Horizontal", horizontalMovementValue, 0.2f, Time.deltaTime);

            // Si se esta de vuelta en este estado, significa que el ataque que se estaba ejecutando ha terminado
            attackState.hasPerformedAttack = false;

            // Si se esta realizando alguna accion, no permitir moverse
            if (enemyManager.isInteracting)
            {
                enemyAnimatorHandler.anim.SetFloat("Vertical", 0);
                enemyAnimatorHandler.anim.SetFloat("Horizontal", 0);
                return this;
            }

            // Si el jugador se va fuera del rango de combate, cambiar al estado de persecucion
            else if (distanceFromTarget > enemyManager.maximumAggroRadius)
                return pursueTargetState;


            // Acercarse al enemigo permutando entre diferentes formas de caminar
            if (!randomDestinationSet)
            {
                randomDestinationSet = true;
                // Decicir la forma de acercarse al enemigo aleatoriamente
                DecideCirclingAction(enemyAnimatorHandler);

                // Decidir dentro de cuanto tiempo se cambiara este estilo de andar
                randomMovementTimer = Random.Range(minMovementTime, maxMovementTime);
            }
            else
            {
                // Si se tiene una destination asignada cambiarla dentro de X segundos
                randomMovementTimer -= Time.deltaTime;

                if (randomMovementTimer <= 0)
                {
                    randomMovementTimer = 0;
                    // Desvincular el movimiento anterior para que se decida uno nuevo
                    randomDestinationSet = false;
                }
            }

            // Mirar constantemente al jugador
            HandleRotateTowardsTarget(enemyManager);

            // Si se ha elegido un ataque para realizar, pasar al estado de ataque
            // Si se esta en un cool down despues de un ataque, seguir en este modo
            if (enemyManager.currentRecoveryTime <= 0 && attackState.currentAttack != null)
            {
                randomDestinationSet = false;
                return attackState;
            }
            else
                // Elegir un nuevo ataque
                GetNewAttack(enemyManager);

            return this;
        }

        // Gira hacia el jugador de forma suave
        static public void HandleRotateTowardsTarget(EnemyManager enemyManager)
        {
            Vector3 direction = enemyManager.currentTarget.transform.position - enemyManager.transform.position;
            direction.y = 0;
            direction.Normalize();

            if (direction == Vector3.zero)
                direction = enemyManager.transform.forward;

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            enemyManager.transform.rotation = Quaternion.Slerp
                (enemyManager.transform.rotation, targetRotation, 3 * Time.deltaTime);
        }

        // Diferentes tipos de acercarse al jugador
        struct CombatWalkingType
        {
            public float verticalMovementValue;
            public float horizontalMovementValue;

            public CombatWalkingType(float _verticalMovementValue, float _horizontalMovementValue)
            {
                verticalMovementValue = _verticalMovementValue;
                horizontalMovementValue = _horizontalMovementValue;
            }
        }

        CombatWalkingType[] combatWalkingTypes;
        int currentWalkTypeIndex = 0;

        private void Awake()
        {
            // Se configuran los 3 tipos diferentes de acercarse al jugador
            combatWalkingTypes = new CombatWalkingType[3];

            // Andar hacia la derecha
            combatWalkingTypes[0] = new CombatWalkingType(0, .5f);
            // Andar hacia la izquierda
            combatWalkingTypes[1] = new CombatWalkingType(0, -.5f);
            // Andar hacia el frente
            combatWalkingTypes[2] = new CombatWalkingType(.6f, 0);
        }

        // Decide la forma de acercarse al enemigo aleatoriamente
        private void DecideCirclingAction(EnemyAnimatorHandler enemyAnimatorHandler)
        {
            // No elegir el mismo tipo de andar dos veces seguidas
            int randomNewWalkType = currentWalkTypeIndex;
            while (randomNewWalkType == currentWalkTypeIndex)
                randomNewWalkType = Random.Range(0, combatWalkingTypes.Length);
            currentWalkTypeIndex = randomNewWalkType;

            // Settear los valores que determinan el movimiento en el enemigo
            CombatWalkingType randomWalkType = combatWalkingTypes[currentWalkTypeIndex];
            verticalMovementValue = randomWalkType.verticalMovementValue;
            horizontalMovementValue = randomWalkType.horizontalMovementValue;
        }

        // Elegir un nuevo ataque, que este dentro del rango, en un angulo permitido de forma aleatoria
        private void GetNewAttack(EnemyManager enemyManager)
        {
            Vector3 targetsDirection = enemyManager.currentTarget.transform.position - transform.position;
            float viewableAngle = Vector3.Angle(targetsDirection, transform.forward);
            float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);

            int maxScore = 0;

            // Todos los ataques permitidos suman su probabilidad de salir elegidos
            for (int i = 0; i < enemyAttacks.Length; i++)
            {
                EnemyAttackAction enemyAttackAction = enemyAttacks[i];

                // Si el ataque seleccionado no puede usarse (angulo no apropiado o distancia)
                // Si es valido, parar el movimiento y atacar el target (jugador)
                if (distanceFromTarget <= enemyAttackAction.maximumDistanceNeededToAttack
                    && distanceFromTarget >= enemyAttackAction.minimumDistanceNeededToAttack)
                {
                    if (viewableAngle <= enemyAttackAction.maximunAttackAngle
                        && viewableAngle >= enemyAttackAction.minimumAttackAngle)
                    {
                        if (lastAttackName == enemyAttackAction.name)
                            continue;

                        maxScore += enemyAttackAction.attackScore;
                    }
                }
            }

            int randomValue = Random.Range(0, maxScore);
            int temporaryScore = 0;

            // Se elige un ataque de esos teniendo en cuenta la probabilidad de cada uno
            for (int i = 0; i < enemyAttacks.Length; i++)
            {
                EnemyAttackAction enemyAttackAction = enemyAttacks[i];

                if (distanceFromTarget <= enemyAttackAction.maximumDistanceNeededToAttack
                    && distanceFromTarget >= enemyAttackAction.minimumDistanceNeededToAttack)
                {
                    if (viewableAngle <= enemyAttackAction.maximunAttackAngle
                        && viewableAngle >= enemyAttackAction.minimumAttackAngle)
                    {
                        if (lastAttackName == enemyAttackAction.name)
                            continue;

                        if (attackState.currentAttack != null)
                            return;

                        temporaryScore += enemyAttackAction.attackScore;

                        if (temporaryScore > randomValue)
                        {
                            attackState.currentAttack = enemyAttackAction;
                            lastAttackName = enemyAttackAction.name;
                        }
                    }
                }
            }
        }
    }
}
