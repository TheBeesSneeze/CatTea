using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantRotation : MonoBehaviour
{
    public float Speed = 0.8f;

    // Update is called once per frame
    void Update()
    {
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, (transform.eulerAngles.z+Speed)%360);
    }
}
