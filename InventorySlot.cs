// СКРИПТ СЛОТОВ ИНВЕНТАРЯ, ОБНОВЛЕНИЕ ХАРАКТЕРИСТИК ПРИ ПОПАДАНИИ ПРЕДМЕТА В СЛОТ

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlot : MonoBehaviour
{
    public ItemScriptableObject item;
    public int amount;
    public bool isEmpty = true;
    public GameObject skillIcon;
    public TMP_Text itemAmountText;
    public EnemyDamage enemyDamage;

    

    private void Awake()
    {
        skillIcon = transform.GetChild(0).gameObject;
        itemAmountText = transform.GetChild(1).GetComponent<TMP_Text>();
    }

    public void SetIcon(Sprite icon)
    {
        skillIcon.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        skillIcon.GetComponent<Image>().sprite = icon;
    }

    public void ApplyAttributesToCharacter(ItemScriptableObject itemScriptableObject)
    {
        // Обновление для предмета типа Damage
        if (itemScriptableObject.itemType == ItemType.Damage)
        {
            DamageItem damageItem = itemScriptableObject as DamageItem;
            if (damageItem != null)
            {
                // Используйте ссылку на enemyDamage
                enemyDamage.ApplyDamage(damageItem.damageAmount);

                // Обновляем общий бонусный урон в InventoryManager
                InventoryManager.instance.UpdateTotalBonusDamage();
            }
        }

        // Обновление для предмета типа AttackSpeed
        if (itemScriptableObject.itemType == ItemType.AttackSpeed)
        {
            AttackSpeedItem attackSpeedItem = itemScriptableObject as AttackSpeedItem;
            if (attackSpeedItem != null)
            {
                
                enemyDamage.ApplyAttackSpeed(attackSpeedItem.attackSpeedAmount);
                enemyDamage.ApplyAttackRadius(attackSpeedItem.radiusMinusAmount);
                enemyDamage.ApplyMinusAttackSpeed(attackSpeedItem.attackSpeedMinusAmount);
                enemyDamage.ApplyAddAttackRadius(attackSpeedItem.radiusPlusAmount);
            }
        }

        // Обновление для предмета типа MoveSpeed
        if (itemScriptableObject.itemType == ItemType.MoveSpeed)
        {
            MoveSpeedItem moveSpeedItem = itemScriptableObject as MoveSpeedItem;
            if (moveSpeedItem != null)
            {
                enemyDamage.ApplyMoveSpeed(moveSpeedItem.moveSpeedAmount);
                enemyDamage.ApplyAttackRadius(moveSpeedItem.radiusMinusAmount);
                
            }
        }


        // Обновление для предмета типа AttackRadius

        if (itemScriptableObject.itemType == ItemType.AttackRadius)
        {
            AttackRadiusItem attackRadiusItem = itemScriptableObject as AttackRadiusItem;
            if (attackRadiusItem != null)
            {
                enemyDamage.ApplyMinusMoveSpeed(attackRadiusItem.moveSpeedMinusAmount);
                enemyDamage.ApplyAddAttackRadius(attackRadiusItem.radiusPlusAmount);
                enemyDamage.ApplyMinusMaxHealth(attackRadiusItem.minusMaxHealthAmount);
                
            }
        }

        // Обновление для предмета типа SplashRate
        
        if (itemScriptableObject.itemType == ItemType.SplashRate)
        {
            SplashRateItem splashRateItem = itemScriptableObject as SplashRateItem;
            if (splashRateItem != null)
            {
                enemyDamage.ApplySplashRate(splashRateItem.splashRatePlusAmount);
                enemyDamage.ApplyMinusDamage(splashRateItem.damageMinusAmount);
                enemyDamage.ApplyPlusMaxHealth(splashRateItem.plusMaxHealthAmount);
                
            }
        }

        
        // Обновление для предмета типа SplashRate
        {
            HealthItem healthItem = itemScriptableObject as HealthItem;
            if (healthItem != null)
            {
                enemyDamage.ApplyMinusMoveSpeed(healthItem.minusMoveSpeed);
                enemyDamage.UpdatePlusHpRegen(healthItem.plusHpRegeneration);
                enemyDamage.ApplyPlusMaxHealth(healthItem.plusMaxHP);
                
            }
        }

    }


}
