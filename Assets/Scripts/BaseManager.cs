using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseManager : MonoBehaviour
{
    [SerializeField] private Transform[] _dronePlaceHolders;
    [SerializeField] private BasePool _basePool;
    [SerializeField] private Color _color;
    [SerializeField] private MeshRenderer _meshRenderer;
    private int collectedResourcesCount = 0;

    public Fraction FractionTag;
    public int amountOfActive; // Меняется ползунком

    public delegate void OnResourceCollected(Fraction fraction, int count);
    public static event OnResourceCollected onResourceCollected;

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
            drone.Initialize(FractionTag, _color, placeHolder);
        }
    }

    public int GetCollectedResourcesCount()
    {
        return collectedResourcesCount;
    }

    public void AddResource(Fraction deliveredFactionTag)
    {
        if (deliveredFactionTag == FractionTag)
        {
            collectedResourcesCount++;
            Debug.Log($"{FractionTag} Base: Collected {collectedResourcesCount} resources.");

            // Notify UI
            if (onResourceCollected != null)
            {
                onResourceCollected(FractionTag, collectedResourcesCount);
            }
        }
        else
        {
            Debug.LogWarning($"Drone from {deliveredFactionTag} tried to deliver to {FractionTag} base!");
        }
    }

    public Drone FindFreeDrone(Resource resource)
    {
        var drone = _basePool.GetFreeDrone();
        if (drone == null || drone.CurrentState != Drone.DroneState.Idle) return null;
        drone.StartDroneCycle(resource);
        return drone;
    }
}

public enum Fraction
{
    Yellow,
    Green
}
