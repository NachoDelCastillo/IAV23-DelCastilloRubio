using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NX
{
    public class EnemyStats : CharacterStats
    {
        EnemyManager enemyManager;
        Animator anim;

        // Elementos de la Interfaz
        [SerializeField]
        Image fill;

        [SerializeField]
        Image delayFill;

        [SerializeField]
        ParticleSystem flameHeart;


        private void Awake()
        {
            // Referencias
            enemyManager = GetComponent<EnemyManager>();
            anim = GetComponentInChildren<Animator>();
        }

        private void Start()
        {
            currentHealth = maxHealth;
        }

        private void Update()
        {
            // Actualizar barra de vida

            float healthAmount = ((float)currentHealth / (float)maxHealth);

            fill.fillAmount = Mathf.Lerp(fill.fillAmount, healthAmount, Time.deltaTime * 2);

            delayFill.fillAmount = Mathf.Lerp(delayFill.fillAmount, healthAmount, Time.deltaTime / 2);
        }


        // Se encarga de procesar la accion de recibir daño, actualizando las variables necesarias
        // y actualizando la interfaz del enemigo en consecuencia
        Vector3 particleOffset = new Vector3(0, 4, 0);
        public void TakeDamage(int damage)
        {
            if (isDead)
                return;

            currentHealth -= damage;

            //anim.Play("Damage");

            ParticleManager.GetInstance().Play("SparksAndLines_Orange", transform.position + particleOffset);

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                anim.Play("Death");
                isDead = true;

                ParticleManager.GetInstance().Play("SparksAndLines_Orange", transform.position + particleOffset);
                ParticleManager.GetInstance().Play("SparksAndLines_Orange", transform.position + particleOffset);
                ParticleManager.GetInstance().Play("SparksAndLines_Orange", transform.position + particleOffset);
                ParticleManager.GetInstance().Play("SparksAndLines_Orange", transform.position + particleOffset);
                ParticleManager.GetInstance().Play("SparksAndLines_Orange", transform.position + particleOffset);

                StartCoroutine(FlameStop());

                enemyManager.SwitchToNextState(FindObjectOfType<IdleState>());
                anim.SetBool("canRotate", false);
            }
        }

        // Cuando el enemigo es derrotado, se llama a esta coroutina para apagar el fuego de su cabeza
        IEnumerator FlameStop()
        {
            yield return new WaitForSeconds(2f);
            flameHeart.Stop();

            MessageManager.GetInstance().PlayMessage("flame extinguished", MessageManager.ColorName.blue, MessageManager.MessagesName.fadeBlackLine);
        }
    }
}
