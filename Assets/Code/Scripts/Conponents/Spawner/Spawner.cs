using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Spawner : MonoBehaviour
{
    [SerializeField] SpawnData[] ObjectsToSpawn = new SpawnData[2];
    Row SpawnOnRaw;

    private void Awake()
    {
        SpawnOnRaw = GetComponent<Row>();
    }

    public virtual void Spawn()
    {

    }
}
