using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace NX
{
    public class CameraHandler : MonoBehaviour
    {
        InputHandler inputHandler;

        public Transform targetTransform;
        public Transform cameraTransform;
        public Transform cameraPivotTransform;
        private Transform myTransform;
        private Vector3 cameraTransformPosition;
        private LayerMask ignoreLayers;
        private Vector3 cameraFollowVelocity = Vector3.zero;

        public static CameraHandler singleton;

        public float lookSpeed = 0.1f;
        public float followSpeed = 0.8f;
        public float pivotSpeed = 0.03f;

        private float targetPosition;
        private float defaultPosition;
        private float lookAngle;
        private float pivotAngle;
        public float minimunPivot = -35;
        public float maximunPivot = 35;

        public float cameraSphereRadius = .2f;
        public float cameraCollisionOffset = .2f;
        public float minimumCollisionOffset = .2f;

        public Transform currentLockOnTarget;

        List<CharacterManager> availableTargets = new List<CharacterManager>();
        public Transform nearestLockOnTarget;
        public Transform leftLockTarget;
        public Transform rightLockTarget;
        public float maxLockOnDistance = 30;

        private void Awake()
        {
            singleton = this;

            myTransform = transform;
            defaultPosition = cameraTransform.localPosition.z;
            ignoreLayers = ~(1 << 8 | 1 << 9 | 1 << 10);
            targetTransform = FindObjectOfType<PlayerManager>().transform;
            inputHandler = FindObjectOfType<InputHandler>();
        }

        public void FollowTarget(float delta)
        {
            Vector3 targetPosition = Vector3.SmoothDamp(myTransform.position,
                targetTransform.position, ref cameraFollowVelocity, delta / followSpeed);
            myTransform.position = targetPosition;

            HandleCameraCollision(delta);
        }

        public void HandleCameraRotation(float delta, float mouseXInput, float mouseYInput)
        {
            if (!inputHandler.lockOnFlag)
            {
                CameraControl_Manual(delta, mouseXInput, mouseYInput);
            }
            else
            {
                if (currentLockOnTarget == null)
                    CameraControl_Manual(delta, mouseXInput, mouseYInput);

                else
                {
                    float velocity = 0;

                    Vector3 dir = currentLockOnTarget.position - transform.position;
                    dir.Normalize();
                    dir.y = 0;

                    Quaternion targetRotation = Quaternion.LookRotation(dir);
                    transform.rotation = targetRotation;

                    dir = currentLockOnTarget.position - cameraPivotTransform.position;
                    dir.Normalize();

                    targetRotation = Quaternion.LookRotation(dir);
                    Vector3 eulerAngle = targetRotation.eulerAngles;
                    eulerAngle.y = 0;
                    cameraPivotTransform.localEulerAngles = eulerAngle;
                }
            }
        }

        void CameraControl_Manual(float delta, float mouseXInput, float mouseYInput)
        {
            lookAngle += (mouseXInput * lookSpeed) / delta;
            pivotAngle -= (mouseYInput * pivotSpeed) / delta;
            pivotAngle = Mathf.Clamp(pivotAngle, minimunPivot, maximunPivot);

            Vector3 rotation = Vector3.zero;
            rotation.y = lookAngle;
            Quaternion targetRotation = Quaternion.Euler(rotation);
            myTransform.rotation = targetRotation;

            rotation = Vector3.zero;
            rotation.x = pivotAngle;

            targetRotation = Quaternion.Euler(rotation);
            cameraPivotTransform.localRotation = targetRotation;
        }




        private void HandleCameraCollision(float delta)
        {
            targetPosition = defaultPosition;
            RaycastHit hit;
            Vector3 direction = cameraTransform.position - cameraPivotTransform.position;
            direction.Normalize();

            if (Physics.SphereCast(cameraPivotTransform.position, cameraSphereRadius,
                direction, out hit, Mathf.Abs(targetPosition), ignoreLayers))
            {
                float dis = Vector3.Distance(cameraPivotTransform.position, hit.point);
                targetPosition = -(dis - cameraCollisionOffset);
            }

            if (Mathf.Abs(targetPosition) < minimumCollisionOffset)
                targetPosition = -minimumCollisionOffset;

            cameraTransformPosition.z = Mathf.Lerp(cameraTransform.localPosition.z, targetPosition, delta / .2f);
            cameraTransform.localPosition = cameraTransformPosition;
        }



        public void HandleLockOn()
        {
            availableTargets.Clear();

            float shortestDistance = Mathf.Infinity;
            float shortestDistanceOfLeftTarget = Mathf.Infinity;
            float shortestDistanceOfRightTarget = Mathf.Infinity;


            Collider[] colliders = Physics.OverlapSphere(targetTransform.position, 26);

            for (int i = 0; i < colliders.Length; i++)
            {
                CharacterManager character = colliders[i].GetComponent<CharacterManager>();

                if (character != null)
                {
                    Vector3 lockTargetDirection = character.transform.position - targetTransform.position;
                    float distanceFromTarget = Vector3.Distance(targetTransform.position, character.transform.position);
                    float viewableAngle = Vector3.Angle(lockTargetDirection, cameraTransform.forward);

                    if (character.transform.root != targetTransform.transform.root
                        && viewableAngle > -50 && viewableAngle < 50 && distanceFromTarget <= maxLockOnDistance)
                    {
                        availableTargets.Add(character);
                    }
                }
            }

            for (int k = 0; k < availableTargets.Count; k++)
            {
                float distanceFromTarget = Vector3.Distance
                    (targetTransform.position, availableTargets[k].transform.position);

                if (distanceFromTarget < shortestDistance)
                {
                    shortestDistance = distanceFromTarget;

                    nearestLockOnTarget = availableTargets[k].lockOnTransform;
                }


                if (inputHandler.lockOnFlag && currentLockOnTarget)
                {
                    Vector3 relativeEnemyPosition = currentLockOnTarget.InverseTransformPoint(availableTargets[k].transform.position);
                    var distanceFromLeftTarget = currentLockOnTarget.transform.position.x - availableTargets[k].transform.position.x;
                    var distanceFromRightTarget = currentLockOnTarget.transform.position.x + availableTargets[k].transform.position.x;

                    if (relativeEnemyPosition.x > 0.00 && distanceFromLeftTarget < shortestDistanceOfLeftTarget)
                    {
                        shortestDistanceOfLeftTarget = distanceFromLeftTarget;
                        leftLockTarget = availableTargets[k].lockOnTransform;
                    }

                    if (relativeEnemyPosition.x < 0.00 && distanceFromRightTarget < shortestDistanceOfRightTarget)
                    {
                        shortestDistanceOfRightTarget = distanceFromRightTarget;
                        rightLockTarget = availableTargets[k].lockOnTransform;
                    }
                }
            }

            //currentLockOnTarget = nearestLockOnTarget;

            //if (inputHandler.lockOnFlag)
            //    for (int k = 0; k < availableTargets.Count; k++)
            //    {
            //        Vector3 relativeEnemyPosition = currentLockOnTarget.InverseTransformPoint(availableTargets[k].transform.position);
            //        var distanceFromLeftTarget = currentLockOnTarget.transform.position.x - availableTargets[k].transform.position.x;
            //        var distanceFromRightTarget = currentLockOnTarget.transform.position.x + availableTargets[k].transform.position.x;

            //        if (relativeEnemyPosition.x > 0.00 && distanceFromLeftTarget < shortestDistanceOfLeftTarget)
            //        {
            //            shortestDistanceOfLeftTarget = distanceFromLeftTarget;
            //            leftLockTarget = availableTargets[k].lockOnTransform;
            //        }

            //        if (relativeEnemyPosition.x < 0.00 && distanceFromRightTarget < shortestDistanceOfRightTarget)
            //        {
            //            shortestDistanceOfRightTarget = distanceFromRightTarget;
            //            rightLockTarget = availableTargets[k].lockOnTransform;
            //        }
            //    }
        }

        public void ClearLockOnTargets()
        {
            availableTargets.Clear();

            nearestLockOnTarget = null;
            currentLockOnTarget = null;
        }
    }
}
