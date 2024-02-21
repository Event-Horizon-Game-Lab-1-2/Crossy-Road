using System.Collections;
using UnityEngine;
using static DeathTypeClass;

public class AnimationComponent : MonoBehaviour
{
    //Transform TargetTransform;
    public float timeDurationSquishSquash = 0.2f;
    public GameObject DeathParticles;

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
        //follow target except for y
        float progress = 0f;
        //get to target
        while (progress < 1f)
        {
            meshTransform.position = Vector3.Lerp(meshTransform.position, new Vector3(Target.position.x, meshTransform.position.y, Target.position.z), progress);
            progress += Time.fixedDeltaTime * MeshSpeed;
            yield return null;
        }

        //stay on target
        while(true)
        {
            if(Target.position == null)
                StopAllCoroutines();
            meshTransform.position = Target.position;
            yield return null;
        }
    }

    private IEnumerator ForceToTarget()
    {
        while(true)
        {
            if(Target.position == null)
                yield break;
            meshTransform.position = Target.position;
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
        StopAllCoroutines();
        StartCoroutine(Jump());
        StartCoroutine(FollowTarget());
    }

    private IEnumerator Jump() // quando passa da una cella all'altra
    {
        Vector3 newPos;
        float timePassed = 0f;

        while (timePassed < 1)
        {
            float myHeight = JumpCurve.Evaluate(timePassed) * MaxJumpHeight;
            newPos = meshTransform.position;
            newPos.y = myHeight;
            meshTransform.position = newPos;

            timePassed += Time.deltaTime * (1 / InputComponent.InputRecoveryTime);
            Debug.Log(timePassed);
            yield return null;
        }
    }


    private IEnumerator Drown() //quando affoga nell'acqua >:D
    {
        StartCoroutine(FollowTarget());
        yield return StartCoroutine(Jump());

        Vector3 initialPosition = meshTransform.position;
        Vector3 finalPosition = new Vector3(meshTransform.position.x, -3f, meshTransform.position.z);

        float timePassed = 0f;

        Instantiate(DeathParticles, Target.position + (Vector3.up * 0.25f), Quaternion.Euler(Vector3.left * 90));

        while (timePassed < 1f)
        {

            Vector3 newPosition = Vector3.Lerp(initialPosition, finalPosition, timePassed);
            meshTransform.position = newPosition;
            timePassed += Time.deltaTime * DrownSpeed;
            yield return null;
        }

        StopAllCoroutines();
    }

    private IEnumerator SquishedByVehicle() //quando viene investito 
    {
        Vector3 lastScale = new Vector3(1.5f, 0.1f, 1);
        Vector3 firstScale = new Vector3(1, 1, 1);

        meshTransform.position = new Vector3(meshTransform.position.x, 0f, meshTransform.position.z);

        float timePassed = 0f;
        while (timePassed < 0.2f)
        {
            timePassed += Time.deltaTime;

            float percentualeCompletamento = timePassed / 0.2f;

            Vector3 currentScale = Vector3.Lerp(firstScale, lastScale, percentualeCompletamento);
            meshTransform.localScale = currentScale;

            yield return null;
        }
        StopAllCoroutines();
    }


    private void Die(DeathType deathType)
    {
        DisconnectAllEvents();
        switch (deathType)
        {
            //Squash
            case DeathType.Squash:
                {
                    StopAllCoroutines();
                    StartCoroutine(SquishedByVehicle());
                    break;
                }
            //Drown
            case DeathType.Drown:
                {
                    StopAllCoroutines();
                    StartCoroutine(Drown());
                    break;
                }
            //Toothless Victory Royale
            case DeathType.Idling:
                {
                    MeshSpeed = float.MaxValue;
                    break;
                }
            case DeathType.OutOfBound:
                {
                    StartCoroutine(FollowTarget());
                    break;
                }
            default:
                break;
        }
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
    }
}