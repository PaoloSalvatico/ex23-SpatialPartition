using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{
    public float checkDistance = 30;
    public Transform player;
    public GameObject prefab;

    public int numElements = 100;
    public float spawnDistance = 50;

    private HashSet<GameObject> _elementsSets = new();
    private MyAPP.Grid<GameObject> _grid = new();
    private void Start()
    {
        for(int i = 0; i < numElements; i++)
        {
            var go = Instantiate(prefab);
            var position = Random.insideUnitCircle * spawnDistance;
            go.transform.position = new Vector3(position.x, 0, position.y);
            _elementsSets.Add(go);
            go.GetComponent<GameElement>().OnItemDestroy += ItemDestroy;
        }
    }

    private void ItemDestroy(GameObject go)
    {
        _elementsSets.Remove(go);
    }

    private void Update()
    {
        _grid.ClearNoAlloc();
        foreach(var el in _elementsSets)
        {
            var gridPosition = MyAPP.Grid<GameObject>.ToGridPosition(el.transform.position, checkDistance / 3);
            _grid.Add(gridPosition, el);
        }
        // if(Input.GetButtonDown("Fire1"))
        UpdateNeighbours();

        if (Input.GetButtonDown("Fire2"))
        {
            ShowAll();
        }
    }

    private void UpdateNeighbours()
    {
        var set = new HashSet<GameObject>();
        Rebuild(player, set);
        foreach(var element in _elementsSets)
        {
            element.SetActive(set.Contains(element));
        }
    }

    private void ShowAll()
    {
        foreach(var el in _elementsSets)
        {
            el.SetActive(true);
        }
    }

    public bool Check(Transform pl, Transform el)
    {
        var plOnGrid = MyAPP.Grid<GameObject>.ToGridPosition(pl.position, checkDistance / 3);
        var elOnGrid = MyAPP.Grid<GameObject>.ToGridPosition(el.position, checkDistance / 3);
        return (plOnGrid - elOnGrid).sqrMagnitude <= 2;
    }

    public void Rebuild(Transform pl, HashSet<GameObject> elements)
    {
        var positionOnGrid = MyAPP.Grid<GameObject>.ToGridPosition(pl.position, checkDistance / 3);
        _grid.GetNeighnours(positionOnGrid, elements);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(player.position, new Vector3(checkDistance, 2, checkDistance));
    }
}
