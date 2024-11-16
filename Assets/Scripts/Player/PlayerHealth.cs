using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using BarthaSzabolcs.Tutorial_SpriteFlash;

public class PlayerHealth : MonoBehaviour, IDataPersistence
{
    public static PlayerHealth Instance { get; private set; } // Singleton instance

    public float maxHealth = 100f;
    public float currentHealth;
    public int money = 0;
    public Image healthbar;

    public Image staminabar;
    public Animator animator;
    public GameObject Player;
    public TextMeshProUGUI healUsesText;

    private bool dead = false;
    private PlayerAttack playerAttack;
    [SerializeField] private SimpleFlash flash; // Reference to the SimpleFlash script
    [SerializeField] private TextMeshProUGUI moneyText;
    public int healUses = 5;
    public float healAmount = 20f;
    private bool isHurt = false;

    public float knockbackForce = 5f;
    public float hurtDuration = 1f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Destroy duplicate player instances
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Keep player across scenes
    }

    private void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthBar();
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
        UpdateHealthBar();
        if (currentHealth > 0)
        {
            animator.SetTrigger("hurt");
            playerAttack.InterruptAttack();
            playerAttack.DisableAttacking(1.5f);
            StartCoroutine(HandleKnockback());
            flash.Flash();
        }
        else if (!dead)
        {
            dead = true;
            animator.SetTrigger("die");
            StartCoroutine(Respawn());
        }

    }

    public void SavePlayerData(PlayerData playerData)
    {
        playerData.maxhealth = (int)maxHealth;
        playerData.money = money;
    }

    public void LoadPlayerData(PlayerData playerData)
    {
        maxHealth = playerData.maxhealth;
        currentHealth = maxHealth;
        money = playerData.money;
        UpdateHealthBar();
        updatemoney();
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

    private void UpdateHealthBar()
    {
        if (healthbar != null)
        {
            healthbar.fillAmount = currentHealth / maxHealth;
        }
    }


    public void UpdateHealUsesText()
    {
        if (healUsesText != null)
        {
            healUsesText.text = healUses.ToString();
        }
    }

    private IEnumerator Respawn()
    {
        // Đợi một chút để hoàn tất hoạt ảnh chết
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        // Reset trạng thái của người chơi
        dead = false;
        currentHealth = maxHealth;
        UpdateHealthBar();

        // Tải lại scene "Home"
        SceneManager.LoadScene("Home");

        // Bật lại Animator nếu đã tắt
        if (!animator.enabled)
        {
            animator.enabled = true;
        }
    }

    public void SetMaxHealth(float newMaxHealth)
    {
        maxHealth = newMaxHealth;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthBar();
    }
}
