using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public LayerMask enemyLayers;
    public Animator animator;
    public bool isAttacking = false; // Trạng thái tấn công
    public static PlayerAttack instance;
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public int damage = 20;
    public RangeWeapon rangeWeapon;
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        if (rangeWeapon == null)
        {
            rangeWeapon = GetComponent<RangeWeapon>();
        }
    }



    void Update()
    {
        Attack();
    }
    private void Attack()
    {
        if (Input.GetKeyDown(KeyCode.C) && !isAttacking)
        {
            isAttacking = true;
        }
        animator.SetTrigger("Attack");
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<EnemyContr>().TakeDamage(damage);
            Debug.Log("We hit " + enemy.name);
        }
        //  cooldowntimer = 0;
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attackPoint.transform.position, 0.5f);
    }
}
