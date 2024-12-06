using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAcceptable
{
    void AcceptItem(GameObject item);

    bool CanAcceptItem(ItemType type);
}
