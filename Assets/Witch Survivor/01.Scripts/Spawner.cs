using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    private Transform[] points = null;

    private void Awake()
    {
        points = GetComponentsInChildren<Transform>();
    }
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.E))
        {
            Transform point = points[Random.Range(1, points.Length)];
            GameObject newEnemy =  PoolManager.Instance.Spawn("Enemy");
            newEnemy.transform.SetParent(point,false);
            newEnemy.transform.parent = null;
        }

    }
}
