using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfProjectile : AttackType
{
    public int speed;
    public Rigidbody2D projectileRB2D;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        StartCoroutine(Moving());
    }

    

    private IEnumerator Moving()
    {
        while (this.gameObject != null)
        {
            projectileRB2D.velocity = speed * transform.up;
            yield return new WaitForSeconds(2);
            Destroy(gameObject);
            yield return null;
        }
    }
}
