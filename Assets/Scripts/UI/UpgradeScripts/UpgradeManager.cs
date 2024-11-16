using UnityEngine;
using TMPro;

public class UpgradeManager : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public PlayerAttack playerAttack;
    public StaminaBar staminaBar;
    public GameObject forgemenu; // Panel for groundDMG, airDMG, maxHealth, maxStamina
    public GameObject priestessmenu; // Panel for the rest of the upgrades

    public TextMeshProUGUI moneyText;

    // Upgrade costs
    public int healthUpgradeCost = 50;
    public int staminaUpgradeCost = 30;
    public int groundDmgUpgradeCost = 40;
    public int airDmgUpgradeCost = 40;
    public int maxManaUpgradeCost = 50;
    public int staminaRecoveryRateCost = 50;
    public int levelPotionCost = 50;
    public int swordWaveDamageCost = 50;
    public int potionLevel = 1;
    public int maxPotionLevel = 5;
    public float healIncreasePerLevel = 5f;


    // References to UI Text elements for values and costs
    public TextMeshProUGUI groundDMGValueText;
    public TextMeshProUGUI airDMGValueText;
    public TextMeshProUGUI maxHealthValueText;
    public TextMeshProUGUI maxStaminaValueText;
    public TextMeshProUGUI maxManaValueText;
    public TextMeshProUGUI staminaRecoveryRateValueText;
    public TextMeshProUGUI levelPotionValueText;
    public TextMeshProUGUI swordWaveDamageValueText;

    public TextMeshProUGUI groundDMGCostText;
    public TextMeshProUGUI airDMGCostText;
    public TextMeshProUGUI maxHealthCostText;
    public TextMeshProUGUI maxStaminaCostText;
    public TextMeshProUGUI maxManaCostText;
    public TextMeshProUGUI staminaRecoveryRateCostText;
    public TextMeshProUGUI levelPotionCostText;
    public TextMeshProUGUI swordWaveDamageCostText;

    private void Start()
    {
        UpdateUI();
    }

    public void UpgradeHealth()
    {
        if (playerHealth.money >= healthUpgradeCost)
        {
            playerHealth.money -= healthUpgradeCost;
            playerHealth.maxHealth += 10; // Increase max health
            playerHealth.SetMaxHealth(playerHealth.maxHealth); // Update health to new max
            UpdateUI();
        }
        else
        {
            Debug.Log("Not enough money to upgrade health.");
        }
    }

    public void UpgradeStamina()
    {
        if (playerHealth.money >= staminaUpgradeCost)
        {
            playerHealth.money -= staminaUpgradeCost;
            staminaBar.maxStamina += 10; // Increase max stamina

            UpdateUI();
        }
        else
        {
            Debug.Log("Not enough money to upgrade stamina.");
        }
    }

    public void UpgradeGroundDamage()
    {
        if (playerHealth.money >= groundDmgUpgradeCost)
        {
            playerHealth.money -= groundDmgUpgradeCost;
            playerAttack.groundDamage += 5; // Increase ground damage

            UpdateUI();
        }
        else
        {
            Debug.Log("Not enough money to upgrade ground damage.");
        }
    }

    public void UpgradeAirDamage()
    {
        if (playerHealth.money >= airDmgUpgradeCost)
        {
            playerHealth.money -= airDmgUpgradeCost;
            playerAttack.airDamage += 5; // Increase air damage

            UpdateUI();
        }
        else
        {
            Debug.Log("Not enough money to upgrade air damage.");
        }
    }
    public void UpgradeMaxMana()
    {
        if (playerHealth.money >= maxManaUpgradeCost)
        {
            playerHealth.money -= maxManaUpgradeCost;
            playerAttack.maxMana += 10; // Increase max mana

            UpdateUI();
        }
        else
        {
            Debug.Log("Not enough money to upgrade max mana.");
        }
    }
    public void UpgradeStaminaRecoveryRate()
    {
        if (playerHealth.money >= staminaRecoveryRateCost)
        {
            playerHealth.money -= staminaRecoveryRateCost;
            staminaBar.staminaRegenRate += 1; // Increase stamina recovery rate

            UpdateUI();
        }
        else
        {
            Debug.Log("Not enough money to upgrade stamina recovery rate.");
        }
    }

    public void UpgradeLevelPotion()
    {
        if (playerHealth.money >= levelPotionCost && potionLevel < maxPotionLevel)
        {
            playerHealth.money -= levelPotionCost;
            potionLevel++; // Tăng potion level
            playerHealth.healAmount += healIncreasePerLevel; // Tăng lượng heal mỗi cấp

            playerHealth.healUses = potionLevel; // Cập nhật số lần heal dựa trên potion level

            UpdateUI();
            playerHealth.UpdateHealUsesText(); // Cập nhật UI hiển thị số lần heal
        }
        else if (potionLevel >= maxPotionLevel)
        {
            Debug.Log("Potion is already at max level.");
        }
        else
        {
            Debug.Log("Not enough money to upgrade potion level.");
        }
    }


    public void UpgradeSwordWaveDamage()
    {
        if (playerHealth.money >= swordWaveDamageCost)
        {
            playerHealth.money -= swordWaveDamageCost;
            playerAttack.swordWaveDamage += 5; // Increase sword wave damage

            UpdateUI();
        }
        else
        {
            Debug.Log("Not enough money to upgrade sword wave damage.");
        }
    }

    public void ExitForgeMenu()
    {
        forgemenu.SetActive(false);

    }
    public void ExitPriestessMenu()
    {
        priestessmenu.SetActive(false);

    }
    private void UpdateUI()
    {
        // Update money display
        moneyText.text = "Money: " + playerHealth.money;

        // Update stat values
        groundDMGValueText.text = playerAttack.groundDamage.ToString();
        airDMGValueText.text = playerAttack.airDamage.ToString();
        maxHealthValueText.text = playerHealth.maxHealth.ToString();
        maxStaminaValueText.text = staminaBar.maxStamina.ToString();
        maxManaValueText.text = playerAttack.maxMana.ToString();
        staminaRecoveryRateValueText.text = staminaBar.staminaRegenRate.ToString();
        levelPotionValueText.text = $"Lv {potionLevel}"; // Display potion level and healing amount
        swordWaveDamageValueText.text = playerAttack.swordWaveDamage.ToString();

        // Update costs
        groundDMGCostText.text = groundDmgUpgradeCost.ToString();
        airDMGCostText.text = airDmgUpgradeCost.ToString();
        maxHealthCostText.text = healthUpgradeCost.ToString();
        maxStaminaCostText.text = staminaUpgradeCost.ToString();
        maxManaCostText.text = maxManaUpgradeCost.ToString();
        staminaRecoveryRateCostText.text = staminaRecoveryRateCost.ToString();
        levelPotionCostText.text = levelPotionCost.ToString();
        swordWaveDamageCostText.text = swordWaveDamageCost.ToString();
    }


}
