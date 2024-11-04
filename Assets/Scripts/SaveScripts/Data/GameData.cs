using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int maxhealth;
    public int atackDamage;/// <summary>
    public int maxMagicdame;

    public int maxmana;
    public int maxStamina;
    public int money;
    public SerializableDictionary<string, EnemyData> enemy;

    public GameData()
    {
        this.maxhealth = 100;
        this.atackDamage = 10;
        this.money = 0;
        this.enemy = new SerializableDictionary<string, EnemyData>();
    }
}
