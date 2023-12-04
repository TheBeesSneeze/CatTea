/*******************************************************************************
* File Name :         ParryProjectiles.cs
* Author(s) :         Toby Schamberger
* Creation Date :     10/16/2023
*
* Brief Description : added to sword in DeflectProjectilesUpgrade
*****************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParryProjectiles : MonoBehaviour
{
    public float ParrySpeed = 10;
    public float AttackDamageMultiplier = 0.5f;
    private GameObject swordPivot;

    public void Start()
    {
        swordPivot = GameObject.Find("Sword Pivot");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        string tag = collision.tag;

        //hit other attack
        if (tag.Equals("Enemy Attack") || tag.Equals("General Attack"))
        {
            //Debug.Log("PARRY!");

            AttackType attack = collision.GetComponent<AttackType>();

            //its like a flight of stairs that the code can fall down
            if (attack == null)
                return;

            if (!attack.CanBeParried && !attack.GetDestroyedByOtherAttacks)
                return;

            Parry( attack);

            //collision.tag = "Player Attack";
            //attack.DetermineAttackOwner();
        }

    }
    private void Parry(AttackType attack)
    {
        Rigidbody2D enemyAttack = attack.GetComponent<Rigidbody2D>();
        if (enemyAttack == null)
            return;

        GameObject newBullet = Instantiate(UniversalVariables.Instance.DefaultPlayerBulletPrefab, attack.transform.position, Quaternion.identity);
        Rigidbody2D newBulletRB = newBullet.GetComponent<Rigidbody2D>();

        newBulletRB.velocity = enemyAttack.velocity.normalized * -1 * ParrySpeed;

        newBulletRB.transform.eulerAngles = swordPivot.transform.eulerAngles + new Vector3(0,0,90);

        attack.Damage = attack.Damage * AttackDamageMultiplier;

        Destroy(attack.gameObject);
    }
}
