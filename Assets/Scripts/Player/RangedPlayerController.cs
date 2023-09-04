/*******************************************************************************
* File Name :         RangedPlayerController.cs
* Author(s) :         Toby Schamberger
* Creation Date :     9/4/2023
*
* Brief Description : Override of DefaultPlayerController for the ranged weapons controls.
* 
* TODO: 
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RangedPlayerController : DefaultPlayerController
{
    protected override void Start()
    {
        base.Start();
    }

    protected override void Primary_performed(InputAction.CallbackContext obj)
    {
        //TODO
    }
    protected override void Primary_canceled(InputAction.CallbackContext obj)
    {
        //TODO
    }
    protected override void Secondary_performed(InputAction.CallbackContext obj)
    {
        //TODO
    }
    protected override void Secondary_canceled(InputAction.CallbackContext obj)
    {
        //TODO
    }
}
