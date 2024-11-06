using UnityEngine;
using Cinemachine;
using System.Collections; // Thêm dòng này để sử dụng IEnumerator

public class CameraSwitch : MonoBehaviour
{
    public CinemachineVirtualCamera PlayerCam;
    public CinemachineVirtualCamera BossCam;
    public Transform player;
    public Transform boss;
    public float focusDuration = 1.0f;

    private bool isInZone = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.transform == player)
        {
            isInZone = true;
            StartCoroutine(SwitchToBossCam());
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.transform == player)
        {
            isInZone = false;
            PlayerCam.Priority = 10;
            BossCam.Priority = 0;
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
