using UnityEngine;
using UnityEditor;
using System.Diagnostics;
using System.Text;
using System;

#if UNITY_EDITOR

using UnityToolbarExtender;
using UnityEditor.SceneManagement;

static class ToolbarStyles
{
    public static readonly GUIStyle commandButtonStyle;
    public static readonly GUIStyle ButtonGit;
    public static readonly GUIStyle IconGit;


    static ToolbarStyles()
    {
        commandButtonStyle = new GUIStyle("Command")
        {
            fontSize = 10,
            alignment = TextAnchor.MiddleCenter,
            imagePosition = ImagePosition.ImageAbove,
            fontStyle = FontStyle.Bold
        };
        IconGit = new GUIStyle("Command")
        {
            fontSize = 12,
            alignment = TextAnchor.MiddleCenter,
            fixedWidth = 30,
            imagePosition = ImagePosition.ImageAbove,
            fontStyle = FontStyle.Bold
        };

        ButtonGit = new GUIStyle("Command")
        {
            fontSize = 12,
            alignment = TextAnchor.MiddleCenter,
            fixedWidth = 40,
            imagePosition = ImagePosition.ImageAbove,
            fontStyle = FontStyle.Bold
        };
    }
}

[InitializeOnLoad]
public static class ToolbarExtensions
{
    static StringBuilder cmdOutput = null;
    static int numOutputLines = 0;
    // static ToolbarExtensions()
    // {
    //     ToolbarExtender.LeftToolbarGUI.Add(DrawButtonPlaynow);
    //     ToolbarExtender.RightToolbarGUI.Add(DrawIconGit);
    //     ToolbarExtender.RightToolbarGUI.Add(DrawButtonGitPull);
    //     ToolbarExtender.RightToolbarGUI.Add(DrawButtonGitPush);
    // }

    static void DrawButtonPlaynow()
    {
        GUILayout.FlexibleSpace();
        if (GUILayout.Button(new GUIContent("[▶]", "Dùng để chạy vào Splash Scene từ scene khác"), ToolbarStyles.commandButtonStyle))
        {
            if (Application.isPlaying)
            {
                UnityEngine.Debug.Break();
            }
            else
            {
                SceneHelper.StartScene("SplashScene");
            }
        }
    }

    static void DrawIconGit()
    {
        Texture icon = Resources.Load<Texture>("Editor/git");
        GUILayout.Space(50);
        if (GUILayout.Button(new GUIContent(icon, "Quick Github desktop"), ToolbarStyles.IconGit))
        {
            // Process p = new Process();
            // p.StartInfo.FileName = @"C:\Windows\System32\cmd.exe";
            // p.StartInfo.WorkingDirectory = @"./";
            // p.Start();
            GoCmd("github");
        }
    }

    static void DrawButtonGitPull()
    {
        Texture icon = Resources.Load<Texture>("Editor/pull");
        GUILayout.Space(2);
        if (GUILayout.Button(new GUIContent(icon, "Dùng update nhanh"), ToolbarStyles.IconGit))
        {
            GoCmd("git pull");
        }
    }

    static void DrawButtonGitPush()
    {
        GUILayout.Space(2);
        Texture icon = Resources.Load<Texture>("Editor/logo");
        if (GUILayout.Button(new GUIContent(icon, "Dùng để vào cms"), ToolbarStyles.ButtonGit))
        {
            Application.OpenURL("https://ninja-cms.aordgame.com/");
        }
    }

    public static void GoCmd(string arguments)
    {
        Process cmd = new Process();
        cmd.StartInfo.FileName = "cmd.exe";
        cmd.StartInfo.RedirectStandardInput = true;
        cmd.StartInfo.RedirectStandardOutput = true;
        cmdOutput = new StringBuilder();
        cmd.OutputDataReceived += SortOutputHandler;

        cmd.StartInfo.CreateNoWindow = true;
        cmd.StartInfo.UseShellExecute = false;

        cmd.Start();
        cmd.StandardInput.WriteLine(arguments);
        cmd.StandardInput.Flush();
        cmd.StandardInput.Close();
        cmd.WaitForExit();
        cmd.Close();
        UnityEngine.Debug.Log("Done!");
    }
    static void SortOutputHandler(object sendingProcess, DataReceivedEventArgs outLine)
    {
        if (!string.IsNullOrEmpty(outLine.Data))
        {
            numOutputLines++;
            cmdOutput.Append(Environment.NewLine +
                   $"[{numOutputLines}] - {outLine.Data}");
        }
    }

    static class SceneHelper
    {
        static string sceneToOpen;

        public static void StartScene(string sceneName)
        {
            if (EditorApplication.isPlaying)
            {
                EditorApplication.isPlaying = false;
            }

            sceneToOpen = sceneName;
            EditorApplication.update += OnUpdate;
        }

        static void OnUpdate()
        {
            if (sceneToOpen == null ||
                EditorApplication.isPlaying || EditorApplication.isPaused ||
                EditorApplication.isCompiling || EditorApplication.isPlayingOrWillChangePlaymode)
            {
                return;
            }

            EditorApplication.update -= OnUpdate;

            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                string[] guids = AssetDatabase.FindAssets("t:scene " + sceneToOpen, null);
                if (guids.Length == 0)
                {
                    UnityEngine.Debug.LogWarning("Couldn't find scene file");
                }
                else
                {
                    string scenePath = AssetDatabase.GUIDToAssetPath(guids[0]);
                    EditorSceneManager.OpenScene(scenePath);
                    EditorApplication.isPlaying = true;
                }
            }
            sceneToOpen = null;
        }
    }
}

#endif
