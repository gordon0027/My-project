using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ItemType {Damage, AttackSpeed, Regeneration, 
                     MoveSpeed, AttackRadius, SplashRate, 
                     HealthItem}
public class ItemScriptableObject : ScriptableObject
{
   public int price;
   public ItemType itemType;
   public string itemName;
   public GameObject itemPrefab;
   public Sprite icon;
   public int maximumAmount;
   public string itemDescription;
   public string[] itemStats;

}



