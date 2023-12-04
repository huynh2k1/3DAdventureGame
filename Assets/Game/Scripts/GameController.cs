using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameController : MonoBehaviour
{
    public static GameController instance;
    public Character player;

    bool isGameOver;

    private void Awake()
    {
        instance = this;    
    }
    public void GameOver()
    {
        CanvasController.instance.ShowLosePopup();
    }
    public void GameIsFinished()
    {
        Debug.Log("END GAME");
        CanvasController.instance.ShowWinPopup();

    }
    private void Update()
    {
        if (isGameOver)
        {
            return;
        }
        if(player.CurrentState == CharacterState.Dead)
        {
            isGameOver = true;
            GameOver();
        }
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void ReturnMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
