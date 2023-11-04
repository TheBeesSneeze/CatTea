/*******************************************************************************
* File Name :         SawBladeMovement.cs
* Author(s) :         Toby Schamberger
* Creation Date :     10/29/2023
*
* Brief Description : sets the velocity to a random value on the connected
* rigidbody.
* Reverses rotation when it collides with walls.
* Maintains a constant speed.
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class SawBladeMovement : MonoBehaviour
{
    public float Speed;

    private SpriteRenderer spriteRenderer;
    private ConstantRotation constantRotation;
    private Rigidbody2D myRigidbody;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        constantRotation = GetComponent<ConstantRotation>();
        myRigidbody = GetComponent<Rigidbody2D>();
        RandomizeVelocity();
    }

    private void RandomizeVelocity()
    {
        if (myRigidbody == null)
        {
            Debug.LogWarning("No rigidbody on " + gameObject.name);
            return;
        }

        float x = Random.Range(-1f, 1f);
        float y = Random.Range(-1f, 1f);

        Vector2 newVelocity = new Vector2(x, y);

        myRigidbody.velocity = newVelocity;

        MaintainConstantVelocity();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        string layer = LayerMask.LayerToName(collision.gameObject.layer);

        if (layer.Equals("Level") || layer.Equals("Attack"))
        {
            constantRotation.Speed *= -1;

            MaintainConstantVelocity();

            spriteRenderer.flipX = !spriteRenderer.flipX;
        }
    }

    /// <summary>
    /// Sets the velocity to a magnitude of Speed
    /// </summary>
    public void MaintainConstantVelocity()
    {
        Vector2 velocity = myRigidbody.velocity.normalized;
        myRigidbody.velocity = velocity * Speed;
    }
}
