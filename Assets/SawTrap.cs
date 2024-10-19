using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawTrap : MonoBehaviour
{
    public int damage = 10; // Damage dealt by the trap

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
                Debug.Log("Player took " + damage + " damage from the saw trap!");
            }
            Debug.Log("Player took " + damage + " damage from the saw trap!");
        }
    }
}