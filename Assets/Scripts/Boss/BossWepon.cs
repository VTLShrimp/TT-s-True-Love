using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWeapon : MonoBehaviour
{
    public int attackDamage = 20;
    public int enragedAttackDamage = 40;

    public Vector3 attackOffset;
    public Vector2 attackSize = new Vector2(1f, 1f); // Width and height of the attack rectangle
    public LayerMask attackMask;

    public float attackCooldown = 1f; // Cooldown duration in seconds
    private float lastAttackTime; // Time when the last attack occurred

    public void Attack()
    {
        if (Time.time >= lastAttackTime + attackCooldown) // Check if the cooldown has expired
        {
            Vector3 pos = transform.position;
            pos += transform.right * attackOffset.x;
            pos += transform.up * attackOffset.y;

            Collider2D colInfo = Physics2D.OverlapBox(pos, attackSize, 0, attackMask);
            if (colInfo != null)
            {
                colInfo.GetComponent<PlayerHealth>().TakeDamage(attackDamage);
            }

            lastAttackTime = Time.time; // Update the last attack time
        }
    }

    public void EnragedAttack()
    {
        if (Time.time >= lastAttackTime + attackCooldown) // Check if the cooldown has expired
        {
            Vector3 pos = transform.position;
            pos += transform.right * attackOffset.x;
            pos += transform.up * attackOffset.y;

            Collider2D colInfo = Physics2D.OverlapBox(pos, attackSize, 0, attackMask);
            if (colInfo != null)
            {
                colInfo.GetComponent<PlayerHealth>().TakeDamage(enragedAttackDamage);
            }

            lastAttackTime = Time.time; // Update the last attack time
        }
    }

    void OnDrawGizmosSelected()
    {
        Vector3 pos = transform.position;
        pos += transform.right * attackOffset.x;
        pos += transform.up * attackOffset.y;

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(pos, attackSize);
    }
}
