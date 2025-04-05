using UnityEngine;

public class FishEnemy : MonoBehaviour
{
    [Header("Enemy Stats")]
    public int baseHealth = 20; 
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
    private int maxHealth; 
    private int currentHealth;
    private bool hasBeenHit = false;
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
        currentHealth = baseHealth;

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
        //DisableColliders();
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


    public int GetMaxHealth()
    {
        return maxHealth; 
    }
}