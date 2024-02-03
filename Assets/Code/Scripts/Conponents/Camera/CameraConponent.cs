using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
public class CameraConponent : MonoBehaviour
{
    [Header("Camera Settings")]
    [Tooltip("Time to reach target position")]
    [SerializeField] private float SmootTime = 0.25f;
    //[Tooltip("Max distance reachable by the camera before the smooting time is reduced")]
    //[SerializeField] private float MaxDistance = 2.0f;
    //[Tooltip("Attuation of smootin time if the max distance is reached")]
    //[SerializeField] private float SmootTimeAttuator = 0.1f;
    [Tooltip("Distance below wich the camera will keep moving linearly")]
    [SerializeField] private float LinearAccelDistance = 1f;
    [Tooltip("Speed of the camera when is moving linearly")]
    [SerializeField] private float LinearAccelSpeed = 1f;
    
    private bool IsMoving;
    private float SmootingTime;
    private Vector3 Velocity = Vector3.zero;
    private Vector3 TargetPosition = Vector3.zero;

    private void Awake()
    {
        IsMoving = false;
        TargetPosition = transform.position;
        SmootingTime = SmootTime;
    }

    private void FixedUpdate()
    {
        if (!IsMoving)
            return;

        float DistanceToTarget = TargetPosition.z - transform.position.z;

        ////accelerate the camera if max distance is reached
        //if (DistanceToTarget > MaxDistance)
        //    SmootingTime -= SmootTimeAttuator;
        //else
        //    SmootingTime = SmootTime;

        if (DistanceToTarget > LinearAccelDistance)
        {
            //smooting between two points
            transform.position = Vector3.SmoothDamp(transform.position, TargetPosition, ref Velocity, SmootingTime);
        }
        else
        {
            //keep moving
            transform.position += Vector3.forward * Time.fixedDeltaTime * LinearAccelSpeed;
        }
    }

    private void DirectionConfirmed()
    {
        IsMoving = true;
    }

    private void IncreaseTargetDistance()
    {
        TargetPosition += Vector3.forward;
    }

    void OnEnable()
    {
        //Connect all Events
        InputConponent.OnDirectionConfirmed += DirectionConfirmed;
        GameManager.OnNewRowAchieved += IncreaseTargetDistance;
    }

    private void OnDisable()
    {
        //Disconnect all Events
        InputConponent.OnDirectionConfirmed -= DirectionConfirmed;
        GameManager.OnNewRowAchieved -= IncreaseTargetDistance;
    }

    
}
