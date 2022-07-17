using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoWorldUI : MonoBehaviour
{
    [SerializeField] private Image ammoBarImage;
    [SerializeField] private ShootController shootController;

    private void OnEnable()
    {
        shootController.OnShoot += ShootController_OnShoot;
    }

    private void Start()
    {
        UpdateAmmoBar();
    }

    private void ShootController_OnShoot(object sender, EventArgs e)
    {
        UpdateAmmoBar();
    }

    private void UpdateAmmoBar()
    {
        ammoBarImage.fillAmount = shootController.GetAmmoNormalized();
        Debug.Log(shootController.GetAmmoNormalized());
    }
}
