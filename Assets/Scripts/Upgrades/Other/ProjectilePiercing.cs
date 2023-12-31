/*******************************************************************************
* File Name :         ProjectilePiercing.cs
* Author(s) :         Toby Schamberger
* Creation Date :     11/12/2023
*
* Brief Description : Increases the attacks damage after every enemy hit
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePiercing : MonoBehaviour
{
    private AttackType attack;
    private Rigidbody2D rb;

    private void Start()
    {
        attack = GetComponent<AttackType>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        string tag = collision.tag;

        if(tag == "Boss" || tag == "Enemy" || tag == "General Character")
        {
            attack.Damage += 1;
            Debug.Log("increase damage");

            if(rb!= null)
            {
                rb.velocity = rb.velocity * 1.3f;
            }
        }
    }
}
