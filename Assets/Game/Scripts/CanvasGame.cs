using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasGame : MonoBehaviour
{
    public Slider healthSlider;
    public Button btnAttack;
    public Button btnRoll;
    public Button btnPause;
    private void Awake()
    {
        btnAttack.onClick.AddListener(OnClickAttack);
        btnRoll.onClick.AddListener(OnClickRoll);
        btnPause.onClick.AddListener(OnClickPause);
    }
    public void UpdateHealthBar(float value)
    {
        healthSlider.value = value;
    }
    private void OnClickAttack()
    {
        PlayerInput.instance.mouseButtonDown = true;
    }
    private void OnClickRoll()
    {
        PlayerInput.instance.spaceKeyDown = true;
    }
    private void OnClickPause()
    {
        CanvasController.instance.TogglePausePopup();
    }
}
