using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DeathTypeClass;

public class DeathSound : MonoBehaviour
{
    [SerializeField] private AudioSource AudioSource;
    [SerializeField] private List<DeathSoundData> SquashDeathSounds;
    [SerializeField] private List<DeathSoundData> DrownDeathSounds;
    [SerializeField] private List<DeathSoundData> OutOfBoundDeathSounds;

    private void SelectSound(DeathType deathType)
    {
        switch (deathType)
        {
            case DeathType.Squash:
            {
                for (int i = 0; i < SquashDeathSounds.Count; i++)
                    StartCoroutine(PlaySound(SquashDeathSounds[i]));
                break;
            }
            case DeathType.Drown:
            {
                for (int i = 0; i < DrownDeathSounds.Count; i++)
                    StartCoroutine(PlaySound(DrownDeathSounds[i]));
                break;
            }
            case DeathType.OutOfBound:
            {
                for (int i = 0; i < OutOfBoundDeathSounds.Count; i++)
                    StartCoroutine(PlaySound(OutOfBoundDeathSounds[i]));
                break;
            }
        }
    }

    private IEnumerator PlaySound(DeathSoundData Data)
    {
        AudioSource.clip = Data.AudioClip;
        yield return new WaitForSeconds(Data.Delay);
        AudioSource.Play();
        while (AudioSource.time < AudioSource.clip.length)
        {
            yield return null;
        }
    }

    private void OnEnable()
    {
        PlayerManager.OnDeath += SelectSound;
    }
}
