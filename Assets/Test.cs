using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class Test
{
    [UnityEditor.InitializeOnLoadMethod]
    public static void ResetPSDImporterFoldout()
    {
        UnityEditor.EditorPrefs.DeleteKey("PSDImporterEditor.m_PlatformSettingsFoldout");
    }
}