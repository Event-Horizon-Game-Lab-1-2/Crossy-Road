using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DeathTrigger;
using static DeathTypeClass;

public class InputComponent : MonoBehaviour
{
    //direction change
    public delegate void NewDirection(Vector3 direction);
    public static event NewDirection OnDirectionChanged = new NewDirection( (Vector3 direction) => { } );
    //direction confirm
    public delegate void ConfirmDirection();
    public static event ConfirmDirection OnDirectionConfirmed = new ConfirmDirection( () => { } );
    //Pause game request
    public delegate void PauseGame(bool pause);
    public static event PauseGame OnPauseGame = new PauseGame( (bool pause) => { } );

    [Header("Option Keys")]
    //Pause Button
    [SerializeField] private KeyCode Pause = KeyCode.Escape;

    //inputs keys
    [Header("Movement Keys")]
    [SerializeField] private KeyCode Up = KeyCode.W;
    [SerializeField] private KeyCode Left = KeyCode.A;
    [SerializeField] private KeyCode Down = KeyCode.S;
    [SerializeField] private KeyCode Right = KeyCode.D;

    //Delays between inputs
    [Header("Movement CoolDown")]
    [SerializeField] private float InputRecoveryTime = 0.1f;

    //direction of movement
    private Vector3 Direction;
    //Delay Timer
    private float CoolDownTimer = 0f;
    
    //Game Paused
    private bool GamePause = false;

    //Disable movement
    private bool CanGetInput = true;

    void Update()
    {
        //check if the pause button is pressed
        if(Input.GetKeyDown(Pause))
        {
            GamePause = !GamePause;
            //Call local game event
            OnPauseGame(GamePause);
        }

        if (!CanGetInput)
            return;

        //check if a new direction is chosen
        if (GetInputDirection(out Direction))
            OnDirectionChanged(Direction);
        //check if the chosen direction is confirmed
        if (IsDirectionConfirmed() && CoolDownTimer <= 0f)
        {
            //Debug.Log("Direction Confirmed");
            CoolDownTimer = InputRecoveryTime;
            OnDirectionConfirmed();
        }


        //Decrease input timer
        if (CoolDownTimer > 0)
            CoolDownTimer -= Time.deltaTime;
    }

    private bool GetInputDirection(out Vector3 direction)
    {
        direction = Vector3.zero;

        //check inputs
        if(Input.GetKeyDown(Up))
            direction = new Vector3(0f, 0f, 1f);

        if (Input.GetKeyDown(Down))
            direction = new Vector3(0f, 0f, -1f);

        if (Input.GetKeyDown(Right))
            direction = new Vector3(1f, 0f, 0f);

        if (Input.GetKeyDown(Left))
            direction = new Vector3(-1f, 0f, 0f);

        //return the value
        if(direction == Vector3.zero)
            return false;
        else
            return true;
    }

    private bool IsDirectionConfirmed()
    {
        //check if a key is relased
        if (Input.GetKeyUp(Up))
            return true;

        if (Input.GetKeyUp(Down))
            return true;

        if (Input.GetKeyUp(Right))
            return true;

        if (Input.GetKeyUp(Left))
            return true;

        return false;
    }

    private void OnEnable()
    {
        OnDirectionChanged += OnDirectionChanged;
        PlayerManager.OnDeath += (DeathType t) => CanGetInput = false;
    }

    private void OnDisable()
    {
        PlayerManager.OnDeath -= (DeathType t) => CanGetInput = false;
    }

    private void OnDestroy()
    {
        OnDirectionChanged -= OnDirectionChanged;
    }
}
