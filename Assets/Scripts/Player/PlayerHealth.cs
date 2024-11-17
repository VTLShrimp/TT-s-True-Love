using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using BarthaSzabolcs.Tutorial_SpriteFlash;

public class PlayerHealth : MonoBehaviour, IDataPersistence
{
    public static PlayerHealth Instance { get; private set; }

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
    [SerializeField] private SimpleFlash flash;
    [SerializeField] private TextMeshProUGUI moneyText;
    public int healUses = 5;
    public float healAmount = 20f;
    private bool isHurt = false;

    // Audio clips for damage and death
    public AudioClip damageSound;
    public AudioClip deathSound;
    private AudioSource audioSource;

    public float knockbackForce = 5f;
    public float hurtDuration = 1f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthBar();
        UpdateHealUsesText();
        UpdateMoney();

        playerAttack = GetComponent<PlayerAttack>();
        if (playerAttack == null)
        {
            Debug.LogError("PlayerAttack component is missing on this GameObject.");
        }

        flash = GetComponent<SimpleFlash>();
        if (flash == null)
        {
            Debug.LogError("SimpleFlash component is missing or not assigned.");
        }

        if (moneyText == null)
        {
            moneyText = GameObject.Find("MoneyText").GetComponent<TextMeshProUGUI>();
            if (moneyText == null)
            {
                Debug.LogError("MoneyText object not found in the scene.");
            }
        }

        // Initialize AudioSource and check if the clips are assigned
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
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

        UpdateMoney();
    }

    public void AddMoney(int amount)
    {
        money += amount;
        UpdateMoney();
    }

    private void UpdateMoney()
    {
        if (moneyText != null)
        {
            moneyText.text = money.ToString();
        }
    }

    public void TakeDamage(float damage)
    {
        if (dead || isHurt) return;

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthBar();

        if (currentHealth > 0)
        {
            animator.SetTrigger("hurt");

            // Play damage sound
            if (audioSource != null && damageSound != null)
            {
                audioSource.PlayOneShot(damageSound);
            }

            StartCoroutine(DisableAttackInput(1.5f));
            StartCoroutine(HandleKnockback());
            flash.Flash();
        }
        else if (!dead)
        {
            dead = true;
            animator.SetTrigger("die");
            // Play death sound
            if (audioSource != null && deathSound != null)
            {
                audioSource.PlayOneShot(deathSound);
            }
            StartCoroutine(Respawn());
        }
    }

    private IEnumerator DisableAttackInput(float duration)
    {
        playerAttack.enabled = false;
        yield return new WaitForSeconds(duration);
        playerAttack.enabled = true;
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
        UpdateMoney();
    }

    private IEnumerator HandleKnockback()
    {
        isHurt = true;
        Vector3 knockbackDirection = (transform.position - Camera.main.transform.position).normalized;
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
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
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        dead = false;
        currentHealth = maxHealth;
        UpdateHealthBar();

        SceneManager.LoadScene("Home");

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
