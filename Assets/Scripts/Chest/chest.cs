using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public GameObject chestClose, chestOpen;
    public GameObject coinPrefab; // Reference to the coin prefab
    private bool isPlayerNear = false;

    // Start is called before the first frame update
    void Start()
    {
        chestClose.SetActive(true);
        chestOpen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayerNear && Input.GetKeyDown(KeyCode.E))
        {
            chestClose.SetActive(false);
            chestOpen.SetActive(true);
            DropCoin();
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
            chestClose.SetActive(false);
            chestOpen.SetActive(true);
        }
    }

    void DropCoin()
    {
        // Instantiate the coin at the chest's position
        Instantiate(coinPrefab, transform.position, Quaternion.identity);
    }
}
