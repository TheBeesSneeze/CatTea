using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*******************************************************************************
* File Name :         DogAttack.cs
* Author(s) :         Aiden Vandeberg
* Creation Date :     10/3/2023
*
* Brief Description : 
*****************************************************************************/

public class DogAttack : AttackType
{
    public int speed;
    public Rigidbody2D waveRB2D;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        StartCoroutine(Moving());
    }

    

    private IEnumerator Moving()
    {
        while(this.gameObject != null)
        {
            waveRB2D.velocity = speed * transform.up;
            yield return new WaitForSeconds(3);
            Destroy(gameObject);
            yield return null;
        }
    }
}
