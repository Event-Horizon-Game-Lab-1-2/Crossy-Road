using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class CameraComponent : MonoBehaviour
{
    [Header("Camera Settings")]
    [Tooltip("Time to reach target position")]
    [SerializeField] private float SmootTime = 0.25f;
    [Tooltip("Distance below wich the camera will keep moving linearly")]
    [SerializeField] private float LinearAccelDistance = 1f;
    [Tooltip("Speed of the camera when is moving linearly")]
    [SerializeField] private float LinearAccelSpeed = 1f;
    [Tooltip("Target on which the camera will zoom on")]
    [SerializeField] private Transform ZoomTarget;
    [Tooltip("Percentage of the distance covered when zooming in")]
    [SerializeField][Range(0f, 1f)] private float ZoomPercentage;
    [Tooltip("Speed of zoom in action")]
    [SerializeField] private float ZoomInSpeed;
    
    private bool IsMoving;
    //local smooting time
    private float SmootingTime;
    //velocity ref as float used is sideway movement
    private float VelocityFloat = 0f;
    //velocity ref as vector3 used in forward movement
    private Vector3 VelocityV3 = Vector3.zero;
    //target position 
    private Vector3 TargetPosition = Vector3.zero;

    private bool Disabled = false;
    private bool UpdateX = false;

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

    IEnumerator ZoomIn()
    {
        Vector3 startPos = transform.position;
        while(true)
        {
            transform.position = Vector3.SmoothDamp(transform.position, Vector3.Lerp(startPos, ZoomTarget.position, ZoomPercentage), ref VelocityV3, ZoomInSpeed);
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

    private void ChangeCameraX()
    {
        UpdateX = true;
    }

    private void ChangeCameraX(Vector3 newDir)
    {
        if(UpdateX == true)
        {
            //move only on x axis -> set to 0 z and y
            newDir.z *= 0;
            newDir.y *= 0;
            TargetPosition += newDir;
            UpdateX = false;
        }
    }

    private void IncreaseTargetDistance()
    {
        TargetPosition += Vector3.forward;
    }

    private void StopCamera()
    {
        Disabled = true;
        StopAllCoroutines();
        StartCoroutine(ZoomIn());
        DisableEvents();
    }

    private void OnEnable()
    {
        //Connect all Events
        MovementComponent.OnMove += ChangeCameraX;
        InputComponent.OnDirectionChanged += ChangeCameraX;
        GameManager.OnNewRowAchieved += () => { IncreaseTargetDistance(); DirectionConfirmed(); };
    }

    private void OnDisable()
    {
        DisableEvents();
    }

    private void DisableEvents()
    {
        //Disconnect all Events
        MovementComponent.OnMove -= ChangeCameraX;
        InputComponent.OnDirectionChanged -= ChangeCameraX;
        GameManager.OnNewRowAchieved -= () => { IncreaseTargetDistance(); DirectionConfirmed(); };
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
            return;

        //draw zoom target
        if(ZoomTarget != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(ZoomTarget.position, 0.2f);
            //draw zoom distance
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(Vector3.Lerp(transform.position, ZoomTarget.position, ZoomPercentage), 0.2f);
        }
        
        if (!IsMoving)
            return;
        //draw target pos
        if (TargetPosition.z - transform.position.z < LinearAccelDistance)
            return;
        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(TargetPosition, 0.2f);
    }
#endif
}
