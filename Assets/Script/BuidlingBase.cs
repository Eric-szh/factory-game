// BuildingBase.cs
using UnityEngine;

public abstract class BuildingBase : MonoBehaviour, IPluggable
{
    public Vector3Int GridPosition { get; set; }

    public abstract void OnPlaced();
    public abstract void OnRemoved();

    // other building when placed will trigger this method to update itself
    public abstract void UpdateSelf();
}
