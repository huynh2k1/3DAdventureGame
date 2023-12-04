using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasController : MonoBehaviour
{
    public static CanvasController instance;
    public CanvasGame canvasGame;


    public GameObject pausePopup;
    public GameObject losePopup;
    public GameObject winPopup;

    GameState currentState;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        SwitchUIState(GameState.GamePlay);
    }
    private void SwitchUIState(GameState state)
    {
        pausePopup.SetActive(false);
        losePopup.SetActive(false);
        winPopup.SetActive(false);

        Time.timeScale = 1;

        switch(state)
        {
            case GameState.GamePlay:
                break;
            case GameState.Pause:
                Time.timeScale = 0;
                pausePopup.SetActive(true);
                break;
            case GameState.GameOver:
                losePopup.SetActive(true);
                break;
            case GameState.GameWin:
                winPopup.SetActive(true);
                break;
        }
        currentState = state;
    }

    public void TogglePausePopup()
    {
        if (currentState == GameState.GamePlay)
        {
            SwitchUIState(GameState.Pause);
        }
        else if (currentState == GameState.Pause)
        {
            SwitchUIState(GameState.GamePlay);
        }
    }

    public void OnClickMainMenu()
    {
        Time.timeScale = 1f;
        GameController.instance.ReturnMainMenu();
    }

    public void OnClickRestart()
    {
        GameController.instance.RestartGame();
    }
    public void ShowLosePopup()
    {
        SwitchUIState(GameState.GameOver);
    }
    public void ShowWinPopup()
    {
        SwitchUIState(GameState.GameWin);
    }
}
public enum GameState
{
    GamePlay,
    Pause,
    GameOver,
    GameWin
}
