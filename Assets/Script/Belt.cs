// Belt.cs
using UnityEngine;
using System.Collections.Generic;

public class Belt : BuildingBase
{
    [Header("Belt Settings")]
    public float speed = 2f;
    public Transform startPoint;
    public Transform endPoint;

    private Belt nextBelt;
    private Queue<GameObject> itemsOnBelt = new Queue<GameObject>();

    public override void OnPlaced()
    {
        UpdateSelf();
        // No need to notify neighbors here
    }

    public override void OnRemoved()
    {
        nextBelt = null;
        // No need to notify neighbors here
    }

    public override void UpdateSelf()
    {
        FindNextBelt();
    }

    void Update()
    {
        MoveItems();
    }

    public void FindNextBelt()
    {
        Debug.Log(GridPosition + " " + transform.right);
        Debug.Log(DirectionToVector3Int(transform.right));
        Debug.Log(GridPosition + DirectionToVector3Int(transform.right));
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

    public bool CanAcceptItem()
    {
        return true;
    }

    public void AcceptItem(GameObject item)
    {
        item.transform.position = startPoint.position;
        item.transform.rotation = transform.rotation;
        itemsOnBelt.Enqueue(item);
    }

    void MoveItems()
    {
        if (itemsOnBelt.Count == 0)
            return;

        foreach (var item in itemsOnBelt)
        {
            Vector3 direction = (endPoint.position - startPoint.position).normalized;
            item.transform.position += direction * speed * Time.deltaTime;
        }

        GameObject firstItem = itemsOnBelt.Peek();
        if (Vector3.Distance(firstItem.transform.position, endPoint.position) < 0.1f)
        {
            itemsOnBelt.Dequeue();

            if (nextBelt != null)
            {
                nextBelt.AcceptItem(firstItem);
            }
            else
            {
                Destroy(firstItem);
            }
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
