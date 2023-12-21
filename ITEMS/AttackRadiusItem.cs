using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "AttackRadius Item", menuName = "Inventory/Items/New AttackRadius Item")]

public class AttackRadiusItem : ItemScriptableObject
{
    public float radiusPlusAmount;
    public float moveSpeedMinusAmount;
    public float minusMaxHealthAmount;


    private void Start()
    {
        itemType = ItemType.AttackRadius;
    }
}
