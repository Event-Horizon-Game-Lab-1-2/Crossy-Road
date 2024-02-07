using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MapGenerationData
{
    [SerializeField] public Row RowPrefab;
    [SerializeField] public int MaxConsecutiveRows;
    [SerializeField][Range(0f, 1f)] public float SpawnProbability;
    [HideInInspector] public float RowContinuityProbability;
}
