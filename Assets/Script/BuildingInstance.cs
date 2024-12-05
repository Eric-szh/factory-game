using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingInstance : MonoBehaviour
{
    public BuildingData buildingData;


    public void Dismantle()
    {
        ResourceManager.Instance.AddResources(buildingData.cost);
        buildingData.currentCount--;
        Destroy(gameObject);
    }
}
