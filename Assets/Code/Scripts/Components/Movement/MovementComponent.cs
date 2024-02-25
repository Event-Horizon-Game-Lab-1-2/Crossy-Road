using System.Collections;
using UnityEngine;
using static DeathTypeClass;

public class MovementComponent : MonoBehaviour
{
    public delegate void Move();
    public static event Move OnMove = new Move(() => { });

    public float raycastDistance = 1.5f; 
    public float RaycastFloorLenght = 2f;

    Vector3 dirToGo;

    private Transform TileTransform;

    IEnumerator StayOnTile()
    {
        while (TileTransform)
        {
            gameObject.transform.position = TileTransform.position + Vector3.up / 2;
            yield return null;
        }
    }

    private void OnEnable()
    {
        InputComponent.OnDirectionChanged += DirectionChanged;
        InputComponent.OnDirectionConfirmed += DirectionConfirmed;
        PlayerManager.OnDeath += OnDeath;
    }

    private void OnDeath(DeathType deathType)
    {
        if (deathType == DeathType.Idling)
            StopAllCoroutines();
        else
            SuspendMovement();
    }

    private void OnDisable()
    {
        InputComponent.OnDirectionChanged -= DirectionChanged;
        InputComponent.OnDirectionConfirmed -= DirectionConfirmed;
        OnMove -= OnMove;
    }


    void DirectionChanged(Vector3 direction)
    {
        dirToGo = direction;
    }

    void DirectionConfirmed()
    {

        if (CanMove())
        {
            // calcola la posizione di destinazione in base alla direzione e alla distanza
            CheckTile();

            StopAllCoroutines();
            StartCoroutine(StayOnTile());

            //MeshHolder.position -= dirToGo;

            OnMove();

        }

        else
        {
            TileTransform = null;
        }
    }

    void CheckTile()
    {   
        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up / 2 + dirToGo, Vector3.down, out hit, RaycastFloorLenght))
        {   
            StopAllCoroutines();
            TileTransform = hit.transform;
            // se il raggio colpisce qualcosa, fai qualcosa
            Debug.DrawRay(transform.position + Vector3.up / 2 + dirToGo, Vector3.down * RaycastFloorLenght, Color.red, 10f);
            transform.position = TileTransform.position;
        }
        else
        {
            TileTransform = null;
        }
    }

    bool CanMove()
    {
        Debug.DrawRay(transform.position, dirToGo, Color.magenta, 10f);
        return !Physics.Raycast(transform.position + Vector3.up / 2, dirToGo, out RaycastHit hit, raycastDistance, 1 << 3);
    }

    void SuspendMovement()
    {
        InputComponent.OnDirectionChanged -= DirectionChanged;
        InputComponent.OnDirectionConfirmed -= DirectionConfirmed;
        PlayerManager.OnDeath -= OnDeath;
        OnMove -= OnMove;
    }
}