using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkills : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackSize = 1.5f;
    public bool isInvisible;

    private bool diceActivated = false;

    //[SerializeField] private GameObject meleeWeapon;
    //[SerializeField] private GameObject isInvisibleEffects;

    private void OnEnable()
    {
        PlayerDice.OnDiceActivated += PlayerDice_OnDiceActivated;
        PlayerDice.OnDiceDeactivated += PlayerDice_OnDiceDeactivated;
    }

    private void OnDisable()
    {
        PlayerDice.OnDiceActivated -= PlayerDice_OnDiceActivated;
        PlayerDice.OnDiceDeactivated -= PlayerDice_OnDiceDeactivated;
    }

    private void PlayerDice_OnDiceActivated(object sender, EventArgs e)
    {
        diceActivated = true;
    }

    private void PlayerDice_OnDiceDeactivated(object sender, EventArgs e)
    {
        diceActivated = false;
    }

    void Start()
    {
        isInvisible = false;
    }

    void Update()
    {
        if (diceActivated) return;

        if (Input.GetKeyDown(KeyCode.Space)) Melee(true);
        if (Input.GetKeyDown(KeyCode.LeftShift)) Invisible(true);

        if (Input.GetKeyUp(KeyCode.Space)) Melee(false);
        if (Input.GetKeyUp(KeyCode.LeftShift)) Invisible(false);

    }

    private void Invisible(bool boolean)
    {
        isInvisible = boolean;
        animator.SetBool("Invisible", boolean);
        //isInvisibleEffects.SetActive(boolean);
    }

    private void Melee(bool boolean)
    {
        //Debug.Log(Mathf.RoundToInt(UnityEngine.Random.Range(1, 6)));
        animator.SetBool("Attack", boolean);
        if (boolean == true)
        {
            MeleeAttack();
        }

        //meleeWeapon.SetActive(boolean);
        // meleeWeapon.SetActive(false) --> at the end of the melee animation
    }

    public void MeleeAttack()
    {
        Collider[] enemiesColliders = Physics.OverlapSphere(transform.position, attackSize, enemyLayer, QueryTriggerInteraction.Ignore);

        foreach (Collider enemy in enemiesColliders)
        {
            if (enemy.TryGetComponent<HealthSystem>(out HealthSystem healthSystem))
            {
                healthSystem.Damage(float.MaxValue);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(attackPoint.position, attackSize);
    }
}
