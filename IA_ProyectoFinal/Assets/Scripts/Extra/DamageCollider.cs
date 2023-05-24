using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NX
{
    public class DamageCollider : MonoBehaviour
    {
        [SerializeField]
        int damage;

        Collider damageCollider;

        private void Awake()
        {
            damageCollider = GetComponent<Collider>();
            damageCollider.gameObject.SetActive(true);
            damageCollider.isTrigger = true;
            damageCollider.enabled = false;
        }

        public void EnableDamageCollider()
        {
            damageCollider.enabled = true;
        }

        public void DisableDamageCollider()
        {
            damageCollider.enabled = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                PlayerStats playerStats = other.GetComponent<PlayerStats>();

                playerStats.TakeDamage(damage);
            }
            else if (other.tag == "Enemy")
            {
                EnemyStats enemyStats = other.GetComponent<EnemyStats>();

                enemyStats.TakeDamage(damage);
            }
        }
    }
}
