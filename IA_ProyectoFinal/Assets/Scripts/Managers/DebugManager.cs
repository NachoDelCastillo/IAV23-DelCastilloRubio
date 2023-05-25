using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace NX
{
    public class DebugManager : MonoBehaviour
    {
        [SerializeField]
        GameObject[] debugObjects;

        [SerializeField]
        GameObject[] notDebugObjects;

        bool debugMode = false;



        EnemyManager enemyManager;

        [SerializeField]
        TMP_Text distanceText;

        [SerializeField]
        TMP_Text angleText;

        [SerializeField]
        TMP_Text stateText;

        [SerializeField]
        TMP_Text performactionText;

        private void Awake()
        {
            enemyManager = FindObjectOfType<EnemyManager>();

            DisableDebugMode();
        }

        float distance;
        float viewableAngle;
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.B))
            {
                if (debugMode)
                    DisableDebugMode();
                else
                    EnableDebugMode();
            }

            if (debugMode)
                UpdateDebugUI();
        }

        void UpdateDebugUI()
        {
            if (enemyManager.currentTarget == null) return;

            distance = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);
            distanceText.text = "DISTANCE - " + ColorText(((int)distance).ToString());

            Vector3 targetsDirection = enemyManager.currentTarget.transform.position - enemyManager.transform.position;
            viewableAngle = Vector3.Angle(targetsDirection, transform.forward);
            angleText.text = "ANGLE - " + ColorText(((int)viewableAngle).ToString());

            stateText.text = "STATE - " + ColorText(enemyManager.gameObject.name);

            string s;
            if (enemyManager.isInteracting) s = "TRUE";
            else s = "FALSE";
            performactionText.text = "PERFORMING ACTION - " + ColorText(s);
        }

        string ColorText(string text)
        { return "<color=#0056FF>" + text + "</color>"; }

        private void OnDrawGizmos()
        {
            if (!debugMode)
                return;

            if (enemyManager == null)
                enemyManager = FindObjectOfType<EnemyManager>();

            Gizmos.DrawWireSphere(enemyManager.transform.position, distance);
            Gizmos.DrawLine(enemyManager.transform.position, enemyManager.currentTarget.transform.position);
        }


        void EnableDebugMode()
        {
            debugMode = true;
            foreach (GameObject obj in debugObjects)
                obj.SetActive(true);
            foreach (GameObject obj in notDebugObjects)
                obj.SetActive(false);
        }

        void DisableDebugMode()
        {
            debugMode = false;
            foreach (GameObject obj in debugObjects)
                obj.SetActive(false);
            foreach (GameObject obj in notDebugObjects)
                obj.SetActive(true);
        }
    }
}
