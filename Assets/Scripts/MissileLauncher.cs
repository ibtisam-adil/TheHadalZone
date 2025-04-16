using UnityEngine;

public class MissileLauncher : WeaponSetting
{
    public MissileAmmoUI ammoUI;

    public int maxAmmo = 8;
    private int currentAmmo;
    private float cooldownTime = 0f;
    private float fireCooldownDuration;

    void Start()
    {
        fireCooldownDuration = 1f / fireRate;
        currentAmmo = maxAmmo;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Time.time >= nextFireTime && currentAmmo > 0)
        {
            Shoot();
            nextFireTime = Time.time + fireCooldownDuration;
            cooldownTime = fireCooldownDuration;
            currentAmmo--;
            ammoUI.UpdateAmmo(currentAmmo, maxAmmo);
        }

        if (cooldownTime > 0)
        {
            cooldownTime -= Time.deltaTime;
            float fill = cooldownTime / fireCooldownDuration;
            ammoUI.SetCooldown(fill);
        }
        else
        {
            ammoUI.SetCooldown(0f);
        }
    }


    public override void Shoot()
    {
        GameObject target = FindClosestEnemy();
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation); // Keep its current facing

        if (target != null)
        {
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            bulletScript.InitializeFromWeapon(this, target, firePoint.right); // Use straight direction
        }

        Rigidbody2D playerRb = GameObject.FindGameObjectWithTag("Submarine").GetComponent<Rigidbody2D>();
        if (playerRb != null)
        {
            Vector2 recoilForce = -(firePoint.right * weaponRecoil);
            playerRb.AddForce(recoilForce);
        }
    }

    private GameObject FindClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject closest = null;
        float minDistance = Mathf.Infinity;
        Vector3 currentPos = firePoint.position;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(enemy.transform.position, currentPos);
            if (distance < minDistance)
            {
                minDistance = distance;
                closest = enemy;
            }
        }

        return closest;
    }
}
