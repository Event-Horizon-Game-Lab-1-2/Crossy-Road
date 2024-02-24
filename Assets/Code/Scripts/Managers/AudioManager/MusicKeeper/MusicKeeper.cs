using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicKeeper : MonoBehaviour
{
    public static MusicKeeper instance;

    [SerializeField] private AudioSource MusicSource;

    private void Awake()
    {
        if (instance)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);
        MusicSource.Play();
    }
}
