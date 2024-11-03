using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coinscripts : MonoBehaviour
{
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Ensure the Rigidbody2D component is present
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
        }

        // Freeze the Y position to prevent the coin from falling out of the map
        rb.constraints = RigidbodyConstraints2D.FreezePositionY;
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Xử lý khi có va chạm với coin
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Logic xử lý khi người chơi thu thập coin
            Debug.Log("Coin collected!");

            // Tăng điểm hoặc thực hiện các hành động khác ở đây

            // Xóa đối tượng coin khỏi game
            Destroy(gameObject);
        }
    }
}