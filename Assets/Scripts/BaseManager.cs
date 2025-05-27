using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseManager : MonoBehaviour
{
    [SerializeField] private Transform[] _dronePlaceHolders;
    [SerializeField] private BasePool _basePool;
    [SerializeField] private Color _color;
    [SerializeField] private MeshRenderer _meshRenderer;

    private void Start()
    {
        _meshRenderer.material.color = _color;
    }

    public void Initialize(int amount)
    {
        _basePool.Initialize();
        InitDrones(amount);
    }

    private void InitDrones(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            var drone = _basePool.GetPooledObject();
            if (drone == null) continue;
            var placeHolder = _dronePlaceHolders[i];
            drone.Initialize(_color, placeHolder);
            
        }
    }
}
