using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Row : MonoBehaviour
{
    [Header("Row Setting")]
    [Tooltip("Tiles upon wich the player can move")]
    [SerializeField] private Tile[] PlayableTiles = new Tile[9];
    [SerializeField] Spawner RowSpawner;
    
    private bool[] OccupiedTiles;

    private void Awake()
    {
        OccupiedTiles = new bool[PlayableTiles.Length];
    }

    private void Start()
    {

    }

}
