using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy : MonoBehaviour
{
    public float speed;
    public float detectionRadius = 5f; // Bán kính phát hiện
    private GameObject player;
    public bool chase = false;
    public Transform starPos;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (player == null) // Kiểm tra nếu player là null
            return;

        // Cập nhật trạng thái đuổi theo dựa trên khoảng cách với nhân vật
        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
        if (distanceToPlayer <= detectionRadius)
        {
            chase = true;
        }
        else
        {
            chase = false;
        }

        if (chase)
            Chase();
        else
            resStarPos();

        Flip();
    }

    private void resStarPos()
    {
        transform.position = Vector2.MoveTowards(transform.position, starPos.position, speed * Time.deltaTime);
    }

    private void Chase()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        if (Vector2.Distance(transform.position, player.transform.position) <= 0.5f)
        {
            // Logic khi tiếp cận nhân vật
        }
    }

    private void Flip()
    {
        if (transform.position.x > player.transform.position.x)
            transform.rotation = Quaternion.Euler(0, 0, 0);
        else
            transform.rotation = Quaternion.Euler(0, 180, 0);
    }
}
