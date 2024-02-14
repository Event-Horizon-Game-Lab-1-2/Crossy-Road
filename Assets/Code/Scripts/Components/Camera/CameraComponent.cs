using System;
using System.Collections;
using UnityEditor.Rendering;
using UnityEngine;

public class CameraComponent : MonoBehaviour
{
    [Header("Camera Settings")]
    [Tooltip("Time to reach target position")]
    [SerializeField] private float SmootTime = 0.25f;
    [Tooltip("Time to reset the camera position during idling death")]
    [SerializeField] private float ResumeSpeed = 0.5f;
    [Tooltip("Distance below wich the camera will keep moving linearly")]
    [SerializeField] private float LinearAccelDistance = 1f;
    [Tooltip("Speed of the camera when is moving linearly")]
    [SerializeField] private float LinearAccelSpeed = 1f;
    [Tooltip("Target on which the camera will zoom on")]
    [SerializeField] private Transform Target;
    [Tooltip("Percentage of the distance covered when zooming in")]
    [SerializeField][Range(0f, 1f)] private float ZoomPercentage = 0.8f;
    [Tooltip("Speed of zoom in action")]
    [SerializeField] private float ZoomInSpeed = 1.5f;
    
    private bool IsMoving;
    //local smooting time
    private float SmootingTime;
    //velocity ref as float used is sideway movement
    private float VelocityFloat = 0f;
    //velocity ref as vector3 used in forward movement
    private Vector3 VelocityV3 = Vector3.zero;
    //target position 
    private Vector3 TargetPosition = Vector3.zero;
    //target offset
    private Vector3 TargetOffset = Vector3.zero;

    private void Awake()
    {
        IsMoving = false;
        TargetPosition = transform.position;
        SmootingTime = SmootTime;
        TargetOffset = transform.position - Target.position;
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
                //keep moving linearly
                transform.position += Vector3.forward * Time.deltaTime * LinearAccelSpeed;
            }

            //change x value
            float smoothX = Mathf.SmoothDamp(transform.position.x, Target.position.x + TargetOffset.x, ref VelocityFloat, SmootingTime);
            transform.position = new Vector3(smoothX, transform.position.y, transform.position.z);

            yield return null;
        }
    }

    IEnumerator ZoomIn()
    {
        //resume position
        yield return StartCoroutine(ResumePosition());
        //zoom variables
        Vector3 startPos = transform.position;    
        Vector3 endPos = Vector3.Lerp(transform.position, Target.position, ZoomPercentage);
        float progress = 0f;
        //zoom in
        while (progress <= 1f)
        {
            transform.position = Vector3.Lerp(startPos, endPos, progress);
            progress += Time.deltaTime * ZoomInSpeed;
            yield return null;
        }
    }

    IEnumerator ResumePosition()
    {
        StopCoroutine(MoveCamera());
        IsMoving = false;
        Vector3 startPos = transform.position;
        float progress = 0f;
        //resume position
        while (progress <= 1f)
        {
            transform.position = Vector3.Lerp(transform.position, startPos + Vector3.back * 5f, progress);
            progress += Time.deltaTime * ResumeSpeed;
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

    private void IncreaseTargetDistance()
    {
        TargetPosition += Vector3.forward;
    }

    private void StopCamera()
    {
        StartCoroutine(ZoomIn());
        DisableEvents();
    }

    private void OnEnable()
    {
        //Connect all Events
        InputComponent.OnDirectionConfirmed += DirectionConfirmed;
        GameManager.OnNewRowAchieved += IncreaseTargetDistance;
        GameManager.OnPlayerDeath += () => StopCamera();
    }

    private void OnDisable()
    {
        DisableEvents();
    }

    private void DisableEvents()
    {
        //Disconnect all Events
        InputComponent.OnDirectionConfirmed -= DirectionConfirmed;
        GameManager.OnNewRowAchieved -= IncreaseTargetDistance;
        GameManager.OnPlayerDeath -= () => StopCamera();
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        //if (Application.isPlaying)
        //    return;

        //draw zoom target
        if(Target != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(Target.position, 0.2f);
            //draw zoom distance
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(Vector3.Lerp(transform.position, Target.position, ZoomPercentage), 0.2f);
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
