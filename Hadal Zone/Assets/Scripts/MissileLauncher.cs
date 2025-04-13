using UnityEngine;

public class MissileLauncher : WeaponSetting
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
        // First, find the closest enemy
        GameObject target = FindClosestEnemy();

        // Then, instantiate the bullet
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

        if (target != null)
        {
            // Assign the target to the bullet
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            bulletScript.target = target;

            // Set the bullet's direction
            bullet.transform.right = (target.transform.position - bullet.transform.position).normalized;

            bulletScript.Initialize(bulletSpeed, bulletLifetime, damage);
            bulletScript.SetDirection(firePoint.right);
        }

        // Recoil code
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
