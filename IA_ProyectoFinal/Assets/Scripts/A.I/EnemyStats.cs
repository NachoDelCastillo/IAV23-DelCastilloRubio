using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NX
{
    public class EnemyStats : CharacterStats
    {
        [SerializeField]
        Image fill;

        [SerializeField]
        Image delayFill;

        Animator anim;

        private void Awake()
        {
            anim = GetComponentInChildren<Animator>();
        }

        private void Start()
        {
            currentHealth = maxHealth;
        }

        private void Update()
        {
            float healthAmount = ((float)currentHealth / (float)maxHealth);

            fill.fillAmount = Mathf.Lerp(fill.fillAmount, healthAmount, Time.deltaTime * 2);

            delayFill.fillAmount = Mathf.Lerp(delayFill.fillAmount, healthAmount, Time.deltaTime / 2);
        }

        public void TakeDamage(int damage)
        {
            if (isDead)
                return;

            currentHealth -= damage;

            //anim.Play("Damage");

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                anim.Play("Death");
                isDead = true;
            }
        }
    }
}
