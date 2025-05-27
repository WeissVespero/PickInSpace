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
    [SerializeField] private Animator _animator;
    

    private Resource _targetResource;
    private Transform _placeHolder;
    private float collectionTimer;

    //private GameObject carriedResource; // Подобранный ресурс

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
                var targetPos = _targetResource.transform.position;
                MoveToTarget(targetPos);
                if (Vector3.Distance(transform.position, targetPos) < navMeshAgent.stoppingDistance + 0.1f)
                {
                    _targetResource.IsOwnedChanged -= OnTargetResourceIsBusy;
                    _targetResource.SetBusy(true);
                    CurrentState = DroneState.CollectingResource;
                    collectionTimer = collectTime;
                }
                break;
            case DroneState.CollectingResource:
               
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
                CurrentState = DroneState.SearchingForResource; // После отгрузки. Может нужен SearchingForResource????
                break;
            case DroneState.Idle:
                _animator.SetBool("Fly", false);
                // Дрон просто стоит на базе
                break;
        }
    }

    void SearchForNearestResource()
    {
        Resource nearestResource = null;
        float minDistance = Mathf.Infinity;

        foreach (Resource resource in ResourcePool.SharedInstance._resources)
        {
            if (resource.IsBusy) continue;
            float distance = Vector3.Distance(transform.position, resource.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestResource = resource;
            }
        }

        if (nearestResource != null)
        {
            _targetResource = nearestResource;
            _targetResource.IsOwnedChanged += OnTargetResourceIsBusy;
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
                
                return;
            }
            navMeshAgent.SetDestination(targetPosition);
            _animator.SetBool("Fly", true);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }
    }

    void CollectResource()
    {
        if (_targetResource != null)
        {
            
            // Visual effect for collection
            _animator.SetTrigger("Res");
            SpawnCollectionEffect(_targetResource.transform.position);
            
            _targetResource.gameObject.SetActive(false);
           
        }
    }

    void UnloadResource()
    {
        if (_targetResource != null)
        {
            SpawnUnloadEffect(transform.position);

            _targetResource = null;
        }
    }

    void SpawnCollectionEffect(Vector3 position)
    {
        Debug.Log("Collection Effect at: " + position);
    }

    void SpawnUnloadEffect(Vector3 position)
    {
        Debug.Log("Unload Effect at: " + position);
    }

    public void StartDroneCycle(Resource resource)
    {
        CurrentState = DroneState.SearchingForResource;
    }

    private void OnTargetResourceIsBusy()
    {
        if (_targetResource.IsBusy)
        {
            CurrentState = DroneState.MovingToBase;
            _targetResource.IsOwnedChanged -= OnTargetResourceIsBusy;
            _targetResource = null;
        }
        
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
