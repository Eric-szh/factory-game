using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils 
{
    public static Vector3Int DirectionToVector3Int(Vector3 direction)
    {
        return new Vector3Int(
            Mathf.Abs(direction.x) > 0.5f ? (int)Mathf.Sign(direction.x) : 0,
            Mathf.Abs(direction.y) > 0.5f ? (int)Mathf.Sign(direction.y) : 0,
            Mathf.Abs(direction.z) > 0.5f ? (int)Mathf.Sign(direction.z) : 0
        );
    }
}
