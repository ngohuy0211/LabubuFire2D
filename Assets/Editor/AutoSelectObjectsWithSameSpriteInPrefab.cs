using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.Experimental.SceneManagement;

namespace Aord.Tools
{
    public class AutoSelectObjectsWithSameSpriteInPrefab : EditorWindow
    {
        private Sprite selectedSprite;

        [MenuItem("Tools/Select Objects With Same Sprite In Prefab")]
        public static void ShowWindow()
        {
            GetWindow<AutoSelectObjectsWithSameSpriteInPrefab>("Select Objects With Same Sprite In Prefab");
        }

        private void OnGUI()
        {
            GUILayout.Label("Select Objects With Same Sprite In Prefab", EditorStyles.boldLabel);
            selectedSprite = (Sprite) EditorGUILayout.ObjectField("Sprite", selectedSprite, typeof(Sprite), true);

            if (GUILayout.Button("Select"))
            {
                SelectObjects();
            }
        }

        private void SelectObjects()
        {
            if (selectedSprite == null)
            {
                Debug.LogWarning("Please assign a sprite.");
                return;
            }

            // Get the root game object of the currently opened prefab
            GameObject rootGameObject = PrefabStageUtility.GetCurrentPrefabStage().prefabContentsRoot;

            if (rootGameObject == null)
            {
                Debug.LogWarning("No prefab is currently opened.");
                return;
            }

            Transform[] allTransforms = rootGameObject.GetComponentsInChildren<Transform>(true);
            System.Collections.Generic.List<GameObject> selectedObjects =
                new System.Collections.Generic.List<GameObject>();

            foreach (Transform transform in allTransforms)
            {
                SpriteRenderer spriteRenderer = transform.GetComponent<SpriteRenderer>();
                if (spriteRenderer != null && spriteRenderer.sprite == selectedSprite)
                {
                    selectedObjects.Add(transform.gameObject);
                }
            }


            Selection.objects = selectedObjects.ToArray();
        }
    }
}