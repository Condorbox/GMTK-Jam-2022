using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Death
{
    public GameObject killer;
    public GameObject murdered;
}

public class HealthSystem : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    private float health;


    public static event EventHandler<Death> OnAnyDead; //TODO Roll Dice
    public event EventHandler OnDamaged;

    private void Awake()
    {
        health = maxHealth;
    }

    public void Damage(float damageAmount, GameObject killerGameObject)
    {
        health -= damageAmount;

        if (health < 0)
        {
            health = 0;
        }

        OnDamaged?.Invoke(this, EventArgs.Empty);

        if (health <= 0)
        {
            Die(killerGameObject);
        }
    }

    private void Die(GameObject killerGameObject)
    {
        OnAnyDead?.Invoke(this, new Death
        {
            killer = killerGameObject,
            murdered = this.gameObject
        });
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
