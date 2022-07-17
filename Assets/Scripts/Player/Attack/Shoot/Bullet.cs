using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float bulletSpeed = 100f;
    [SerializeField] private string targetTag = "Enemy";
    private float damage;
    private float timeOfLive = 3f;
    private PoolObjectType bulletType;
    GameObject shooter;
    public void SetUp(Vector3 moveDir, float damage, PoolObjectType bulletType, GameObject shooter)
    {
        this.bulletType = bulletType;
        this.damage = damage;
        this.shooter = shooter;

        Rigidbody rigidbody = GetComponent<Rigidbody>();

        rigidbody.AddForce(moveDir * bulletSpeed, ForceMode.Impulse);
        Invoke("CoolBullet", timeOfLive);
    }

    protected void CoolBullet()
    {
        PoolManager.Instance.CoolObject(this.gameObject, bulletType);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(targetTag))
        {
            if (TryGetComponent<HealthSystem>(out HealthSystem healthSystem))
            {
                healthSystem.Damage(damage, shooter);
                Debug.Log("Hit to: " + collision.gameObject.name);
                CoolBullet();
            }
        }
    }
}
