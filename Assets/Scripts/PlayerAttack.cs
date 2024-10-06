using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public LayerMask enemyLayers;
    public Animator animator;
    public bool isAttacking = false;
    public static PlayerAttack instance;
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public int damage = 20;
    public float attackCooldown = 0.5f; // Thời gian giữa các lần tấn công
    private float nextAttackTime = 0f;
    private int attackIndex = 0; // Chỉ số combo
    private float comboResetTime = 1f; // Thời gian để reset combo nếu không tấn công
    private float lastAttackTime = 0f; // Thời gian lần tấn công cuối
    private int maxCombo = 3; // Số lượng đòn trong chuỗi combo

    private void Awake()
    {
        instance = this;
    }

    void Update()
    {
        if (Time.time >= nextAttackTime)
        {
            if (Input.GetKeyDown(KeyCode.C) && !isAttacking)
            {
                PerformComboAttack();
                nextAttackTime = Time.time + attackCooldown; 
            }

            // Reset combo nếu không tấn công trong một khoảng thời gian
            if (Time.time - lastAttackTime > comboResetTime)
            {
                attackIndex = 0; // Reset combo về đòn đầu tiên
            }
        }
    }

    private void PerformComboAttack()
    {
        isAttacking = true;
        lastAttackTime = Time.time; // Cập nhật thời gian lần tấn công cuối

        // Tăng chỉ số combo
        attackIndex++;
        if (attackIndex > maxCombo) attackIndex = 1; // Quay lại đòn đầu tiên nếu vượt quá số đòn tối đa

        // Kích hoạt hoạt ảnh dựa trên chỉ số combo
        switch (attackIndex)
        {
            case 1:
                animator.SetTrigger("Attack1");
                break;
            case 2:
                animator.SetTrigger("Attack2");
                break;
            case 3:
                animator.SetTrigger("Attack3");
                break;
        }

        // Bắt đầu kiểm tra va chạm với kẻ địch
        StartCoroutine(HandleAttack());
    }

    private IEnumerator HandleAttack()
    {
        // Đợi một chút trước khi thực hiện kiểm tra va chạm (phù hợp với thời gian hoạt ảnh)
        yield return new WaitForSeconds(0.1f);

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<EnemyController>().TakeDamage(damage);
            Debug.Log("We hit " + enemy.name);
        }

        // Đợi để kết thúc hành động tấn công và cho phép tiếp tục tấn công
        yield return new WaitForSeconds(0.3f);
        isAttacking = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attackPoint.transform.position, attackRange);
    }
}
