using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    /// <summary>
    /// 0:MainMenu|1:Playing|2:Paused|3:Victory|4:Loading
    /// </summary>
    [SerializeField]
    private List<GameObject> ListUI;


    public static UIManager Instance { get; private set; }

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
    private void ActiveUI(int index)
    {
        for(int i= 0; i<ListUI.Count; i++)
        {
            ListUI[i].SetActive(false);
        }
        ListUI[index].SetActive(true);
    }
    public void ShowUIMainMenu()
    {
        ActiveUI(0);
    }
    public void ShowUIPlaying()
    {
        ActiveUI(1);
    }
    public void ShowUIPaused()
    {
        ActiveUI(2);
    }
    public void ShowUIVictory()
    {
        ActiveUI(3);
    }

    public void ShowUILoading()
    {
        ActiveUI(4);
    }
}
