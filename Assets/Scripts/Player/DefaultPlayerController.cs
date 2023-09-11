/*******************************************************************************
* File Name :         DefaultPlayerController.cs
* Author(s) :         Toby Schamberger
* Creation Date :     9/4/2023
*
* "Brief" Description : Base class for the player controller scripts to inherit from.
* Specifically deals with controls.
* Moving is slightly slippery, slippiness can be changed in code (make the variables
* public if it bothers you)
* 
* Controls Include: Movement, Dashing, Primary weapon function (does nothing),
* Secondary weapon function (does nothing), Pause.
*
* TODO:
* Dashing
* Other input settings
* PAUSE
* player invincible while dashing
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.InputSystem;

public class DefaultPlayerController : MonoBehaviour
{
    //Really boring settings:
    [Tooltip("The amount of slippiness the player experiences when changing movement directions (THIS VALUE MUST BE BETWEEN 0 and 1)")]
    private static float slideAmount = 0.7f; // this needs to be a number between 0 and 1
    private static float slideSeconds = 0.6f;
    private static float slowAmount = 0.3f; //also a number between 0 and 1
    private static float slowSeconds = 0.1f;

    //Player input:
    protected PlayerInput playerInput;

    protected InputAction move;
    protected InputAction dash;
    protected InputAction primary;
    protected InputAction secondary;
    protected InputAction pause;
    public InputAction Select;

    // components:
    protected Rigidbody2D myRigidbody;
    protected PlayerBehaviour playerBehaviour;
    public Gamepad myGamepad;

    // etcetera:
    protected Vector2 moveDirection;
    protected bool moving;
    protected bool ignoreMove;
    protected Coroutine movingCoroutine; //shared between SlideMovementDirection and SlowMovement intentionally. They shouldnt run at the same time.
    protected bool canDash=true;

    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    protected virtual void Start()
    {
        //initialize a lot of variables
        myRigidbody = GetComponent<Rigidbody2D>();
        playerBehaviour = GetComponent<PlayerBehaviour>();

        //Initialize input stuff
        playerInput = GetComponent<PlayerInput>();
        myGamepad = playerInput.GetDevice<Gamepad>();
        playerInput.currentActionMap.Enable();

        move = playerInput.currentActionMap.FindAction("Move");
        dash = playerInput.currentActionMap.FindAction("Dash");
        primary = playerInput.currentActionMap.FindAction("Primary Attack");
        secondary = playerInput.currentActionMap.FindAction("Secondary Attack");
        pause = playerInput.currentActionMap.FindAction("Pause");
        Select = playerInput.currentActionMap.FindAction("Select");

        move.performed += Move_performed;
        move.canceled += Move_canceled;

        dash.performed += Dash_started;
        dash.canceled += Dash_canceled;

        primary.performed += Primary_performed;
        primary.canceled += Primary_canceled;

        secondary.performed += Secondary_performed;
        secondary.canceled += Secondary_canceled;

        pause.started += Pause_started;
    }

    /// <summary>
    /// Kind of averages the players input direction with the last input direction when
    /// a key is pressed, so it creates a slightly slippery effect.
    /// </summary>
    /// <param name="obj"></param>
    protected virtual void Move_performed(InputAction.CallbackContext obj)
    {
        //formatting it like this bc we'll need this variable for animation stuff probably
        if(movingCoroutine != null)
            StopCoroutine(movingCoroutine);

        //if this is the first movement pretty much
        if(!moving)
            moveDirection = obj.ReadValue<Vector2>() * playerBehaviour.Speed / 2;

        moving = true;

        movingCoroutine = StartCoroutine(SlideMovementDirection(obj));
    }

    protected virtual void Move_canceled(InputAction.CallbackContext obj)
    {
        moving = false;

        if(movingCoroutine != null)
            StopCoroutine(movingCoroutine);

        movingCoroutine = StartCoroutine(SlowMovement());
    }

    protected virtual void Dash_started(InputAction.CallbackContext obj)
    {
        if(canDash)
            StartCoroutine(PerformDash());
    }

    protected virtual void Dash_canceled(InputAction.CallbackContext obj)
    {
        //TODO
    }

    protected virtual void Primary_performed(InputAction.CallbackContext obj){Debug.Log("Primary Attack Button pressed");}
    protected virtual void Primary_canceled(InputAction.CallbackContext obj){Debug.Log("Primary Attack Button released");}
    protected virtual void Secondary_performed(InputAction.CallbackContext obj){Debug.Log("Secondary Attack Button pressed");}
    protected virtual void Secondary_canceled(InputAction.CallbackContext obj){Debug.Log("Secondary Attack Button released");}

    protected virtual void Pause_started(InputAction.CallbackContext obj)
    {
        //TODO
    }

    /// <summary>
    /// transitions the players movement velocity by doing cool math stuff.
    /// then just becomes regular movement
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    protected IEnumerator SlideMovementDirection(InputAction.CallbackContext obj)
    {
        Vector2 newMoveDiection = obj.ReadValue<Vector2>() * playerBehaviour.Speed;

        //cool slide
        for (int i = 0; i < 10 && moving && !ignoreMove; i++)
        {
            moveDirection = BlendMovementDirections(moveDirection, newMoveDiection, slideAmount);
            myRigidbody.velocity = moveDirection;

            yield return new WaitForSeconds(slideSeconds/10);
        }

        //may fuck things up:
        moveDirection = newMoveDiection;

        //regular movement
        while (moving)
        {
            if(!ignoreMove)
            {
                myRigidbody.velocity = newMoveDiection;
            }
            yield return null;
        }

        movingCoroutine = null;
    }

    /// <summary>
    /// combines the new movement value with the old one so its kinda slidey
    /// </summary>
    /// <param name="OldMoveDirection">moveDirection</param>
    /// <param name="newReadValue">obj.ReadValue<Vector2>()</param>
    /// <param name="slideWeight">number between 0 and 1. 0 for instant change. 0.999 for very slidey.param>
    /// <returns>An averaged movement direction</returns>
    protected Vector2 BlendMovementDirections(Vector2 OldMoveDirection, Vector2 newReadValue, float slideWeight)
    {
        Vector2 a = OldMoveDirection * slideWeight;                         // 10 * 0.7 = 7
        Vector2 b = newReadValue * (1-slideAmount); // 10 * 0.3 = 3

        return a + b;                                                       // 7 + 3 = 10
    }

    /// <summary>
    /// runs after player stops moving
    /// </summary>
    /// <returns></returns>
    protected IEnumerator SlowMovement()
    {
        for(int i = 0; i<10 && !moving; i++)
        {
            //notice this doesnt update moveDirection
            myRigidbody.velocity = BlendMovementDirections(moveDirection, Vector2.zero, slowAmount);

            yield return new WaitForSeconds(slowSeconds / 10);
        }
        myRigidbody.velocity = Vector2.zero;
        movingCoroutine = null;
    }

    protected virtual IEnumerator PerformDash()
    {
        canDash = false;

        if(movingCoroutine != null)
            StopCoroutine(movingCoroutine);

        ignoreMove = true;
        myRigidbody.AddForce(moveDirection * playerBehaviour.DashForce, ForceMode2D.Impulse);

        //test
        if (myGamepad != null)
        {
            myGamepad.SetMotorSpeeds(0.3f, 0.3f);
        }

        StartCoroutine(NoMovementRoutine(0.2f));

        yield return new WaitForSeconds(playerBehaviour.DashRechargeSeconds);
        canDash = true;
    }
    
    /// <summary>
    /// no movement for dash reasons
    /// </summary>
    /// <param name="Seconds">length of dash ig</param>
    private IEnumerator NoMovementRoutine(float Seconds)
    {
        ignoreMove = true;
        yield return new WaitForSeconds(Seconds);
        ignoreMove = false;

        //test
        if (myGamepad != null)
        {
            myGamepad.SetMotorSpeeds(0f, 0f);
        }

        if (moving)
            myRigidbody.velocity = moveDirection;
        else
            movingCoroutine = StartCoroutine(SlowMovement());

    }

}
