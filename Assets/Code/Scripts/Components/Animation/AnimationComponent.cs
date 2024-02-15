using System.Collections;
using UnityEngine;
using static DeathTypeClass;

public class AnimationComponent : MonoBehaviour
{

    Vector3 direction;

    Transform TargetTransform;

    public float timeDurationSquishSquash = 0.2f;
    public float timeJump = 0.2f;

    public GameObject particles;

    [SerializeField] float movementTime = 0.1f;
    
    [SerializeField] Transform meshTransform;

    private bool IsDead = false;

    private void Awake()
    {
        TargetTransform = gameObject.transform;
    }

    private IEnumerator Squish() //movimento di quando si squisha mentre si holda un tasto
    {
        Vector3 lastScale = new Vector3(1, 0.5f, 1); 
        Vector3 firstScale = new Vector3(1, 1, 1);    

        float timePassed = 0f;
        while (timePassed < timeDurationSquishSquash)
        {
            timePassed += Time.deltaTime;

            float percentualeCompletamento = timePassed / timeDurationSquishSquash;

            Vector3 currentScale = Vector3.Lerp(firstScale, lastScale, percentualeCompletamento);
            meshTransform.localScale = currentScale;

            yield return null; 
        }
    }

    private void SquashFunc()
    {
        StartCoroutine(Squash());
    }

    private IEnumerator Squash() //movimento di quando torna allo stato normale 
    {
        Vector3 lastScale = new Vector3(1,1, 1);
        Vector3 firstScale = new Vector3(1, 0.5f, 1);

        float timePassed = 0f;
        while (timePassed < timeDurationSquishSquash)
        {

            float percentualeCompletamento = timePassed / timeDurationSquishSquash;

            Vector3 currentScale = Vector3.Lerp(firstScale, lastScale, percentualeCompletamento);
            meshTransform.localScale = currentScale;

            timePassed += Time.deltaTime;
            yield return null;
        }
    }
    
    private IEnumerator Rotate(Vector3 dirToGo) //quando cambia direzione ruota
    {
        Vector3 rotationToDo = Vector3.zero;
        direction = dirToGo;

        if   (dirToGo == new Vector3(1, 0, 0)) //davanti a destra
        {
            rotationToDo = new Vector3(0, 90, 0);
        }
        else if ( dirToGo == new Vector3(-1, 0, 0))  //davanti a sinistra 
        {
            rotationToDo = new Vector3(0, -90, 0);
        }
        else if (dirToGo == new Vector3(0, 0, 1))  //avanti
        {
            rotationToDo = new Vector3(0, 0, 0);
        }
        else if (dirToGo == new Vector3(0, 0, -1))  //dietro
        {
            rotationToDo = new Vector3(0, 180, 0); 
        }

        meshTransform.rotation = Quaternion.Euler(rotationToDo);

        yield return null;
    }


    private void Move()
    {
        StartCoroutine(Jump());
        StartCoroutine(MoveCoroutine());
    }
    
    private IEnumerator Jump() // Quando passa da una cella all'altra
    {
        Vector3 initialPosition = meshTransform.position;
        Vector3 finalPosition = meshTransform.position + Vector3.up;

        float timePassed = 0f;
        while (timePassed < timeJump)
        {
            timePassed += Time.deltaTime;

            float percentageComplete = timePassed / timeJump;

            
            //Vector3 newPosition = Vector3.Lerp(initialPosition, finalPosition, percentageComplete);
            float myHeight = Mathf.Lerp(initialPosition.y, finalPosition.y, percentageComplete);
            meshTransform.position = new Vector3 (meshTransform.position.x, myHeight,meshTransform.position.z);

            yield return null;
        }
        StartCoroutine(JumpDown());
    }


