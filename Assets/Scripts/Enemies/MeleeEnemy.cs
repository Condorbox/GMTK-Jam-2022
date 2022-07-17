using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeleeEnemy : AIAgent
{
    protected enum State
    {
        Patrolling,
        Chasing
    }

    [Header("Melee Enemy")]
    [SerializeField] protected NavMeshAgent agent;
    protected State state;

    [Header("Patrolling")]
    [SerializeField] private Transform[] wayPoints;
    [SerializeField] private float patrollingSpeed = 4.5f;
    [SerializeField] private float minDistance = .2f;
    [SerializeField] private float timeToWaitBtwPoints = .5f;

    private int index = 0;
    protected bool waiting = false;

    [Header("Chasing")]
    [SerializeField] private float chasingSpeed = 7f;
    [SerializeField] private float timeToForget = 1.5f;
    protected Transform playerTransform;
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
                AttackAction();
                break;
        }
    }

    protected virtual void AttackAction()
    {
        Chasing();
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

    protected void Chasing()
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

        if (Vector3.Distance(playerTransform.position, transform.position) < attackDistance)
        {
            Debug.Log("Attack");
            Collider[] playerCollider = Physics.OverlapSphere(attackPoint.position, attackSize, playerLayer, QueryTriggerInteraction.Ignore);

            foreach (Collider player in playerCollider)
            {
                if (player.gameObject.TryGetComponent<PlayerMovement>(out PlayerMovement playerMovement)) //TODO change this xd
                {
                    Debug.Log($"player {player.gameObject.name} hit"); //TODO Roll Dice, base Attack?
                    break;
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
        StateToAttackAction();
    }

    protected virtual void StateToAttackAction()
    {
        counter = timeToForget;
        agent.speed = chasingSpeed;
        state = State.Chasing;
    }

    protected void StateToPatrolling()
    {
        waiting = false;
        state = State.Patrolling;
    }

    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();

        if (attackPoint == null) return;
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(attackPoint.position, attackSize);
    }
}
