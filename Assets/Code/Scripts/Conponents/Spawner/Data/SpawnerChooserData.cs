using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerChooserData : MonoBehaviour
{
    [SerializeField] public Spawner Spawner;
    [SerializeField][Range(0f, 1f)] public float SpawnProbability;
}
