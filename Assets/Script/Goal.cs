using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : BuildingBase, IAcceptable
{
    public override BuildingType buildingType => BuildingType.Convert;

    public void AcceptItem(GameObject item)
    {
        // check the type of the item, for copper, add 1 resource, for copper plate, add 5 resources
        ItemType type = item.GetComponent<Item>().itemType;
        TaskManager.Instance.AddItem(type);

        /*
        if (type == ItemType.Copper)
        {
            ResourceManager.Instance.AddResources(1);
        }
        else if (type == ItemType.CopperPlate)
        {
            ResourceManager.Instance.AddResources(5);
        }
        */

        // first , destroy the item
        Destroy(item);

    }

    public bool CanAcceptItem(ItemType type, Vector3 direction)
    {
        return true;
    }

    public override void OnPlaced()
    {
        return;
    }

    public override void OnRemoved()
    {
        return;
    }

    public override void UpdateSelf()
    {
        return;
    }

}
