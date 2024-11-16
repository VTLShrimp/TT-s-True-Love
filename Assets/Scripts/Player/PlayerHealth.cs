using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using BarthaSzabolcs.Tutorial_SpriteFlash;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;
    public int money = 0;
    public Image healthbar;
    public float maxStamina = 100f;
    public float currentStamina;

    public int maxMana = 100;
    public int currentMana;
    public Image staminabar;
    public Animator animator;
    public GameObject Player;
    public TextMeshProUGUI healUsesText;

    private bool dead = false;
    private PlayerAttack playerAttack;
    [SerializeField] private SimpleFlash flash; // Reference to the SimpleFlash script
    [SerializeField] private TextMeshProUGUI moneyText;
    private int healUses = 5;
    private float healAmount = 20f;
    private bool isHurt = false;

    public float knockbackForce = 5f;
    public float hurtDuration = 1f;

    private void Start()
    {
        currentMana = maxMana;
        currentHealth = maxHealth;
        currentStamina = maxStamina;
        DontDestroyOnLoad(gameObject);
        UpdateHealthBar();
        UpdateStaminaBar();
        UpdateHealUsesText();
        updatemoney();
        playerAttack = GetComponent<PlayerAttack>();
        flash = GetComponent<SimpleFlash>(); // Ensure the SimpleFlash component is assigned
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha6) && healUses > 0)
        {
            Heal(healAmount);
            healUses--;
            Debug.Log("Used healing, remaining: " + healUses + " uses");
            UpdateHealUsesText();
        }
        updatemoney();
    }
    public void AddMoney(int amount)
    {
        money += amount;
        moneyText.text = money.ToString();
    }
    private void updatemoney()
    {
        moneyText.text = money.ToString();
    }
    public void TakeDamage(float damage)
    {
        Debug.Log("Player is taking damage: " + damage);  // In ra log khi người chơi nhận sát thương

        if (dead || isHurt) return;  // Tránh sát thương nếu đã chết hoặc đang bị thương

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (currentHealth > 0)
        {
            animator.SetTrigger("hurt");
            playerAttack.InterruptAttack(); // Ngừng tấn công khi bị thương
            playerAttack.DisableAttacking(1.5f); // Ngừng tấn công trong 1.5 giây
            StartCoroutine(HandleKnockback());
            flash.Flash();  // Gọi hiệu ứng flash khi bị thương
        }
        else if (!dead)
        {
            dead = true;
            animator.SetTrigger("die");
            Respanwn();
            StartCoroutine(DisableAnimatorAndDestroy());
        }

        UpdateHealthBar();
    }


    private IEnumerator HandleKnockback()
    {
        isHurt = true;
        Vector3 knockbackDirection = (transform.position - Camera.main.transform.position).normalized;
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(knockbackDirection * knockbackForce, ForceMode.Impulse);
        }

        yield return new WaitForSeconds(hurtDuration);
        isHurt = false;
    }

    public void Heal(float healingAmount)
    {
        if (dead) return;

        currentHealth += healingAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthBar();
    }

    public void ConsumeStamina(float amount)
    {
        currentStamina -= amount;
        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
        UpdateStaminaBar();
    }

    public void RegenerateStamina(float amount)
    {
        currentStamina += amount;
        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
        UpdateStaminaBar();
    }

    private void UpdateHealthBar()
    {
        if (healthbar != null)
        {
            healthbar.fillAmount = currentHealth / maxHealth;
        }
    }

    private void UpdateStaminaBar()
    {
        if (staminabar != null)
        {
            staminabar.fillAmount = currentStamina / maxStamina;
        }
    }

    private void UpdateHealUsesText()
    {
        if (healUsesText != null)
        {
            healUsesText.text = healUses.ToString();
        }
    }

    private IEnumerator DisableAnimatorAndDestroy()
    {
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        GetComponent<Animator>().enabled = false;
        Destroy(Player);
    }

    public void SetMaxHealth(float newMaxHealth)
    {
        maxHealth = newMaxHealth;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthBar();
    }

    void Respanwn()
    {
        SceneManager.LoadScene("Home");
    }
}
