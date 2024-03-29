using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolingSystem : Singleton<PoolingSystem>
{
    [SerializeField] private List<Pool> _pools = new List<Pool>();
    [SerializeField] private int _defaultSize = 10;

    private void Start()
    {
        InitPoolObjects();
    }

    private void InitPoolObjects()
    {
        int _tempSize = _defaultSize;

        for (int i = 0; i < _pools.Count; i++)
        {
            if (_pools[i].InitSize != 0)
                _tempSize = _pools[i].InitSize;

            for (int j = 0; j < _tempSize; j++)
            {
                GameObject obj = Instantiate(_pools[i].Prefab, transform);
                obj.SetActive(false);

                if (!obj.TryGetComponent(out PoolObject _))
                    obj.AddComponent<PoolObject>().Initialize(_pools[i].PoolID);

                _pools[i].cloneObjects.Add(obj);
            }
        }
    }

    public GameObject InstantiatePoolObject(string poolID)
    {
        for (int i = 0; i < _pools.Count; i++)
        {
            if (string.Equals(_pools[i].PoolID, poolID))
            {
                for (int j = 0; j < _pools[i].cloneObjects.Count; j++)
                {
                    if (!_pools[i].cloneObjects[j].activeInHierarchy)
                    {
                        _pools[i].cloneObjects[j].SetActive(true);

                        return _pools[i].cloneObjects[j];
                    }
                }
            }
        }
        return null;
    }

    public GameObject InstantiatePoolObject(string ID, Vector3 position)
    {
        GameObject obj = InstantiatePoolObject(ID);
        if (obj)
        {
            obj.transform.position = position;
            return obj;
        }
        else
            return null;
    }

    public GameObject InstantiatePoolObject(string ID, Vector3 position, Quaternion rotation)
    {
        GameObject obj = InstantiatePoolObject(ID);
        if (obj)
        {
            obj.transform.position = position;
            obj.transform.rotation = rotation;
            return obj;
        }
        else
            return null;
    }

    public void DestroyPoolObject(GameObject cloneObj)
    {
        cloneObj.transform.position = transform.position;
        cloneObj.transform.rotation = transform.rotation;
        cloneObj.GetComponent<PoolObject>().OnBackToPool();
        cloneObj.transform.SetParent(transform);

        cloneObj.SetActive(false);
    }

    public void DestroyPoolObject(GameObject cloneObj, float waitTime)
    {
        StartCoroutine(DestroyPoolObjectCo(cloneObj, waitTime));
    }

    private IEnumerator DestroyPoolObjectCo(GameObject clone, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        DestroyPoolObject(clone);
    }
}
