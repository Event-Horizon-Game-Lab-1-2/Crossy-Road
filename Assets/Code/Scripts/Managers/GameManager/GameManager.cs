using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public delegate void NewRowAchieved();
    public static event NewRowAchieved OnNewRowAchieved;

    [Header("Starting Options")]
    //[SerializeField] private int StarRow = 5;

    [Header("Gizsmos Options")]
    [SerializeField] Vector3 PlayerRowVisualizer = new Vector3(15f, 5f, 1f);
    [SerializeField] Vector3 PlayerRowRecordVisualizer = new Vector3(15f, 5f, 1f);
    [SerializeField] Color PlayerRowVisualizerPlayColor = Color.blue;
    [SerializeField] Color PlayerRowVisualizerEditorColor = Color.magenta;
    [SerializeField] Color PlayerRecordVisualizerEditorColor = Color.black;

    //player Score
    [HideInInspector] public static int Score = 0;
    //Player raw Rappresent the raw on wich the player is on
    [HideInInspector] public int PlayerRow = 0;

    Vector3 PlayerDirection;
    private void Awake()
    {
        if(Instance == null)
            Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void DirectionChanged(Vector3 direction)
    {
        PlayerDirection = direction;
    }

    private void DirectionConfirmed()
    {
        //Update Player Raw number
        PlayerRow += (int)PlayerDirection.z;

        if(PlayerRow > Score)
        {
            Score = PlayerRow;
            OnNewRowAchieved();
        }
    }

    #region Utility
    private void Reset()
    {
        Score = 0;
        PlayerRow = 0;
    }

    void OnEnable()
    {
        //Connect all Events
        InputConponent.OnDirectionChanged += DirectionChanged;
        InputConponent.OnDirectionConfirmed += DirectionConfirmed;
    }

    private void OnDisable()
    {
        //Disconnect all Events
        InputConponent.OnDirectionChanged -= DirectionChanged;
        InputConponent.OnDirectionConfirmed -= DirectionConfirmed;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if(UnityEditor.EditorApplication.isPlaying)
        {
            Gizmos.color = PlayerRowVisualizerPlayColor;
            Gizmos.DrawWireCube(Vector3.up * PlayerRowVisualizer.y/2 + Vector3.forward * PlayerRow, PlayerRowVisualizer);

            Gizmos.color = PlayerRecordVisualizerEditorColor;
            Gizmos.DrawWireCube(Vector3.up * PlayerRowVisualizer.y / 2 + Vector3.forward * Score, PlayerRowRecordVisualizer);
        }
        else
        {
            Gizmos.color = PlayerRowVisualizerEditorColor;
            Gizmos.DrawWireCube(Vector3.up * PlayerRowVisualizer.y / 2, PlayerRowVisualizer);
        }
    }
#endif
    #endregion
}
