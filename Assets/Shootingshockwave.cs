using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shootingshockwave : MonoBehaviour
{
    public Transform player; // Tham chiếu đến người chơi
    public GameObject bulletPrefab; // Prefab của viên đạn hoặc đối tượng boss bắn ra
    public Transform shootPoint; // Vị trí bắn
    public float attackCooldown = 3f; // Thời gian hồi chiêu cho kỹ năng bắn
    public float bulletSpeed = 10f; // Tốc độ viên đạn

    private bool canShoot = true; // Kiểm tra xem boss có thể bắn không

    private Animator animator; // Animator để điều khiển animation

    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform; // Lấy người chơi nếu chưa gán
        }

        animator = GetComponent<Animator>(); // Lấy tham chiếu tới Animator
    }

    void Update()
    {
        // Kiểm tra xem boss có thể bắn được không và thực hiện bắn nếu có thể
        if (canShoot)
        {
            float attackChance = Random.Range(0f, 1f); // Tạo ra xác suất bắn

            if (attackChance <= 0.2f) // 20% xác suất bắn
            {
                StartCoroutine(ShootBulletAfterAnimation());
            }
        }
    }

    // Coroutine để đợi animation và sau đó bắn viên đạn
    IEnumerator ShootBulletAfterAnimation()
    {
        canShoot = false; // Ngừng bắn cho đến khi cooldown xong

        // Kích hoạt animation bắn (nếu có)
        animator.SetTrigger("Shoot");

        // Đợi cho đến khi animation bắn hoàn thành (sử dụng thời gian của animation)
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        // Tạo viên đạn tại vị trí bắn (shootPoint)
        GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);

        // Tính toán hướng bắn về phía người chơi
        Vector3 direction = (player.position - shootPoint.position).normalized;

        // Đưa viên đạn di chuyển về phía người chơi
        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
        bulletRb.velocity = direction * bulletSpeed;

        // Đợi một thời gian trước khi cho phép bắn lại
        yield return new WaitForSeconds(attackCooldown);

        canShoot = true; // Cho phép bắn lại
    }
}
