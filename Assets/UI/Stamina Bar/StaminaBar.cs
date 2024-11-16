using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour, IDataPersistence
{
    public float maxStamina = 100f;      // Thể lực tối đa
    public float currentStamina;         // Thể lực hiện tại
    public Image staminaBar;             // UI Image để hiển thị thanh thể lực
    public float staminaRegenRate = 10f;  // Tốc độ hồi thể lực mỗi giây
    public float jumpStaminaCost = 5f;  // Thể lực tiêu hao khi nhảy
    public float dashStaminaCost = 10f;  // Thể lực tiêu hao khi lướt
    public float dodgeStaminaCost = 50f; // Thể lực tiêu hao khi né
    // Start is called before the first frame update
    private void Start()
    {
        currentStamina = maxStamina;     // Khởi tạo thể lực ban đầu là thể lực tối đa
        UpdateStaminaBar();              // Cập nhật thanh thể lực khi bắt đầu game
    }

    private void Update()
    {
        RegenerateStamina();             // Hồi thể lực mỗi khung hình
        UpdateStaminaBar();              // Cập nhật thanh thể lực
    }

    public void UseStamina(float amount)
    {
        currentStamina -= amount;        // Giảm thể lực
        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina); // Giới hạn thể lực từ 0 đến maxStamina
        UpdateStaminaBar();              // Cập nhật thanh thể lực sau khi sử dụng
    }

    private void RegenerateStamina()
    {
        if (currentStamina < maxStamina)
        {
            currentStamina += staminaRegenRate * Time.deltaTime; // Hồi thể lực theo thời gian
            currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina); // Giới hạn thể lực từ 0 đến maxStamina
        }
    }

    private void UpdateStaminaBar()
    {
        // Cập nhật thanh thể lực dựa trên giá trị hiện tại của thể lực
        if (staminaBar != null)
        {
            staminaBar.fillAmount = currentStamina / maxStamina;
        }
    }
    public void SavePlayerData(PlayerData playerData)
    {
        playerData.maxStamina = (int)maxStamina;
        playerData.staminaRegenRate = (int)staminaRegenRate;
    }
    public void LoadPlayerData(PlayerData playerData)
    {
        maxStamina = playerData.maxStamina;
        staminaRegenRate = playerData.staminaRegenRate;
    }
}
