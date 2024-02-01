using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

public class MapManager : MonoBehaviour
{

    [SerializeField] private MapGenerationData[] Data = new MapGenerationData[4];
    [SerializeField] private int MaxVisibleRows = 10;

    [SerializeField] List<Transform> GeneratedRows = null;

    private void Awake()
    {
        int ck = 0;
        for (int i = 0; i < Data.Length; i++)
            ck += Data[i].MaxRows;
        if (ck < MaxVisibleRows)
            Debug.LogError("ERROR: UNABLE TO GENERATE MAP\nMax Raws is lower than Max Visible Raws");
    }

    private void AddRawAtTheEnd()
    {

    }

    //Active in hierarchy
}
