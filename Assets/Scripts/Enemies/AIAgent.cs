using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        Debug.Log("Player detected in: " + e.position);
    }
}
