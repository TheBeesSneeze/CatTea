/*******************************************************************************
* File Name :         GunTutorial.cs
* Author(s) :         Toby Schamberger
* Creation Date :     11/20/2023
*
* Brief Description : Destroys itself a few seconds after the player shoots the gun.
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class GunTutorial : MonoBehaviour
{
    public float SecondsToDisappear;
    public Image GunSprite;
    public Image MouseSprite;

    public bool FadeIfTutorialComplete;

    public bool ThisIsForTheGun = true;

    // Start is called before the first frame update
    void Start()
    {
        if(FadeIfTutorialComplete && SaveDataManager.Instance.SaveData.TutorialCompleted)
        {
            Destroy(gameObject);
        }

        if(ThisIsForTheGun)
            PlayerController.Instance.GunAction.started += OnFirstShoot;
    }

    public void OnFirstShoot(InputAction.CallbackContext obj)
    {
        PlayerController.Instance.GunAction.started -= OnFirstShoot;

        StartCoroutine(FadeAway());
    }

    public void ThisIsDueLikeTomorrowPleaseLetMeStopCoding()
    {
        StartCoroutine(FadeAway());
    }

    private IEnumerator FadeAway()
    {
        float time = 0;
        while(time < SecondsToDisappear)
        {
            time += Time.deltaTime;
            float t = time / SecondsToDisappear;

            Color color= GunSprite.color;
            color.a = 1-t;

            GunSprite.color = color;
            MouseSprite.color = color;

            yield return null;
        }

        Destroy(this.gameObject);
    }
}
