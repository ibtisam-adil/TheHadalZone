using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public int damage = 10;
    public float lifetime = 3f;
    public float turnSpeed = 200f;
    public float rotationSpeed = 5f;
    public float bulletSpeed = 10f;

    public GameObject target;
    private Rigidbody2D rb;
    private bool hasHit;

    public void Initialize(float bulletSpeed, float bulletLifetime, int bulletDamage)
    {
        speed = bulletSpeed;
        lifetime = bulletLifetime;
        damage = bulletDamage;
        Destroy(gameObject, lifetime);
    }

    public void SetDirection(Vector2 initialDirection)
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = initialDirection.normalized * speed;
    }

    public void SetTarget(GameObject newTarget)
    {
        target = newTarget;
    }

    void FixedUpdate()
    {
        if (target != null)
        {
            Vector2 directionToTarget = target.transform.position - transform.position;
            directionToTarget.Normalize();

            // Get the angle to rotate towards the target
            float angleToTarget = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg;

            // Smoothly rotate the missile to the target using LerpAngle
            float angle = Mathf.LerpAngle(transform.eulerAngles.z, angleToTarget, rotationSpeed * Time.deltaTime);

            // Apply the rotation to the missile
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

            // Move the missile towards the target
            rb.linearVelocity = transform.right * bulletSpeed;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasHit) return;
        if (other.CompareTag("Bullets")) return;

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