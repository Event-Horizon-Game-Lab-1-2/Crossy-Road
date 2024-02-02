using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{

    [SerializeField] private MapGenerationData[] Data = new MapGenerationData[4];
    [SerializeField] private int InitialSafeZone = 5;
    [SerializeField] private int MaxVisibleRows = 10;

    List<Transform> GeneratedRows = null;

    private int CurrentRowIndex = 0;
    private int ContinuousRow = 0;

    public int RowCount = 0;

    private void Awake()
    {
        Data[CurrentRowIndex].RowContinuityProbability = 1f;
        GeneratedRows = new List<Transform>();

        //check if the sum of probability is != 1 -> error
        float ck = 0f;
        for (int i = 0; i < Data.Length; i++)
            ck += Data[i].SpawnProbability;
        ck = Mathf.Round(ck);
        if (ck != 1f)
        {
            Debug.LogError("UNABLE TO GENERATE MAP:\n\t-Sum of all spawn probabilities is not 1");
            this.enabled = false;
        }
    }

    private void Start()
    {
        for (int i = 0; i< InitialSafeZone; i++)
            SpawnRow(0);

        for (int i = 0; i < MaxVisibleRows - InitialSafeZone; i++)
            GenerateNewRow();
    }

    private void GenerateNewRow()
    {
        if(ChangeRow())
        {
            //chose new row type
            //Reset ContinuousRow
            //Reset Data[CurrentRowIndex].RowContinuityProbability

            CurrentRowIndex = GetNewRowType();

            //reset row type values
            ContinuousRow = 0;
            Data[CurrentRowIndex].RowContinuityProbability = 1;
        }
        SpawnRow();
    }
    
    private bool ChangeRow()
    {
        return Data[CurrentRowIndex].RowContinuityProbability <= UnityEngine.Random.value;
    }

    private int GetNewRowType()
    {
        int newRowIndex = 0;
        //get new row type
        for (int i = 0; i < Data.Length; i++)
        {
            if (i == CurrentRowIndex)
                continue;
            if (UnityEngine.Random.value < Data[i].SpawnProbability)
                newRowIndex = i;
        }
        if (newRowIndex == CurrentRowIndex)
            return GetNewRowType();
        return newRowIndex;
    }
    
    private void SpawnRow()
    {
        //Instanciate new Row
        Transform newRow = Instantiate(Data[CurrentRowIndex].RowPrefab.transform, Vector3.forward * RowCount, Quaternion.identity).transform;
        GeneratedRows.Add(newRow);
        ContinuousRow++;
        RowCount++;

        //Update spawns values
        Data[CurrentRowIndex].RowContinuityProbability = (float)(Data[CurrentRowIndex].MaxConsecutiveRows - ContinuousRow) / (float)Data[CurrentRowIndex].MaxConsecutiveRows;
    }

    private void SpawnRow(int index)
    {
        //Instanciate new Row
        Transform newRow = Instantiate(Data[index].RowPrefab.transform, Vector3.forward * RowCount, Quaternion.identity).transform;
        GeneratedRows.Add(newRow);
        RowCount++;
    }

    private void RemoveLastRow()
    {
        GeneratedRows.RemoveAt(0);
        Destroy(GeneratedRows[0].gameObject);
    }

    //Connect all events
    private void OnEnable()
    {
        GameManager.OnNewRawAchieved += GenerateNewRow;
        GameManager.OnNewRawAchieved += RemoveLastRow;
    }
    //Disconnect all events
    private void OnDisable() 
    {
        GameManager.OnNewRawAchieved -= GenerateNewRow;
        GameManager.OnNewRawAchieved -= RemoveLastRow;
    }
    //Active in hierarchy
}
