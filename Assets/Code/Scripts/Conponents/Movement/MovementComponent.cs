using System.Collections;
using System.Collections.Generic;
using TMPro;
//using System.Numerics;
using UnityEngine;

public class MovementComponent : MonoBehaviour
{

    //[SerializeField] public float speed = 5f;
    [SerializeField] public float time = 0.25f;


    public float raycastDistance = 5f; // Lunghezza del raggio
    public float downwardOffset = 0.5f; // Offset verso il basso

    Vector3 dir;

    private void OnEnable()
    {
        InputConponent.OnDirectionChanged += DirectionChanged;
        InputConponent.OnDirectionConfirmed += DirectionConfirmed;
    }

    private void OnDisable()
    {
        InputConponent.OnDirectionChanged -= DirectionChanged;
        InputConponent.OnDirectionConfirmed -= DirectionConfirmed;
    }

    IEnumerator MoveCoroutine(Vector3 targetPosition, float time)
    {
        Vector3 startPosition = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < time)

        {   //progressione dell'animazione in base al tempo
            float t = elapsedTime / time;

            // interpolazione lineare tra la posizione iniziale e la posizione finale
            Vector3 newPosition = Vector3.Lerp(startPosition, targetPosition, t);

            transform.position = newPosition;

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        transform.position = targetPosition;
    }

    void DirectionChanged(Vector3 direction)
    {
        dir = direction;
    }

    void DirectionConfirmed()
    {
        if (dir != Vector3.zero)
        {
            dir.Normalize();

            // calcola la posizione di destinazione in base alla direzione e alla distanza
            Vector3 targetPosition = transform.position + dir;

            // avvia la coroutine per spostarsi verso la posizione di destinazione
            StartCoroutine(MoveCoroutine(targetPosition, time));
        }
    }

    void MovementRaycast() //checka se ci sta il log del river con collider o river
    {
        //origine del raggio
        Vector3 raycastOrigin = transform.position;
        raycastOrigin.y -= downwardOffset; // sposta l'origine leggermente verso il basso

        // calcola la direzione del raggio (verso il basso)
        Vector3 raycastDirection = Vector3.down;

        
        RaycastHit hit;
        if (Physics.Raycast(raycastOrigin, raycastDirection, out hit, raycastDistance))
        {
            // se il raggio colpisce qualcosa, fai qualcosa
            Debug.DrawLine(raycastOrigin, hit.point, Color.red); 
            Debug.Log("Il raggio ha colpito " + hit.point);
        }
        else
        {
            Vector3 endPoint = raycastOrigin + raycastDirection * raycastDistance;
            Debug.DrawLine(raycastOrigin, endPoint, Color.green); 
            Debug.Log("Il raggio non ha colpito nulla, raggiunge il punto " + endPoint);
        }
    }
}


//if (direction != Vector3.zero)
//    direction.Normalize();


//Vector3 movement = direction * speed * Time.deltaTime;
//transform.Translate(movement);
