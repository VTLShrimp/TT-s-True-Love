using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ReSpawn : MonoBehaviour
{
    Vector2 starPos;

    private void Start()
    {
        starPos = transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Die();
        }
    }

    void Die()
    {
        Respawn();
    }

    public void Respawn()
    {
        transform.position = starPos;
    }
}
