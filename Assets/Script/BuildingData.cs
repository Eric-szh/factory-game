using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BuildingData
{
    public string buildingName;
    public GameObject buildingPrefab;
    public int cost;
    public int maxAllowance;
    [HideInInspector]
    public int currentCount;
}

