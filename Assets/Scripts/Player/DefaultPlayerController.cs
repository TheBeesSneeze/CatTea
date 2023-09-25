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
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.UI.Image;

public class DefaultPlayerController : MonoBehaviour
{
    //Really boring settings:
    [Tooltip("The amount of slippiness the player experiences when changing movement directions (THIS VALUE MUST BE BETWEEN 0 and 1)")]
    private static float slideAmount = 0.75f; // this needs to be a number between 0 and 1. higher number for more slidey
    private static float slideSeconds = 0.6f;
    private static float slideIterations = 25;
    private static float slowAmount = 0.3f; //also a number between 0 and 1
    private static float slowSeconds = 0.1f;
    private static float dashFrames = 50;

    //Player input:
    protected PlayerInput playerInput;

    protected InputAction move;
    protected InputAction dash;
    protected InputAction primary;
    protected InputAction secondary;
    public InputAction Pause;
    public InputAction Select;

    // components:
    protected Rigidbody2D myRigidbody;
    protected PlayerBehaviour playerBehaviour;
    public Gamepad MyGamepad;
    protected Animator myAnimator;

    // etcetera:
    protected Vector2 moveDirection;
    protected bool moving; //if a movement key is being pressed rn
    protected Coroutine movingCoroutine; //shared between SlideMovementDirection and SlowMovement intentionally. They shouldnt run at the same time.
    protected bool canDash=true;
    protected bool canAttack = true;

    private bool ignoreMove;
    [HideInInspector]public bool IgnoreAllInputs;

    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    protected virtual void Start()
    {
        //initialize a lot of variables
        myRigidbody = GetComponent<Rigidbody2D>();
        playerBehaviour = GetComponent<PlayerBehaviour>();
        myAnimator = GetComponent<Animator>();

        //Initialize input stuff
        playerInput = GetComponent<PlayerInput>();
        MyGamepad = playerInput.GetDevice<Gamepad>();
        playerInput.currentActionMap.Enable();

        move = playerInput.currentActionMap.FindAction("Move");
        dash = playerInput.currentActionMap.FindAction("Dash");
        primary = playerInput.currentActionMap.FindAction("Primary Attack");
        secondary = playerInput.currentActionMap.FindAction("Secondary Attack");
        Pause = playerInput.currentActionMap.FindAction("Pause");
        Select = playerInput.currentActionMap.FindAction("Select");

        move.performed += Move_performed;
        move.canceled += Move_canceled;

        dash.performed += Dash_started;
        dash.canceled += Dash_canceled;

        primary.performed += Primary_performed;
        primary.canceled += Primary_canceled;

        secondary.performed += Secondary_performed;
        secondary.canceled += Secondary_canceled;

        Pause.started += Pause_started;

        StartCoroutine(UpdateAnimation());
    }

    /// <summary>
    /// Kind of averages the players input direction with the last input direction when
    /// a key is pressed, so it creates a slightly slippery effect.
    /// </summary>
    /// <param name="obj"></param>
    protected virtual void Move_performed(InputAction.CallbackContext obj)
    {
        if(ignoreMove || IgnoreAllInputs)
            return;

        //formatting it like this bc we'll need this variable for animation stuff probably
        if(movingCoroutine != null)
            StopCoroutine(movingCoroutine);

        if (!moving)
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
        if (IgnoreAllInputs)
            return;

        if(canDash)
            StartCoroutine(PerformDash());
    }

    protected virtual void Dash_canceled(InputAction.CallbackContext obj)
    {
        //TODO
    }

    protected virtual void Primary_performed(InputAction.CallbackContext obj)
    {
        if (IgnoreAllInputs)
            return;
    }
    protected virtual void Primary_canceled(InputAction.CallbackContext obj){}
    protected virtual void Secondary_performed(InputAction.CallbackContext obj)
    {
        if (IgnoreAllInputs)
            return;
    }
    protected virtual void Secondary_canceled(InputAction.CallbackContext obj){}

    protected virtual void Pause_started(InputAction.CallbackContext obj)
    {
        if (IgnoreAllInputs)
            return;
    }

    /// <summary>
    /// transitions the players movement velocity by doing cool math stuff.
    /// then just becomes regular movement
    /// </summary>
    protected IEnumerator SlideMovementDirection(InputAction.CallbackContext obj)
    {
        Vector2 newMoveDiection = obj.ReadValue<Vector2>() * playerBehaviour.Speed;

        //cool slide
        for (int i = 0; i < slideIterations && moving; i++)
        {
            moveDirection = BlendMovementDirections(moveDirection, newMoveDiection, slideAmount);

            if(!ignoreMove)
                myRigidbody.velocity = moveDirection;

            yield return new WaitForSeconds(slideSeconds/ slideIterations);
        }

        //may fuck things up:
        moveDirection = newMoveDiection;

        //regular movement

        StartCoroutine(RegularMovement());
        
    }

    protected IEnumerator RegularMovement()
    {
        while (moving)
        {
            if (!ignoreMove)
            {
                myRigidbody.velocity = moveDirection;
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
        ignoreMove = true;

        StartCoroutine(NoMovementRoutine(playerBehaviour.DashTime));

        if (movingCoroutine != null)
            StopCoroutine(movingCoroutine);

        myRigidbody.velocity = Vector2.zero;
        //myRigidbody.AddForce(moveDirection * playerBehaviour.DashUnits, ForceMode2D.Impulse);
        myRigidbody.AddForce(moveDirection * (playerBehaviour.DashUnits / playerBehaviour.DashTime), ForceMode2D.Impulse);

        if (MyGamepad != null)
        {
            MyGamepad.SetMotorSpeeds(0.1f, 0.1f);
        }

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
        if (MyGamepad != null)
        {
            MyGamepad.SetMotorSpeeds(0f, 0f);
        }

        if (moving)
            myRigidbody.velocity = moveDirection;
        else
            movingCoroutine = StartCoroutine(SlowMovement());

    }

    protected IEnumerator UpdateAnimation()
    {
        while (true)
        {
            myAnimator.SetFloat("XMovement", moveDirection.x);
            myAnimator.SetFloat("YMovement", moveDirection.y);
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void OnDestroy()
    {
        StopAllCoroutines();
    }
}
