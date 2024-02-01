using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raw : MonoBehaviour
{
    [Header("Raw Setting")]
    [Tooltip("Tiles upon wich the player can move")]
    [SerializeField] private Tile[] PlayableTiles = new Tile[9];


    private bool[] OccupiedTiles;

    private void Awake()
    {
        OccupiedTiles = new bool[PlayableTiles.Length];
    }

    private void Start()
    {

    }
}
