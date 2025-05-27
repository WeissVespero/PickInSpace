using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ResourceManager : MonoBehaviour
{
    [SerializeField] Bounds _bounds;
    [SerializeField] private Transform _leftBotPoint;
    [SerializeField] private Transform _rightTopPoint;
    private float _xRange, _zRange;
    public float SpawnTime; // Менять через поле ввода

    public event Action<Resource> ResourceSpawned;

    public void Initialize()
    {
        Vector3 range = _leftBotPoint.position - _rightTopPoint.position;
        _xRange = _bounds.extents.x; _zRange = _bounds.extents.z;
        StartCoroutine(ResourcesSpawn());
    }

    IEnumerator ResourcesSpawn()
    {
        
        while (true)
        {
            yield return new WaitForSeconds(SpawnTime);
            var xRand = UnityEngine.Random.Range(-_xRange / 2, _xRange / 2);
            var zRand = UnityEngine.Random.Range(-_zRange / 2, _zRange / 2);
            var inBoundsPosition = new Vector3(xRand, 0, zRand);
            var spawnPosition = inBoundsPosition + _bounds.center;
            
            var resource = ResourcePool.SharedInstance.GetPooledObject();
            if (resource != null)
            {
                resource.transform.position = spawnPosition;
                resource.gameObject.SetActive(true);
                ResourceSpawned?.Invoke(resource);
            }
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(_bounds.center, _bounds.extents);

    }
}
