using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private float rotationSpeed = 360f;

    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform modelTransform;
    [SerializeField] private Animator animator;

    private Vector3 input;

    private PlayerSkills playerSkills;
    private void Start()
    {
        playerSkills = GetComponent<PlayerSkills>();
    }

    private void Update()
    {
        if (playerSkills.isInvisible) moveSpeed = 2f;
        else moveSpeed = 4f;

        GetInput();
        Look();
    }

    private void FixedUpdate()
    {
        if (input == Vector3.zero)
        {
            animator.SetBool("Walk", false);
            return;
        }

        Move();
    }

    private void Look()
    {
        if (input == Vector3.zero) return;

        Quaternion rot = Quaternion.LookRotation(input.ToIsometric(), Vector3.up);
        modelTransform.rotation = Quaternion.RotateTowards(modelTransform.rotation, rot, rotationSpeed * Time.deltaTime);

    }

    void GetInput()//TODO Change for the new Input System
    {
        input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
    }

    void Move() 
    {
        animator.SetBool("Walk", true);
        rb.MovePosition(transform.position + input.ToIsometric() * input.normalized.magnitude * moveSpeed * Time.deltaTime);
    }
}
