using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using DG.Tweening;
using UnityEditor;
using UnityEngine;

namespace Aord.Tools
{
    public class AutoRenameSpriteFollowIDUpper : EditorWindow
    {
        private string _dragDropText = "Drag and drop a folder here to select.";
        private string _folderPath;
        private string _newInfixSpriteName = "";

        [MenuItem("Tools/Auto Rename Sprite Follow ID Upper")]
        public static void ShowWindow()
        {
            GetWindow<AutoRenameSpriteFollowIDUpper>("Auto Rename Sprite Follow ID Upper");
        }

        private void OnGUI()
        {
            GUILayout.Label("Auto Rename Sprite Follow ID Upper", EditorStyles.boldLabel);

            // Drag and drop area
            GUILayout.BeginArea(new Rect(10, 100, position.width - 20, 40)); // Adjusted Y position
            GUI.Box(new Rect(0, 0, position.width - 20, 40), _dragDropText, EditorStyles.helpBox);
            GUILayout.EndArea();

            // Selected folder display
            GUILayout.Label("Selected Folder:", EditorStyles.boldLabel);
            GUILayout.Label(_folderPath);

            _newInfixSpriteName = EditorGUILayout.TextField("Infix New Name", _newInfixSpriteName);

            if (GUILayout.Button("Rename"))
                RenameAllSprite();

            Repaint();
        }

        private void RenameAllSprite()
        {
            string[] allFiles = Directory.GetFiles(_folderPath, "*", SearchOption.TopDirectoryOnly);

            List<string> validFiles = new List<string>();
            foreach (string assetPath in allFiles)
            {
                if (!assetPath.EndsWith(".meta"))
                    validFiles.Add(assetPath);
            }

            validFiles.Sort(StringLogicalComparer);

            int index = 0;
            foreach (string assetPath in validFiles)
            {
                index++;
                string newName = _newInfixSpriteName + index;
                AssetDatabase.RenameAsset(assetPath, newName);
            }

            AssetDatabase.Refresh();
        }

        private void OnInspectorUpdate()
        {
            if (DragAndDrop.paths != null && DragAndDrop.paths.Length > 0)
            {
                _folderPath = DragAndDrop.paths[0];
                _dragDropText = "";
                Repaint();
            }
        }

        private int StringLogicalComparer(string a, string b)
        {
            return AlphanumComparatorFast(a, b);
        }

        private int AlphanumComparatorFast(string s1, string s2)
        {
            Regex regex = new Regex(@"(\d+)");
            MatchCollection s1Matches = regex.Matches(s1);
            MatchCollection s2Matches = regex.Matches(s2);

            if (s1Matches.Count > 0 && s2Matches.Count > 0)
            {
                int s1Number = int.Parse(s1Matches[0].Value);
                int s2Number = int.Parse(s2Matches[0].Value);
                return s1Number.CompareTo(s2Number);
            }

            return string.Compare(s1, s2, StringComparison.Ordinal);
        }
    }
}