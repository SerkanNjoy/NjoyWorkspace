using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonTest : MonoBehaviour
{
    public Test test;

    public Text text;

    private bool _locked = true;

    private void Awake()
    {
        _locked = true;
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
    }

    public void ButtonFunc()
    {
        if (_locked)
        {
            text.text = "Unlocked";
            _locked = false;
            Application.targetFrameRate = 0;
        }
        else
        {
            text.text = "Locked";
            _locked = true;
            Application.targetFrameRate = 60;
        }
    }
}