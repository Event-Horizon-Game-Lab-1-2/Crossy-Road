using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnerChooserData
{
    [SerializeField] public Spawner Spawner;
    [SerializeField][Range(0f, 1f)] public float SpawnProbability;
}
