using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private float currentHealth;
    public Animator animator;
    public bool dead;

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth > 0)
        {
            animator.SetTrigger("hurt");
        }
        else
        {
            if (!dead)
            {
                dead = true;
                animator.SetTrigger("die");
                StartCoroutine(DisableAnimatorAndDestroy());
            }
        }
    }

    private IEnumerator DisableAnimatorAndDestroy()
    {
        // Chờ cho đến khi hoạt ảnh "die" hoàn thành
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        // Sau khi hoạt ảnh hoàn thành, vô hiệu hóa Animator
        GetComponent<Animator>().enabled = false;

        // Xóa đối tượng khỏi game
        Destroy(gameObject);
    }
}