using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image healthbar; // UI Image that represents the health bar
    public float healthAmount = 100f;

    public void TakeDamage(float damage)
    {
        // Subtract damage and clamp the healthAmount so it doesn't drop below 0
        healthAmount -= damage;
        healthAmount = Mathf.Clamp(healthAmount, 0, 100);
        UpdateHealthBar();
    }

    public void Heal(float healingAmount)
    {
        // Add healing and clamp healthAmount so it doesn't exceed 100
        healthAmount += healingAmount;
        healthAmount = Mathf.Clamp(healthAmount, 0, 100);
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        // Update the health bar fill amount based on the current health amount
        healthbar.fillAmount = healthAmount / 100f;
    }

    // Sets the max health and updates the health bar UI
    public void SetMaxHealth(int maxHealth)
    {
        healthAmount = maxHealth;
        healthAmount = Mathf.Clamp(healthAmount, 0, 100);
        UpdateHealthBar();
    }
}
