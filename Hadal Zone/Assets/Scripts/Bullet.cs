using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public int damage = 10;
    public float lifetime = 3f;
    private Vector2 direction;
    private GameObject shooter;

    private Rigidbody2D rb;
    private bool hasHit;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = direction * speed;
        Destroy(gameObject, lifetime);
    }

    public void SetDamage(int newDamage)
    {
        damage = newDamage;
    }

    public void Initialize(float bulletSpeed, float bulletLifetime, int bulletDamage)
    {
        speed = bulletSpeed;
        lifetime = bulletLifetime;
        damage = bulletDamage;
    }

    public void SetDirection(Vector2 aimDirection)
    {
        direction = aimDirection.normalized;
        if (rb != null) rb.linearVelocity = direction * speed;
    }

    private void Update()
    {
        transform.position += (Vector3)direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasHit) return;
        if (other.CompareTag("Bullets")) return;
        Debug.Log("Bullet hit: " + other.gameObject.name);

        if (other.CompareTag("Enemy") && other.gameObject != shooter)
        {
            Debug.Log("Bullet hit an enemy!");

            FishEnemy fishEnemy = other.GetComponent<FishEnemy>();
            if (fishEnemy != null)
            {
                fishEnemy.TakeDamage(damage);


                HandleHit();
        }


        Destroy(gameObject); // Destroy bullet on impact
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




