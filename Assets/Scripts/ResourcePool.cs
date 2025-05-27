using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;


public class ResourcePool : MonoBehaviour
{
    public static ResourcePool SharedInstance;
    public int AmountToPool;
    [SerializeField] private Transform _resourcesTransform;
    [SerializeField] private Resource _resourcePrefab;
    public List<Resource> _resources;

    private void Awake()
    {
        SharedInstance = this;
    }

    private void Start()
    {
        _resources = new List<Resource>();
        for (int i = 0; i < AmountToPool; i++)
        {
            var resource = Instantiate(_resourcePrefab, _resourcesTransform);
            resource.gameObject.SetActive(false);
            _resources.Add(resource);
        }
    }

    public Resource GetPooledObject()
    {
        for (int i = 0; i < AmountToPool; i++)
        {
            if (!_resources[i].gameObject.activeInHierarchy)
            {
                return _resources[i];
            }
        }
        return null;
    }
}
