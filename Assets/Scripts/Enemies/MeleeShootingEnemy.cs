using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class MeleeShootingEnemy : MeleeEnemy
{
    [Header("MeleeShootingEnemy")]
    [SerializeField] private float timeChasing = 1.5f;
    private float counterStopChasing;
    [SerializeField][Range(0f, 1f)] private float probabilityChase = .3f;
    private float numberProbChase;
    [Header("Shooting")]
    [SerializeField] private float rotationShootingSpeed = .5f;
    [SerializeField] private PoolObjectType bulletType;
    [SerializeField] private int bulletsInOneRound = 5;
    [SerializeField] private float timeBtwBullets = .15f;
    [SerializeField] private float timeBtwRounds = 2.05f;
    [SerializeField] private Transform shootPoint;
    [SerializeField][Range(0f, 1f)] private float maxPrecision = 1f;
    [SerializeField][Range(0f, 1f)] private float minPrecision = .9f;
    private float counterRounds;
    private float counterForget;

    protected override void AttackAction()
    {
        if (numberProbChase <= probabilityChase)
        {
            Chasing();

            counterStopChasing -= Time.deltaTime;
            if (counterStopChasing <= 0)
            {
                numberProbChase = 10; //Always Shooting :D
                counterStopChasing = timeChasing;
            }
        }
        else
        {
            Shooting();
        }
    }

    private void Shooting()
    {
        agent.destination = transform.position;
        if (playerTransform == null)
        {
            Debug.LogError($"<color=red><b> playerTransform null in Chasing state </b></color>");
            StateToPatrolling();
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
                StateToPatrolling();
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
            bulletGameObject.GetComponent<Bullet>().SetUp(precisionVector, 1, bulletType, this.gameObject);

            yield return new WaitForSeconds(timeBtwBullets);
        }
    }

    private Vector3 RotateTowardsTarget(Vector3 target, float speed)
    {
        Vector3 rotationDir = (target - transform.position).normalized;
        transform.forward = Vector3.Lerp(transform.forward, rotationDir, speed * Time.deltaTime);

        return rotationDir;
    }

    protected override void StateToAttackAction()
    {
        numberProbChase = Random.Range(0f, 1f);
        base.StateToAttackAction();
    }
}
