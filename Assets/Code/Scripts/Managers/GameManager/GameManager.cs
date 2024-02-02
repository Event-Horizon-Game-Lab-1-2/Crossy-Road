using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public delegate void NewRowAchieved();
    public static event NewRowAchieved OnNewRowAchieved;

    //player Score
    [HideInInspector] public static int Score = 0;
    //Player raw Rappresent the raw on wich the player is on
    [HideInInspector] public int PlayerRaw = 0;

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
        PlayerRaw += (int)PlayerDirection.z;

        if(PlayerRaw > Score)
        {
            Score = PlayerRaw;
            OnNewRowAchieved();
        }
    }

    #region Utility
    private void Reset()
    {
        Score = 0;
        PlayerRaw = 0;
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
    #endregion
}
