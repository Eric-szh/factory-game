using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance;

    public int totalResources = 100;
    public List<BuildingData> buildingDataList;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public bool SpendResources(int amount)
    {
        if (totalResources >= amount)
        {
            totalResources -= amount;
            return true;
        }
        else
        {
            Debug.Log("Not enough resources!");
            return false;
        }
    }

    public void AddResources(int amount)
    {
        totalResources += amount;
    }

    public BuildingData GetBuildingData(GameObject prefab)
    {
        return buildingDataList.Find(b => b.buildingPrefab == prefab);
    }
}

