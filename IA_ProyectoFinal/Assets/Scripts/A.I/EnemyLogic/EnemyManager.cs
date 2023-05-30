using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.AI;

namespace NX
{
    public class EnemyManager : CharacterManager
    {
        // Referencias a otros scripts del enemigo
        IgnoreCollisions enemyLocomotion;
        EnemyAnimatorHandler enemyAnimatorHandler;
        EnemyStats enemyStats;

        // Variables relacionadas con la maquina de estados
        public State currentState;
        public CharacterStats currentTarget;
        public Rigidbody enemyRigidbody;
        public NavMeshAgent navMeshAgent;

        // Variables extra necesarias para el comportamiento del enemigo
        public bool isPerformingAction;
        public bool isInteracting;
        public float rotationSpeed = 15;
        public float maximumAggroRadius = 5f;

        public float currentRecoveryTime = 0;

        private void Awake()
        {
            // Asignaciones iniciales
            enemyLocomotion = GetComponent<IgnoreCollisions>();
            enemyAnimatorHandler = GetComponentInChildren<EnemyAnimatorHandler>();
            enemyStats = GetComponent<EnemyStats>();
            enemyRigidbody = GetComponent<Rigidbody>();
            navMeshAgent = GetComponentInChildren<NavMeshAgent>();
            navMeshAgent.enabled = false;
        }

        private void Start()
        {
            enemyRigidbody.isKinematic = false;
        }

        private void Update()
        {
            // Actualiza todas las variables dependiendo de sus valores en el animator
            HandleRecoveryTimer();
            isInteracting = enemyAnimatorHandler.anim.GetBool("isInteracting");
            isRotatingWithRootMotion = enemyAnimatorHandler.anim.GetBool("isRotatingWithRootMotion");
            canRotate = enemyAnimatorHandler.anim.GetBool("canRotate");
        }


        private void FixedUpdate()
        {
            HandleStateMachine();
        }

        // Este metodo se encarga de ejecutar el estado actual de la maquina de estados
        // Y de cambiarlo en caso de que cambie mediante el metodo "SwitchToNextState"
        private void HandleStateMachine()
        {
            if (enemyStats.isDead)
                return;

            if (currentState != null)
            {
                State nextState = currentState.Tick(this, enemyStats, enemyAnimatorHandler);

                if (nextState != null)
                    SwitchToNextState(nextState);
            }
        }

        // Cambia el estado actual de la maquina de estados
        public void SwitchToNextState(State state)
        {
            currentState = state;
        }

        // Se encarga de actualizar las variables relacionadas con el
        // tiempo de recovery (tiempo que tiene que esperar antes de realizar otra accion)
        // Y el flag que muestra si se esta realizando una accion o no
        void HandleRecoveryTimer()
        {
            if (currentRecoveryTime > 0)
                currentRecoveryTime -= Time.deltaTime;

            if (isPerformingAction)
                if (currentRecoveryTime <= 0)
                    isPerformingAction = false;
        }
    }
}
