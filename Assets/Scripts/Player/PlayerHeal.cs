using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private float currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
        // Không cần cập nhật UI nữa
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log("Player took damage! Current health: " + currentHealth);

        // Không cần cập nhật UI nữa

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Hàm xử lý khi nhân vật chết
    private void Die()
    {
        Debug.Log("Player died!");
        // Thoát game sau khi nhân vật chết
        QuitGame();
    }

    // Hàm thoát game
    void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
