using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NinjaAnimation : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private float startTimeNinja = 1.5f;
    [SerializeField] private float timeBtwNinja = 7.5f;
    private float counter;

    private void Start()
    {
        counter = startTimeNinja;
    }

    private void Update()
    {
        counter -= Time.deltaTime;
        if (counter <= 0)
        {
            animator.SetTrigger("Ninja");
            counter = timeBtwNinja;
        }
    }
}
