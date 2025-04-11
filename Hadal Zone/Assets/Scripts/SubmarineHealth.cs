using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SubmarineHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 20;
    public int currentHealth;

    public Slider healthBar;


    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthBar();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);
        UpdateHealthBar();


        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void IncreaseHealth(int health)
    {
        currentHealth += health;
        currentHealth = Mathf.Min(currentHealth, maxHealth);
        UpdateHealthBar(); 
    }

    private void UpdateHealthBar()
    {
        healthBar.value = currentHealth;
    }

    private void Die()
    {
        Debug.Log("Submarine sunk!");
        gameObject.SetActive(false);
    }
}
