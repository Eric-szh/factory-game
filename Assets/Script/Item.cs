using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemType itemType;
}

public enum ItemType
{
    Copper,
    CopperPlate,
    Lead,
    LeadPlate,
}
