using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "SplashRate Item", menuName = "Inventory/Items/New SplashRate Item")]

public class SplashRateItem: ItemScriptableObject
{
    public float splashRatePlusAmount;
    public float damageMinusAmount;
    public float plusMaxHealthAmount;


    private void Start()
    {
        itemType = ItemType.SplashRate;
    }
}
