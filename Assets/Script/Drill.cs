// Drill.cs
using UnityEngine;

public class Drill : BuildingBase
{
    [Header("Drill Settings")]
    public float productionInterval = 2f;
    public GameObject itemPrefab;
    public Transform outputPoint;

    private float productionTimer = 0f;
    private Belt outputBelt;

    public override BuildingType buildingType { get => BuildingType.Produce; }

    public override void OnPlaced()
    {
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

    public override bool CanAcceptItem()
    {
        return false;
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
        Vector3Int outputCell = GridPosition + DirectionToVector3Int(transform.up);

        BuildingBase building = BuildingRegistry.Instance.GetBuildingAtPosition(outputCell);

        if (building is Belt belt)
        {
            outputBelt = belt;
        }
        else
        {
            outputBelt = null;
        }
    }

    void ProduceItem()
    {
        if (outputBelt != null && outputBelt.CanAcceptItem())
        {
            GameObject item = Instantiate(itemPrefab, outputPoint.position, Quaternion.identity);
            outputBelt.AcceptItem(item);
        }
    }

    Vector3Int DirectionToVector3Int(Vector3 direction)
    {
        if (direction == transform.up)
            return Vector3Int.RoundToInt(direction.normalized);
        else
            return Vector3Int.zero;
    }
}
