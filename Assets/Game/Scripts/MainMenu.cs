using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEditor;
public class MainMenu : MonoBehaviour
{
    public Button BtnStart;
    public Button BtnQuit;
    private void Awake()
    {
        BtnStart.onClick.AddListener(OnClickStart);
        BtnQuit.onClick.AddListener(OnClickQuit);
    }
    public void OnClickStart()
    {
        SceneManager.LoadScene("GameScene");
    }
    public void OnClickQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif

        Application.Quit();
    }
}
