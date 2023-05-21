using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NX
{
    public class EnemyStats : CharacterStats
    {
        Animator anim;

        private void Awake()
        {
            anim = GetComponentInChildren<Animator>();
        }

        private void Start()
        {
            currentHealth = maxHealth;
        }

        public void TakeDamage(int damage)
        {
            if (isDead)
                return;

            currentHealth -= damage;

            anim.Play("Damage");

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                anim.Play("Death");
                isDead = true;
            }
        }
    }
}
