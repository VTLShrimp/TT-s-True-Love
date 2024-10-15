using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FallingPlatform : MonoBehaviour
{
  [SerializeField]  private float fallDelay = 0.1f;
  [SerializeField] private float fallTime = 0.1f;
    private float respawnDelay = 3f; // Thời gian cho nền xuất hiện lại
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private bool isFalling = false;

    [SerializeField] private Rigidbody2D rb;
    private TilemapCollider2D tilemapCollider;

    private void Start()
    {
        // Lưu lại vị trí và góc xoay ban đầu của nền
        originalPosition = transform.position;
        originalRotation = transform.rotation;
        tilemapCollider = GetComponent<TilemapCollider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isFalling)
        {
            StartCoroutine(Fall());
        }
    }

    private IEnumerator Fall()
    {
        isFalling = true;
        yield return new WaitForSeconds(fallDelay);

        // Làm nền rơi
        rb.bodyType = RigidbodyType2D.Dynamic;

        yield return new WaitForSeconds(fallTime);

        // Ngừng va chạm để tránh tác động
        tilemapCollider.enabled = false;

        // Đợi trước khi đặt lại nền
        yield return new WaitForSeconds(respawnDelay);

        // Đặt lại nền và Rigidbody
        ResetPlatform();
    }

    private void ResetPlatform()
    {
        // Đặt lại vị trí và góc xoay
        transform.position = originalPosition;
        transform.rotation = originalRotation;

        // Đặt lại Rigidbody2D về trạng thái Static
        rb.bodyType = RigidbodyType2D.Static;

        // Bật lại va chạm
        tilemapCollider.enabled = true;

        // Đặt lại trạng thái rơi
        isFalling = false;
    }
}
