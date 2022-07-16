using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AISensor))]
public class AIAgent : MonoBehaviour
{
    [SerializeField] private AISensor aiSensor;

    private void OnEnable()
    {
        aiSensor.OnPlayerDetected += AISensor_OnPlayerDetected;
    }

    private void OnDisable()
    {
        aiSensor.OnPlayerDetected -= AISensor_OnPlayerDetected;
    }

    private void AISensor_OnPlayerDetected(object sender, Transform e)
    {
        PlayerDetected(e);
    }

    protected virtual void PlayerDetected(Transform player)
    {
        Debug.Log("Player detected in: " + player.position);
    }
}
