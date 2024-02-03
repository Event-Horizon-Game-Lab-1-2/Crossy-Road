using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Row : MonoBehaviour
{

    [Header("Row Setting")]
    [Tooltip("Spawner activated on enable")]
    [SerializeField] Spawner RowSpawner;
    [Tooltip("Feature visible when even, can be empty")]
    [SerializeField] Transform EvenFeature;

    private void Awake()
    {
        EvenFeature.gameObject.SetActive(false);
    }

    public void EvenRow()
    {
        if(EvenFeature.gameObject != null)
            EvenFeature.gameObject.SetActive(true);
    }
}
