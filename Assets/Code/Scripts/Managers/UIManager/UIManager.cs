using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public delegate void ResetRequest();
    public static ResetRequest OnResetRequest;

    [SerializeField] MenuComponent TitleScreenMenu;
    [SerializeField] MenuComponent PauseMenu;
    [SerializeField] MenuComponent PlayMenu;

    [SerializeField] TMP_Text Score;

    private void Awake()
    {
        PauseMenu.gameObject.SetActive(false);
        PlayMenu.gameObject.SetActive(true);
        ShowTitleScreen(true);
    }

    private void ShowTitleScreen(bool show)
    {
        TitleScreenMenu.gameObject.SetActive(show);
    }

    private void ShowPauseMenu(bool pause)
    {
        PauseMenu.gameObject.SetActive(pause);
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
        GameManager.OnScoreChange += UpdateScore;
        GameManager.OnGameStarted += () => ShowTitleScreen(false);
    }

    private void OnDisable()
    {
        InputComponent.OnPauseGame -= ShowPauseMenu;
        GameManager.OnScoreChange -= UpdateScore;
        GameManager.OnGameStarted -= () => ShowTitleScreen(false);
    }

}
