using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace NX
{
    public class FollowObject : MonoBehaviour
    {
        [SerializeField]
        Transform target;

        [SerializeField]
        float followSpeed;

        [SerializeField]
        Rigidbody rb;

        float velocityLerper = 0;
        Vector3 direction;
        Vector3 directionLerped;

        private void Awake()
        {
            PlayerManager player = FindObjectOfType<PlayerManager>();
        }

        private void Update()
        {
            transform.position = Vector3.Lerp(transform.position, target.position, followSpeed * Time.deltaTime);

            Vector3 velThisFrame = -rb.velocity;

            if (velThisFrame != Vector3.zero)
            {
                direction = Vector3.Lerp(direction, -rb.velocity, Time.deltaTime * 2);
                //direction = -playerRb.velocity;
                direction.y = 0;
                direction.Normalize();
            }

            float velocityMagnitude = rb.velocity.magnitude;
            float velocityValueOverOne = velocityMagnitude / 20;

            velocityLerper = Mathf.Lerp(velocityLerper, velocityValueOverOne, Time.deltaTime * 2);

           // transform.forward = Vector3.Lerp(transform.forward, direction, velocityLerper);

            transform.up = Vector3.Lerp(Vector3.up, direction, velocityLerper + .1f);
        }
    }
}
