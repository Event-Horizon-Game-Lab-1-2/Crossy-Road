using System.Collections;
using System.Collections.Generic;
using TMPro;
//using System.Numerics;
using UnityEngine;

public class MovementComponent : MonoBehaviour
{

    //[SerializeField] public float speed = 5f;
    [SerializeField] public float time = 0.25f;
    [SerializeField] Transform scaleSquish;


    public float raycastDistance = 5f; // Lunghezza del raggio
    public float downwardOffset = 0.5f; // Offset verso il basso

    public bool buttonPressed;

    Vector3 dirToGo;


    public AnimationComponent animationComponent;
    private void Start()
    {
       //animationComponent = GetComponent<AnimationComponent>();
    }


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

            //interpolazione lineare tra la posizione iniziale e la posizione finale
            Vector3 newPosition = Vector3.Lerp(startPosition, targetPosition, t);

            transform.position = newPosition;
            //float myX = Mathf.Lerp(startPosition.x, targetPosition.x, t);
            //float myZ = Mathf.Lerp(startPosition.z, targetPosition.z, t);
            //transform.position = new Vector3(myX, myY, t);

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        transform.position = targetPosition;
    }

    void DirectionChanged(Vector3 direction)
    {
        dirToGo = direction;
        //StopAllCoroutines();

        if (buttonPressed == false)
        {
            StartCoroutine(animationComponent.Squish());
            buttonPressed = true;
        }
        
        StartCoroutine(animationComponent.Rotate(dirToGo));

        //prevDir = dirToGo;
    }

    void DirectionConfirmed()
    {
        MovementRaycast();
        if (dirToGo != Vector3.zero)
        {
            //dir.Normalize();

            // calcola la posizione di destinazione in base alla direzione e alla distanza

            Vector3 targetPosition = transform.position + dirToGo;

            StartCoroutine(animationComponent.Squash());
            StartCoroutine(animationComponent.Jump());

            //if(!MovementRaycast())
            //    return;

            //StartCoroutine(animationComponent.Jump());
            // avvia la coroutine per spostarsi verso la posizione di destinazione
            StartCoroutine(MoveCoroutine(targetPosition, time));

            buttonPressed = false;
            
        }
    }

    bool MovementRaycast() //checka se ci sta il log del river con collider o river
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
            Debug.DrawLine(raycastOrigin, hit.point, Color.red, 1f);
            Debug.Log("Il raggio ha colpito " + hit.point);

            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Log"))
            {
                gameObject.transform.SetParent(hit.transform);

                //poi dovrebbe tornare non figlio ma non so come, se invece vuoi che prenda la pos del tile, idk either lol
            }

            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Water"))
            {
                StartCoroutine(animationComponent.Drown());
                //Destroy(GameObject);
            }


            return false;
        }
        else
        {
            Vector3 endPoint = raycastOrigin + raycastDirection * raycastDistance;
            Debug.DrawLine(raycastOrigin, endPoint, Color.green,1f);
            Debug.Log("Il raggio non ha colpito nulla, raggiunge il punto " + endPoint);
            return true;
        }
    }
}


//if (direction != Vector3.zero)
//    direction.Normalize();


//Vector3 movement = direction * speed * Time.deltaTime;
//transform.Translate(movement);