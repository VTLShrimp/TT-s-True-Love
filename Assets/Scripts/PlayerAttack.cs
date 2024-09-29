using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public Animator animator;
    public bool isAttacking;
    public int combo;
    private float lastAttackTime;
    public float comboDelay = 0.5f; // Adjust this value based on your combo timing

    void Start()
    {
        animator = GetComponent<Animator>();
        isAttacking = false;
        combo = 0;
    }

    public void StartCombo()
    {
        isAttacking = false;

        // Reset combo if the time since the last attack exceeds the combo delay
        if (Time.time - lastAttackTime > comboDelay)
        {
            combo = 0;
        }

        if (combo < 3)
        {
            combo++;
        }
    }

    public void FinishAnimation()
    {
        isAttacking = false;
        combo = 0;
    }

    public void PerformCombo()
    {
        if (Input.GetMouseButtonDown(0) && !isAttacking)
        {
            isAttacking = true;
            lastAttackTime = Time.time; // Record the time of the attack

            // Trigger the appropriate combo animation
            animator.SetTrigger("Combo" + combo);
        }
    }

    void Update()
    {
        PerformCombo();
    }
}
