using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    public bool IsBusy {  get; private set; }

    public event Action IsOwnedChanged;

    public void SetBusy(bool isOwned)
    {
        if (isOwned == IsBusy) return;
        IsBusy = isOwned;
        IsOwnedChanged?.Invoke();
    }

}
