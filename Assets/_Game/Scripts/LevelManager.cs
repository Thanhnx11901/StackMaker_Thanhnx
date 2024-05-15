using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public PlayerCtl PlayerCtl;
    public Level curLevel;
    public List<Level> PrefabLevels;

    private float BodyPlayerPosY = 2.7f;

    public static LevelManager Instance { get; private set; }

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
    public void LoadLevel(int indexLevel)
    {
        if (curLevel != null) Destroy(curLevel.gameObject);
        curLevel = Instantiate(PrefabLevels[indexLevel], transform);
        PlayerCtl.transform.position = curLevel.StartPoint.position + Vector3.up * BodyPlayerPosY;
        PlayerCtl.OnInit();
    }
    public void RetryLevel()
    {
        if (curLevel != null) Destroy(curLevel.gameObject);
        curLevel = Instantiate(PrefabLevels[PlayerPrefs.GetInt("Level")], transform);
        PlayerCtl.transform.position = curLevel.StartPoint.position + Vector3.up * BodyPlayerPosY;
        PlayerCtl.OnInit();
    }
    public void NextLevel()
    {
        if (curLevel != null) Destroy(curLevel.gameObject);

        if ((PrefabLevels.Count - 1) >= (PlayerPrefs.GetInt("Level") + 1))
        {
            curLevel = Instantiate(PrefabLevels[PlayerPrefs.GetInt("Level") + 1], transform);
            PlayerCtl.transform.position = curLevel.StartPoint.position + Vector3.up * BodyPlayerPosY;
            PlayerCtl.OnInit();
            PlayerPrefs.SetInt("Level", (PlayerPrefs.GetInt("Level") + 1));
        }
        else
        {
            UIManager.Instance.ShowUILoading(() =>
            {
                GameManager.Instance.ChangeGameState(GameState.MainMenu);
                UIManager.Instance.ShowUIMainMenu();
                PlayerPrefs.SetInt("Level", 0);
            });

        }

    }

}
