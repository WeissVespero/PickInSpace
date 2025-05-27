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

    private void Start()
    {
        Vector3 range = _leftBotPoint.position - _rightTopPoint.position;

        _xRange = _bounds.extents.x; _zRange = _bounds.extents.z;
        StartCoroutine(ResourcesSpawn());
    }



    IEnumerator ResourcesSpawn()
    {
        GameObject tmp;
        while (true)
        {
            yield return new WaitForSeconds(SpawnTime);
            var xRand = Random.Range(-_xRange / 2, _xRange / 2);
            var zRand = Random.Range(-_zRange / 2, _zRange / 2);
            var inBoundsPosition = new Vector3(xRand, 0, zRand);
            var spawnPosition = inBoundsPosition + _bounds.center;
            
            tmp = ResourcePool.SharedInstance.GetPooledObject();
            if (tmp != null)
            {
                tmp.transform.position = spawnPosition;
                tmp.SetActive(true);
            }
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        Gizmos.DrawWireCube(_bounds.center, _bounds.extents);

    }
}
