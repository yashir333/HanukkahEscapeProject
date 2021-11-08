using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Menus
/// </summary>
public class ciitt_EscapeGameKit_Menus
{

    /// <summary>
    /// Show ChangeVertexColorWindow
    /// </summary>
    [MenuItem("Tools/Ciitt Escape Game Kit/Change Vertex Color", false, 0)]
    static void ShowChangeVertexColorWindow()
    {
        (EditorWindow.GetWindow(typeof(ciitt.EscapeGameKit.ChangeVertexColorWindow)) as ciitt.EscapeGameKit.ChangeVertexColorWindow).Show();
    }

    /// <summary>
    /// Show CopyAndTransformWindow
    /// </summary>
    [MenuItem("Tools/Ciitt Escape Game Kit/Copy and Transform", false, 0)]
    static void ShowCopyAndTransformWindow()
    {
        (EditorWindow.GetWindow(typeof(ciitt.EscapeGameKit.CopyAndTransformWindow)) as ciitt.EscapeGameKit.CopyAndTransformWindow).Show();
    }

    /// <summary>
    /// Show HelpWindow (EN)
    /// </summary>
    [MenuItem("Tools/Ciitt Escape Game Kit/Help (EN)", false, 0)]
    static void ShowHelpWindowEn()
    {
        ciitt.EscapeGameKit.HelpWindow window = (EditorWindow.GetWindow(typeof(ciitt.EscapeGameKit.HelpWindow)) as ciitt.EscapeGameKit.HelpWindow);
        window.setLanguage(SystemLanguage.English);
        window.Show();
    }

    /// <summary>
    /// Show HelpWindow (JP)
    /// </summary>
    [MenuItem("Tools/Ciitt Escape Game Kit/Help (JP)", false, 0)]
    static void ShowHelpWindowJp()
    {
        ciitt.EscapeGameKit.HelpWindow window = (EditorWindow.GetWindow(typeof(ciitt.EscapeGameKit.HelpWindow)) as ciitt.EscapeGameKit.HelpWindow);
        window.setLanguage(SystemLanguage.Japanese);
        window.Show();
    }

}