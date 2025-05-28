using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private BaseManager _base1;
    [SerializeField] private BaseManager _base2;
    [SerializeField] private ResourceManager _resourceManager;
    [SerializeField] private UIManager _uIManager;
    private List<ResourceManager> _resources = new List<ResourceManager>(); // нужен список свободных ресурсов

    private void Start()
    {
        _base1.Initialize(3);
        _base2.Initialize(3);
        _resourceManager.Initialize();
        _uIManager.Subscribe();
        Subscribe();
    }

    private void Subscribe()
    {
        _resourceManager.ResourceSpawned += FindFreeDrone;
    }

    private void FindFreeDrone(Resource resource)
    {
        _base1.FindFreeDrone(resource);
        _base2.FindFreeDrone(resource);
    }
}