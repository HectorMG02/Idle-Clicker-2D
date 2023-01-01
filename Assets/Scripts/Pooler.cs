using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pooler : MonoBehaviour
{
    [SerializeField] private GameObject gameObjectToCreate;
    [SerializeField] private int quantity;

    private List<GameObject> instances = new List<GameObject>();
    private GameObject _poolerParent;

    public Transform PoolerParent => _poolerParent.transform;

    private void Awake()
    {
        _poolerParent = new GameObject($"Pooler - {gameObjectToCreate.name}");
        CreatePooler();
    }

    private void CreatePooler()
    {
        for (int i = 0; i < quantity; i++)
        {
            GameObject instance = Instantiate(gameObjectToCreate, _poolerParent.transform);
            instance.SetActive(false);
            instances.Add(instance);
        }
    }


    public GameObject GetPoolerInstance()
    {
        for (int i = 0; i < instances.Count; i++)
        {
            if (!instances[i].activeInHierarchy)
            {
                return instances[i];
            }
        }

        return null;
    }
}
