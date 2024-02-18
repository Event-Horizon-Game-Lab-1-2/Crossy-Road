using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public delegate void ResetRequest();
    public static ResetRequest OnResetRequest = new ResetRequest(() => { });

    //screens
    [SerializeField] MenuComponent TitleScreenMenu;
    [SerializeField] MenuComponent PauseMenu;
    [SerializeField] MenuComponent PlayMenu;
    [SerializeField] MenuComponent DeathScreen;
    [SerializeField] MenuComponent SkinSelection;
    //player current score
    [SerializeField] TMP_Text Score;

    [SerializeField] int SkinSelectorScene = 1;

    private MenuState CurrentState;
    private MenuState OldState;

    private enum MenuState
    {
        TitleScreen,
        Pause,
        Play,
        DeathScreen
    };

    private void Awake()
    {
        SetState(MenuState.TitleScreen);
    }

    private void SetState(MenuState menuState)
    {
        switch(menuState)
        {
            //Title
            case MenuState.TitleScreen:
            {
                ShowTitleScreen(true);
                ShowPlayMenu(true);
                ShowPauseMenu(false);
                ShowDeathMenu(false);
                ShowSkinSelectionMenu(true);
                break;
            }
            //Pause
            case MenuState.Pause:
            {
                ShowTitleScreen(false);
                ShowPlayMenu(false);
                ShowPauseMenu(true);
                ShowDeathMenu(false);
                ShowSkinSelectionMenu(false);
                break;
            }
            //Play
            case MenuState.Play:
            {
                ShowTitleScreen(false);
                ShowPlayMenu(true);
                ShowPauseMenu(false);
                ShowDeathMenu(false);
                ShowSkinSelectionMenu(false);
                break;
            }
            //Death
            case MenuState.DeathScreen:
            {
                ShowTitleScreen(false);
                ShowPlayMenu(false);
                ShowPauseMenu(false);
                ShowDeathMenu(true);
                ShowSkinSelectionMenu(false);
                break;
            }
            default: { break; }
        }
        CurrentState = menuState;
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
        if(show)
            DeathScreen.Show();
    }

    private void ShowPlayMenu(bool show)
    {
        PlayMenu.gameObject.SetActive(show);
    }

    private void ShowSkinSelectionMenu(bool show)
    {
        SkinSelection.gameObject.SetActive(show);
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

    public void ChangeSkin()
    {
        SceneManager.LoadScene(SkinSelectorScene);
    }

    private void OnEnable()
    {
        //All events are unsubscribed OnDisable instance of that class
        //Start Game
        GameManager.OnGameStarted += () => SetState(MenuState.Play);
        //Player Death
        GameManager.OnPlayerDeath += () => SetState(MenuState.DeathScreen);
        //Pause
        GameManager.OnPauseRequest += (bool show) =>
        {
            if (show)
            {
                OldState = CurrentState;
                SetState(MenuState.Pause);
            }
            else
                SetState(OldState);
        };

        //Player Score
        GameManager.OnScoreChange += UpdateScore;
    }

    private void OnDisable()
    {
        OnResetRequest -= OnResetRequest;
    }
}
