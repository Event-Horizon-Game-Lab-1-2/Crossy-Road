using System;
using System.Collections;
using TMPro;
using UnityEngine;
using static DeathTypeClass;

public class CameraComponent : MonoBehaviour
{
    [Header("Camera Settings")]
    [Tooltip("Time to reach target position")]
    [SerializeField] private float SmootTime = 1f;
    [Tooltip("Time to reset the camera position during idling death")]
    [SerializeField] private float ResumeSpeed = 0.5f;
    [Tooltip("Distance below wich the camera will keep moving linearly")]
    [SerializeField] private float LinearAccelDistance = 1f;
    [Tooltip("Speed of the camera when is moving linearly")]
    [SerializeField] private float LinearAccelSpeed = 1f;
    [Tooltip("Target on which the camera will zoom on")]
    [SerializeField] private Transform Target;
    //[Tooltip("Percentage of the distance covered when zooming in")]
    //[SerializeField][Range(0f, 1f)] private float ZoomPercentage = 0.8f;
    //[Tooltip("Speed of zoom in action")]
    //[SerializeField] private float ZoomInSpeed = 1.5f;

    private Vector3 Velocity;
    private Vector3 CameraOffset = Vector3.zero;
    private float InitialZOffset = 0;
    private float VelocityFloat = 0f;


    private void Start()
    {
        InitialZOffset = GetDistToTarget();
        CameraOffset = transform.position - Target.position;
    }

    private void StartMoving()
    {
        StartCoroutine(FollowTarget());
        InputComponent.OnDirectionConfirmed -= StartMoving;
    }

    private IEnumerator FollowTarget()
    {
        float StartX = Target.position.x + transform.position.x;
        while (true)
        {
            //linear Acceleration
            if(GetDistToTarget() < LinearAccelDistance)
            {
                transform.position += Vector3.forward * Time.deltaTime * LinearAccelSpeed;
            }
            //smooth Movement
            else
            {
                transform.position = Vector3.SmoothDamp(transform.position, new Vector3(transform.position.x, transform.position.y, Target.position.z - InitialZOffset), ref Velocity, SmootTime);
            }

            float smoothX = Mathf.SmoothDamp(transform.position.x, Target.position.x + CameraOffset.x, ref VelocityFloat, SmootTime);
            transform.position = new Vector3(smoothX, transform.position.y, transform.position.z);
            yield return null;
        }
    }

    private IEnumerator GoBack()
    {
        Vector3 initialPos = transform.position;
        while (true)
        {
            transform.position = Vector3.SmoothDamp(transform.position, initialPos + (Vector3.back * InitialZOffset), ref Velocity, ResumeSpeed);
            yield return null;
        }
    }

    private float GetDistToTarget()
    {
        return Mathf.Abs(transform.position.z - Target.position.z) - InitialZOffset;
    }

    private void ResumePos()
    {
        StartCoroutine(GoBack());
    }

    private void OnPlayerDeath(DeathType deathType)
    {
        StopAllCoroutines();
        switch (deathType)
        {
            case DeathType.Idling:
            {
                ResumePos();
                break;
            }
            default:
            {
                break;
            }
        }
    }

    private void OnEnable()
    {
        //Connect all Events
        InputComponent.OnDirectionConfirmed += StartMoving;
        PlayerManager.OnDeath += (DeathType deathType) => OnPlayerDeath(deathType);
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        DisableEvents();
    }

    private void DisableEvents()
    {
        //Disconnect all Events
        InputComponent.OnDirectionConfirmed -= StartMoving;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        //if (Application.isPlaying)
        //    return;

        //draw zoom target
        if (Target != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(Target.position, 0.2f);
            //draw zoom distance
            //Gizmos.color = Color.blue;
            //Gizmos.DrawSphere(Vector3.Lerp(transform.position, Target.position, ZoomPercentage), 0.2f);
        }
    }
#endif
}
