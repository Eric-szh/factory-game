using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// BuildingRegistry.cs
using UnityEngine;
using System.Collections.Generic;

public class BuildingRegistry : MonoBehaviour
{
    public static BuildingRegistry Instance { get; private set; }

    private Dictionary<Vector3Int, BuildingBase> buildingMap = new Dictionary<Vector3Int, BuildingBase>();

    private void Awake()
    {
        Instance = this;
    }

    public void AddBuilding(Vector3Int position, BuildingBase building)
    {
        buildingMap[position] = building;
    }

    public void RemoveBuilding(Vector3Int position)
    {
        buildingMap.Remove(position);
    }

    public BuildingBase GetBuildingAtPosition(Vector3Int position)
    {
        buildingMap.TryGetValue(position, out BuildingBase building);
        return building;
    }
}
