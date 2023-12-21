using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Attack Speed Item", menuName = "Inventory/Items/New AttackSpeed Item")]
public class AttackSpeedItem : ItemScriptableObject
{
    public float attackSpeedAmount;
    public float attackSpeedMinusAmount;
    public float radiusMinusAmount;
    public float radiusPlusAmount;
   
    

    private void Start()
    {
        itemType = ItemType.AttackSpeed;
    }
}
