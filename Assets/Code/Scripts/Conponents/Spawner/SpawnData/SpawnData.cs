using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnData: MonoBehaviour
{
    [SerializeField] public Transform ObjectTransform;
    [SerializeField][Range(0f, 1f)] public float SpawnProbability;
}
