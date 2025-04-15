using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float speed;
    private int damage;
    private float lifetime;
    private float turnSpeed;
    private float rotationSpeed;

    public GameObject target;
    private Rigidbody2D rb;
    private bool hasHit;

    public void InitializeFromWeapon(WeaponSetting weapon, GameObject newTarget, Vector2 initialDirection)
    {
        speed = weapon.bulletSpeed;
        damage = weapon.damage;
        lifetime = weapon.bulletLifetime;
        turnSpeed = 200f;
        rotationSpeed = 5f;

        target = newTarget;

        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = initialDirection.normalized * speed;

        Destroy(gameObject, lifetime);
    }

    void FixedUpdate()
    {
        if (target != null && rb != null)
        {
            Vector2 directionToTarget = target.transform.position - transform.position;
            directionToTarget.Normalize();

            float angleToTarget = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg;
            float angle = Mathf.LerpAngle(transform.eulerAngles.z, angleToTarget, rotationSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            rb.linearVelocity = transform.right * speed;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasHit || other.CompareTag("Bullets")) return;

        if (other.CompareTag("Enemy"))
        {
            FishEnemy fishEnemy = other.GetComponent<FishEnemy>();
            if (fishEnemy != null)
            {
                fishEnemy.TakeDamage(damage);
                HandleHit();
            }

            Destroy(gameObject);
        }
    }

    private void HandleHit()
    {
        hasHit = true;
        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null) spriteRenderer.enabled = false;

        Destroy(gameObject, 0.1f);
    }
}
