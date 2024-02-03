using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Row : MonoBehaviour
{
    [Header("Row Setting")]
    [Tooltip("Spawner activated on enable")]
    [SerializeField] Spawner RowSpawner;

    private void OnEnable()
    {
        //RowSpawner.Spawn();
    }
}
