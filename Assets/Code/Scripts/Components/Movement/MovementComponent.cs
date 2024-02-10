using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class MovementComponent : MonoBehaviour
{
    public delegate void Move();
    public static event Move OnMove;

    public float raycastDistance = 1.5f; // Lunghezza del raggio
    public float RaycastFloorLenght = 2f;

    public bool buttonPressed;

    Vector3 dirToGo;

    private Transform TileTransform;

    IEnumerator StayOnTile ()
    {
        while (TileTransform != null) { 

        transform.position = TileTransform.position + Vector3.up /2;
            yield return null;
        }
    }

    private void OnEnable()
    {
        InputComponent.OnDirectionChanged += DirectionChanged;
        InputComponent.OnDirectionConfirmed += DirectionConfirmed;
        
    }

    private void OnDisable()
    {
        InputComponent.OnDirectionChanged -= DirectionChanged;
        InputComponent.OnDirectionConfirmed -= DirectionConfirmed;
    }


    void DirectionChanged(Vector3 direction)
    {
        dirToGo = direction;
        
    }

    void DirectionConfirmed()
    {

        //MovementRaycast();

        if (CanMove() )
        {
            // calcola la posizione di destinazione in base alla direzione e alla distanza
           transform.position += dirToGo;
            StopAllCoroutines();
           CheckTile();
           OnMove();

        }

        else
        {
            TileTransform = null;
        }
    }
    
    bool CheckTile()
    {

        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up / 2, Vector3.down, out hit, RaycastFloorLenght))
        {
            // se il raggio colpisce qualcosa, fai qualcosa
            Debug.DrawLine(transform.position, hit.point, Color.red, 1f);

            TileTransform = hit.transform;
            StartCoroutine(StayOnTile());
            return false;
        }
        else
        {
            TileTransform = null;
        }

        return false;
    }

    bool CanMove()
    {

        Debug.DrawRay(transform.position, dirToGo, Color.magenta, 10f);
            return !Physics.Raycast(transform.position + Vector3.up /2, dirToGo, out RaycastHit hit, raycastDistance);
    }

    void SuspendMovement()
    {
        this.enabled = false;
    }
}


//if (direction != Vector3.zero)
//    direction.Normalize();


//Vector3 movement = direction * speed * Time.deltaTime;
//transform.Translate(movement);