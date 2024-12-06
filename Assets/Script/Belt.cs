// Belt.cs
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class Belt : BuildingBase, IAcceptable
{
    [Header("Belt Settings")]
    public float speed = 2f;
    public Transform startPoint;
    public Transform endPoint;

    private bool xdirection;
    public Vector3 direction;
    private IAcceptable nextBelt;
    public List<GameObject> itemsOnBelt = new List<GameObject>();
    public Queue<GameObject> itemsQueue = new Queue<GameObject>();
    private GameObject endItem;
    public bool frontFilled = false;

    public override BuildingType buildingType { get => BuildingType.Transport; }

    public override void OnPlaced()
    {
        UpdateSelf();
        // determine if the belt is horizontal or vertical
        xdirection = Mathf.Abs(transform.right.x) > 0.5f;
        direction = transform.right;
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
        PutItemOn();
        ClearNullItems();
        MoveItems();
        CheckItemLeave();
    }

    public void FindNextBelt()
    {

        Vector3Int outputCell = GridPosition + Utils.DirectionToVector3Int(transform.right);

        BuildingBase building = BuildingRegistry.Instance.GetBuildingAtPosition(outputCell);

        if (building != null && building is IAcceptable)
        {
            nextBelt = building as IAcceptable;
        }
        else
        {
            nextBelt = null;
        }
    }

    public bool CanAcceptItem(ItemType itemType, Vector3 direction)
    {
        // quick check if the belt is full and the queue have item
        return !frontFilled;
    }

    public void AcceptItem(GameObject item)
    {
       itemsQueue.Enqueue(item);
    }

    private void PutItemOn()
    {
        // loop through the queue and put the item on the belt
        
        if (!frontFilled && itemsQueue.Count != 0)
        {
            GameObject item = itemsQueue.Dequeue();
            item.transform.position = startPoint.position;
            item.transform.rotation = transform.rotation;
            if (xdirection)
            {
                item.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionY;
            }
            else
            {
                item.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX;
            }
            itemsOnBelt.Add(item);
        }
    }

    private void ClearNullItems()
    {
        itemsOnBelt.RemoveAll(item => item == null);
    }

    void MoveItems()
    {
        if (itemsOnBelt.Count == 0)
            return;

        foreach (var item in itemsOnBelt)
        {
            item.GetComponent<Rigidbody2D>().velocity = direction * speed;
        }
    }

    public void itemReachEnd(GameObject item) {
        
        itemsOnBelt.Remove(item);
        // if the next belt is not found, lock the item in place
        if (nextBelt == null)
        {
            item.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        }
        endItem = item;

    }

    private void CheckItemLeave() { 
        if (endItem == null)
            return;
        if (nextBelt != null && nextBelt.CanAcceptItem(endItem.GetComponent<Item>().itemType, direction)) {
            endItem.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
            nextBelt.AcceptItem(endItem);
            endItem = null;
        }
    }
       

    
}
