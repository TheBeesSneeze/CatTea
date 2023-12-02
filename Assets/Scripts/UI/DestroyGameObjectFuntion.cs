/*******************************************************************************
* File Name :         DestroyGameObjectFuntion.cs
* Author(s) :         Toby Schamberger
* Creation Date :     12/2/2023
*
* Brief Description : hi zach dont look at the creation date.
* literally just a function to destroy the gameobject, to be used in the animator.
* because man i dont know what im doing
* *****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyGameObjectFuntion : MonoBehaviour
{
    /// <summary>
    /// called in animatior. destroys attached gameobject.
    /// </summary>
    public void MyFinalMessageGoodbye()
    {
        //you can tell by my function names that i am losing it
        Destroy(gameObject);
    }
}
