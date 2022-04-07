using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System.IO;

[CustomEditor(typeof(SceneGridGenerator))]
public class SceneGridGeneratorEditor : Editor
{
    SceneGridGenerator _generator;

    private void OnEnable()
    {
        _generator = target as SceneGridGenerator;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (!GUILayout.Button("GenerateGrid")) return;

        _generator.gridContainer.position = Vector3.zero;
        _generator.gridContainer.rotation = Quaternion.identity;
        _generator.gridContainer.localScale = Vector3.one;

        var allElements = SceneManager.GetActiveScene().GetRootGameObjects();
        var grid = new MyAPP.Grid<GameObject>();

        foreach(var go in allElements)
        {
            if (_generator.exclusion.Contains(go)) continue;
            Vector2Int position = MyAPP.Grid<GameObject>.ToGridPosition(go.transform.position, _generator.cellSize);
            grid.Add(position, go);
        }

        // creo le cartelle che mi servono
        var sectionsFolderPath = Application.dataPath + "/Resources/Sections/";
        var sceneFolderPath = Application.dataPath + "/Addressables/Scenes/";
        if (!Directory.Exists(sectionsFolderPath)) Directory.CreateDirectory(sectionsFolderPath);
        if (!Directory.Exists(sceneFolderPath)) Directory.CreateDirectory(sceneFolderPath);

        // Creo i prefab
        foreach(var key in grid.GetGrid.Keys)
        {
            var set = new HashSet<GameObject>();
            grid.GetAtPosition(key, set);

            var section = new GameObject("Section " + key);
            section.transform.position = new Vector3(key.x * _generator.cellSize, 0, key.y * _generator.cellSize);
            section.transform.SetParent(_generator.gridContainer);

            foreach (var go in set)
            {
                if (_generator.cloneElements)
                {
                    var clone = Instantiate(go, section.transform);
                    clone.transform.localPosition = Vector3.zero;
                }
                else
                {
                    go.transform.SetParent(section.transform);
                }
            }
            var prefabPath = sectionsFolderPath + section.name + ".prefab";
            PrefabUtility.SaveAsPrefabAsset(section, prefabPath);
        }


        // Creo le scene
        foreach(var key in grid.GetGrid.Keys)
        {
            var sceneName = "Scene " + key + ".unity";
            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);
            scene.name = sceneName;
            var go = Resources.Load<GameObject>("Sections/Section " + key);
            Instantiate(go);

            var scenePath = sceneFolderPath + sceneName;
            EditorSceneManager.SaveScene(scene, scenePath);

        }



        AssetDatabase.Refresh();
    }
}
