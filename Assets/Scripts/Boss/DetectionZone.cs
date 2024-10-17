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
        if (other.CompareTag("Player"))
        {
            boss.playerInzone = false;
            Debug.Log("Player left zone");
        }
    }
}