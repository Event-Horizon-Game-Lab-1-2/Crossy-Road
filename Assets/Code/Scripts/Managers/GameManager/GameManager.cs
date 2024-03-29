using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using static DeathTypeClass;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public delegate void NewRowAchieved();
    public static event NewRowAchieved OnNewRowAchieved = new NewRowAchieved(()=> { });

    public delegate void ScoreChange(int score);
    public static event ScoreChange OnScoreChange = new ScoreChange((int score) => { });

    public delegate void GameStarted();
    public static event GameStarted OnGameStarted = new GameStarted(() => { });
    
    public delegate void PlayerDeath();
    public static event PlayerDeath OnPlayerDeath = new PlayerDeath(() => { });

    public delegate void PauseRequest(bool pause);
    public static event PauseRequest OnPauseRequest = new PauseRequest((bool pause) => { });

    private enum GameState
    {
        Playing,
        Paused,
        Menu
    }

    [Header("Game Option")]
    [SerializeField] int MoneyAfterRowAmount = 5;

    [Header("Gizsmos Options")]
    [SerializeField] Vector3 PlayerRowVisualizer = new Vector3(15f, 5f, 1f);
    [SerializeField] Vector3 PlayerRowRecordVisualizer = new Vector3(15f, 5f, 1f);
    [SerializeField] Color PlayerRowVisualizerEditorColor = Color.magenta;
    [SerializeField] Color PlayerRowVisualizerPlayColor = Color.blue;
    [SerializeField] Color PlayerRecordVisualizerEditorColor = Color.black;

    //Player Top Score
    [HideInInspector] public static int PlayerTopScore = 0;
    //Player Score for this run
    [HideInInspector] public static int Score = 0;
    //Player raw Rappresent the raw on wich the player is on
    [HideInInspector] private int PlayerRow = 0;

    Vector3 PlayerDirection;
    //Current game state
    GameState CurrentGameState = GameState.Menu;

    public static bool IsPlayerAlive;
    //Used to avoid event errors
    public static bool Resetting;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this.gameObject);

        //DontDestroyOnLoad(gameObject);
        IsPlayerAlive = true;
        Resetting = false;

        if (!PlayerPrefs.HasKey("selectedSkin"))
            PlayerPrefs.SetInt("selectedSkin", 0);

        if (!PlayerPrefs.HasKey("TopScore"))
            PlayerPrefs.SetInt("TopScore", 0);
        else
            PlayerTopScore = PlayerPrefs.GetInt("TopScore");

        if(!PlayerPrefs.HasKey("PlayerMoney"))
            PlayerPrefs.SetInt("PlayerMoney", 0);
        else
            PlayerTopScore = PlayerPrefs.GetInt("TopScore");
    }

    private void DirectionChanged(Vector3 direction)
    {
        PlayerDirection = direction;
        if(CurrentGameState != GameState.Playing)
        {
            CurrentGameState = GameState.Playing;
            OnGameStarted();
        }
    }

    private void DirectionConfirmed()
    {
        //Update Player Raw number
        PlayerRow += (int)PlayerDirection.z;

        if(PlayerRow > Score)
        {
            Score = PlayerRow;
            //Call new row event
            OnNewRowAchieved();
            //Call new score event
            OnScoreChange(Score);

            if(Score >= PlayerTopScore)
            { 
                PlayerTopScore = Score +1;
                PlayerPrefs.SetInt("TopScore", PlayerTopScore);
            }
        }
    }

    private void SetGamePause(bool pause)
    {
        CurrentGameState = pause? GameState.Paused : GameState.Playing;
        Time.timeScale = pause? 0 : 1;

        OnPauseRequest(pause);
    }

    #region Utility
    private void Reset()
    {
        Score = 0;
        PlayerRow = 0;
        Time.timeScale = 1;
        IsPlayerAlive = true;
        Resetting = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void OnEnable()
    {
        //Connect all Events
        //Input events
        InputComponent.OnDirectionChanged += DirectionChanged;
        MovementComponent.OnMove += DirectionConfirmed;
        InputComponent.OnPauseGame += SetGamePause;
        //UI events
        UIManager.OnResetRequest += Reset;
        //gameplay events
        PlayerManager.OnDeath += (DeathType deathType) =>
        {
            int newMoney = Score / MoneyAfterRowAmount;
            newMoney += PlayerPrefs.GetInt("PlayerMoney");
            PlayerPrefs.SetInt("PlayerMoney", newMoney);
            if (OnPlayerDeath != null)
                OnPlayerDeath();
            IsPlayerAlive = false;
        };
    }

    private void OnDisable()
    {
        //Disconnect all Events
        OnGameStarted -= OnGameStarted;
        OnPlayerDeath -= OnPlayerDeath;
        OnNewRowAchieved -= OnNewRowAchieved;
        //Input events
        InputComponent.OnDirectionChanged -= DirectionChanged;
        MovementComponent.OnMove -= DirectionConfirmed;
        InputComponent.OnPauseGame -= SetGamePause;
        //UI events
        UIManager.OnResetRequest -= Reset;
    }

    private void OnDestroy()
    {
        OnGameStarted -= OnGameStarted;
        OnPlayerDeath -= OnPlayerDeath;
        OnNewRowAchieved -= OnNewRowAchieved;
        OnPauseRequest -= OnPauseRequest;
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
