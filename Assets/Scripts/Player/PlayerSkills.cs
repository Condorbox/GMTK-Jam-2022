using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkills : MonoBehaviour
{
    [SerializeField] private GameObject meleeWeapon;

    [SerializeField] private Material[] materials;

    private bool isInvisible;

    [SerializeField] private GameObject isInvisibleEffects;

    void Start()
    {
        isInvisible = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) Melee(true);
        if (Input.GetKeyDown(KeyCode.LeftShift)) Invisible(true);

        if (Input.GetKeyUp(KeyCode.Space)) Melee(false);
        if (Input.GetKeyUp(KeyCode.LeftShift)) Invisible(false);

    }

    private void Invisible(bool boolean)
    {
        isInvisible = boolean;
        isInvisibleEffects.SetActive(boolean);

        float alpha;

        if (boolean)
        {
            alpha = 0.5f;
        }
        else
        {
            alpha = 1f;
        }

        foreach (Material mat in materials)
        {
            Color newColor = mat.color;
            newColor.a = alpha;
            mat.color = newColor;

        }
    }

    private void Melee(bool boolean)
    {
        //Debug.Log(Mathf.RoundToInt(UnityEngine.Random.Range(1, 6)));


        meleeWeapon.SetActive(boolean);
        // meleeWeapon.SetActive(false) --> at the end of the melee animation
    }
}
