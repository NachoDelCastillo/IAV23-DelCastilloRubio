using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.AI;

namespace NX
{
    public class EnemyManager : CharacterManager
    {
        IgnoreCollisions enemyLocomotion;
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

        [SerializeField]
        TMP_Text distanceText;

        [SerializeField]
        TMP_Text angleText;

        [SerializeField]
        TMP_Text stateText;

        [SerializeField]
        TMP_Text animationText;



        private void Awake()
        {
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

        //float distance = Vector3.Distance(currentTarget.transform.position, transform.position);
        //Debug.Log("DISTANCE = " + distance);

        //    Vector3 targetsDirection = currentTarget.transform.position - transform.position;
        //float viewableAngle = Vector3.Angle(targetsDirection, transform.forward);
        //Debug.Log("VIEW ANGLE = " + viewableAngle);

        private void Update()
        {
            HandleRecoveryTimer();
            isInteracting = enemyAnimatorHandler.anim.GetBool("isInteracting");
            isRotatingWithRootMotion = enemyAnimatorHandler.anim.GetBool("isRotatingWithRootMotion");
            canRotate = enemyAnimatorHandler.anim.GetBool("canRotate");



            // IA TEXTS
            distance = Vector3.Distance(currentTarget.transform.position, transform.position);
            distanceText.text = "DISTANCE - " + ColorText(((int)distance).ToString());


            Vector3 targetsDirection = currentTarget.transform.position - transform.position;
            viewableAngle = Vector3.Angle(targetsDirection, transform.forward);
            angleText.text = "ANGLE - " + ColorText(((int)viewableAngle).ToString());

            stateText.text = "STATE - " + ColorText(currentState.gameObject.name);

            string s;
            if (isInteracting) s = "TRUE";
            else s = "FALSE";
            animationText.text = "PERFORMING ACTION - " + ColorText(s);
        }

        string ColorText(string text)
        { return "<color=#0056FF>" + text + "</color>"; }


        float distance;
        float viewableAngle;
        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, distance);

            Gizmos.DrawLine(transform.position, currentTarget.transform.position);
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

        public void SwitchToNextState(State state)
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
