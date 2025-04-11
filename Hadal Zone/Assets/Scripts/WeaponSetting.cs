using UnityEngine;

public abstract class WeaponSetting : MonoBehaviour
{
    public int damage;
    public float fireRate;
    public GameObject bulletPrefab;
    public Transform firePoint;
    public AimWeapon AimWeapon;
    public float bulletSpeed;
    public float bulletLifetime;
    public float weaponRecoil;

    protected float nextFireTime = 0f;

    public abstract void Shoot();
}
