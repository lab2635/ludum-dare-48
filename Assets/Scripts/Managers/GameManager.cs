using Doozy.Engine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : SingletonBehaviour<GameManager>
{
    public string TitleScene;
    public string MainGame;

    public UIView WinContainer;

    //public bool SkipTitle;
    public bool NeverLose;

    public bool IsPlayerControllerEnabled;


    public delegate void ResetAction();
    public static event ResetAction OnReset;

    private bool isPlaying;
    private float startTime;
    private bool alreadyDead;
    private PlayerDeathLoop playerDeathLoop;
    private PlayerMovement player;

    void Start()
    {
        playerDeathLoop = GetComponent<PlayerDeathLoop>();

        DontDestroyOnLoad(this.gameObject);

        this.isPlaying = false;
        this.alreadyDead = true;

        SceneManager.sceneLoaded += OnSceneLoaded;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();

        this.StartGame();
    }

    public void StartGame()
    {
        this.alreadyDead = false;
        this.IsPlayerControllerEnabled = true;
        BGMPlayer.Instance.PlayBGM();
    }

    public void WinGame()
    {
        this.isPlaying = false;
        this.IsPlayerControllerEnabled = false;
        this.WinContainer.Show();
        
        player.enabled = false;
        BGMPlayer.Instance.StopBGM();
    }

    public void ReturnToTitle()
    {
        this.WinContainer.Hide();
        this.IsPlayerControllerEnabled = false;
        SceneManager.LoadScene(this.TitleScene);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void ResetGameState()
    {
        this.NeverLose = false;

        this.startTime = Time.deltaTime;
        this.isPlaying = true;
        this.IsPlayerControllerEnabled = true;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (scene.name == "Main")
        {
            this.ResetGameState();
            this.StartGame();
        }
    }

    private void Update()
    {

    }

    public void KillRespawnPlayer()
    {
        if (!NeverLose)
        {
            IsPlayerControllerEnabled = false;
            alreadyDead = true;

            StopAllCoroutines();
            StartCoroutine(RespawnPlayer());
        }
    }

    private IEnumerator RespawnPlayer()
    {
        if (!player)
            yield break;

        playerDeathLoop.KillPlayer(player);

        OnReset();
        
        yield return new WaitForSeconds(3);

        playerDeathLoop.RespawnPlayer(player);

        yield return null;

        player.gameObject.SetActive(true);

        IsPlayerControllerEnabled = true; 
        alreadyDead = false; 
    }
}
