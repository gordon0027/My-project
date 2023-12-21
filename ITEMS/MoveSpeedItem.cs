using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Move Speed Item", menuName = "Inventory/Items/New MoveSpeed Item")]
public class MoveSpeedItem : ItemScriptableObject
{
    public float moveSpeedAmount;
    public float radiusMinusAmount;

    private void Start()
    {
        itemType = ItemType.MoveSpeed;
    }
}
