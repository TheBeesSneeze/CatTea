/*******************************************************************************
* File Name :         playerController.cs
* Author(s) :         Toby Schamberger
* Creation Date :     9/4/2023
*
* "Brief" Description : Base class for the player controller scripts to inherit from.
* Specifically deals with controls.
* Moving is slightly slippery, slippiness can be changed in code (make the variables
* public if it bothers you)
* 
* Controls Include: Movement, Dashing, Pause.
*
* TODO:
* Other input settings
* PAUSE
* player invincible while dashing
*****************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    [Header("Unity")]
    public SpriteRenderer GunSprite;
    public SpriteRenderer SwordSprite;

    //magic numbers
    [Tooltip("The amount of slippiness the player experiences when changing movement directions (THIS VALUE MUST BE BETWEEN 0 and 1)")]
    private static float slideAmount = 0.70f; // this needs to be a number between 0 and 1. higher number for more slidey
    private static float slideSeconds = 0.2f;
    private static float slowAmount = 0.3f; //also a number between 0 and 1
    private static float slowSeconds = 0.1f;

    //Player input:
    protected PlayerInput playerInput;

    protected InputAction move;
    protected InputAction dash;
    protected InputAction primary;
    protected InputAction secondary;
    protected InputAction aim;
    [HideInInspector] public InputAction Pause;
    [HideInInspector] public InputAction Select;
    [HideInInspector] public InputAction SkipText;
    protected InputAction cheat;
    protected InputAction mouse;
    private InputAction roomSkip;

    // components:
    protected Rigidbody2D myRigidbody;
    public Gamepad MyGamepad;
    protected Animator myAnimator;
    
    //le sound
    public AudioSource walkSound;

    protected RangedPlayerController rangedPlayerController;
    protected MeleePlayerController meleePlayerController;
    private SpriteRenderer gunSprite;

    protected enum WeaponMode { Gun, Sword };
    protected WeaponMode CurrentWeapon;

    public enum ControllerType {Keyboard, Controller};
    [HideInInspector] public ControllerType PlayerControllerType;

    // etcetera:
    [HideInInspector] public Vector2 MoveDirection;
    [HideInInspector] public Vector2 InputDirection;
    [HideInInspector] public Vector2 AimingDirection;
    protected bool moving; //if a movement key is being pressed rn
    protected Coroutine movingCoroutine; //shared between SlideMovementDirection and SlowMovement intentionally. They shouldnt run at the same time.
    protected bool canDash=true;
    [HideInInspector] public bool CanAttack = true;

    private bool readShootingDirection;
    private Coroutine aimingCoroutine;

    private bool ignoreMove;
    [HideInInspector]public bool IgnoreAllInputs;

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    protected virtual void Start()
    {
        //initialize a lot of variables
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();

        //Initialize input stuff
        playerInput = GetComponent<PlayerInput>();
        MyGamepad = playerInput.GetDevice<Gamepad>();
        playerInput.currentActionMap.Enable();

        rangedPlayerController = GetComponent<RangedPlayerController>();
        meleePlayerController = GetComponent<MeleePlayerController>();

        gunSprite = rangedPlayerController.Gun.GetComponent<SpriteRenderer>();

        InitializeControls();

        DetectInputDevice();

        UpdateAimingDirection();
        StartCoroutine(UpdateAnimation());
    }

    /// <summary>
    /// Sorry Start was getting crowded.
    /// initalizes every control from every player script
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
        aim = playerInput.currentActionMap.FindAction("Aiming");
        mouse = playerInput.currentActionMap.FindAction("Mouse");
        cheat = playerInput.currentActionMap.FindAction("Cheat");
        roomSkip = playerInput.currentActionMap.FindAction("Room Skip Cheat");

        move.performed += Move_performed;
        move.canceled += Move_canceled;

        dash.performed += Dash_started;
        dash.canceled += Dash_canceled;

        primary.performed += rangedPlayerController.Gun_performed;
        primary.canceled += rangedPlayerController.Gun_canceled;

        secondary.performed += meleePlayerController.Sword_started;
        secondary.canceled += meleePlayerController.Sword_canceled;

        Pause.started += Pause_started;

        cheat.started += Cheat_started;
        roomSkip.started += RoomSkipCheat_started;

        aim.performed += Aim_Performed;
    }

    private void Aim_Performed(InputAction.CallbackContext obj)
    {
        //AimingDirection = Camera.main.ScreenToWorldPoint(obj.ReadValue<Vector2>());
        //Debug.Log("A_P " + AimingDirection);
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
            InputDirection = obj.ReadValue<Vector2>();
            MoveDirection = InputDirection * PlayerBehaviour.Instance.Speed / 2;
            walkSound.Play();
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
        walkSound.Stop();
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
    }

    protected virtual void Pause_started(InputAction.CallbackContext obj)
    {
        if (IgnoreAllInputs)
            return;

        //THIS IS TEMP CODE
        // Application.Quit();
        if (Settings.Instance.Paused)
        {
            Settings.Instance.ClosePauseMenu();
        }
        else
        {
            Settings.Instance.OpenPauseMenu();
        }
    }

    protected virtual void Cheat_started(InputAction.CallbackContext obj)
    {
        if (IgnoreAllInputs) return;

        GameManager.Instance.CurrentRoom.Cheat();

        SaveDataManager.Instance.SaveData.GunUnlocked = true;
        PlayerBehaviour.Instance.OnGunUnlocked();
    }

    private void RoomSkipCheat_started(InputAction.CallbackContext obj)
    {
        if (IgnoreAllInputs) return;

        RoomTransition.Instance.ForceRoomSkip();
    }

    /// <summary>
    /// Kind of unnessecary rn, but im cooking, ok
    /// </summary>
    public void UpdateAimingDirection()
    {
        if (aimingCoroutine != null)
            StopCoroutine(aimingCoroutine);

        readShootingDirection = true;

        if (PlayerControllerType.Equals(PlayerController.ControllerType.Keyboard))
        {
            rangedPlayerController.RangedIcon.SetActive(true);
            aimingCoroutine = StartCoroutine(UpdateShootingDirectionByKeyboard());
        }

        if (PlayerControllerType.Equals(PlayerController.ControllerType.Controller))
        {
            rangedPlayerController.RangedIcon.SetActive(false);
 //           aimingCoroutine = StartCoroutine(UpdateShootingDirectionByController());
        }
    }

    private IEnumerator UpdateShootingDirectionByKeyboard()
    {
        while (readShootingDirection)
        {
            Vector2 MousePosition = Camera.main.ScreenToWorldPoint(mouse.ReadValue<Vector2>());
            rangedPlayerController.RangedIcon.transform.position = MousePosition;

            //update shootingDirection. normalize MousePosition to be relative to player
            MousePosition -= (Vector2)transform.position;
            MousePosition = MousePosition.normalized;

            AimingDirection = MousePosition;

            //rotate awesome style

            RotateGun(MousePosition);

            yield return null;
        }
        aimingCoroutine = null;
    }

    private void RotateGun(Vector2 aimDirection)
    {
        //MousePosition = new Vector2(Mathf.Abs(MousePosition.x), MousePosition.y);

        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;

        //angle = Mathf.Clamp(angle, rangedPlayerController.MaxDownAngle, rangedPlayerController.MaxUpAngle);

        rangedPlayerController.RotationPivot.transform.localEulerAngles = new Vector3(0, 0, angle);

        //flip gun to aim in right direction
        if(aimDirection.x < 0)
            GunSprite.flipY = true;

        if (aimDirection.x > 0)
            GunSprite.flipY = false;

        //change layer to look right
        if(aimDirection.y > 0)
            GunSprite.sortingOrder = -5;

        if (aimDirection.y < 0)
            GunSprite.sortingOrder = 5;
    }

    private IEnumerator UpdateShootingDirectionByController()
    {
        while (readShootingDirection)
        {
            AimingDirection = InputDirection;

            rangedPlayerController.CorrectGunPosition();

            yield return null;
        }
        aimingCoroutine = null;
    }

    /// <summary>
    /// transitions the players movement velocity by doing cool math stuff.
    /// then just becomes regular movement
    /// </summary>
    protected IEnumerator SlideMovementDirection(InputAction.CallbackContext obj)
    {
        InputDirection = obj.ReadValue<Vector2>();
        Vector2 newMoveDiection = InputDirection;
    
        //cool slide
        float t = 0; // 0 <= t <= slideSeconds
        while (t < slideSeconds)
        {
            t += Time.deltaTime;

            MoveDirection = BlendMovementDirections(MoveDirection, newMoveDiection, slideAmount);

            if (!ignoreMove)
                myRigidbody.velocity = MoveDirection * PlayerBehaviour.Instance.Speed;

            yield return null;
        }

        //may fuck things up:
        MoveDirection = newMoveDiection;

        //regular movement

        StartCoroutine(RegularMovement());
        
    }

    protected IEnumerator RegularMovement()
    {
        while (moving)
        {
            if (!ignoreMove)
            {
                myRigidbody.velocity = MoveDirection * PlayerBehaviour.Instance.Speed; //no Time.deltaTime bc its just velocity being changed
            }
            yield return null;
        }
        movingCoroutine = null;
    }

    /// <summary>
    /// combines the new movement value with the old one so its kinda slidey
    /// </summary>
    /// <param name="OldMoveDirection">MoveDirection</param>
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
        float t = 0; // 0 <= t <= slowSeconds
        while(t < slowSeconds)
        {
            t+= Time.deltaTime;

            //notice this doesnt update MoveDirection
            myRigidbody.velocity = BlendMovementDirections(MoveDirection, Vector2.zero, slowAmount);

            yield return null;
        }
        myRigidbody.velocity = Vector2.zero;
        movingCoroutine = null;
    }

    protected virtual IEnumerator PerformDash()
    {
        ignoreMove = true;
        canDash = false;

        GameEvents.Instance.OnPlayerDash();

        PlayerBehaviour.Instance.BecomeInvincible(slideSeconds / 0.9f, true);

        myRigidbody.velocity = Vector2.zero;
        myRigidbody.AddForce(MoveDirection * PlayerBehaviour.Instance.DashUnits, ForceMode2D.Impulse);

        //test
        if (Settings.Instance.ControllerVibration && MyGamepad != null)
        {
            MyGamepad.SetMotorSpeeds(0.3f, 0.3f);
        }

        StartCoroutine(NoMovementRoutine(slideSeconds));

        yield return new WaitForSeconds(PlayerBehaviour.Instance.DashRechargeSeconds);
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
            myRigidbody.velocity = MoveDirection;
        else
            movingCoroutine = StartCoroutine(SlowMovement());

    }

    protected IEnumerator UpdateAnimation()
    {
        while(true)
        {
            myAnimator.SetFloat("XMovement", AimingDirection.x);
            myAnimator.SetFloat("YMovement", AimingDirection.y);

            rangedPlayerController.CorrectGunPosition();

            yield return null;
        }
    }

    public void OnDestroy()
    {
        StopAllCoroutines();

        move.performed -= Move_performed;
        move.canceled -= Move_canceled;

        dash.performed -= Dash_started;
        dash.canceled -= Dash_canceled;

        primary.performed -= rangedPlayerController.Gun_performed;
        primary.canceled -= rangedPlayerController.Gun_canceled;

        secondary.performed -= meleePlayerController.Sword_started;
        secondary.canceled -= meleePlayerController.Sword_canceled;

        Pause.started -= Pause_started;

        cheat.started -= Cheat_started;
        roomSkip.started -= RoomSkipCheat_started;

        Pause.started += Pause_started;

        aim.performed += Aim_Performed;
    }
}
