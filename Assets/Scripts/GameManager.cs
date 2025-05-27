using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private BaseManager _base1;
    [SerializeField] private BaseManager _base2;
    [SerializeField] private ResourceManager _resourceManager;

    private void Start()
    {
        _base1.Initialize(3);
        _base2.Initialize(3);
    }
}
