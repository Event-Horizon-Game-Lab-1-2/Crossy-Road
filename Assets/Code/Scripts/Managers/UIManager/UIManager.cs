using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public delegate void ResetRequest();
    public static ResetRequest OnResetRequest = new ResetRequest( () => {} );

    [SerializeField] MenuComponent TitleScreenMenu;
    [SerializeField] MenuComponent PauseMenu;
    [SerializeField] MenuComponent PlayMenu;
    [SerializeField] MenuComponent DeathScreen;

    [SerializeField] TMP_Text Score;

    private void Awake()
    {
        ShowTitleScreen(true);
        ShowPlayMenu(true);
        ShowPauseMenu(false);
        ShowDeathMenu(false);
    }

    private void ShowTitleScreen(bool show)
    {
        TitleScreenMenu.gameObject.SetActive(show);
    }

    private void ShowPauseMenu(bool show)
    {
        PauseMenu.gameObject.SetActive(show);
    }

    private void ShowDeathMenu(bool show)
    {
        DeathScreen.gameObject.SetActive(show);
    }

    private void ShowPlayMenu(bool show)
    {
        PlayMenu.gameObject.SetActive(show);
    }

    private void UpdateScore(int score)
    {
        Score.text = score.ToString();
    }

    public void SendRestartRequest()
    {
        OnResetRequest();
        ShowPauseMenu(false);
    }

    private void OnEnable()
    {
        InputComponent.OnPauseGame += ShowPauseMenu;
        GameManager.OnPauseRequest += ShowPauseMenu;
        GameManager.OnScoreChange += UpdateScore;
        GameManager.OnGameStarted += () => ShowTitleScreen(false);
        GameManager.OnPlayerDeath += () => ShowDeathMenu(true);
    }

    private void OnDisable()
    {
        InputComponent.OnPauseGame -= ShowPauseMenu;
        GameManager.OnPauseRequest -= ShowPauseMenu;
        GameManager.OnScoreChange -= UpdateScore;
        GameManager.OnGameStarted -= () => ShowTitleScreen(false);
        GameManager.OnPlayerDeath -= () => ShowDeathMenu(true);
    }

    private void OnDestroy()
    {
        InputComponent.OnPauseGame -= ShowPauseMenu;
        GameManager.OnPauseRequest -= ShowPauseMenu;
        GameManager.OnScoreChange -= UpdateScore;
        GameManager.OnGameStarted -= () => ShowTitleScreen(false);
        GameManager.OnPlayerDeath -= () => ShowDeathMenu(true);
    }

}
