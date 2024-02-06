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
    }

    public override void Spawn()
    {
        int numberOfSpaws = UnityEngine.Random.Range(MinSpawnAmount, MaxSpawnAmount);
        float objectToSpawn = UnityEngine.Random.value;

        //Spawn on center spawn an obstacle at the row center
        if(SpawnOnCenter)
            ObjectSpawned.Add(Instantiate(ObjectsToSpawn[0].ObjectTransform, transform.position + SpawnTilesOffset + (Vector3.right * (SpawnTilesAmount / 2) ), Quaternion.identity));

        for (int i = 0; i < numberOfSpaws; i++)
        {
            int newObjectIndex = GetObjectToSpawn(objectToSpawn);
            ObjectSpawned.Add(Instantiate(ObjectsToSpawn[newObjectIndex].ObjectTransform, GetSpawnTile(), Quaternion.identity));
        }
    }

    private int GetObjectToSpawn(float probability)
    {
        for (int i = 0; i < ObjectsToSpawn.Length; i++)
        {
            if (ObjectsToSpawn[i].SpawnProbability < probability)
                return i;
        }
        return (int)UnityEngine.Random.Range(0, ObjectsToSpawn.Length);
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
