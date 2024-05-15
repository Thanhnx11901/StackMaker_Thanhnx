using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

    public GameState currentGameState;

    public int curScore;
    public int totalScore;
    public int bestScore;

    public Text textCurScore;
    public Text textTotalScore;
    public Text textBestScore;
    public Text curLevel;


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
        UIManager.Instance.ShowUILoading(() =>
        {
            ChangeGameState(GameState.MainMenu);
            UIManager.Instance.ShowUIMainMenu();
        });

    }

    public void ChangeGameState(GameState newStatus)
    {
        currentGameState = newStatus;

        switch (currentGameState)
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
        UIManager.Instance.ShowUILoading(() =>
        {
            ChangeGameState(GameState.Playing);
            UIManager.Instance.ShowUIPlaying();
            LevelManager.Instance.LoadLevel(PlayerPrefs.GetInt("Level"));
            LevelManager.Instance.PlayerCtl.OnInit();
        });
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
        UIManager.Instance.ShowUILoading(() =>
        {
            ChangeGameState(GameState.Playing);
            UIManager.Instance.ShowUIPlaying();
            LevelManager.Instance.RetryLevel();
            ResetScore();
            LevelManager.Instance.PlayerCtl.OnInit();
        });
    }
    public void BtnContinue()
    {
        Time.timeScale = 1;
        StartCoroutine(MyCoroutine(() =>
        {
            ChangeGameState(GameState.Playing);
            UIManager.Instance.ShowUIPlaying();
        }, 0.00001f));
    }
    public void BtnNextLevel()
    {
        Time.timeScale = 1;
        UIManager.Instance.ShowUILoading(() =>
        {
            ChangeGameState(GameState.Playing);
            UIManager.Instance.ShowUIPlaying();
            LevelManager.Instance.NextLevel();
            LevelManager.Instance.PlayerCtl.OnInit();
        });
    }

    IEnumerator MyCoroutine(Action onComplete, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        onComplete?.Invoke();
    }

    public void UpdateScore(int indexScore)
    {
        curScore += indexScore;
        textCurScore.text = curScore.ToString();
    }
    public void ResetScore()
    {
        curScore = 0;
        textCurScore.text = curScore.ToString();
    }
    public void UpdateBestScore()
    {
        curLevel.text = "Level " + (PlayerPrefs.GetInt("Level") + 1);
        PlayerPrefs.SetInt("BestScore", PlayerPrefs.GetInt("BestScore") + curScore);
        textBestScore.text = "BEST SCORE: " + PlayerPrefs.GetInt("BestScore").ToString();
        textTotalScore.text = "SCORE: " + curScore;
    }
}
