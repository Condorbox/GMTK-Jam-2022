using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIAgent : MonoBehaviour
{
    [SerializeField] protected AISensor aiSensor;
    [SerializeField] private float tellOthersPositionOfPlayerRadius = 3f;
    [SerializeField] private LayerMask enemiesLayers;

    private bool diceActivated = false;

    private void OnEnable()
    {
        aiSensor.OnPlayerDetected += AISensor_OnPlayerDetected;
        PlayerDice.OnDiceActivated += PlayerDice_OnDiceActivated;
        PlayerDice.OnDiceDeactivated += PlayerDice_OnDiceDeactivated;
    }

    private void OnDisable()
    {
        aiSensor.OnPlayerDetected -= AISensor_OnPlayerDetected;
        PlayerDice.OnDiceActivated -= PlayerDice_OnDiceActivated;
        PlayerDice.OnDiceDeactivated -= PlayerDice_OnDiceDeactivated;
    }

    protected virtual void PlayerDice_OnDiceActivated(object sender, EventArgs e)
    {
        diceActivated = true;
    }

    protected virtual void PlayerDice_OnDiceDeactivated(object sender, EventArgs e)
    {
        diceActivated = false;
    }

    private void AISensor_OnPlayerDetected(object sender, Transform e)
    {
        PlayerDetected(e);
        TellOthersPlayerIsInSight(e);
    }

    protected abstract void PlayerDetected(Transform playerTransform);

    protected virtual void TellOthersPlayerIsInSight(Transform playerTransform)
    {
        Collider[] enemiesColliders = Physics.OverlapSphere(transform.position, tellOthersPositionOfPlayerRadius, enemiesLayers, QueryTriggerInteraction.Ignore);

        foreach (Collider enemy in enemiesColliders)
        {
            if (enemy.TryGetComponent<AIAgent>(out AIAgent agent) && enemy.gameObject != this.gameObject)
            {
                agent.ReceivedPlayerIsInSight(playerTransform);
            }
        }
    }

    protected virtual void ReceivedPlayerIsInSight(Transform playerTransform)
    {

    }

    protected bool GetDiceActivated()
    {
        return diceActivated;
    }

    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, tellOthersPositionOfPlayerRadius);
    }
}
