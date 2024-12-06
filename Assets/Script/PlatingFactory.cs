using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatingFactory : BuildingBase, IAcceptable
{
    [Header("Plating Factory Settings")]
    public float productionInterval = 2f;
    public GameObject itemPrefab;
    public Transform outputPoint;
    public Vector3 inputDir;
    public Vector3 outputDir;


    private float productionTimer = 0f;
    private Belt outputBelt;
    private bool itemProcessing = false;

    public override BuildingType buildingType { get => BuildingType.Convert; }

    public override void OnPlaced()
    {
        UpdateSelf();
        inputDir = -transform.right;
        outputDir = transform.right;
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
        if (itemProcessing)
        {
            productionTimer += Time.deltaTime;
        }


        if (productionTimer >= productionInterval)
        {
            ProduceItem();
            productionTimer = 0f;
        }
    }

    void FindOutputBelt()
    {
        Vector3Int outputCell = GridPosition + Utils.DirectionToVector3Int(transform.right);

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
            else
            {
                outputBelt = belt;
            }
        }
        else
        {
            outputBelt = null;
        }
    }

    void ProduceItem()
    {
        if (outputBelt == null)
        {
            return;
        }

        if (outputBelt.CanAcceptItem(ItemType.CopperPlate, outputDir))
        {
            GameObject outputitem = Instantiate(itemPrefab, outputPoint.position, Quaternion.identity);
            outputBelt.AcceptItem(outputitem);
            itemProcessing = false;
            return;
        }
            
    }

    public void AcceptItem(GameObject item)
    {
        // first destroy the item
        Destroy(item);
        itemProcessing = true;
    }

    public bool CanAcceptItem(ItemType type, Vector3 direction)
    {
        if (type == ItemType.Copper && direction == -inputDir)
        {
            return !itemProcessing;
        }
        else
        {
            return false;
        }
    }
}   
