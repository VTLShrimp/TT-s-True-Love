using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;      // Máu tối đa của nhân vật
    public float currentHealth;         // Máu hiện tại của nhân vật
    public Image healthbar;             // UI Image để hiển thị thanh máu
    public Animator animator;           // Animator để xử lý các hoạt ảnh
    public GameObject Player;           // Đối tượng Player trong game
    private bool dead = false;          // Trạng thái chết của nhân vật

    private void Start()
    {
        currentHealth = maxHealth;      // Khởi tạo máu ban đầu là máu tối đa
        Debug.Log("Máu ban đầu của nhân vật: " + currentHealth); // Log kiểm tra máu ban đầu
        UpdateHealthBar();              // Cập nhật thanh máu khi bắt đầu game
    }

    private void Update()
    {
        // Logic update khác nếu cần
    }

    public void TakeDamage(float damage)
    {
        if (dead) return;               // Nếu đã chết, không tiếp tục nhận sát thương

        currentHealth -= damage;        // Trừ sát thương vào máu hiện tại
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Giới hạn máu từ 0 đến maxHealth

        if (currentHealth > 0)
        {
            animator.SetTrigger("hurt"); // Kích hoạt animation bị thương
        }
        else if (!dead)
        {
            dead = true;
            animator.SetTrigger("die");  // Kích hoạt animation chết
            StartCoroutine(DisableAnimatorAndDestroy());
        }

        UpdateHealthBar();              // Cập nhật thanh máu sau khi nhận sát thương
    }

    public void Heal(float healingAmount)
    {
        if (dead) return;               // Nếu đã chết, không thể hồi máu

        currentHealth += healingAmount; // Tăng máu khi hồi máu
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Giới hạn máu từ 0 đến maxHealth

        UpdateHealthBar();              // Cập nhật thanh máu sau khi hồi máu
    }

    private void UpdateHealthBar()
    {
        // Cập nhật thanh máu dựa trên giá trị hiện tại của máu
        if (healthbar != null)
        {
            healthbar.fillAmount = currentHealth / maxHealth;
        }
    }
  
    private IEnumerator DisableAnimatorAndDestroy()
    {
        // Chờ cho đến khi hoạt ảnh "die" hoàn thành
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        // Sau khi hoạt ảnh hoàn thành, vô hiệu hóa Animator
        GetComponent<Animator>().enabled = false;

        // Xóa đối tượng Player khỏi game
        Destroy(Player);
    }

    // Thiết lập lại giá trị máu tối đa nếu cần thiết
    public void SetMaxHealth(float newMaxHealth)
    {
        maxHealth = newMaxHealth;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthBar();              // Cập nhật thanh máu
    }

    void Respanwn()
    {
        SceneManager.LoadScene("Home");
    }

}