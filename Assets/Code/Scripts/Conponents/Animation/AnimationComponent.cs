using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationComponent : MonoBehaviour
{

    public IEnumerator Squish () //movimento di quando si schiaccia per più tempo il tasto
    {
        transform.localScale = new Vector3 (1,0.5f,1);
        transform.position = new Vector3(transform.position.x, -0.25f, transform.position.z);

        yield return null;
    }

    public IEnumerator Squash () //movimento di quando torna allo stato normale 
    {
        transform.localScale = new Vector3 (1,1,1);
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        yield return null;
    }

    public IEnumerator Rotate (Vector3 vector3) //quando cambia direzione ruota
    {
        
        yield return null;
    }

    public IEnumerator Jump () //quando passa da una cella all'altra
    {
        yield return null;
    }

    public IEnumerator Drown () //quando affoga nell'acqua >:D
    {
        yield return null;
    }

    public IEnumerator SquishedByVehicle () //quando viene investito 
    {
        yield return null;
    }
}
