using UnityEngine;
using TMPro;

public class UpgradeManager : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public PlayerAttack playerAttack;
    public GameObject upgradeMenu;
    public TextMeshProUGUI moneyText;

    // Upgrade costs
    public int healthUpgradeCost = 50;
    public int staminaUpgradeCost = 30;
    public int groundDmgUpgradeCost = 40;
    public int airDmgUpgradeCost = 40;
    public int manaUpgradeCost = 50;

    // References to UI Text elements for values and costs
    public TextMeshProUGUI groundDMGValueText;
    public TextMeshProUGUI airDMGValueText;
    public TextMeshProUGUI maxHealthValueText;
    public TextMeshProUGUI maxStaminaValueText;

    public TextMeshProUGUI groundDMGCostText;
    public TextMeshProUGUI airDMGCostText;
    public TextMeshProUGUI maxHealthCostText;
    public TextMeshProUGUI maxStaminaCostText;

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
            playerHealth.maxStamina += 10; // Increase max stamina

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
    public void ExitUpgradeMenu()
    {
        upgradeMenu.SetActive(false);
    }
    private void UpdateUI()
    {
        // Update money display
        moneyText.text = "Money: " + playerHealth.money;

        // Update stat values
        groundDMGValueText.text = playerAttack.groundDamage.ToString();
        airDMGValueText.text = playerAttack.airDamage.ToString();
        maxHealthValueText.text = playerHealth.maxHealth.ToString();
        maxStaminaValueText.text = playerHealth.maxStamina.ToString();

        // Update costs
        groundDMGCostText.text = groundDmgUpgradeCost.ToString();
        airDMGCostText.text = airDmgUpgradeCost.ToString();
        maxHealthCostText.text = healthUpgradeCost.ToString();
        maxStaminaCostText.text = staminaUpgradeCost.ToString();
    }
}
