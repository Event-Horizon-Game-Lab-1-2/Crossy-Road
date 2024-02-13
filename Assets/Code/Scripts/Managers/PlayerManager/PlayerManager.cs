using UnityEngine;
using static DeathTypeClass;

public class PlayerManager : MonoBehaviour
{
    public delegate void DeathTriggered(DeathType deathType);
    public static event DeathTriggered OnDeath;

    private bool dead = false;

    private void OnBecameInvisible()
    {
        if (dead)
            return;
        OnDeath(DeathType.Idling);
        dead = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (dead)
            return;
        if (other == null)
            return;

        DeathTrigger deathTrigger = other.gameObject.GetComponent<DeathTrigger>();
        if (deathTrigger == null)
            return;
        else
        {
            dead = true;
            OnDeath(deathTrigger.deathType);
        }
    }
}
