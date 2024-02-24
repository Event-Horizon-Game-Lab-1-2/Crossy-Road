using UnityEngine;
using static DeathTypeClass;

public class PlayerManager : MonoBehaviour
{
    public delegate void DeathTriggered(DeathType deathType);
    public static event DeathTriggered OnDeath = new DeathTriggered( (DeathType deathType) => { } );

    private bool Dead;
    private bool CanDie;

    private void Awake()
    {
        Dead = false;
        CanDie = true;
    }

    private void OnBecameInvisible()
    {
        if (GameManager.Resetting)
            return;
        if (Dead)
            Destroy(gameObject, 1f);
        if (OnDeath != null && !Dead)
            OnDeath(DeathType.Idling);
        Dead = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (Dead)
            return;
        if (other == null)
            return;

        DeathTrigger deathTrigger = other.gameObject.GetComponent<DeathTrigger>();
        if (deathTrigger == null)
            return;
        else
        {
            Dead = true;
            OnDeath(deathTrigger.deathType);
            if (deathTrigger.deathType == DeathType.Idling)
                CanDie = false;
        }
    }

    private void OnDisable()
    {
        OnDeath -= OnDeath;
    }
}
