/*******************************************************************************
* File Name :         BossRoom.cs
* Author(s) :         Toby Schamberger
* Creation Date :     9/13/2023
*
* Brief Description : 
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoom : EnemyRoom
{

    public override void EnterRoom()
    {
        //copy and paste the code from roomType
        cameraManager.MoveCamera(CameraCenterPoint);
        playerBehaviour.transform.position = PlayerSpawnPoint.transform.position;
    }


}
