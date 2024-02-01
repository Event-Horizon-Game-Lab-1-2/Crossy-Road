using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputConponent : MonoBehaviour
{
    //direction change
    public delegate void NewDirection(Vector3 direction);
    public static event NewDirection OnDirectionChanged;
    //direction confirm
    public delegate void ConfirmDirection();
    public static event ConfirmDirection OnDirectionConfirmed;

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

    void Update()
    {
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
        if(CoolDownTimer > 0)
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
}
