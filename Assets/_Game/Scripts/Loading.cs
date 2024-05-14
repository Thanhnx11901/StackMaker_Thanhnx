using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    public Text LoadingText;
    public int dotCount;
    void Start()
    {
        InvokeRepeating("ChangeText", .25f, .25f);
    }
    void ChangeText()
    {
        dotCount--;
        if (dotCount == -1)
        {
            dotCount = 3;
        }
        LoadingText.text = "";
        for (int i = 1; i < dotCount; i++)
        {
            LoadingText.text += ".";
        }

    }
}
