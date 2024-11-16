using Cinemachine;
using System.Collections;
using UnityEngine;

public class BossActions : MonoBehaviour
{
    public Transform player; // Tham chiếu đến người chơi
    public float jumpSpeed = 15f; // Tốc độ nhảy
    public float jumpHeight = 5f; // Độ cao nhảy
    public float jumpCooldown = 2f; // Thời gian hồi chiêu nhảy
    public float attackCooldown = 3f; // Thời gian hồi chiêu cho kỹ năng bắn
    public float bulletSpeed = 10f; // Tốc độ viên đạn
    public GameObject bulletPrefab; // Prefab của viên đạn
    public Transform shootPoint; // Vị trí bắn

    private bool isJumping = false; // Kiểm tra xem boss có đang nhảy không
    private bool canJump = true; // Kiểm tra boss có thể nhảy lại không
    private bool canShoot = true; // Kiểm tra boss có thể bắn không
    private bool canAct = false; // Kiểm tra xem boss có thể hành động không (khi vào zone)

    private int jumpCount = 0; // Đếm số lần nhảy
    private Animator animator; // Animator để điều khiển animation

    // AudioSource and AudioClip references
    public AudioSource audioSource;
    public AudioClip jumpSound; // Sound for jumping
    public AudioClip shootSound; // Sound for shooting
    public AudioClip landSound; // Sound for landing

    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform; // Lấy người chơi nếu chưa gán
        }

        animator = GetComponent<Animator>(); // Lấy tham chiếu tới Animator
        audioSource = GetComponent<AudioSource>(); // Get the AudioSource component
    }

    void Update()
    {
        if (!canAct) return; // Nếu không thể hành động, dừng lại

        // Kiểm tra xem boss có thể nhảy được không
        if (!isJumping && canJump)
        {
            if (jumpCount < 3)
            {
                float jumpChance = Random.Range(0f, 1f); // Xác suất nhảy
                if (jumpChance <= 0.2f) // 20% xác suất nhảy
                {
                    StartCoroutine(JumpToPlayer());
                }
            }
            else
            {
                // Sau khi nhảy 3 lần, boss sẽ chuyển sang bắn
                if (canShoot)
                {
                    StartCoroutine(ShootBulletAfterAnimation());
                }
            }
        }

        // Quay boss về hướng người chơi liên tục
        Flip();
    }

    // Coroutine để thực hiện nhảy
    IEnumerator JumpToPlayer()
    {
        isJumping = true;

        // Kích hoạt animation nhảy
        animator.SetTrigger("Jump");

        // Play jump sound
        if (jumpSound != null)
        {
            audioSource.PlayOneShot(jumpSound);
        }

        // Tính toán vị trí đích mà boss cần nhảy tới
        Vector3 targetPosition = new Vector3(player.position.x, transform.position.y, player.position.z);

        // Lưu lại vị trí hiện tại để nhảy
        Vector3 startPosition = transform.position;

        float elapsedTime = 0f;
        while (elapsedTime < jumpCooldown)
        {
            elapsedTime += Time.deltaTime;

            // Tính độ cao của cú nhảy
            float height = Mathf.Sin(Mathf.PI * (elapsedTime / jumpCooldown)) * jumpHeight;

            // Lerp cho vị trí X và Z
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / jumpCooldown);

            // Cập nhật độ cao cho trục Y mà không ảnh hưởng đến X và Z
            transform.position = new Vector3(transform.position.x, startPosition.y + height, transform.position.z);

            yield return null;
        }

        // Đảm bảo boss đến được vị trí chính xác
        transform.position = new Vector3(targetPosition.x, transform.position.y, targetPosition.z);

        // Kích hoạt animation khi boss chạm đất
        animator.SetTrigger("Land");

        // Play landing sound
        if (landSound != null)
        {
            audioSource.PlayOneShot(landSound);
        }

        // Kích hoạt hiệu ứng rung khi chạm đất
        CinemachineImpulseSource impulseSource = GetComponent<CinemachineImpulseSource>();
        if (impulseSource != null)
        {
            impulseSource.GenerateImpulse();
        }

        // Chờ một khoảng thời gian để boss dừng lại
        yield return new WaitForSeconds(1.5f);

        // Tăng số lần nhảy
        jumpCount++;

        // Cho phép boss nhảy lại sau khi dừng
        isJumping = false;
        canJump = true;
    }

    // Coroutine để đợi animation và sau đó bắn viên đạn
    IEnumerator ShootBulletAfterAnimation()
    {
        canShoot = false; // Dừng bắn cho đến khi hồi chiêu

        // Kích hoạt animation bắn
        animator.SetTrigger("Shoot");

        // Play shoot sound
        if (shootSound != null)
        {
            audioSource.PlayOneShot(shootSound);
        }

        // Đợi cho đến khi animation bắn kết thúc
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        float animationLength = stateInfo.length;
        float animationTime = -0.3f;

        while (animationTime < animationLength)
        {
            animationTime += Time.deltaTime; // Cộng dồn thời gian
            yield return null;
        }

        // Tạo viên đạn tại vị trí bắn
        GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
        if (bulletRb != null)
        {
            // Determine the direction based on the boss's local scale
            float direction = transform.localScale.x > 0 ? 1f : -1f;
            bulletRb.velocity = Vector2.right * direction * bulletSpeed; // Fire to the right if facing right, to the left if facing left
        }

        // Đợi trước khi cho phép bắn lại
        yield return new WaitForSeconds(attackCooldown);

        canShoot = true; // Cho phép bắn lại

        // Reset jump count sau khi bắn
        jumpCount = 0;
    }

    // Phương thức để quay boss về phía người chơi
    private void Flip()
    {
        if (player.position.x > transform.position.x && transform.localScale.x < 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (player.position.x < transform.position.x && transform.localScale.x > 0)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }

    // Kích hoạt hành động của boss
    public void EnableBossActions()
    {
        canAct = true;
        Debug.Log("Boss actions enabled");
    }

    // Dừng hành động của boss
    public void DisableBossActions()
    {
        canAct = false;
        Debug.Log("Boss actions disabled");
    }
}
