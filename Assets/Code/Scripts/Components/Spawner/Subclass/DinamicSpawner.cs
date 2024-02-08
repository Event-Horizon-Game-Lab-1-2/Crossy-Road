using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DinamicSpawner : Spawner
{
    //ObjectsToSpawn is taken from base class
    [Header("Spawner Option")]
    //bounds for the spawner
    [Tooltip("Start bound for spawners")]
    [SerializeField] private float SpawnStartPos = 0f;
    [Tooltip("End bound for spawners")]
    [SerializeField] private float SpawnEndPos = 0f;
    [Space]
    //Speed options
    [Header("Speed Option")]
    [Tooltip("If not selected the speed is the max speeds\nIf chosen the speed is chosen randomly between max speed and min speed")]
    [SerializeField] private bool RandomSpeed = false;
    //[Tooltip("If selected the speed is modified between start and start bound")]
    //[SerializeField] private bool UseSpeedCurve = false;
    //[Tooltip("Speed edit bounds, if the object is between the start/end pos and speed curve the speed is edited")]
    //[SerializeField][Range(0.0f, 10f)] private float SpeedCurveBound = 0f;
    //[Tooltip("Speed edit curve")]
    //[SerializeField] private AnimationCurve SpeedCurve;
    [Tooltip("Max Speed of the spawned objects")]
    [SerializeField] private float MaxSpeed = 1.5f;
    [Tooltip("Min Speed of the spawned objects")]
    [SerializeField] private float MinSpeed = 0.5f;
    [Space]
    //Object amount
    [Header("Object Settings")]
    [Tooltip("Max objects that can be spawned")]
    [SerializeField] private int MaxObject = 3;
    [Tooltip("Min objects that can be spawned")]
    [SerializeField] private int MinObject = 1;
    [Space]
    //Distance between objects
    [Tooltip("Max distance between objects")]
    [SerializeField] private float MaxDistance = 2f;
    [Tooltip("Min distance between objects")]
    [SerializeField] private float MinDistance = 1.5f;
    [Space]
    [Header("Spawn Gizmos Options")]
    [SerializeField] private Color SpawnerPreviewColor_Start = Color.green;
    [SerializeField] private Color SpawnerPreviewColor_End = Color.red;
    //[SerializeField] private Color SpeedEditBound_color = Color.magenta;
    [Space]

    private List<Transform> ListOfTransform = new List<Transform>();
    private Quaternion Rotation = Quaternion.identity;
    public bool StartLeft;
    private float Speed;
    private int ObjectSpawnAmount;

    private void Awake()
    {
        //Speed Update
        if (RandomSpeed)
        {
            //Select a random Speed
            Speed = UnityEngine.Random.Range(MinSpeed, MaxSpeed);
        }
        else
        {
            //Speed is the max speeds
            Speed = MaxSpeed;
        }
        //Spawn amount
        ObjectSpawnAmount = (int)UnityEngine.Random.Range(MinObject, MaxObject);
    }

    //Generate objects
    public override void Spawn()
    {
        //Get random direction
        StartLeft = UnityEngine.Random.value > 0.5f;
        if (!StartLeft)
        {
            float c = SpawnStartPos;
            SpawnStartPos = SpawnEndPos;
            SpawnEndPos = c;
            Rotation = Quaternion.Euler(Vector3.right);
        }

        //Get Spawn object type
        int objectToSpawnIndex = 0;

        //create probability array
        float[] probabilityArray = new float[ObjectsToSpawn.Length];
        probabilityArray[0] = ObjectsToSpawn[0].SpawnProbability;
        for (int i = 1; i < ObjectsToSpawn.Length; i++)
        {
            probabilityArray[i] = probabilityArray[i - 1] + ObjectsToSpawn[i].SpawnProbability;
        }
        //Select a random Spawner
        float randomValue = UnityEngine.Random.value;
        bool f = false;
        for (int i = 0; i < probabilityArray.Length && !f; i++)
        {
            if (randomValue <= probabilityArray[i])
            {
                objectToSpawnIndex = i;
                f = true;
            }
        }

        //Instanciate all object
        for (int i = 0; i < ObjectSpawnAmount; i++)
        {
            Transform instance = ObjectsToSpawn[objectToSpawnIndex].ObjectTransform;
            ListOfTransform.Add(Instantiate(instance, instance.position, Rotation));
        }

        //Adjust generated object position
        //First object position defines the position of all the other
        //get the first object position
        ListOfTransform[0].position = transform.position + (Vector3.right * UnityEngine.Random.Range(SpawnStartPos, SpawnEndPos));
        //load all positions based on the previous object position
        for (int i = 1; i < ListOfTransform.Count; i++)
        {
            ListOfTransform[i].position = ListOfTransform[i - 1].position + (Vector3.right * UnityEngine.Random.Range(MinDistance, MaxDistance));
        }

        //start moving the objects
        StartCoroutine(MoveObjects());
    }

    //Move The Objects
    private IEnumerator MoveObjects()
    {
        float[] progresses = new float[ListOfTransform.Count];
        //update all progresses
        for(int i= 0; i < progresses.Length; i++)
            progresses[i] = ListOfTransform[i].position.x/Mathf.Abs(SpawnStartPos - SpawnEndPos);
        
        //keep moving
        while (true)
        {
            //move spawned object of i index
            for (int i = 0; i < ListOfTransform.Count; i++)
            {

                ListOfTransform[i].position = transform.position + Vector3.Lerp(Vector3.right * SpawnStartPos, Vector3.right * SpawnEndPos, progresses[i]);
                progresses[i] += Time.deltaTime * Speed;
                if (progresses[i] >= 1f)
                    progresses[i] = 0f;
            }
            //whait for one frame
            yield return null;
        }
    }

    public void OnDestroy()
    {
        //Stop All
        StopAllCoroutines();
        //Destroy all spawned objects
        for (int i = 0; i < ListOfTransform.Count; i++)
        {
            if (ListOfTransform[i] != null)
                Destroy(ListOfTransform[i].gameObject);
        }
        //empty the list
        ListOfTransform.Clear();
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        //draw dinamic spawner border
        Gizmos.color = SpawnerPreviewColor_Start;
        Gizmos.DrawWireCube((transform.position + Vector3.right * SpawnStartPos) + Vector3.up / 2, new Vector3(0f, 1f, 1f));
        Gizmos.color = SpawnerPreviewColor_End;
        Gizmos.DrawWireCube((transform.position + Vector3.right * SpawnEndPos) + Vector3.up / 2, new Vector3(0f, 1f, 1f));

        //if (!UseSpeedCurve)
        //    return;
        ////Draw speed curve border
        //Gizmos.color = SpeedEditBound_color;
        //if(SpawnStartPos > 0)
        //{
        //    Gizmos.DrawWireCube((transform.position + Vector3.right * (SpawnStartPos - SpeedCurveBound)) + Vector3.up / 2, new Vector3(0f, 1f, 1f));
        //    Gizmos.DrawWireCube((transform.position + Vector3.right * (SpawnEndPos + SpeedCurveBound)) + Vector3.up / 2, new Vector3(0f, 1f, 1f));
        //}
        //else
        //{
        //    Gizmos.DrawWireCube((transform.position + Vector3.right * (SpawnStartPos + SpeedCurveBound)) + Vector3.up / 2, new Vector3(0f, 1f, 1f));
        //    Gizmos.DrawWireCube((transform.position + Vector3.right * (SpawnEndPos - SpeedCurveBound)) + Vector3.up / 2, new Vector3(0f, 1f, 1f));
        //}
    }
#endif
}
