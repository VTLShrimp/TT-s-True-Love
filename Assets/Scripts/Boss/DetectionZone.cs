using UnityEngine;

public class DetectionZone : MonoBehaviour
{
    public Boss boss;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            boss.playerInzone = true;
            Debug.Log("Player in zone");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("OnTriggerExit2D called");
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player left the zone");
            if (boss != null)
            {
                boss.playerInzone = false;
                Debug.Log("Player in zone set to false");
            }
            else
            {
                Debug.LogError("Boss is not assigned!");
            }
        }
    }
}
