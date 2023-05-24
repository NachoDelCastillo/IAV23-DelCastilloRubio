using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

namespace NX
{
    public class CombatStanceState : State
    {
        public IdleState idleState;
        public AttackState attackState;
        public PursueTargetState pursueTargetState;

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
            // Comprobar si el jugador ha muerto
            if (enemyManager.currentTarget.GetComponent<PlayerStats>().isDead)
            {
                enemyAnimatorHandler.PlayTargetAnimation("EnemyVictory", true);
                return idleState;
            }

            float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);

            enemyAnimatorHandler.anim.SetFloat("Vertical", verticalMovementValue, 0.2f, Time.deltaTime);
            enemyAnimatorHandler.anim.SetFloat("Horizontal", horizontalMovementValue, 0.2f, Time.deltaTime);

            attackState.hasPerformedAttack = false;

            if (enemyManager.isInteracting)
            {
                enemyAnimatorHandler.anim.SetFloat("Vertical", 0);
                enemyAnimatorHandler.anim.SetFloat("Horizontal", 0);
                return this;
            }
            else if (distanceFromTarget > enemyManager.maximumAggroRadius)
            {
                //enemyManager.navMeshAgent.transform.rotation = enemyManager.transform.rotation;
                return pursueTargetState;
            }


            // Caminar alrededor del jugador
            if (!randomDestinationSet)
            {
                randomDestinationSet = true;
                // Decicir el destino aleatoriamente
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


            HandleRotateTowardsTarget(enemyManager);


            // Comprobar si se esta a rango de ataque
            if (enemyManager.currentRecoveryTime <= 0 && attackState.currentAttack != null)
            {
                randomDestinationSet = false;
                return attackState;
            }
            else
            {
                GetNewAttack(enemyManager);
            }

            return this;


            // Si esta en rango de ataque, cambiar a estado de ataque

            // Si se esta en un cool down despues de un ataque, seguir en este modo

            // Si el jugador se va fuera del rango de combate, cambiar al estado de persecucion
        }

        static public void HandleRotateTowardsTarget(EnemyManager enemyManager)
        {
            Vector3 direction = enemyManager.currentTarget.transform.position - enemyManager.transform.position;
            direction.y = 0;
            direction.Normalize();

            if (direction == Vector3.zero)
                direction = enemyManager.transform.forward;

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            //enemyManager.transform.rotation = Quaternion.Slerp(enemyManager.transform.rotation, targetRotation, (enemyManager.rotationSpeed * .005f) / Time.deltaTime);
            enemyManager.transform.rotation = Quaternion.Slerp
                (enemyManager.transform.rotation, targetRotation, 3 * Time.deltaTime);
        }


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
            combatWalkingTypes = new CombatWalkingType[3];

            combatWalkingTypes[0] = new CombatWalkingType(0, 1);
            combatWalkingTypes[1] = new CombatWalkingType(0, -.5f);
            combatWalkingTypes[2] = new CombatWalkingType(.6f, 0);
        }

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

        private void WalkBack(EnemyAnimatorHandler enemyAnimatorHandler)
        {
            verticalMovementValue = -0.4f;

            horizontalMovementValue = 0;
        }

        private void WalkFront(EnemyAnimatorHandler enemyAnimatorHandler)
        {
            verticalMovementValue = 0.4f;
            horizontalMovementValue = 0;
        }

        private void GetNewAttack(EnemyManager enemyManager)
        {
            Vector3 targetsDirection = enemyManager.currentTarget.transform.position - transform.position;
            float viewableAngle = Vector3.Angle(targetsDirection, transform.forward);
            float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);

            int maxScore = 0;

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

                        maxScore += enemyAttackAction.attackScore;
                    }
                }
            }

            int randomValue = Random.Range(0, maxScore);
            int temporaryScore = 0;

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
