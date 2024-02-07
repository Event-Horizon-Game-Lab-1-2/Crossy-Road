using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class SpawnData
{
    [SerializeField] public Transform ObjectTransform;
    [SerializeField][Range(0f, 1f)] public float SpawnProbability;
}