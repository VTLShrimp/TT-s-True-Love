using UnityEngine;

public class BossZoneTrigger : MonoBehaviour
{
    public BossActions bossActions; // Tham chiếu đến script BossActions

    void OnTriggerEnter2D(Collider2D other) // Sử dụng OnTriggerEnter2D thay vì OnTriggerEnter
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player has entered the zone");
            if (bossActions != null)
            {
                bossActions.EnableBossActions(); // Kích hoạt hành động của boss
            }
        }
    }

    void OnTriggerExit2D(Collider2D other) // Sử dụng OnTriggerExit2D thay vì OnTriggerExit
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player has exited the zone");
            if (bossActions != null)
            {
                bossActions.DisableBossActions(); // Dừng hành động của boss
            }
        }
    }
}
