using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float bulletSpeed = 100f;
    private float damage;
    private float timeOfLive = 3f;
    private PoolObjectType bulletType;
    public void SetUp(Vector3 moveDir, float damage, PoolObjectType bulletType)
    {
        this.bulletType = bulletType;
        this.damage = damage;

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
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (TryGetComponent<HealthSystem>(out HealthSystem healthSystem))
            {
                healthSystem.Damage(damage);
                CoolBullet();
            }
        }
    }
}
