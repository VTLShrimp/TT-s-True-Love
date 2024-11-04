using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int maxhealth;
    public int atackDamage;
    public int money;
    public int currentHealth;
    public Vector3 playerPosition;
    public SerializableDictionary<string, EnemyData> enemy;

    public GameData()
    {
        this.maxhealth = 100;
        this.atackDamage = 10;
        this.money = 0;
        this.currentHealth = 100;
        this.playerPosition = new Vector3(8, 2, 0);
        this.enemy = new SerializableDictionary<string, EnemyData>();
    }
}
