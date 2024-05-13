using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public PlayerCtl PlayerCtl;
    public Level curLevel;
    public List<Level> PrefabLevels;

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

    private void Start()
    {
    }
    public void LoadLevel(int indexLevel)
    {
        if(curLevel != null) Destroy(curLevel.gameObject);
        curLevel = Instantiate(PrefabLevels[indexLevel], transform);
        PlayerCtl.transform.position = curLevel.StartPoint.position + new Vector3(0, 2.7f, 0);
        PlayerCtl.OnInit();
    }
    public void RetryLevel()
    {
        Debug.Log("retry");
        if (curLevel != null) Destroy(curLevel.gameObject);
        curLevel = Instantiate(PrefabLevels[PlayerPrefs.GetInt("Level")], transform);
        PlayerCtl.transform.position = curLevel.StartPoint.position + new Vector3(0, 2.7f, 0);
        PlayerCtl.OnInit();
    }
    public void NextLevel()
    {
        if (curLevel != null) Destroy(curLevel.gameObject);
        curLevel = Instantiate(PrefabLevels[PlayerPrefs.GetInt("Level")+1], transform);
        PlayerCtl.transform.position = curLevel.StartPoint.position + new Vector3(0, 2.7f, 0);
        PlayerCtl.OnInit();
        PlayerPrefs.SetInt("Level", (PlayerPrefs.GetInt("Level")+1));
    }

}
