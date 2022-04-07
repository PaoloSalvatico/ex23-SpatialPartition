using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneSpawner : MonoBehaviour
{
    [Range(1, 1000)]
    public int numElements = 100;
    public GameObject prefab;
    [Range(10,300)]
    public float spawnRange = 100;


}
