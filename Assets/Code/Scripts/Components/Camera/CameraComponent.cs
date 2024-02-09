using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;
public class CameraComponent : MonoBehaviour
{
    [Header("Camera Settings")]
    [Tooltip("Time to reach target position")]
    [SerializeField] private float SmootTime = 0.25f;
    [Tooltip("Distance below wich the camera will keep moving linearly")]
    [SerializeField] private float LinearAccelDistance = 1f;
    [Tooltip("Speed of the camera when is moving linearly")]
    [SerializeField] private float LinearAccelSpeed = 1f;
    
    private bool IsMoving;
    //local smooting time
    private float SmootingTime;
    //velocity ref as float used is sideway movement
    private float VelocityFloat = 0f;
    //velocity ref as vector3 used in forward movement
    private Vector3 VelocityV3 = Vector3.zero;
    //target position 
    private Vector3 TargetPosition = Vector3.zero;

    private void Awake()
    {
        IsMoving = false;
        TargetPosition = transform.position;
        SmootingTime = SmootTime;
    }

    IEnumerator MoveCamera()
    {
        while (IsMoving)
        {
            float DistanceToTarget = TargetPosition.z - transform.position.z;

            if (DistanceToTarget > LinearAccelDistance)
            {
                //smooting between two points
                transform.position = Vector3.SmoothDamp(transform.position, TargetPosition, ref VelocityV3, SmootingTime);
            }
            else
            {
                //change x value
                float smoothX = Mathf.SmoothDamp(transform.position.x, TargetPosition.x, ref VelocityFloat, SmootingTime);
                transform.position = new Vector3(smoothX, transform.position.y, transform.position.z);
                //keep moving linearly
                transform.position += Vector3.forward * Time.deltaTime * LinearAccelSpeed;
            }

            yield return null;
        }
    }

    private void DirectionConfirmed()
    {
        if(!IsMoving)
        {
            IsMoving = true;
            StartCoroutine(MoveCamera());
        }
    }

    private void ChangeCameraX(Vector3 newDir)
    {
        //move only on x axis -> set to 0 z and y
        newDir.z *= 0;
        newDir.y *= 0;
        TargetPosition += newDir;
    }

    private void IncreaseTargetDistance()
    {
        TargetPosition += Vector3.forward;
    }

    private void StopCamera()
    {

    }

    private void OnEnable()
    {
        //Connect all Events
        InputConponent.OnDirectionConfirmed += DirectionConfirmed;
        InputConponent.OnDirectionChanged += ChangeCameraX;
        GameManager.OnNewRowAchieved += IncreaseTargetDistance;
    }

    private void OnDisable()
    {
        //Disconnect all Events
        InputConponent.OnDirectionConfirmed -= DirectionConfirmed;
        InputConponent.OnDirectionChanged -= ChangeCameraX;
        GameManager.OnNewRowAchieved -= IncreaseTargetDistance;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        //draw target pos
        if (TargetPosition.z - transform.position.z < LinearAccelDistance)
            return;

        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(TargetPosition, 0.2f);
    }
#endif
}
