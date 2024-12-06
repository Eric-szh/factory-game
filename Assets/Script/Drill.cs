// Drill.cs
using NUnit.Framework.Constraints;
using UnityEngine;

public class Drill : BuildingBase
{
    [Header("Drill Settings")]
    public float productionInterval = 2f;
    public GameObject itemPrefab;
    public Transform outputPoint;

    public Vector3 outputDir;
    private float productionTimer = 0f;
    private Belt outputBelt;

    public override BuildingType buildingType { get => BuildingType.Produce; }

    public override void OnPlaced()
    {
        outputDir = transform.up;
        UpdateSelf();
    }

    public override void OnRemoved()
    {
        outputBelt = null;
    }

    public override void UpdateSelf()
    {
        FindOutputBelt();
    }

    void Update()
    {
        productionTimer += Time.deltaTime;

        if (productionTimer >= productionInterval)
        {
            ProduceItem();
            productionTimer = 0f;
        }
    }

    void FindOutputBelt()
    {
        Vector3Int outputCell = GridPosition + Utils.DirectionToVector3Int(outputDir);

        BuildingBase building = BuildingRegistry.Instance.GetBuildingAtPosition(outputCell);

        if (building is Belt belt)
        {
            // check the belt's direction
            Vector3 beltDirection = belt.direction;
            // if the belt is pointing to the factory, the factory cannot output to it
            if (beltDirection == -outputDir)
            {
                outputBelt = null;
                return;
            }
            outputBelt = belt;
        }
        else
        {
            outputBelt = null;
        }
    }

    void ProduceItem()
    {
        if (outputBelt != null && outputBelt.CanAcceptItem(itemPrefab.GetComponent<Item>().itemType, transform.up))
        {
            GameObject item = Instantiate(itemPrefab, outputPoint.position, Quaternion.identity);
            outputBelt.AcceptItem(item);
        }
    }

}
