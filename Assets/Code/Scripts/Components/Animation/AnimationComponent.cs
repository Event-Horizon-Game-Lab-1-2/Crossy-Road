using System.Collections;
using UnityEngine;
using static DeathTypeClass;

public class AnimationComponent : MonoBehaviour
{

    Vector3 direction;

    //Transform TargetTransform;

    public float timeDurationSquishSquash = 0.2f;

    public GameObject particles;


    [SerializeField] public float MeshSpeed = 5f;
    [SerializeField] public float DrownSpeed = 0.2f;
    [SerializeField] private float MaxJumpHeight = 1f;


    [SerializeField] Transform meshTransform;

    [HideInInspector] public Transform Target;

    [SerializeField] private AnimationCurve JumpCurve;


    private void Awake()

    {
        StartCoroutine(FollowTarget());
    }
    private IEnumerator FollowTarget()
    {
        //if(!IsDead)
        //    TargetTransform.position += direction;
        while (true)
        {
            if (Target != null)
            {

                if (Vector3.Distance(meshTransform.position, Target.position) > 0.1f)

                {

                    //smooth damp -> interpolazione che dipende dal tempo che viene passata
                    meshTransform.position = Vector3.Lerp(meshTransform.position, Target.position, Time.fixedDeltaTime * MeshSpeed);
                }
                else
                {
                    meshTransform.position = Target.position;
                }
            }
            //Vector3 velocityRef = Vector3.zero;


            yield return null;
        }

    }
    private IEnumerator ForceToTarget()
    {
        while (Vector3.Distance(meshTransform.position, Target.position) > 0.1f)
        {
            //smooth damp -> interpolazione che dipende dal tempo che viene passata
            meshTransform.position = Vector3.Lerp(meshTransform.position, Target.position, Time.fixedDeltaTime * MeshSpeed);

            yield return null;
        }
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
        Vector3 lastScale = new Vector3(1, 1, 1);
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

        if (dirToGo == new Vector3(1, 0, 0)) //davanti a destra
        {
            rotationToDo = new Vector3(0, 90, 0);
        }
        else if (dirToGo == new Vector3(-1, 0, 0))  //davanti a sinistra 
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
        StopCoroutine(FollowTarget());

        StartCoroutine(Jump());
        StartCoroutine(FollowTarget());
    }

    private bool isJumping = false;

    private IEnumerator Jump() // quando passa da una cella all'altra
    {

        if (isJumping) yield break;
        isJumping = true;

        Vector3 position;


        float timePassed = 0f;

        while (timePassed < 1)
        {
            timePassed += Time.deltaTime * 1 / InputComponent.InputRecoveryTime;

            float myHeight = JumpCurve.Evaluate(timePassed) * MaxJumpHeight;


            position = meshTransform.position;
            position.y = myHeight;
            meshTransform.position = position;

            yield return null;
        }
        isJumping = false;
    }


    private IEnumerator Drown() //quando affoga nell'acqua >:D
    {
        StopCoroutine(FollowTarget());
        //StartCoroutine(Jump());
        StartCoroutine(Jump());
        yield return StartCoroutine(ForceToTarget());

        Vector3 initialPosition = meshTransform.position;
        Vector3 finalPosition = new Vector3(meshTransform.position.x, -3f, meshTransform.position.z);

        float timePassed = 0f;

        Instantiate(particles, Target.position + (Vector3.up * 0.25f), Quaternion.Euler(Vector3.left * 90));

        while (timePassed < DrownSpeed)
        {
            float percentageComplete = timePassed / DrownSpeed;
            Vector3 newPosition = Vector3.Lerp(initialPosition, finalPosition, percentageComplete);
            //float myHeight = Mathf.Lerp(initialPosition.y, finalPosition.y, percentageComplete);
            meshTransform.position = newPosition;


            timePassed += Time.deltaTime;
            yield return null;
        }

        StopAllCoroutines();
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


    private void Die(DeathType deathType)
    {
        if (deathType == DeathType.Squash)
            StartCoroutine(SquishedByVehicle());
        else if (deathType == DeathType.Drown)
            StartCoroutine(Drown());
        //StartCoroutine(MoveCoroutine());
    }

    private void MoveEvent(Vector3 dir)
    {
        StartCoroutine(Rotate(dir));
        StartCoroutine(Squish());
    }

    private void OnEnable()
    {
        StopAllCoroutines();

        InputComponent.OnDirectionChanged += MoveEvent;

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
        InputComponent.OnDirectionChanged -= MoveEvent;

        InputComponent.OnDirectionConfirmed -= SquashFunc;

        MovementComponent.OnMove -= Move;

        PlayerManager.OnDeath -= Die;

        StopAllCoroutines();
    }
}
