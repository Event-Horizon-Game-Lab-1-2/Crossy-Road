using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [Header("Map Generation Settings")]
    [SerializeField] private MapGenerationData[] Data = new MapGenerationData[4];
    [SerializeField] private int InitialSafeZone = 5;
    [SerializeField] private int MaxVisibleRows = 10;
    [Header("Gizsmo Settings")]
    [SerializeField] private Color SafeZoneColor = Color.white;
    [SerializeField] private Color MapZoneColor = Color.green;
    List<Transform> GeneratedRows = null;

    private int CurrentRowIndex = 0;
    private int ContinuousRow = 0;

    private int RowCount = 0;

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

    //Row type is chosen using a similar like Weighted Random Algorithm
    private int GetNewRowType()
    {
        int newRowIndex = 0;

        float[] probabilityArray = new float[Data.Length];
        probabilityArray[0] = Data[0].SpawnProbability;
        for (int i = 1; i < Data.Length; i++)
        {
            probabilityArray[i] = probabilityArray[i - 1] + Data[i].SpawnProbability;
        }
        //Select a random Spawner
        float randomValue = UnityEngine.Random.value;

        bool valueFound = false;
        for (int i = 0; i < probabilityArray.Length && !valueFound; i++)
        {
            if (randomValue <= probabilityArray[i])
            {
                newRowIndex = i;
                valueFound = true;
            }
        }

        if (newRowIndex == CurrentRowIndex)
            return GetNewRowType();

        return newRowIndex;
    }

    private void SpawnRow()
    {
        //Instanciate new Row
        Transform newRow = Instantiate(Data[CurrentRowIndex].RowPrefab.transform, transform.position + Vector3.forward * RowCount, Quaternion.identity).transform;
        GeneratedRows.Add(newRow);
        ContinuousRow++;
        RowCount++;

        //feature enabling
        Row row = GeneratedRows[GeneratedRows.Count - 1].GetComponent<Row>();

        //if is a even row enable the even feature
        if (row.FeatureOnEven)
        {
            if (ContinuousRow % 2 == 0)
                row.ShowFeature();
        }
        else if(ContinuousRow-1>0)
            row.ShowFeature();
        
        //Spawn Obstacles
        row.Spawn();

        //Update spawns values
        Data[CurrentRowIndex].RowContinuityProbability = (float)(Data[CurrentRowIndex].MaxConsecutiveRows - ContinuousRow) / (float)Data[CurrentRowIndex].MaxConsecutiveRows;
    }

    private void SpawnRow(int index)
    {
        //Instanciate new Row
        Transform newRow = Instantiate(Data[index].RowPrefab.transform, transform.position + Vector3.forward * RowCount, Quaternion.identity).transform;
        GeneratedRows.Add(newRow);
        ContinuousRow++;
        RowCount++;

        //feature enabling
        Row row = GeneratedRows[GeneratedRows.Count - 1].GetComponent<Row>();
        //if is a even row enable the even feature
        if (row.FeatureOnEven)
        {
            if (ContinuousRow % 2 == 0)
                row.ShowFeature();
        }
        else if (ContinuousRow - 1 > 0)
            row.ShowFeature();

        //Update spawns values
        Data[index].RowContinuityProbability = (float)(Data[index].MaxConsecutiveRows - ContinuousRow) / (float)Data[index].MaxConsecutiveRows;
    }

    private void RemoveLastRow()
    {
        GeneratedRows.RemoveAt(0);
        Destroy(GeneratedRows[0].gameObject);
    }

    //Connect all events
    private void OnEnable()
    {
        GameManager.OnNewRowAchieved += GenerateNewRow;
        GameManager.OnNewRowAchieved += RemoveLastRow;
    }
    //Disconnect all events
    private void OnDisable() 
    {
        GameManager.OnNewRowAchieved -= GenerateNewRow;
        GameManager.OnNewRowAchieved -= RemoveLastRow;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if(!UnityEngine.Application.isPlaying)
        {
            Gizmos.color = SafeZoneColor;
            for (int i = 0; i < InitialSafeZone; i++)
            {
                Gizmos.DrawWireCube((transform.position - Vector3.up/2) + Vector3.forward * i, new Vector3(15f, 1f, 1f));
            }

            Gizmos.color = MapZoneColor;
            for (int i = InitialSafeZone; i < MaxVisibleRows; i++)
            {
                Gizmos.DrawWireCube((transform.position - Vector3.up / 2) + Vector3.forward * i, new Vector3(15f, 1f, 1f));
            }
        }
    }

#endif
}
