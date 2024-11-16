using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class PlayerData
{
    public int money;
    public int maxhealth;
    public int groundDMG;
    public int airDMG;
    public int maxMana;
    public int maxStamina;
    public int swordWaveDMG;
    public int levelPotion;
    public int staminaRegenRate;
    public PlayerData()
    {
        this.money = 0;
        this.maxhealth = 100;
        this.groundDMG = 10;
        this.airDMG = 10;
        this.maxMana = 100;
        this.maxStamina = 100;
        this.swordWaveDMG = 10;
        this.levelPotion = 0;
        this.staminaRegenRate = 1;
    }
}
