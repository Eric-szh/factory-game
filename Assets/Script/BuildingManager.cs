using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// BuildingManager.cs
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    public static BuildingManager Instance { get; private set; }

    private GameObject buildingToPlace;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void SelectBuilding(GameObject buildingPrefab)
    {
        buildingToPlace = buildingPrefab;
    }

    public GameObject GetSelectedBuilding()
    {
        return buildingToPlace;
    }
}

