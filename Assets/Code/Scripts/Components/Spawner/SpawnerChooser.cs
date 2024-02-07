using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpawnerChooser : MonoBehaviour
{
    [Header("Spawner Chooser Setting")]
    [Tooltip("Possible spawner for the row, the spawner is chosen once for each row")]
    [SerializeField] private SpawnerChooserData[] PossibleSpawners;

    private Spawner ChoosenSpawner;
    int SpawnerIndex = 0;

    public void Awake()
    {
        //disable if spawn is not possible
        float ck = 0f;
        for (int i = 0; i < PossibleSpawners.Length; i++)
            ck += PossibleSpawners[i].SpawnProbability;
        ck = Mathf.Round(ck);
        if (ck != 1f)
        {
            Debug.LogError("UNABLE TO CHOOSE SPAWNER:\n\t-Sum of all spawn probabilities is not 1");
            this.enabled = false;
        }

        //create probability array
        float[] probabilityArray = new float[PossibleSpawners.Length];
        probabilityArray[0] = PossibleSpawners[0].SpawnProbability;
        for (int i = 1; i < PossibleSpawners.Length; i++)
        {
            probabilityArray[i] = probabilityArray[i - 1] + PossibleSpawners[i].SpawnProbability;
        }
        //Select a random Spawner
        float randomValue = UnityEngine.Random.value;
        bool f = false;
        for (int i = 0; i < probabilityArray.Length && !f; i++)
        {
            if (randomValue <= probabilityArray[i])
            {
                SpawnerIndex = i;
                f = true;
            }
        }

        ChoosenSpawner = PossibleSpawners[SpawnerIndex].Spawner;
        Array.Clear(PossibleSpawners, 0, PossibleSpawners.Length);
    }

    public void Spawn()
    {
        ChoosenSpawner.Spawn();
    }
}
