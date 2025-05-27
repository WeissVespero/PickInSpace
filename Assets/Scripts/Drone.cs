using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.XR;

public class Drone : MonoBehaviour
{
    public DroneState CurrentState = DroneState.Idle;

    public float moveSpeed = 5f; // Меняется ползунком
    public float collectTime = 2f;
    public string fractionTag;

    [SerializeField] private NavMeshAgent navMeshAgent;
    [SerializeField] private MeshRenderer _meshRenderer;

    private Transform targetResource;
    private Transform _placeHolder;
    private float collectionTimer;

    private GameObject carriedResource; // Подобранный ресурс

    public void Initialize(Color color, Transform placeHolder)
    {
        _placeHolder = placeHolder;
        _meshRenderer.material.color = color;
        transform.position = placeHolder.position;
    }

    void Update()
    {
        HandleState();
    }

    void HandleState()
    {
        switch (CurrentState)
        {
            case DroneState.SearchingForResource:
                SearchForNearestResource();
                break;
            case DroneState.MovingToResource:
                MoveToTarget(targetResource.position);
                if (Vector3.Distance(transform.position, targetResource.position) < navMeshAgent.stoppingDistance + 0.1f)
                {
                    CurrentState = DroneState.CollectingResource;
                    collectionTimer = collectTime;
                }
                break;
            case DroneState.CollectingResource:
                print("collecting begin");
                collectionTimer -= Time.deltaTime;
                if (collectionTimer <= 0)
                {
                    CollectResource();
                    CurrentState = DroneState.MovingToBase;
                }
                break;
            case DroneState.MovingToBase:
                MoveToTarget(_placeHolder.position);
                if (Vector3.Distance(transform.position, _placeHolder.position) < navMeshAgent.stoppingDistance + 0.1f)
                {
                    CurrentState = DroneState.UnloadingResource;
                }
                break;
            case DroneState.UnloadingResource:
                UnloadResource();
                CurrentState = DroneState.Idle; // После отгрузки. Может нужен SearchingForResource????
                break;
            case DroneState.Idle:
                // Дрон просто стоит на базе
                break;
        }
    }

    void SearchForNearestResource()
    {
        GameObject[] resources = GameObject.FindGameObjectsWithTag("Resource");
        Transform nearestResource = null;
        float minDistance = Mathf.Infinity;

        foreach (GameObject resource in resources)
        {
            float distance = Vector3.Distance(transform.position, resource.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestResource = resource.transform;
            }
        }

        if (nearestResource != null)
        {
            targetResource = nearestResource;
            CurrentState = DroneState.MovingToResource;
        }
        else
        {
            // No resources found, stay in searching state or idle
        }
    }

    void MoveToTarget(Vector3 targetPosition)
    {
        if (navMeshAgent != null)
        {
            if (!navMeshAgent.isOnNavMesh)
            {
                // Re-enable NavMeshAgent if it somehow got disabled or off-mesh
                // This can happen if the drone is moved by other forces (e.g., avoidance)
                // and then tries to pathfind again.
                // For a more robust solution, ensure your environment's NavMesh covers all drone movement areas.
                return;
            }
            navMeshAgent.SetDestination(targetPosition);
            // Optionally, visualize path if enabled in UI
            // You'd need to draw this separately, NavMeshAgent doesn't inherently draw its path.
        }
        else
        {
            // Simple direct movement if not using NavMesh
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }
    }

    void CollectResource()
    {
        if (targetResource != null)
        {
            // Visual effect for collection
            SpawnCollectionEffect(targetResource.position);

            // "Collect" the resource:
            // 1. Mark it as collected (e.g., destroy it or pool it)
            // 2. Associate it with the drone (e.g., parent it to drone, or just store a reference)
            carriedResource = targetResource.gameObject; // Now the drone "carries" it
            carriedResource.SetActive(false); // Make it disappear from the scene
            targetResource = null; // Clear target
        }
    }

    void UnloadResource()
    {
        if (carriedResource != null)
        {
            // Visual effect for unloading
            SpawnUnloadEffect(transform.position);

            // Dispose of the carried resource (e.g., destroy it, or return to object pool)
            Destroy(carriedResource);
            carriedResource = null;
        }
    }

    void SpawnCollectionEffect(Vector3 position)
    {
        // Implement particles, flash, scale effect here
        // Example: Instantiate(collectionParticlesPrefab, position, Quaternion.identity);
        Debug.Log("Collection Effect at: " + position);
    }

    void SpawnUnloadEffect(Vector3 position)
    {
        // Implement particles, flash, scale effect here
        // Example: Instantiate(unloadParticlesPrefab, position, Quaternion.identity);
        Debug.Log("Unload Effect at: " + position);
    }

    


    // Call this from GameManager to start the drone's cycle
    public void StartDroneCycle()
    {
        CurrentState = DroneState.SearchingForResource;
    }

    // You'll need to set the speed from the UI
    public void SetMoveSpeed(float newSpeed)
    {
        moveSpeed = newSpeed;
        if (navMeshAgent != null)
        {
            navMeshAgent.speed = newSpeed;
        }
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
