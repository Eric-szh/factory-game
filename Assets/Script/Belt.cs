// Belt.cs
using UnityEngine;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;

public class Belt : BuildingBase
{
    [Header("Belt Settings")]
    public float speed = 2f;
    public Transform startPoint;
    public Transform endPoint;

    private bool xdirection;
    private Belt nextBelt;
    private List<GameObject> itemsOnBelt = new List<GameObject>();
    private GameObject endItem;
    public bool frontFilled = false;

    public override BuildingType buildingType { get => BuildingType.Transport; }

    public override void OnPlaced()
    {
        UpdateSelf();
        // determine if the belt is horizontal or vertical
        xdirection = Mathf.Abs(transform.right.x) > 0.5f;
    }

    public override void OnRemoved()
    {
        nextBelt = null;
        // destroy all items on the belt
        foreach (var item in itemsOnBelt)
        {
            Destroy(item);
        }
        if (endItem != null)
        {
            Destroy(endItem);
        }
    }

    public override void UpdateSelf()
    {
        FindNextBelt();
    }

    void Update()
    {
        MoveItems();
        CheckItemLeave();
    }

    public void FindNextBelt()
    {

        Vector3Int outputCell = GridPosition + DirectionToVector3Int(transform.right);

        BuildingBase building = BuildingRegistry.Instance.GetBuildingAtPosition(outputCell);

        if (building is Belt belt && belt != this)
        {
            nextBelt = belt;
        }
        else
        {
            nextBelt = null;
        }
    }

    public override bool CanAcceptItem()
    {
        return !frontFilled;
    }

    public void AcceptItem(GameObject item)
    {
        item.transform.position = startPoint.position;
        item.transform.rotation = transform.rotation;
        if (xdirection) { 
            item.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionY;
        } else
        {
            item.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX;
        }
        itemsOnBelt.Add(item);
    }

    void MoveItems()
    {
        if (itemsOnBelt.Count == 0)
            return;

        Vector2 direction = (endPoint.position - startPoint.position).normalized;

        foreach (var item in itemsOnBelt)
        {
            item.GetComponent<Rigidbody2D>().velocity = direction * speed;
        }
    }

    public void itemReachEnd(GameObject item) {
        endItem = item;
        itemsOnBelt.Remove(item);
        // lock the item in place
        item.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;

    }

    private void CheckItemLeave() { 
        if (endItem == null)
            return;
        if (nextBelt != null && nextBelt.CanAcceptItem()) {
            endItem.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
            nextBelt.AcceptItem(endItem);
            endItem = null;
        }
    }
       

    Vector3Int DirectionToVector3Int(Vector3 direction)
    {
        return new Vector3Int(
            Mathf.Abs(direction.x) > 0.5f ? (int)Mathf.Sign(direction.x) : 0,
            Mathf.Abs(direction.y) > 0.5f ? (int)Mathf.Sign(direction.y) : 0,
            Mathf.Abs(direction.z) > 0.5f ? (int)Mathf.Sign(direction.z) : 0
        );
    }
}
