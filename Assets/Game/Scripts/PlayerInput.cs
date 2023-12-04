using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public static PlayerInput instance;
    public float horizontalInput;
    public float verticalInput;
    public bool mouseButtonDown;
    public bool spaceKeyDown;
    public FixedJoystick joystick;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (!mouseButtonDown && Time.timeScale != 0)
        {
            mouseButtonDown = Input.GetMouseButtonDown(0);
        }
        if (!spaceKeyDown && Time.timeScale != 0)
        {
            spaceKeyDown = Input.GetKeyDown(KeyCode.Space);
        }

        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        //horizontalInput = joystick.Horizontal;
        //verticalInput = joystick.Vertical;
    }

    public void ClearCache()
    {
        mouseButtonDown = false;
        spaceKeyDown = false;
        horizontalInput = 0;
        verticalInput = 0;
    }

    private void OnDisable()
    {
        ClearCache();
    }
}