    private IEnumerator JumpDown()
    {
        
        Vector3 initialPosition = new Vector3(meshTransform.position.x, meshTransform.position.y +1, meshTransform.position.z);
        Vector3 finalPosition = new Vector3(meshTransform.position.x, 0, meshTransform.position.z);

        float timePassed = 0f;
        while (timePassed < timeJump)
        {
            timePassed += Time.deltaTime;

            float percentageComplete = timePassed / timeJump;

     
            //Vector3 newPosition = Vector3.Lerp(initialPosition, finalPosition, percentageComplete);
            float myHeight = Mathf.Lerp(initialPosition.y, finalPosition.y, percentageComplete);
            meshTransform.position = new Vector3(meshTransform.position.x, myHeight, meshTransform.position.z);


            yield return null;
        }
    }

    private IEnumerator Drown() //quando affoga nell'acqua >:D
    {
        StartCoroutine(Jump());
        yield return StartCoroutine(MoveCoroutine());

        Vector3 initialPosition = meshTransform.position;
        Vector3 finalPosition = new Vector3(meshTransform.position.x, -3f, meshTransform.position.z);

        float timePassed = 0f;

        Instantiate(particles, transform.position + (Vector3.up*0.25f), Quaternion.Euler(Vector3.left*90));

        while (timePassed < timeJump)
        {
            float percentageComplete = timePassed / timeJump;
            Vector3 newPosition = Vector3.Lerp(initialPosition, finalPosition, percentageComplete);
            //float myHeight = Mathf.Lerp(initialPosition.y, finalPosition.y, percentageComplete);
            meshTransform.position = newPosition;


            timePassed += Time.deltaTime;
            yield return null;
        }
        
        yield return null;
    }

    private IEnumerator SquishedByVehicle() //quando viene investito 
    {
        Vector3 lastScale = new Vector3(1.5f, 0.1f, 1);
        Vector3 firstScale = new Vector3(1, 1, 1);

        float timePassed = 0f;
        while (timePassed < 0.2f)
        {
            timePassed += Time.deltaTime;

            float percentualeCompletamento = timePassed / 0.2f;

            Vector3 currentScale = Vector3.Lerp(firstScale, lastScale, percentualeCompletamento);
            meshTransform.localScale = currentScale;

            yield return null;
        }
    }

    private IEnumerator MoveCoroutine()
    {
        if(!IsDead)
            TargetTransform.position += direction;
        
        Vector3 velocityRef = Vector3.zero;
         
        while (Vector3.Distance(meshTransform.position, TargetTransform.position) > 0.1f)

        {
            //smooth damp -> interpolazione che dipende dal tempo che viene passata
            meshTransform.position = Vector3.SmoothDamp(meshTransform.position, TargetTransform.position, ref velocityRef, movementTime);

            yield return null;
        }
        meshTransform.position = TargetTransform.position;
    }

    private void Die(DeathType deathType)
    {
        if (deathType == DeathType.Squash)
            StartCoroutine(SquishedByVehicle());
        else if (deathType == DeathType.Drown)
            StartCoroutine(Drown());
        //StartCoroutine(MoveCoroutine());
    }


    private void OnEnable()
    {
        StopAllCoroutines();

        InputComponent.OnDirectionChanged += (Vector3 dir) =>
        {
            StartCoroutine(Rotate(dir));
            StartCoroutine(Squish());
        };

        InputComponent.OnDirectionConfirmed += SquashFunc;

        MovementComponent.OnMove += Move;

        PlayerManager.OnDeath += Die;
    }

    private void OnDisable()
    {
        DisconnectAllEvents();
    }

    private void OnDestroy()
    {
        DisconnectAllEvents();
    }

    private void DisconnectAllEvents()
    {
        InputComponent.OnDirectionChanged -= (Vector3 dir) =>
        {
            StartCoroutine(Rotate(dir));
            StartCoroutine(Squish());
        };

        InputComponent.OnDirectionConfirmed -= SquashFunc;

        MovementComponent.OnMove -= Move;

        PlayerManager.OnDeath -= Die;

        StopAllCoroutines();
    }
}
