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
        //PlayerCtl.transform.position = Level.StartPoint.position + new Vector3(0, 2.7f, 0);
    }
    public void RetryLevel()
    {

    }
    public void NextLevel()
    {

    }

}
