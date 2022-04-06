using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameElement : MonoBehaviour
{
    public delegate void MyDelegate(GameObject go);
    public event MyDelegate OnItemDestroy;

    private void OnDestroy()
    {
        if (OnItemDestroy != null) OnItemDestroy.Invoke(gameObject);
    }
}
