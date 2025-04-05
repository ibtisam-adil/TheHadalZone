using UnityEngine;

public class Pistol : Gun
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Time.time >= nextFireTime)
        {

            Shoot();
            nextFireTime = Time.time + 1f / fireRate;
        }
    }


    public override void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Bullet bulletScript = bullet.GetComponent<Bullet>();

        bulletScript.Initialize(bulletSpeed, bulletLifetime, damage);
        bulletScript.SetDirection(firePoint.right);

        // Recoil
        Rigidbody2D playerRb = GameObject.FindGameObjectWithTag("Submarine").GetComponent<Rigidbody2D>();
        if (playerRb != null)
        {
            Vector2 recoilForce = -(AimWeapon.aimDirection * weaponRecoil);
            playerRb.AddForce(recoilForce);
        }

        //foreach (var enemy in EnemyManager.Instance.GetAllEnemies())
        //{
        //    enemy.ResetHitFlag();
        //}

    }
}