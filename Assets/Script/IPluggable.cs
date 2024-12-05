using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPluggable
{
    void OnPlaced();
    void OnRemoved();
}