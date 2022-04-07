using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SceneSpawner))]
public class SceneSpawnerEditor : Editor
{
    private SceneSpawner _sceneSpawner;

    private void OnEnable()
    {
        _sceneSpawner = target as SceneSpawner;
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if(GUILayout.Button("Generate"))
        {
            for(int i = 0; i < _sceneSpawner.numElements; i++)
            {
                var go = Instantiate(_sceneSpawner.prefab);
                var pos = Random.insideUnitCircle * _sceneSpawner.spawnRange;
                go.transform.position = new Vector3(pos.x, 0, pos.y);
                //var scale = Random.Range(0, 20f);
                //go.transform.localScale = scale * Vector3.one;
                go.name = "Object " + pos;
            }
        }
    }
}
