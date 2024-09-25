using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{

    public LayerMask enemyLayers;
    public Transform attackPoint;
    public Animator animator;
    public float attackRange = 0.5f;
    public int damage = 20;
    public RangeWeapon rangeWeapon;
    public float attackcooldown;
    public float bowcooldown;
    public float cooldowntimer = Mathf.Infinity;

    void Start()
    {
        if (rangeWeapon == null)
        {
            rangeWeapon = GetComponent<RangeWeapon>();
        }
        animator = GetComponent<Animator>();
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0) && cooldowntimer > attackcooldown)
        {
            Attack();

        }
        cooldowntimer += Time.deltaTime;
        if (Input.GetMouseButtonDown(1))
        {
            Bow();
        }
    }

    public void Attack()
    {
        animator.SetTrigger("Attack");
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            // enemy.GetComponent<BoarScript>().TakeDamage(damage);    
            Debug.Log("We hit " + enemy.name);
        }
        cooldowntimer = 0;
    }

    public void Bow()
    {
        animator.SetTrigger("Bow");
        rangeWeapon.Shoot();
    }
    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
        {
            return;
        }
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
