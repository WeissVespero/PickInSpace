using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;


public class ResourcePool : MonoBehaviour
{
    public static ResourcePool SharedInstance;
    public int AmountToPool;
    [SerializeField] private Transform _resourcesTransform;
    [SerializeField] private GameObject _resourcePrefab;
    [SerializeField] private List<GameObject> _resources;

    private void Awake()
    {
        SharedInstance = this;
    }

    private void Start()
    {
        _resources = new List<GameObject>();
        GameObject tmp;
        for (int i = 0; i < AmountToPool; i++)
        {
            tmp = Instantiate(_resourcePrefab, _resourcesTransform);
            tmp.SetActive(false);
            _resources.Add(tmp);
        }
    }

    public GameObject GetPooledObject()
    {
        for (int i = 0; i < AmountToPool; i++)
        {
            if (!_resources[i].activeInHierarchy)
            {
                return _resources[i];
            }
        }
        return null;
    }
}
