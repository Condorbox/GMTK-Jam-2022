using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeleeEnemy : AIAgent
{
    private enum State
    {
        Patrolling,
        Chasing
    }

    [Header("Melee Enemy")]
    [SerializeField] private NavMeshAgent agent;
    private State state;

    [Header("Patrolling")]
    [SerializeField] private Transform[] wayPoints;
    [SerializeField] private float patrollingSpeed = 4.5f;
    [SerializeField] private float minDistance = .2f;
    [SerializeField] private float timeToWaitBtwPoints = .5f;

    private int index = 0;
    private bool waiting = false;

    [Header("Chasing")]
    [SerializeField] private float chasingSpeed = 7f;
    [SerializeField] private float timeToForget;
    private Transform playerTransform;
    private float counter;

    [Header("Attack")]
    [SerializeField] private float attackDistance = .3f;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackSize = .3f;

    private void Awake()
    {
        state = State.Patrolling;
        agent.speed = patrollingSpeed;
    }

    private void Update()
    {
        switch (state)
        {
            case State.Patrolling:
                Patrolling();
                break;
            case State.Chasing:
                Chasing();
                break;
        }
    }
    private void Patrolling()
    {
        if (waiting) return;
        agent.destination = wayPoints[index].position;

        if (Vector3.Distance(transform.position, wayPoints[index].position) < minDistance)
        {
            index = NextIndex(index);
            waiting = true;
            StartCoroutine(WaitSeconds());
        }
    }

    private int NextIndex(int index)
    {
        index++;
        if (index >= wayPoints.Length)
        {
            index = 0;
        }

        return index;
    }

    private IEnumerator WaitSeconds()
    {
        yield return new WaitForSeconds(timeToWaitBtwPoints);
        waiting = false;
    }

    private void Chasing()
    {
        if (playerTransform == null)
        {
            Debug.LogError($"<color=red><b> playerTransform null in Chasing state </b></color>");
            StateToPatrolling();
            return;
        }

        agent.destination = playerTransform.position;

        if (!aiSensor.IsInSight(playerTransform.gameObject) || !aiSensor.GetObjectsDetected().Contains(playerTransform.gameObject))
        {
            counter -= Time.deltaTime;
            if (counter <= 0)
            {
                StateToPatrolling();
                return;
            }
        }

        if (Vector3.Distance(playerTransform.position, transform.position) < attackDistance) //TODO Roll Dice, base Attack?
        {
            Collider[] playerCollider = Physics.OverlapSphere(attackPoint.position, attackSize, playerLayer, QueryTriggerInteraction.Ignore);

            foreach (Collider player in playerCollider)
            {
                if (TryGetComponent<PlayerMovement>(out PlayerMovement playerMovement))
                {
                    Debug.Log($"player {playerMovement.gameObject.name} hit");
                }
            }
        }
    }

    protected override void ReceivedPlayerIsInSight(Transform playerTransform)
    {
        PlayerDetected(playerTransform);
    }

    protected override void PlayerDetected(Transform playerTransform)
    {
        this.playerTransform = playerTransform;
        counter = timeToForget;
        agent.speed = chasingSpeed;
        state = State.Chasing;
    }

    private void StateToPatrolling()
    {
        waiting = false;
        state = State.Patrolling;
    }
}
