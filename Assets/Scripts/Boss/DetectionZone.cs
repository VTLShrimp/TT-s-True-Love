using UnityEngine;
using Cinemachine;
using System.Collections;

public class DetectionZone : MonoBehaviour
{
    public Boss boss;
    public CinemachineVirtualCamera PlayerCam;
    public CinemachineVirtualCamera BossCam;
    public Transform player;
    public float focusDuration = 1.0f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            boss.playerInzone = true;
            Debug.Log("Player in zone");
            StartCoroutine(SwitchToBossCam());
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
                PlayerCam.Priority = 10;
                BossCam.Priority = 0;
            }
            else
            {
                Debug.LogError("Boss is not assigned!");
            }
        }
    }

    private IEnumerator SwitchToBossCam()
    {
        // Switch to BossCam
        PlayerCam.Priority = 0;
        BossCam.Priority = 10;

        // Wait for focusDuration
        yield return new WaitForSeconds(focusDuration);

        // Switch back to PlayerCam
        PlayerCam.Priority = 10;
        BossCam.Priority = 0;
    }
}
