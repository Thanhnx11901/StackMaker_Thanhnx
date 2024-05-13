using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameManager;
public enum GameState
{
    MainMenu,
    Playing,
    Paused,
    Victory
}
public class GameManager : MonoBehaviour
{

    private GameState currentStatus;

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        ChangeGameState(GameState.MainMenu);
        UIManager.Instance.ShowUIMainMenu();
    }

    public void ChangeGameState(GameState newStatus)
    {
        currentStatus = newStatus;

        switch (currentStatus)
        {
            case GameState.MainMenu:
                break;
            case GameState.Playing:
                break;
            case GameState.Paused:
                break;
            case GameState.Victory:
                break;
        }
    }

    public void BtnPlay()
    {
        ChangeGameState(GameState.Playing);
        UIManager.Instance.ShowUIPlaying();
        LevelManager.Instance.LoadLevel(PlayerPrefs.GetInt("Level"));
    }
    public void BtnSetting()
    {
        ChangeGameState(GameState.Paused);
        UIManager.Instance.ShowUIPaused();
        Time.timeScale = 0; 
    }
    public void BtnRetry()
    {
        Time.timeScale = 1;
        ChangeGameState(GameState.Playing);
        UIManager.Instance.ShowUIPlaying();
        LevelManager.Instance.RetryLevel();

    }
    public void BtnContinue()
    {
        Time.timeScale = 1;
        ChangeGameState(GameState.Playing);
        UIManager.Instance.ShowUIPlaying();
    }
    public void BtnNextLevel()
    {
        Time.timeScale = 1;
        ChangeGameState(GameState.Playing);
        UIManager.Instance.ShowUIPlaying();
        LevelManager.Instance.NextLevel();

    }
}
