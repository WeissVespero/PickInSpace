using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BasePool : MonoBehaviour
{
    public int amountToPool;
    public BaseManager MyBase;

    [SerializeField] private Transform _baseTransform;
    [SerializeField] private Drone _dronePrefab;
    [SerializeField] private List<Drone> _drones;
    
    public void Initialize()
    {
        _drones = new List<Drone>();
        Drone tmp;
        for (int i = 0; i < amountToPool; i++)
        {
            tmp = Instantiate(_dronePrefab, _baseTransform);
            tmp.gameObject.SetActive(false);
            tmp._myBase = MyBase;
            _drones.Add(tmp);
        }
    }

    public Drone GetFreeDrone()
    {
        for (int i = 0; i < amountToPool; i++)
        {
            if (_drones[i].gameObject.activeInHierarchy && _drones[i].CurrentState == Drone.DroneState.Idle)
            {
                return _drones[i];
            }
        }
        return null;
    }

    public Drone GetPooledObject()
    {
        for (int i = 0; i < amountToPool; i++)
        {
            if (!_drones[i].gameObject.activeInHierarchy)
            {
                _drones[i].gameObject.SetActive(true);
                return _drones[i];
            }
        }
        return null;
    }
}
