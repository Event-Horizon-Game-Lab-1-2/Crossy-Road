using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationComponent : MonoBehaviour
{
    
    Vector3 startingRotation;
    Vector3 rotationToDo;

    public float timeDurationSquishSquash = 0.2f;
    public float timeJump = 0.2f;

    public GameObject jumpingSubject;
    public IEnumerator Squish() //movimento di quando si squisha mentre si holda un tasto
    {
    Vector3 lastScale = new Vector3(1, 0.5f, 1); 
    Vector3 firstScale = new Vector3(1, 1, 1);    

    float timePassed = 0f;
    while (timePassed < timeDurationSquishSquash)
    {
        timePassed += Time.deltaTime;

        float percentualeCompletamento = timePassed / timeDurationSquishSquash;

        Vector3 currentScale = Vector3.Lerp(firstScale, lastScale, percentualeCompletamento);
        transform.localScale = currentScale;

        yield return null; 
    }
}


    public IEnumerator Squash() //movimento di quando torna allo stato normale 
    {
        Vector3 lastScale = new Vector3(1,1, 1);
        Vector3 firstScale = new Vector3(1, 0.5f, 1);

        float timePassed = 0f;
        while (timePassed < timeDurationSquishSquash)
        {
            timePassed += Time.deltaTime;

            float percentualeCompletamento = timePassed / timeDurationSquishSquash;

            Vector3 currentScale = Vector3.Lerp(firstScale, lastScale, percentualeCompletamento);
            transform.localScale = currentScale;

            yield return null;
        }

    }

    public IEnumerator Rotate(Vector3 dirToGo) //quando cambia direzione ruota
    {


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

        //ho provato a fare lo switch ma fallendo perchè mi dava solo errori ;-;


        transform.rotation = Quaternion.Euler(rotationToDo);


        yield return null;
    }

    public IEnumerator Jump() // Quando passa da una cella all'altra
    {
        Vector3 initialPosition = transform.position;
        Vector3 finalPosition = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);

        float timePassed = 0f;
        while (timePassed < timeJump)
        {
            timePassed += Time.deltaTime;

            float percentageComplete = timePassed / timeJump;

            
            //Vector3 newPosition = Vector3.Lerp(initialPosition, finalPosition, percentageComplete);
            float myHeight = Mathf.Lerp(initialPosition.y, finalPosition.y, percentageComplete);
            jumpingSubject.transform.position = new Vector3 (transform.position.x, myHeight,transform.position.z);


            StartCoroutine(JumpDown());
            yield return null;
        }
    }


    public IEnumerator JumpDown()
    {
        yield return new WaitForSeconds(timeJump);
        Vector3 initialPosition = new Vector3(transform.position.x, transform.position.y +1, transform.position.z);
        Vector3 finalPosition = new Vector3(transform.position.x, 0, transform.position.z);

        float timePassed = 0f;
        while (timePassed < timeJump)
        {
            timePassed += Time.deltaTime;

            float percentageComplete = timePassed / timeJump;

     
            //Vector3 newPosition = Vector3.Lerp(initialPosition, finalPosition, percentageComplete);
            float myHeight = Mathf.Lerp(initialPosition.y, finalPosition.y, percentageComplete);
            jumpingSubject.transform.position = new Vector3(transform.position.x, myHeight, transform.position.z);


            yield return null;
        }
    }

    public IEnumerator Drown() //quando affoga nell'acqua >:D
{
        Vector3 initialPosition = transform.position;
        Vector3 finalPosition = new Vector3(transform.position.x, transform.position.y -1, transform.position.z);

        float timePassed = 0f;
        while (timePassed < timeJump)
        {
            timePassed += Time.deltaTime;

            float percentageComplete = timePassed / timeJump;

           
            float myHeight = Mathf.Lerp(initialPosition.y, finalPosition.y, percentageComplete);
            transform.position = new Vector3(transform.position.x, myHeight, transform.position.z);


            StartCoroutine(JumpDown());
            yield return null;
        }  //per la caduta nel river
        yield return null;
}

    public IEnumerator SquishedByVehicle() //quando viene investito 
    {
        Vector3 lastScale = new Vector3(1.5f, 0.1f, 1);
        Vector3 firstScale = new Vector3(1, 1, 1);

        float timePassed = 0f;
        while (timePassed < 0.2f)
        {
            timePassed += Time.deltaTime;

            float percentualeCompletamento = timePassed / 0.2f;

            Vector3 currentScale = Vector3.Lerp(firstScale, lastScale, percentualeCompletamento);
            transform.localScale = currentScale;

            yield return null;
        }
    }
}
