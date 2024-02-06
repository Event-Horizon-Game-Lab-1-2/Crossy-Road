using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerationData : MonoBehaviour
{
    [SerializeField] public Row RowPrefab;
    [SerializeField] public int MaxConsecutiveRows;
    [SerializeField][Range(0f, 1f)] public float SpawnProbability;
    [HideInInspector] public float RowContinuityProbability;
}
