using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class EditorUtils : MonoBehaviour
{
    public static bool CheckExitsFolderExports(string path)
    {
        if (Directory.Exists(path)) return true;
        var folder = Directory.CreateDirectory(path);
        return folder!=null;

    }
}
