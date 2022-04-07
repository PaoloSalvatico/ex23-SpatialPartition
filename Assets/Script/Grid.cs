using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyAPP
{
    public class Grid<T>
    {
        private Vector2Int[] neighbourOffsets = {
        Vector2Int.zero,
        Vector2Int.up,
        Vector2Int.down,
        Vector2Int.right,
        Vector2Int.left,
        Vector2Int.up + Vector2Int.right,
        Vector2Int.up + Vector2Int.left,
        Vector2Int.down + Vector2Int.right,
        Vector2Int.down + Vector2Int.left
    };

        private Dictionary<Vector2Int, HashSet<T>> _grid = new();

        public Dictionary<Vector2Int, HashSet<T>> GetGrid => _grid;

        public Vector2Int[] OffSets => neighbourOffsets;

        // Funzione che svuota tutti gl ielementi della griglia senza distruggerli
        public void ClearNoAlloc()
        {
            foreach (var hashList in _grid.Values)
            {
                hashList.Clear();
            }
        }

        // Aggiunge elementi che si trovano in una data posizione
        public void GetAtPosition(Vector2Int position, HashSet<T> results)
        {
            if (!_grid.TryGetValue(position, out var hashSet)) return;
            foreach (var val in hashSet)
            {
                results.Add(val);
            }
        }

        // Cerca tutti gli elementi che si trovano nelle 9 celle in considerazione
        public void GetNeighnours(Vector2Int position, HashSet<T> result)
        {
            result.Clear();
            foreach (var offset in neighbourOffsets)
            {
                GetAtPosition(position + offset, result);
            }
        }

        // Aggiungo l'elemento alla grilgia nella posizione corretta
        public void Add(Vector2Int position, T value)
        {
            if (!_grid.TryGetValue(position, out var hashSet))
            {
                // è come dire => hashSet = new HashSet<T>();
                hashSet = new();
                _grid[position] = hashSet;
            }
            hashSet.Add(value);
        }

        // Funzione che prende le coordinate di un elemento 
        public static Vector2Int ToGridPosition(Vector3 position, float cellSize)
        {
            var pos = new Vector2(position.x, position.z) / cellSize;
            return Vector2Int.RoundToInt(pos);
        }
    }
}
