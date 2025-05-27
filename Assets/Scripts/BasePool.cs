using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BasePool : MonoBehaviour
{
    public int amountToPool;
    public int amountOfActive; // Меняется ползунком

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
            _drones.Add(tmp);
        }
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
