using UnityEngine;

public class FishEnemy : MonoBehaviour
{
    [Header("Enemy Stats")]
    public int baseHealth = 20; 
    public float speed = 2.0f;
    public int damage = 1;

    [Header("Separation Behavior")]
    public float separationDistance = 2.5f;
    public float separationForce = 10f; 

    [Header("References")]
    public Transform submarine;
    private SpriteRenderer spriteRenderer;
    private Collider2D[] colliders;
    private Rigidbody2D rb;

    [Header("State")]
    private int maxHealth; // Maximum health for this enemy
    private int currentHealth; // Current health
    private bool hasBeenHit = false; // Prevents multiple hits in quick succession
    private bool isAlive = true;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        colliders = GetComponentsInChildren<Collider2D>();
        rb = GetComponent<Rigidbody2D>();

        if (rb == null)
        {
            Debug.LogError("Rigidbody2D is missing on the enemy!");
        }
    }

    private void Start()
    {
        currentHealth = baseHealth; // Initialize with baseHealth (you had maxHealth before which wasn't set)

        FindSubmarineReference();

        // Ignore collisions between enemies using the modern method
        Collider2D[] allEnemies = FindObjectsByType<Collider2D>(FindObjectsSortMode.None);
        Collider2D thisCollider = GetComponent<Collider2D>();

        foreach (Collider2D enemyCollider in allEnemies)
        {
            if (enemyCollider != thisCollider && enemyCollider.CompareTag("Enemy"))
            {
                Physics2D.IgnoreCollision(thisCollider, enemyCollider);
            }
        }
    }


    public void TakeDamage(int damage)
    {
        if (!isAlive || hasBeenHit) return;

        currentHealth -= damage;
        currentHealth = Mathf.Max(0, currentHealth);
        hasBeenHit = true;
        Debug.Log($"Took {damage} damage. Health: {currentHealth}");

        // Visual feedback (flash red)
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.red;
            Invoke(nameof(ResetColor), 0.1f);
        }

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            Invoke(nameof(ResetHitFlag), 0.1f); // Reset hit flag after a short delay
        }
    }

    private void Die()
    {
        if (!isAlive) return; // Prevent multiple death calls
        isAlive = false;

        Debug.Log($"Enemy: Dying at {transform.position}");

        // Disable colliders and destroy the enemy
        DisableColliders();
        enabled = false; // Disable this script
        Destroy(gameObject, 0.1f); // Destroy after a short delay
    }

    private void DisableColliders()
    {
        foreach (Collider2D collider in colliders)
        {
            collider.enabled = false;
        }
        Debug.Log("Enemy: Colliders disabled");
    }

    private void Update()
    {
        if (submarine == null)
        {
            FindSubmarineReference();
            return;
        }

        Vector2 moveDirection = (submarine.position - transform.position).normalized;
        rb.linearVelocity = moveDirection * speed;
    }


    private void AvoidOtherEnemies()
    {
        Vector2 separationVector = GetSeparationVector();
        if (separationVector != Vector2.zero)
        {
            rb.AddForce(separationVector * separationForce * Time.deltaTime, ForceMode2D.Impulse);
        }
    }

    private Vector2 GetSeparationVector()
    {
        Vector2 separationVector = Vector2.zero;
        int nearbyEnemies = 0;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, separationDistance);

        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject != gameObject && collider.CompareTag("Enemy"))
            {
                Vector2 awayDirection = (transform.position - collider.transform.position).normalized;

                float distance = Vector2.Distance(transform.position, collider.transform.position);
                float forceScale = 1 - (distance / separationDistance);

                separationVector += awayDirection * forceScale;
                nearbyEnemies++;
            }
        }

        if (nearbyEnemies > 0)
        {
            separationVector /= nearbyEnemies;
            separationVector *= separationForce;
        }

        return separationVector;
    }

    private void FindSubmarineReference()
    {
        if (submarine == null)
        {
            submarine = GameObject.FindGameObjectWithTag("Submarine")?.transform;
            if (submarine == null)
            {
                Debug.LogWarning("submarine not found. Retrying in 1 second.");
                Invoke(nameof(FindSubmarineReference), 1f);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"Enemy collided with {collision.gameObject.name}");

        if (collision.CompareTag("Player"))
        {
            Debug.Log($"Enemy dealt {damage} damage to player.");
            //PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
            //if (playerHealth != null)
            //{
            //    playerHealth.TakeDamage(damage);
            //    Debug.Log($"Enemy dealt {damage} damage to player.");
            //}
        }
    }

    private void ResetColor()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.white; 
        }
    }

    public void ResetHitFlag()
    {
        if (isAlive)
        {
            hasBeenHit = false;
        }
    }

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed; 
    }

    public int GetMaxHealth()
    {
        return maxHealth; 
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, separationDistance);
    }
}