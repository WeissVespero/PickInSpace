using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.XR;

public class Drone : MonoBehaviour
{
    public DroneState currentState = DroneState.Idle;

    [SerializeField] NavMeshAgent agent;
    [SerializeField] private MeshRenderer _meshRenderer;

    private Transform _placeHolder;

    public void Initialize(Color color, Transform placeHolder)
    {
        _placeHolder = placeHolder;
        _meshRenderer.material.color = color;
        transform.position = placeHolder.position;
        
}

    public enum DroneState
    {
        Idle,
        SearchingForResource,
        MovingToResource,
        CollectingResource,
        MovingToBase,
        UnloadingResource
    }
}
