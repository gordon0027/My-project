using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Health Item", menuName = "Inventory/Items/New Health Item")]
public class HealthItem : ItemScriptableObject
{
    
    public float plusHpRegeneration;
    public float plusMaxHP;
    public float minusMoveSpeed;
    

    private void Start()
    {
        itemType = ItemType.HealthItem;
    }
}

