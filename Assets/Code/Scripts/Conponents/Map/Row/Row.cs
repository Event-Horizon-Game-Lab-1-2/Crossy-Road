using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Row : MonoBehaviour
{

    [Header("Row Setting")]
    [Tooltip("Spawner activated on enable")]
    [SerializeField] Spawner RowSpawner;
    [Tooltip("Feature visible when raws is repeated, can be empty")]
    [SerializeField] Transform Feature;
    [Tooltip("Show feature when even, if false the feature will be shown when continuous")]
    [SerializeField] public bool FeatureOnEven = true;

    private void Awake()
    {
        Feature.gameObject.SetActive(false);
    }

    public void Spawn()
    {
        if (RowSpawner != null)
            RowSpawner.Spawn();
    }

    public void ShowFeature()
    {
        if(Feature.gameObject != null)
            Feature.gameObject.SetActive(true);
    }
}
