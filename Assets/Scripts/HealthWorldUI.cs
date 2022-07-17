using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthWorldUI : MonoBehaviour
{
    [SerializeField] private Image healthBarImage;
    [SerializeField] HealthSystem healthSystem;

    private void OnEnable()
    {
        healthSystem.OnDamaged += HealthSystem_OnDamaged;
    }

    private void OnDisable()
    {
        healthSystem.OnDamaged -= HealthSystem_OnDamaged;
    }

    private void Start()
    {
        UpdateHealthBar();
    }

    private void HealthSystem_OnDamaged(object sender, EventArgs e)
    {
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        healthBarImage.fillAmount = healthSystem.GetHealthNormalized();
    }
}
