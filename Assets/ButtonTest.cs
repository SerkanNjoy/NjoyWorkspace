using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonTest : MonoBehaviour
{
    public Test test;

    public Text text;
    public Text sourceText;

    private bool _locked = true;
    private bool _sourceTexture = true;

    private void Awake()
    {
        _locked = true;
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
    }

    private void Start()
    {
        if (_sourceTexture)
        {
            test.SourceTexture = true;
            sourceText.text = "Source";
        }
        else
        {
            test.SourceTexture = false;
            sourceText.text = "Generated";
        }
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

    public void SourceTextureButton()
    {
        if (_sourceTexture)
        {
            _sourceTexture = false;
            test.SourceTexture = false;
            sourceText.text = "Generated";
        }
        else
        {
            _sourceTexture = true;
            test.SourceTexture = true;
            sourceText.text = "Source";
        }
    }
}