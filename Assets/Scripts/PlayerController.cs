using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private CharacterController controller;
    [SerializeField] private float normalMoveSpeed = 10f;
    [SerializeField] private float invisibleSpeed = 5f;
    private float moveSpeed;
    [SerializeField] private float rotationSpeed = 400f;

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
    void Update()
    {
        GetInput();

        if (input == Vector3.zero)
        {
            animator.SetBool("Walk", false);
            return;
        }

        if (diceActivated) return;

        if (playerSkills.isInvisible) moveSpeed = invisibleSpeed; //FIX maybe change this with an observer pattern or something similar
        else moveSpeed = normalMoveSpeed;

        Move();
        Look();
    }

    void GetInput()//TODO Change for the new Input System
    {
        input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
    }

    private void Move()
    {
        if (input == Vector3.zero) return;

        Vector3 moveVector = IsoToConvert(input);
        animator.SetBool("Walk", true);
        controller.Move(moveVector * moveSpeed * Time.deltaTime);
    }

    private void Look()
    {
        /*Vector3 mousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, Camera.main.transform.position.y - transform.position.y));
        var targetRotation = Quaternion.LookRotation(mousePos - new Vector3(transform.position.x, 0, transform.position.z));
        transform.eulerAngles = Vector3.up * Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetRotation.eulerAngles.y, rotationSpeed * Time.deltaTime);*/

        if (input == Vector3.zero) return;

        Quaternion targetRotation = Quaternion.LookRotation(input.ToIsometric(), Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private Vector3 IsoToConvert(Vector3 vector)
    {
        Quaternion rotation = Quaternion.Euler(0, 45.0f, 0);
        Matrix4x4 isoMatrix = Matrix4x4.Rotate(rotation);
        Vector3 result = isoMatrix.MultiplyPoint3x4(vector);

        return result;
    }
}
