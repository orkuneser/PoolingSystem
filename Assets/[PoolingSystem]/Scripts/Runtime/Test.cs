using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public string id;

    private GameObject _obj;

    private void Start()
    {
        InvokeRepeating("GetObject", 1f, 3f);
        InvokeRepeating("DestoryObject", 1.5f, 3f);
    }

    public void GetObject()
    {
        _obj = PoolingSystem.Instance.InstantiatePoolObject(id);
    }

    public void DestoryObject()
    {
        PoolingSystem.Instance.DestroyPoolObject(_obj);
    }
}
