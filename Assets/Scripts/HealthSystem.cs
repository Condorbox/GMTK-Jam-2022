using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    private float health;


    public static event EventHandler<GameObject> OnAnyDead; //TODO Roll Dice
    public event EventHandler OnDamaged;

    private void Awake()
    {
        health = maxHealth;
    }

    public void Damage(float damageAmount)
    {
        health -= damageAmount;

        if (health < 0)
        {
            health = 0;
        }

        OnDamaged?.Invoke(this, EventArgs.Empty);

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        OnAnyDead?.Invoke(this, this.gameObject);
    }

    public float GetHealthNormalized()
    {
        return health / maxHealth;
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }
}
