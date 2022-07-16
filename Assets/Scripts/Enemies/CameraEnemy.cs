using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraEnemy : AIAgent
{
    private enum State
    {
        Searching,
        Chasing
    }

    [Header("Camera Enemy")]
    [SerializeField] private Light cameraLight;

    [Header("Searching")]
    [SerializeField] private float rotationSearchingSpeed = .25f;
    [SerializeField] private Transform[] wayPoints;
    [SerializeField] private float minDistanceRotation = .2f;
    [SerializeField] private float timeToWaitBtwPoints = .5f;
    [SerializeField] private Color lightColorSearching;
    bool waiting = false;

    [Header("Chasing")]
    [SerializeField] private float rotationChasingSpeed = .5f;
    [SerializeField] private float timeToForget = .5f;
    [SerializeField] private Color lightColorChasing;
    private float counter;

    private State state = State.Searching;
    private Transform playerTransform;

    private int index = 0;

    private void Awake()
    {
        cameraLight.color = lightColorSearching;
    }

    private void Update()
    {
        switch (state)
        {
            case State.Searching:
                Searching();
                break;
            case State.Chasing:
                Chasing();
                break;
        }
    }

    private void Searching()
    {
        if (waiting) return;
        Vector3 rotationDir = RotateTowardsTarget(wayPoints[index], rotationSearchingSpeed);
        if (Vector3.Distance(rotationDir, transform.forward) < minDistanceRotation)
        {
            index = NextIndex(index);
            waiting = true;
            StartCoroutine(WaitSeconds());
        }
    }

    private Vector3 RotateTowardsTarget(Transform target, float speed)
    {
        Vector3 rotationDir = (target.position - transform.position).normalized;
        transform.forward = Vector3.Lerp(transform.forward, rotationDir, speed * Time.deltaTime);

        return rotationDir;
    }

    private IEnumerator WaitSeconds()
    {
        yield return new WaitForSeconds(timeToWaitBtwPoints);
        waiting = false;
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

    private void Chasing()
    {
        if (playerTransform == null)
        {
            Debug.LogError($"<color=red><b> playerTransform null in Chasing state </b></color>");
            waiting = false;
            cameraLight.color = lightColorSearching;
            state = State.Searching;
            return;
        }

        RotateTowardsTarget(playerTransform, rotationChasingSpeed);

        if (!aiSensor.IsInSight(playerTransform.gameObject) || !aiSensor.GetObjectsDetected().Contains(playerTransform.gameObject))
        {
            counter -= Time.deltaTime;
            if (counter <= 0)
            {
                waiting = false;
                cameraLight.color = lightColorSearching;
                state = State.Searching;
            }
        }
    }

    protected override void PlayerDetected(Transform player)
    {
        playerTransform = player;
        counter = timeToForget;
        cameraLight.color = lightColorChasing;
        state = State.Chasing;
    }
}
