using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkills : MonoBehaviour
{
    [SerializeField] private Animator animator;
    public bool isInvisible;

    //[SerializeField] private GameObject meleeWeapon;
    //[SerializeField] private GameObject isInvisibleEffects;


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
        animator.SetBool("Invisible", boolean);
        //isInvisibleEffects.SetActive(boolean);
    }

    private void Melee(bool boolean)
    {
        //Debug.Log(Mathf.RoundToInt(UnityEngine.Random.Range(1, 6)));
        animator.SetBool("Attack", boolean);

        //meleeWeapon.SetActive(boolean);
        // meleeWeapon.SetActive(false) --> at the end of the melee animation
    }
}
