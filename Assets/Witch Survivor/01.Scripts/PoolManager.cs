using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PoolManager : MonoSingleton<PoolManager>
{
    [System.Serializable]

    private class ObjectInfo
    {
        public string objectName;
        public GameObject prefab;
        public int count;
    }

    public bool IsReady { get; private set; }

    [SerializeField]
    private ObjectInfo[] objectInfos = null;

    private string objectName;
    private Dictionary<string, IObjectPool<GameObject>> objectPoolDictionary = new Dictionary<string, IObjectPool<GameObject>>();
    private Dictionary<string, GameObject> gameObjectDitionary = new Dictionary<string, GameObject>();

    private void Start()
    {
        OnInit();
    }

    private void OnInit()
    {
        IsReady = false;

        for (int idx = 0; idx < objectInfos.Length; idx++)
        {
            IObjectPool<GameObject> pool = new ObjectPool<GameObject>(CreateNewObject, OnGetPoolObject, OnRelease, OndestroyPoolObject, true, objectInfos[idx].count);
            if (gameObjectDitionary.ContainsKey(objectInfos[idx].objectName))
            {
                Debug.LogFormat("{0} : Already assigned.", objectInfos[idx].objectName);
            }

            gameObjectDitionary.Add(objectInfos[idx].objectName, objectInfos[idx].prefab);
            objectPoolDictionary.Add(objectInfos[idx].objectName, pool);
            
            for (int i = 0; i < objectInfos[idx].count; i++)
            {
                objectName = objectInfos[idx].objectName;
                Poolable poolable = CreateNewObject().GetComponent<Poolable>();
                poolable.pool.Release(poolable.gameObject);
            }
        }
        Debug.Log("[PoolManager] Ready to Pool");
    }

    private void OndestroyPoolObject(GameObject obj)
    {
        Destroy(obj);
    }

    private void OnRelease(GameObject obj)
    {
        obj.SetActive(false);
    }

    private void OnGetPoolObject(GameObject obj)
    {
        obj.SetActive(true);
    }

    private GameObject CreateNewObject()
    {
        GameObject newObject = Instantiate(gameObjectDitionary[objectName]);
        newObject.GetComponent<Poolable>().pool = objectPoolDictionary[objectName];
        return newObject;
    }

    public GameObject Spawn(string name)
    {
        objectName = name;
        if(!gameObjectDitionary.ContainsKey(objectName))
        {
            Debug.LogFormat("{0} : Key not found in Pool", name);
            return null;
        }

        return objectPoolDictionary[name].Get();
    }
}
