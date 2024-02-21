using System.Collections;
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
    [SerializeField] TMP_Text TopScore;
    [SerializeField] TMP_Text Money;
    [SerializeField] TMP_Text NewMoneyAmount;
    //Skin Selector scene index
    [SerializeField] int SkinSelectorScene = 1;

    private int LastMoneyAmount;

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

    private void Start()
    {
        UpdateScore(0);
        UpdateMoneyAmount();
        LastMoneyAmount = PlayerPrefs.GetInt("PlayerMoney");
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
        if (show)
        {
            TitleScreenMenu.gameObject.SetActive(show);
            TitleScreenMenu.StartAnimation();
        }
        else
        {
            TitleScreenMenu.EndAnimation();
        }
    }

    private void ShowPauseMenu(bool show)
    {
        if (show)
        {
            PauseMenu.gameObject.SetActive(show);
            PauseMenu.StartAnimation();
        }
        else
        {
            PauseMenu.EndAnimation();
        }
    }

    private void ShowDeathMenu(bool show)
    {
        if (show)
        {
            DeathScreen.gameObject.SetActive(show);
            DeathScreen.StartAnimation();
            int moneyObtained = PlayerPrefs.GetInt("PlayerMoney") - LastMoneyAmount;
            string newtext = "Money Obtained: " + moneyObtained + "$";
            NewMoneyAmount.text = newtext;
        }
        else
        {
            DeathScreen.EndAnimation();
        }
    }

    private void ShowPlayMenu(bool show)
    {
        if (show)
        {
            PlayMenu.gameObject.SetActive(show);
            PlayMenu.StartAnimation();
        }
        else
        {
            PlayMenu.EndAnimation();
        }
    }

    private void ShowSkinSelectionMenu(bool show)
    {
        if (show)
        {
            SkinSelection.gameObject.SetActive(show);
            SkinSelection.StartAnimation();
        }
        else
        {
            SkinSelection.EndAnimation();
        }
    }

    private void UpdateScore(int score)
    {
        Score.text = score.ToString();
        TopScore.text = PlayerPrefs.GetInt("TopScore").ToString();
    }

    private void UpdateMoneyAmount()
    {
        Money.text = PlayerPrefs.GetInt("PlayerMoney").ToString();
    }

    public void SendRestartRequest()
    {
        OnResetRequest();
        ShowPauseMenu(false);
    }

    public void ChangeSkin()
    {
        StartCoroutine(ChangeSkinAnimation());
    }

    private IEnumerator ChangeSkinAnimation()
    {
        SkinSelection.StartAnimation();
        yield return new WaitForSeconds(0.2f);
        SceneManager.LoadScene(SkinSelectorScene);
    }

    public void FullScreen()
    {
        if (Screen.fullScreenMode == FullScreenMode.Windowed)
        {
            Screen.SetResolution(1920, 1080, FullScreenMode.FullScreenWindow);
        }
        else
        {
            Screen.SetResolution(960, 540, FullScreenMode.Windowed);
        }
    }


    private void OnEnable()
    {
        //All events are unsubscribed OnDisable instance of that class
        //Start Game
        GameManager.OnGameStarted += () => SetState(MenuState.Play);
        //Player Death
        GameManager.OnPlayerDeath += () => SetState(MenuState.DeathScreen);
        GameManager.OnPlayerDeath += () => UpdateMoneyAmount();
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
