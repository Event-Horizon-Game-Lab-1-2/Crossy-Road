using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DinamicSpawner : Spawner
{
    //ObjectsToSpawn is taken from base class

    [Header("Spawner Option")]
    [SerializeField] private float SpawnStartPos = 0f;
    [SerializeField] private float SpawnEndPos = 0f;
    [SerializeField] private int MinObject = 1;
    [SerializeField] private int MaxObject = 3;

    [Header("Spawn Positions Options")]
    [SerializeField] private Color SpawnerPreviewColor_Start = Color.green;
    [SerializeField] private Color SpawnerPreviewColor_End = Color.red;
    private bool StartLeft;
    Quaternion Rotation = Quaternion.identity;
    private List<Transform> listOfTransform = new List<Transform>();
    float Speed = 0.5f;

    private IEnumerator SpawnObject()
    {
        float lerpingValue = 0f;
        while(true)
        {
            for(int jj = 0; jj < listOfTransform.Count; jj++)
            {
                listOfTransform[jj].position = Vector3.Lerp(transform.position + Vector3.right * SpawnStartPos, transform.position + Vector3.right * SpawnEndPos, lerpingValue);
                //move each model
                if (lerpingValue >= 1f)
                {
                    listOfTransform[0].position = transform.position + (Vector3.right * SpawnStartPos);
                    lerpingValue = 0f;
                }

                lerpingValue += Time.deltaTime * Speed;
                //whait for one frame
                yield return null;
            }
        }
    }

    public override void Spawn()
    {
        StartLeft = UnityEngine.Random.value > 0.5f;
        if(!StartLeft)
        {
            float c = SpawnStartPos;
            SpawnStartPos = SpawnEndPos;
            SpawnEndPos = c;
            Rotation = Quaternion.Euler(Vector3.right);
        }
        listOfTransform.Add(Instantiate(ObjectsToSpawn[0].ObjectTransform, transform.position + (Vector3.right * SpawnStartPos), Rotation));
        StartCoroutine(SpawnObject());
    }

    public void OnDestroy()
    {
        Array.Clear(ObjectsToSpawn, 0, ObjectsToSpawn.Length);
        for(int i = 0; i < listOfTransform.Count; i++)
        {
            if(listOfTransform[i] != null)
                Destroy(listOfTransform[i].gameObject);
        }
        listOfTransform.Clear();
        StopAllCoroutines();
    }
    
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = SpawnerPreviewColor_Start;
        Gizmos.DrawWireCube((transform.position + Vector3.right * SpawnStartPos) + Vector3.up / 2, new Vector3(0f, 1f, 1f));
        Gizmos.color = SpawnerPreviewColor_End;
        Gizmos.DrawWireCube((transform.position + Vector3.right * SpawnEndPos) + Vector3.up / 2, new Vector3(0f, 1f, 1f));
    }
#endif
}
