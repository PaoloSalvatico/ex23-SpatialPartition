using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;

public class SceneStreamer : MonoBehaviour
{
    public Transform player;
    public float refreshTime = 0.5f;
    public float checkdistance = 30;

    private MyAPP.Grid<GameObject> _grid = new();

    private List<Vector2Int> _scenePositions = new();
    private Dictionary<Vector2Int, SceneInstance> _loadedScenes = new();

    private void Start()
    {
        StartCoroutine(UpdateScene());
    }

    IEnumerator UpdateScene()
    {
        while(true)
        {
            yield return new WaitForSeconds(refreshTime);

            // Logica di check della griglia e caricamento delle scene
            var pos = MyAPP.Grid<GameObject>.ToGridPosition(player.position, checkdistance / 3);
            var newPositions = _grid.OffSets.Select(offset => pos + offset).ToList();

            var toLoad = newPositions.Except(_scenePositions);
            var toRemove = _scenePositions.Except(newPositions);

            // Carico le scene
            foreach(var posToLoad in toLoad)
            {
                var handle = Addressables.LoadSceneAsync("Scene " + posToLoad, LoadSceneMode.Additive);
                yield return handle;
                if (handle.Status == AsyncOperationStatus.Failed) continue;
                _loadedScenes.Add(posToLoad, handle.Result);
            }

            // Rimuovo le scene
            foreach (var posToRemove in toRemove)
            {
                if (!_loadedScenes.TryGetValue(posToRemove, out var scene)) continue;
                _loadedScenes.Remove(posToRemove);
                Addressables.UnloadSceneAsync(scene);
            }


            _scenePositions = newPositions;
        }
    }
}
