using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public GameObject chestClose, chestOpen;
    public GameObject coinPrefab; // Reference to the coin prefab
    private bool isPlayerNear = false;
    private bool isOpened = false; // Flag to track if the chest has been opened
    public float spawnDelay = 0.1f; // Delay between spawning coins

    // Start is called before the first frame update
    void Start()
    {
        chestClose.SetActive(true);
        chestOpen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayerNear && Input.GetKeyDown(KeyCode.E) && !isOpened)
        {
            chestClose.SetActive(false);
            chestOpen.SetActive(true);
            StartCoroutine(DropCoinsWithDelay());
            isOpened = true; // Mark the chest as opened
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerNear = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerNear = false;
        }
    }

    private IEnumerator DropCoinsWithDelay()
    {
        if (coinPrefab != null)
        {
            int coinCount = Random.Range(3, 6); // Drop between 3 and 5 coins
            for (int i = 0; i < coinCount; i++)
            {
                GameObject coin = Instantiate(coinPrefab, transform.position, Quaternion.identity);
                Rigidbody2D rb = coin.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    // Apply a random force to the coin to make it "jump" in different directions
                    float randomX = Random.Range(-1f, 1f);
                    float randomY = Random.Range(1f, 2f);
                    rb.AddForce(new Vector2(randomX, randomY), ForceMode2D.Impulse);
                }
                yield return new WaitForSeconds(spawnDelay);
            }
        }
    }
}
