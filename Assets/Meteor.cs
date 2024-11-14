using UnityEngine;

public class Meteor : MonoBehaviour
{
    public float fallSpeed = 5f;
    public GameObject explosionEffect;
    public int damage = 20;

    void Update()
    {
        transform.position += Vector3.down * fallSpeed * Time.deltaTime;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Instantiate(explosionEffect, transform.position, Quaternion.identity);

        if (collision.collider.CompareTag("Player"))
        {
            collision.collider.GetComponent<PlayerHealth>().TakeDamage(damage);
        }

        Destroy(gameObject); // Destroy meteor on impact
    }
}
