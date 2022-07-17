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

    private bool diceActivated = false;

    //[SerializeField] private Animator animator;

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
    }

    private void PlayerDice_OnDiceDeactivated(object sender, EventArgs e)
    {
        diceActivated = false;
    }

    private void Awake()
    {
        ammoCounter = maxAmmo;
    }

    private void Update()
    {
        if (diceActivated) return;

        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        if (ammoCounter < weaponData.ammoConsume)
        {
            OnFailShoot?.Invoke(this, EventArgs.Empty);
            return;
        }

        ammoCounter -= weaponData.ammoConsume;

        GameObject bulletGameObject = PoolManager.Instance.GetPoolObject(weaponData.bulletType);
        bulletGameObject.transform.position = shootPoint.position;
        bulletGameObject.transform.rotation = Quaternion.identity;
        bulletGameObject.SetActive(true);

        Vector3 shootDir = (shootPoint.position - transform.position).normalized.ToIsometric();
        bulletGameObject.GetComponent<Bullet>().SetUp(shootDir, weaponData.damage, weaponData.bulletType, this.gameObject);

        OnShoot?.Invoke(this, EventArgs.Empty);
    }
}
