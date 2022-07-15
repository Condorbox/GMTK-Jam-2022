using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkills : MonoBehaviour
{
    [SerializeField] private GameObject meleeWeapon;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) Melee(true);

        else
        {
            Melee(false);
        }
    }

    private void Melee(bool boolean)
    {
        meleeWeapon.SetActive(boolean);
        // meleeWeapon.SetActive(false) --> at the end of the melee animation
    }
}
