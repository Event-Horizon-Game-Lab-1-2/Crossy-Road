using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public class CameraConponent : MonoBehaviour
{
    [SerializeField] private float SpeedChangeTimer = 1;
    [SerializeField][Range(0.1f, 5f)] float speed = 0.5f;
    private bool IsMoving;

    Vector3 PlayerDirection;
    private Transform HolderTransform;
    private void DirectionChanged(Vector3 direction)
    {
        PlayerDirection = direction;
    }

    private void DirectionConfirmed()
    {
        if(!IsMoving)
            StartCoroutine(Move());
    }

    IEnumerator Move()
    {
        IsMoving = true;
        while(true)
        {
            transform.position += Vector3.forward*Time.deltaTime;
            yield return null;
        }
    }

    void OnEnable()
    {
        //Connect all Events
        InputConponent.OnDirectionChanged += DirectionChanged;
        InputConponent.OnDirectionConfirmed += DirectionConfirmed;
    }

    private void OnDisable()
    {
        //Disconnect all Events
        InputConponent.OnDirectionChanged -= DirectionChanged;
        InputConponent.OnDirectionConfirmed -= DirectionConfirmed;
    }

    
}
