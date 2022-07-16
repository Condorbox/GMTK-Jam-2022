using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using Random = UnityEngine.Random;

public class ShootController : MonoBehaviour
{
    [SerializeField] private Transform shootPoint;
    [SerializeField] private Transform bulletPrefab;
    [SerializeField] private WeaponData weaponData;
    [SerializeField] private int maxAmmo = 100;
    private int ammoCounter;

    private event EventHandler OnShoot;
    private event EventHandler OnFailShoot;

    //[SerializeField] private Animator animator;


    private void Awake()
    {
        ammoCounter = maxAmmo;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        if (ammoCounter < weaponData.ammoComsum)
        {
            OnFailShoot?.Invoke(this, EventArgs.Empty);
            return;
        }

        ammoCounter -= weaponData.ammoComsum;

        GameObject bulletGameObject = PoolManager.Instance.GetPoolObject(weaponData.bulletType);
        bulletGameObject.transform.position = shootPoint.position;
        bulletGameObject.transform.rotation = Quaternion.identity;
        bulletGameObject.SetActive(true);

        Vector3 shootDir = (shootPoint.position - transform.position).normalized.ToIsometric();
        bulletGameObject.GetComponent<Bullet>().SetUp(shootDir, weaponData.damage,weaponData.bulletType);

        OnShoot?.Invoke(this, EventArgs.Empty);
    }
}
