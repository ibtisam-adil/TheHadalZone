using UnityEngine;

public class FishEnemy : MonoBehaviour
{
    [Header("Enemy Stats")]
    public int baseHealth = 20; 
    public int damage = 5;

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

 
    }

    private void Start()
    {
        currentHealth = baseHealth;

        FindSubmarineReference();

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
            Invoke(nameof(ResetHitFlag), 0.1f);
        }
    }

    private void Die()
    {
        if (!isAlive) return;
        isAlive = false;

        DisableColliders();
        enabled = false; 
        Destroy(gameObject, 0.1f);
    }

    private void DisableColliders()
    {
        foreach (Collider2D collider in colliders)
        {
            collider.enabled = false;
        }
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
                Invoke(nameof(FindSubmarineReference), 1f);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"Enemy collided with {collision.gameObject.name}");

        if (collision.CompareTag("Submarine"))
        {
            SubmarineHealth submarineHealth = collision.GetComponent<SubmarineHealth>();
            if (submarineHealth != null)
            {
                submarineHealth.TakeDamage(damage);
            }
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