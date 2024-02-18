using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TrainSpawner : Spawner
{
    //ObjectsToSpawn is taken from base class

    [Header("Spawner Option")]

    //bounds for the spawner
    [Tooltip("Start bound for spawners")]
    [SerializeField] private float SpawnStartPos = 0f;
    [Tooltip("End bound for spawners")]
    [SerializeField] private float SpawnEndPos = 0f;
    //Object amount
    [Tooltip("Max objects that can be spawned")]
    [SerializeField] private int MaxObject = 1;
    [Tooltip("Min objects that can be spawned")]
    [SerializeField] private int MinObject = 1;
    //Speed
    [Tooltip("Speed of the object")]
    [SerializeField] private float Speed = 1f;
    //Delay between Spawn
    [Tooltip("max wait time between object start moving")]
    [SerializeField] private float MaxWaitTime = 9f;
    [Tooltip("min wait time between object start moving")]
    [SerializeField] private float MinWaitTime = 4f;

    [Header("Special Options")]
    [Tooltip("Special Feature triggered before the object start moving")]
    [SerializeField] private SpecialObstacle SpecialFeature = null;
    [Tooltip("Special Feature trigger anticipation before the object start moving")]
    [SerializeField] private float SpecialFeatureAnticipation = 2f;

    [Header("Spawn Positions Options")]
    [SerializeField] private Color SpawnerPreviewColor_Start = Color.green;
    [SerializeField] private Color SpawnerPreviewColor_End = Color.red;

    private List<Transform> ListOfTransform = new List<Transform>();
    private Quaternion Rotation = Quaternion.identity;
    private bool StartLeft;

    private IEnumerator MoveObjects()
    {
        float lerpingValue = 0f;
        SpecialFeature.Trigger();
        while (true)
        {
            ListOfTransform[0].position = Vector3.Lerp(transform.position + Vector3.right * SpawnStartPos, transform.position + Vector3.right * SpawnEndPos, lerpingValue);
            //move model
            //show Special feature
            if (lerpingValue >= 1f)
            {
                ListOfTransform[0].position = transform.position + (Vector3.right * SpawnStartPos);
                SpecialFeature.Untrigger();
                float randomTime = UnityEngine.Random.Range(MinWaitTime, MaxWaitTime);
                yield return new WaitForSeconds(SpecialFeatureAnticipation);
                SpecialFeature.Trigger();
                yield return new WaitForSeconds(randomTime - SpecialFeatureAnticipation);
                lerpingValue = 0f;
            }

            lerpingValue += Time.deltaTime * Speed;
            //whait for one frame
            yield return null;
        }
    }

    public override void Spawn()
    {
        //Get random direction
        StartLeft = UnityEngine.Random.value > 0.5f;
        if (!StartLeft)
        {
            float c = SpawnStartPos;
            SpawnStartPos = SpawnEndPos;
            SpawnEndPos = c;
            Rotation = Quaternion.Euler(Vector3.up * -180);
        }
        else
            Rotation = Quaternion.Euler(Vector3.zero);

        //Instanciate only one object
        ListOfTransform.Add(Instantiate(ObjectsToSpawn[0].ObjectTransform, transform.position, Rotation));

        //Adjust generated object position
        //First object position defines the position of all the other
        //get the first object position
        ListOfTransform[0].position = transform.position + (Vector3.right * UnityEngine.Random.Range(SpawnStartPos, SpawnEndPos));

        //start moving the objects
        StartCoroutine(MoveObjects());
    }

    public void OnDestroy()
    {
        Array.Clear(ObjectsToSpawn, 0, ObjectsToSpawn.Length);
        for(int i = 0; i < ListOfTransform.Count; i++)
        {
            if(ListOfTransform[i] != null)
                Destroy(ListOfTransform[i].gameObject);
        }
        ListOfTransform.Clear();
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
