using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class EnemyData
{
    public float maxHealth;
    public float currentHealth;
    public Vector3 enemyPosition;
    public bool isDead;
    public EnemyData()
    {
        this.maxHealth = 100;
        this.currentHealth = 100;
        this.enemyPosition = new Vector3(0, 0, 0);
        this.isDead = false;
    }

    public EnemyData(float maxHealth, float currentHealth, Vector3 enemyPosition, bool isDead)
    {
        this.maxHealth = maxHealth;
        this.currentHealth = currentHealth;
        this.enemyPosition = enemyPosition;
        this.isDead = isDead;
    }
}
