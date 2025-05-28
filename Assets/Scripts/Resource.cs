using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    public bool IsBusy {  get; private set; }

    public event Action IsBusyChanged;

    public void SetBusy(bool isBusy)
    {
        if (isBusy == IsBusy) return;
        IsBusy = isBusy;
        IsBusyChanged?.Invoke();
    }

}
