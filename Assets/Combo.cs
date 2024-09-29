using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{


    public Animator animator;
    public bool ataccando;
    public int combo;

    void Start()
    {

        animator = GetComponent<Animator>();
    }
    public void start_combo()
    {
        ataccando = false;
        if (combo < 2)
        {
            combo++;
        }
    }
    public void Finish_Ani()
    {
        ataccando = false;
        combo = 0;
    }




    public void Combos_()
    {

        if (Input.GetMouseButton(0) && !ataccando)
        {
            ataccando = true;
            animator.SetTrigger("" + combo);
        }

    }
    void Update()
    {

        Combos_();



    }


}