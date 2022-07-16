using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TurretEnemy : AIAgent
{

    private enum State
    {
        Searching,
        Shooting,
        Listened
    }

    [Header("Turret Enemy")]
    private State state;

    [Header("Searching")]
    [SerializeField] private float rotationSearchingSpeed = .2f;
    [SerializeField] private Transform[] wayPoints;
    [SerializeField] private float minDistanceRotation = .2f;
    [SerializeField] private float timeToWaitBtwPoints = 2f;

    [Header("Shooting")]
    [SerializeField] private float rotationShootingSpeed = .5f;
    [SerializeField] private PoolObjectType bulletType;
    [SerializeField] private int bulletsInOneRound = 5;
    [SerializeField] private float timeBtwBullets = .15f;
    [SerializeField] private float timeBtwRounds = 2.05f;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private float timeToForget;
    [SerializeField][Range(0f, 1f)] private float maxPrecision = 1f;
    [SerializeField][Range(0f, 1f)] private float minPrecision = .9f;
    private float counterRounds;
    private float counterForget;
    private Transform playerTransform;
    private int index = 0;
    bool waiting = false;

    [Header("Listened")]
    private Vector3 listenedPosition;

    private void Update()
    {
        switch (state)
        {
            case State.Searching:
                Searching();
                break;
            case State.Shooting:
                Shooting();
                break;
            case State.Listened:
                Listened();
                break;
        }
    }

    private void Searching()
    {
        if (waiting) return;
        Vector3 rotationDir = RotateTowardsTarget(wayPoints[index].position, rotationSearchingSpeed);
        if (Vector3.Distance(rotationDir, transform.forward) < minDistanceRotation)
        {
            index = NextIndex(index);
            waiting = true;
            StartCoroutine(WaitSeconds(timeToWaitBtwPoints));
        }
    }

    private Vector3 RotateTowardsTarget(Vector3 target, float speed)
    {
        Vector3 rotationDir = (target - transform.position).normalized;
        transform.forward = Vector3.Lerp(transform.forward, rotationDir, speed * Time.deltaTime);

        return rotationDir;
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

    private IEnumerator WaitSeconds(float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);
        waiting = false;
    }

    private void Shooting()
    {
        if (playerTransform == null)
        {
            Debug.LogError($"<color=red><b> playerTransform null in Chasing state </b></color>");
            waiting = false;
            state = State.Searching;
            return;
        }

        RotateTowardsTarget(playerTransform.position, rotationShootingSpeed);

        counterRounds -= Time.deltaTime;
        if (counterRounds <= 0)
        {
            StartCoroutine(ShootBullets());
            counterRounds = timeBtwRounds;
        }

        if (!aiSensor.IsInSight(playerTransform.gameObject) || !aiSensor.GetObjectsDetected().Contains(playerTransform.gameObject))
        {
            counterForget -= Time.deltaTime;
            if (counterForget <= 0)
            {
                waiting = false;
                state = State.Searching;
            }
        }
    }

    private IEnumerator ShootBullets()
    {
        for (int i = 0; i < bulletsInOneRound; i++)
        {
            GameObject bulletGameObject = PoolManager.Instance.GetPoolObject(bulletType);
            bulletGameObject.transform.position = shootPoint.position;
            bulletGameObject.transform.rotation = shootPoint.rotation;
            bulletGameObject.SetActive(true);

            Vector3 shootDir = (playerTransform.position - shootPoint.position).normalized;
            Vector3 precisionVector = new Vector3(Random.Range(shootDir.x * minPrecision, shootDir.x * maxPrecision), shootDir.y, Random.Range(shootDir.z * minPrecision, shootDir.z * maxPrecision));
            bulletGameObject.GetComponent<Bullet>().SetUp(precisionVector, 1, bulletType);

            yield return new WaitForSeconds(timeBtwBullets);
        }
    }

    protected override void ReceivedPlayerIsInSight(Transform playerTransform)
    {
        counterForget = timeToForget;
        listenedPosition = playerTransform.position;
        state = State.Listened;
    }
    private void Listened()
    {
        Vector3 rotationDir = RotateTowardsTarget(listenedPosition, rotationShootingSpeed);
        Debug.Log(Vector3.Distance(rotationDir, listenedPosition));

        if (Vector3.Distance(rotationDir, transform.forward) < minDistanceRotation)
        {
            counterForget -= Time.deltaTime;
            Debug.Log($"Listened: {counterForget}");
            if (counterForget <= 0)
            {
                waiting = false;
                state = State.Searching;
            }
        }
    }

    protected override void PlayerDetected(Transform playerTransform)
    {
        this.playerTransform = playerTransform;
        counterForget = timeToForget;
        if (state != State.Shooting) counterRounds = timeBtwRounds / 2;
        state = State.Shooting;
    }


}
