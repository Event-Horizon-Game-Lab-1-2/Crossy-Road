using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : Spawner
{
    //ObjectsToSpawn is taken from Spawner class

    [Header("Spawn Positions Options")]
    [SerializeField] private int SpawnTilesAmount = 9;
    [SerializeField] private Vector3 SpawnTilesOffset = Vector3.zero;
    [SerializeField] private bool SpawnOnCenter = false;
    [SerializeField] private bool AutoSpawn = false;
    [Space]
    [Header("Max Spawnable obstacles")]
    [SerializeField] private int MaxSpawnAmount = 5;
    [Header("Min Spawnable obstacles")]
    [SerializeField] private int MinSpawnAmount = 1;
    [Space]
    [Header("Spawn Gizsmos Options")]
    [SerializeField] private Color SpanwerPreviewColor = Color.magenta;
    private float GismoSize = 1f;

    List<Vector3> TilePosition = null;
    List<Transform> ObjectSpawned = null;

    private void Awake()
    {
        TilePosition = new List<Vector3>();
        //load all possible tiles into a list, except for the middle one
        for (int i = 0; i < SpawnTilesAmount; i++)
            TilePosition.Add(transform.position + SpawnTilesOffset + (Vector3.right * i));
        //remove the center Tile
        TilePosition.RemoveAt( SpawnTilesAmount / 2 );

        ObjectSpawned = new List<Transform>();
        if (AutoSpawn)
            Spawn();
    }

    public override void Spawn()
    {
        int numberOfSpaws = UnityEngine.Random.Range(MinSpawnAmount, MaxSpawnAmount);
        float objectToSpawn = UnityEngine.Random.value;

        //Spawn on center spawn an obstacle at the row center
        if(SpawnOnCenter)
        {
            Transform instance = Instantiate(ObjectsToSpawn[0].ObjectTransform, transform);
            instance.position = transform.position + SpawnTilesOffset + (Vector3.right * (SpawnTilesAmount / 2));
            ObjectSpawned.Add(instance);
        }

        for (int i = 0; i < numberOfSpaws; i++)
        {
            int newObjectIndex = GetObjectToSpawn(objectToSpawn);
            Transform instance = Instantiate(ObjectsToSpawn[newObjectIndex].ObjectTransform, transform);
            instance.position = GetSpawnTile();
            ObjectSpawned.Add(instance);
        }
    }

    private int GetObjectToSpawn(float probability)
    {
        //create probability array
        float[] probabilityArray = new float[ObjectsToSpawn.Length];
        probabilityArray[0] = ObjectsToSpawn[0].SpawnProbability;
        for (int i = 1; i < ObjectsToSpawn.Length; i++)
        {
            probabilityArray[i] = probabilityArray[i - 1] + ObjectsToSpawn[i].SpawnProbability;
        }
        //Select a random object
        float randomValue = UnityEngine.Random.value;
        bool f = false;
        for (int i = 0; i < probabilityArray.Length && !f; i++)
        {
            if (randomValue <= probabilityArray[i])
            {
                return i;
            }
        }
        return 0;
    }

    private Vector3 GetSpawnTile()
    {
        int newTile = UnityEngine.Random.Range(0, TilePosition.Count);
        Vector3 result = TilePosition[newTile];
        TilePosition.RemoveAt(newTile);
        return result;
    }

    private void OnDestroy()
    {
        for (int i = 0; i < ObjectSpawned.Count; i++)
        {
            if (ObjectSpawned[i] != null)
                Destroy(ObjectSpawned[i].gameObject);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = SpanwerPreviewColor;
        for (int i = 0; i < SpawnTilesAmount; i++)
        {
            Gizmos.DrawWireCube((transform.position + SpawnTilesOffset) + Vector3.right * i + Vector3.up / 2, Vector3.one * GismoSize);
        }
    }
#endif

}
