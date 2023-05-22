using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace NX
{
    public class EnemyManager : CharacterManager
    {
        EnemyLocomotion enemyLocomotion;
        EnemyAnimatorHandler enemyAnimatorHandler;
        EnemyStats enemyStats;

        public State currentState;
        public CharacterStats currentTarget;
        public Rigidbody enemyRigidbody;
        public NavMeshAgent navMeshAgent;

        public bool isPerformingAction;
        public bool isInteracting;
        public float rotationSpeed = 15;
        public float maximumAggroRadius = 5f;

        public float currentRecoveryTime = 0;

        private void Awake()
        {
            enemyLocomotion = GetComponent<EnemyLocomotion>();
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

            //float distance = Vector3.Distance(currentTarget.transform.position, transform.position);
            //Debug.Log("DISTANCE = " + distance);

            //Vector3 targetsDirection = currentTarget.transform.position - transform.position;
            //float viewableAngle = Vector3.Angle(targetsDirection, transform.forward);
            //Debug.Log("VIEW ANGLE = " + viewableAngle);


            HandleRecoveryTimer();
            isInteracting = enemyAnimatorHandler.anim.GetBool("isInteracting");
            isRotatingWithRootMotion = enemyAnimatorHandler.anim.GetBool("isRotatingWithRootMotion");
            canRotate = enemyAnimatorHandler.anim.GetBool("canRotate");

            Debug.Log("canRotate = " + canRotate);
        }

        private void FixedUpdate()
        {
            HandleStateMachine();
        }

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

        void SwitchToNextState(State state)
        {
            currentState = state;
        }

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
