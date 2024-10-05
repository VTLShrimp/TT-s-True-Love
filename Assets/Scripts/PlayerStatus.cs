using UnityEngine;
using UnityEngine.UI;

public class PlayerStatus : MonoBehaviour
{
    public Image healthbar; // UI Image that represents the health bar
    public float healthAmount = 100f;
    public GameObject Player;

    private void Start() {
        healthAmount = 100;
    }
    private void Update() 
    {
        // Nhấn phím H để trừ máu
        if (Input.GetKeyDown(KeyCode.H))
        {
            TakeDamage(20);
        }

        // Nếu máu bằng 0 thì hủy đối tượng
        if (healthAmount == 0) 
        {
            Destroy(Player); // Hủy đối tượng Player khi hết máu
        }
    }

    public void TakeDamage(float damage)
    {
        // Giảm máu và giới hạn giá trị từ 0 đến 100
        healthAmount -= damage;
        healthAmount = Mathf.Clamp(healthAmount, 0, 100);
        UpdateHealthBar();
    }

    public void Heal(float healingAmount)
    {
        // Tăng máu và giới hạn giá trị từ 0 đến 100
        healthAmount += healingAmount;
        healthAmount = Mathf.Clamp(healthAmount, 0, 100);
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        // Cập nhật thanh máu dựa trên giá trị hiện tại của máu
        healthbar.fillAmount = healthAmount / 100f;
    }

    // Thiết lập lại giá trị máu tối đa
    public void SetMaxHealth(int maxHealth)
    {
        healthAmount = maxHealth;
        healthAmount = Mathf.Clamp(healthAmount, 0, 100);
        UpdateHealthBar();
    }
}
