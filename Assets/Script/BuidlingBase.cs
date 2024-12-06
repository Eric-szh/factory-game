// BuildingBase.cs
using UnityEngine;

public abstract class BuildingBase : MonoBehaviour, IPluggable
{
    public Vector3Int GridPosition { get; set; }
    public abstract BuildingType buildingType { get; }

    public abstract void OnPlaced();
    public abstract void OnRemoved();

    // other building when placed will trigger this method to update itself
    public abstract void UpdateSelf();

    public abstract bool CanAcceptItem();
}

public enum BuildingType
{
    Produce,
    Transport,
    Convert
}