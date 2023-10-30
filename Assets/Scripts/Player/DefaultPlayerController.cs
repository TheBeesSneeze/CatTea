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
using UnityEngine;
using UnityEngine.InputSystem;


public class DefaultPlayerController : MonoBehaviour
{
    //wrong script. go to PlayerController.cs

    /*
    //Really boring settings:
    [Tooltip("The amount of slippiness the player experiences when changing movement directions (THIS VALUE MUST BE BETWEEN 0 and 1)")]
    private static float slideAmount = 0.75f; // this needs to be a number between 0 and 1. higher number for more slidey
    private static float slideSeconds = 0.6f;
    private static float slideIterations = 25;
    private static float slowAmount = 0.3f; //also a number between 0 and 1
    private static float slowSeconds = 0.1f;

    //Player input:
    protected PlayerInput playerInput;

    protected InputAction move;
    protected InputAction dash;
    protected InputAction primary;
    protected InputAction secondary;
    [HideInInspector] public InputAction Pause;
    [HideInInspector] public InputAction Select;
    [HideInInspector] public InputAction SkipText;
    protected InputAction swapWeapon;
    protected InputAction cheat;

    // components:
    protected Rigidbody2D myRigidbody;
    protected PlayerBehaviour playerBehaviour;
    public Gamepad MyGamepad;
    protected Animator myAnimator;
    protected GameManager gameManager;

    protected enum ControllerType {Keyboard, Controller};
    protected ControllerType PlayerControllerType;

    // etcetera:
    protected Vector2 moveDirection;
    protected Vector2 inputDirection;
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

        gameManager = GameObject.FindObjectOfType<GameManager>();

        InitializeControls();

        DetectInputDevice();

        StartCoroutine(UpdateAnimation());
    }

    /// <summary>
    /// Sorry Start was getting crowded
    /// </summary>
    private void InitializeControls()
    {
        move = playerInput.currentActionMap.FindAction("Move");
        dash = playerInput.currentActionMap.FindAction("Dash");
        primary = playerInput.currentActionMap.FindAction("Primary Attack");
        secondary = playerInput.currentActionMap.FindAction("Secondary Attack");
        Pause = playerInput.currentActionMap.FindAction("Pause");
        Select = playerInput.currentActionMap.FindAction("Select");
        SkipText = playerInput.currentActionMap.FindAction("Skip Text");
        swapWeapon = playerInput.currentActionMap.FindAction("Swap Weapon");
        cheat = playerInput.currentActionMap.FindAction("Cheat");

        move.performed += Move_performed;
        move.canceled += Move_canceled;

        dash.performed += Dash_started;
        dash.canceled += Dash_canceled;

        primary.performed += Primary_performed;
        primary.canceled += Primary_canceled;

        secondary.performed += Secondary_performed;
        secondary.canceled += Secondary_canceled;

        Pause.started += Pause_started;

        swapWeapon.started += SwapWeapon_started;

        cheat.started += Cheat_started;
    }

    protected void DetectInputDevice()
    {
        try { MyGamepad = playerInput.GetDevice<Gamepad>(); }
        catch { MyGamepad = null; }
        

        bool isKeyboardAndMouse = false ;
        if (MyGamepad == null)
            isKeyboardAndMouse = true;
        //bool isKeyboardAndMouse = MyGamepad.description.deviceClass.Equals("Keyboard") || MyGamepad.description.deviceClass.Equals("Mouse");

        if (isKeyboardAndMouse)
            PlayerControllerType = ControllerType.Keyboard;
        else
            PlayerControllerType = ControllerType.Controller;
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
        {
            inputDirection = obj.ReadValue<Vector2>();
            moveDirection = inputDirection * playerBehaviour.Speed / 2;
        }

        moving = true;

        movingCoroutine = StartCoroutine(SlideMovementDirection(obj));
    }

    protected virtual void Move_canceled(InputAction.CallbackContext obj)
    {
        if (IgnoreAllInputs) return;

        moving = false;

        if(movingCoroutine != null)
            StopCoroutine(movingCoroutine);

        movingCoroutine = StartCoroutine(SlowMovement());
    }

    protected virtual void Dash_started(InputAction.CallbackContext obj)
    {
        if (IgnoreAllInputs) return;

        if(canDash)
            StartCoroutine(PerformDash());
    }

    protected virtual void Dash_canceled(InputAction.CallbackContext obj)
    {
        if (IgnoreAllInputs) return;

        //TODO
    }

    protected virtual void Primary_performed(InputAction.CallbackContext obj)
    {
        if (IgnoreAllInputs)
            return;
    }
    protected virtual void Primary_canceled(InputAction.CallbackContext obj){ if (IgnoreAllInputs) return; }
    protected virtual void Secondary_performed(InputAction.CallbackContext obj)
    {
        if (IgnoreAllInputs)
            return;
    }
    protected virtual void Secondary_canceled(InputAction.CallbackContext obj){ if (IgnoreAllInputs) return; }

    protected virtual void Pause_started(InputAction.CallbackContext obj)
    {
        if (IgnoreAllInputs)
            return;
    }

    protected virtual void SwapWeapon_started(InputAction.CallbackContext obj) 
    {
        if (IgnoreAllInputs) return;
        //gameManager.SwapPlayerAttackType(playerBehaviour);
    }

    protected virtual void Cheat_started(InputAction.CallbackContext obj)
    {
        if (IgnoreAllInputs) return;
        gameManager.CurrentRoom.Cheat();
    }

    /// <summary>
    /// transitions the players movement velocity by doing cool math stuff.
    /// then just becomes regular movement
    /// </summary>
    protected IEnumerator SlideMovementDirection(InputAction.CallbackContext obj)
    {
        inputDirection = obj.ReadValue<Vector2>();
        Vector2 newMoveDiection = inputDirection * playerBehaviour.Speed;

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

        moveDirection = inputDirection * playerBehaviour.Speed;
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
            myAnimator.SetFloat("XMovement", inputDirection.x);
            myAnimator.SetFloat("YMovement", inputDirection.y);
            yield return new WaitForSeconds(0.1f);
        }
    }


    public void OnDestroy()
    {
        StopAllCoroutines();

        move.performed -= Move_performed;
        move.canceled -= Move_canceled;

        dash.performed -= Dash_started;
        dash.canceled -= Dash_canceled;

        primary.performed -= Primary_performed;
        primary.canceled -= Primary_canceled;

        secondary.performed -= Secondary_performed;
        secondary.canceled -= Secondary_canceled;

        Pause.started -= Pause_started;

        swapWeapon.started -= SwapWeapon_started;

        cheat.started -= Cheat_started;
    }
    */
}
