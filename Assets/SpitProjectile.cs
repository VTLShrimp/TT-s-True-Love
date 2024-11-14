using UnityEngine;

public class SpitProjectile : MonoBehaviour
{
    public GameObject groundTremorEffect;
    public float tremorRadius = 2f;
    public int damage = 15;

    void OnCollisionEnter2D(Collision2D collision)
    {
        Instantiate(groundTremorEffect, transform.position, Quaternion.identity);

        // Check for player within tremor radius and deal damage
        Collider2D[] hitObjects = Physics2D.OverlapCircleAll(transform.position, tremorRadius);
        foreach (Collider2D obj in hitObjects)
        {
            if (obj.CompareTag("Player"))
            {
                obj.GetComponent<PlayerHealth>().TakeDamage(damage);
            }
        }

        Destroy(gameObject); // Destroy projectile after impact
    }
}
