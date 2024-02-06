using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Objects to Spawn")]
    [SerializeField] public SpawnData[] ObjectsToSpawn = new SpawnData[2];

    public virtual void Spawn()
    {

    }
}
