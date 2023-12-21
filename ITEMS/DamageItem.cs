using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Damage Item", menuName = "Inventory/Items/New Damage Item")]
public class DamageItem : ItemScriptableObject
{
    public float damageAmount;

    private void Start()
    {
        itemType = ItemType.Damage;
    }
}