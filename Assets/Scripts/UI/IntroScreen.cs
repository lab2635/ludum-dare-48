using System.Collections;
using System.Collections.Generic;
using Doozy.Engine.UI;
using UnityEngine;

[RequireComponent(typeof(UIView))]
[RequireComponent(typeof(PlayerInput))]
public class IntroScreen : MonoBehaviour
{
    public UIView titleScreen;
    public UIView hud;
    
    private UIView view;
    private PlayerInput input;
    private bool acknowledged;

    private float time;

    void Start()
    {
        input = GetComponent<PlayerInput>();
        view = GetComponent<UIView>();
        titleScreen.enabled = false;
        hud.enabled = false;
    }

    public void OnClickStart()
    {
        StartGame();
    }

    void StartGame()
    {
        if (!acknowledged)
        {
            acknowledged = true;
            view.Hide();
            titleScreen.enabled = true;
            titleScreen.Show();
            hud.enabled = true;
            hud.Show();
        }
    }
    
    void Update()
    {
        if (input.accept && time >= 3f)
        {
            StartGame();
        }
        
        time += Time.deltaTime;
    }
}
