using UnityEngine;

[System.Serializable]
public class Weapon
{
    public string name;
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float fireCooldownDuration;
    public int damage;
    public float range;
}