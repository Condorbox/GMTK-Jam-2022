using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float normalMoveSpeed = 10f;
    [SerializeField] private float invisibleSpeed = 5f;
    private float moveSpeed = 4f;
    [SerializeField] private float rotationSpeed = 360f;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform modelTransform;
    [SerializeField] private Animator animator;

    private Vector3 input;
    private PlayerSkills playerSkills;

    private bool diceActivated = false;

    private void OnEnable()
    {
        PlayerDice.OnDiceActivated += PlayerDice_OnDiceActivated;
        PlayerDice.OnDiceDeactivated += PlayerDice_OnDiceDeactivated;
    }

    private void OnDisable()
    {
        PlayerDice.OnDiceActivated -= PlayerDice_OnDiceActivated;
        PlayerDice.OnDiceDeactivated -= PlayerDice_OnDiceDeactivated;
    }
    private void PlayerDice_OnDiceActivated(object sender, EventArgs e)
    {
        diceActivated = true;
        input = Vector3.zero;
    }

    private void PlayerDice_OnDiceDeactivated(object sender, EventArgs e)
    {
        diceActivated = false;
    }

    private void Start()
    {
        playerSkills = GetComponent<PlayerSkills>();
    }

    private void Update()
    {
        if (diceActivated) return;

        if (playerSkills.isInvisible) moveSpeed = invisibleSpeed; //FIX maybe change this with an observer pattern or something similar
        else moveSpeed = normalMoveSpeed;

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

        if (diceActivated) return;

        Move();
    }

    private void Look()
    {
        /*Vector3 mousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, Camera.main.transform.position.y - transform.position.y));
        var targetRotation = Quaternion.LookRotation(mousePos - new Vector3(transform.position.x, 0, transform.position.z));
        transform.eulerAngles = Vector3.up * Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetRotation.eulerAngles.y, rotationSpeed * Time.deltaTime);*/

        if (input == Vector3.zero) return;

        Quaternion targetRotation = Quaternion.LookRotation(input.ToIsometric(), Vector3.up);
        modelTransform.rotation = Quaternion.RotateTowards(modelTransform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    void GetInput()//TODO Change for the new Input System
    {
        input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
    }

    void Move()
    {
        if (input == Vector3.zero) return;
        animator.SetBool("Walk", true);
        rb.MovePosition(transform.position + input.ToIsometric() * input.normalized.magnitude * moveSpeed * Time.deltaTime);
    }
}
