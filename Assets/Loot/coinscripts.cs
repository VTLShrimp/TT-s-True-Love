using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinScripts : MonoBehaviour
{
    private Rigidbody2D rb;
    private PlayerHealth playerHealth;
    private int coinValue;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Ensure the Rigidbody2D component is present
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
        }
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            playerHealth = player.GetComponent<PlayerHealth>();
        }
        else
        {
            Debug.LogError("Player object not found.");
        }
        // Apply an upward force to make the coin "jump"
        rb.AddForce(new Vector2(0, 5), ForceMode2D.Impulse);
        coinValue = Random.Range(10, 51);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            // Change Rigidbody2D to kinematic when it hits the ground
            rb.isKinematic = true;
            rb.velocity = Vector2.zero; // Stop any remaining movement
            Debug.Log("Coin hit the ground.");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Coin collected!");
            playerHealth.money += coinValue;
            Destroy(gameObject);
        }
        if (collision.gameObject.CompareTag("Ground"))
        {
            // Change Rigidbody2D to kinematic when it hits the ground
            rb.isKinematic = true;
            rb.velocity = Vector2.zero; // Stop any remaining movement
            Debug.Log("Coin hit the ground.");
        }
    }
}