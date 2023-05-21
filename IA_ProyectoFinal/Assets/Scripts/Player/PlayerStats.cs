using NX;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : CharacterStats
{
    PlayerManager playerManager;
    AnimatorHandler animatorHandler;

    [SerializeField]
    HealthBar healthBar;


    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
        animatorHandler = GetComponentInChildren<AnimatorHandler>();
    }

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            TakeDamage(1);
        }
    }

    public void TakeDamage(int damage)
    {
        if (playerManager.isInvulnerable)
            return;

        if (isDead)
            return;

        currentHealth -= damage;

        healthBar.SetCurrentHealth(currentHealth);

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            animatorHandler.PlayTargetAnimation("Death", true);
            isDead = true;
        }
        else
        {
            animatorHandler.PlayTargetAnimation("Damage", true);
        }
    }
}
