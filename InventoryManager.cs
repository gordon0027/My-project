// СКРИПТ КОНТРОЛЯ ИНВЕНТАРЯ, ОТОБРАЖЕНИЕ ХАРАКЕРИСТИК В ИНВЕНТАРЕ + ИХ ОБНОВЛЕНИЕ
// ДОБАВЛЕНИЕ ПРЕДМЕТОВ В СЛОТ AddItem()
// ПОДБОР ПРЕДМЕТОВ КОТОРЫЕ НАХОДЯТСЯ НА СЦЕНЕ

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using TMPro;

public class InventoryManager : MonoBehaviour
{

    public bool isOpened;
    public GameObject UIPanel;
    public Transform inventoryPanel;
    public List<InventorySlot> slots = new List<InventorySlot>();
    private Camera mainCamera;
    public static InventoryManager instance;
    // TEXT TEXT TEXT //
    public Text totalBonusDamageText; // Текст бонусного урона
    public Text enemyDamageText; // Текст базового урона
    public Text enemyDamageAttackSpeed; // Текст скорости атаки
    public CustomCharacterController customCharacterController; // ссылка на скрипт передвижения игрока
    public Text attackRadiusText; // Текст радиуса атаки
    public Text hpRegenText; // Текст regen hp
    public Text moveSpeedText; // Текст скорости движения
    public Text splashRateText; // Text of splash rate
    public EnemyDamage enemyDamage; // ссылка на скрипт урона игрока
    public Health health; // ссылка на скрипт здоровья игрока
    private float totalBonusDamage; // общий бонусный урона
    // TEXT TEXT TEXT //
    public ItemCard itemCard; // Ссылка на карточку предмета



    void Awake()
    {
        instance = this; // Присваиваем статическую ссылку
        UIPanel.SetActive(false);

        // Отображение статов в инвентаре при запуске игры
        enemyDamageText.text = enemyDamage.playerDamage.ToString();
        enemyDamageAttackSpeed.text = enemyDamage.attackInterval.ToString("0.00") + "sec";
        moveSpeedText.text = (customCharacterController._moveSpeed * 100).ToString("0");
        attackRadiusText.text = (enemyDamage.attackRadius * 100).ToString("0");
        splashRateText.text = (enemyDamage.splashRate * 100).ToString("0.00") + "%";
        hpRegenText.text = health.regenerationPercentage.ToString("0.0") + "%";

    
    }

    void Start()
    {
        mainCamera = Camera.main;
        for (int i = 0; i < inventoryPanel.childCount; i++)
        {
            if(inventoryPanel.GetChild(i).GetComponent<InventorySlot>() != null)
            {
                slots.Add(inventoryPanel.GetChild(i).GetComponent<InventorySlot>());
            }
        }
    }

    void Update()
    {
        // обновление статов в инвентаре
        enemyDamageAttackSpeed.text = enemyDamage.attackInterval.ToString("0.00") + "sec";
        moveSpeedText.text = (customCharacterController._moveSpeed * 100).ToString("0");
        attackRadiusText.text = (enemyDamage.attackRadius * 100).ToString("0");
        splashRateText.text = (enemyDamage.splashRate * 100).ToString("0.00") + "%";
        hpRegenText.text = health.regenerationPercentage.ToString("0.0") + "%";
    }

    /// <summary>
    /// При нажатии на слот инвентаря показать/скрыть карточку с информацией о предмете
    /// </summary>
    /// <param name="slot"></param>
    public void OnSlotClicked(InventorySlot slot)
    {
        if (slot.item != null)
        {
            // При нажатии на слот инвентаря показать/скрыть карточку с информацией о предмете
            itemCard.ToggleCard(slot.item);
        }
    }

    // Расчет и обновление бонусного урона от предметов // по моему это только текст, надо будет проверить
    private float CalculateTotalBonusDamage()
    {
    float total = 0;

    foreach (InventorySlot slot in slots)
    {
        if (slot.item != null && slot.item.itemType == ItemType.Damage)
        {
            DamageItem damageItem = slot.item as DamageItem;
            if (damageItem != null)
            {
                total += damageItem.damageAmount * slot.amount; // Умножаем на количество предметов в слоте
            }
        }
    }

    return total;
    }

    public void UpdateTotalBonusDamage()
    {
        // Обновляем текст с общим бонусным уроном
        totalBonusDamage = CalculateTotalBonusDamage();
        totalBonusDamageText.text = totalBonusDamage.ToString();

        if (enemyDamage != null)
        {
        enemyDamageText.text = enemyDamage.playerDamage.ToString();
        }
        else
        {
        Debug.LogError("EnemyDamage script reference is missing!");
        }
    }
    ///////////////////////////////////////////////////////////
    

    // Закрыть открыть кнопки инвентаря //
    public void CloseMenu()
    {
        {
            UIPanel.SetActive(false); // Скрыть меню
        }
    }

    public void OpenMenu()
    {
        {
            UIPanel.SetActive(true); // Открыть меню
        }
    }
   // Закрыть открыть кнопки инвентаря //
   


    /// <summary>
    /// Добавить вещи в инветарь
    /// </summary>
    /// <param name="_item"></param>
    /// <param name="_amount"></param>
    public void AddItem(ItemScriptableObject _item, int _amount) 
    {
        foreach(InventorySlot slot in slots)
        {
            if(slot.item == _item)
            {
                slot.amount += _amount;
                slot.itemAmountText.text = slot.amount.ToString();
                slot.ApplyAttributesToCharacter(slot.item);
                return;
            }
        }
        foreach(InventorySlot slot in slots)
        {
            if(slot.isEmpty == true)
            {
                slot.item = _item;
                slot.amount = _amount;
                slot.isEmpty = false;
                slot.SetIcon(_item.icon);
                slot.itemAmountText.text = _amount.ToString();
                slot.ApplyAttributesToCharacter(slot.item);
                break;
            }
        }

    }

    // Подбор предмета при столкновения игрока с колайдером предмета
    private void OnTriggerEnter(Collider other)
    {
    if(other.gameObject.tag == "Item")
    {
        Item itemComponent = other.gameObject.GetComponent<Item>();
        if (itemComponent != null)
        {
            AddItem(itemComponent.item, itemComponent.amount);
            Destroy(other.gameObject);
        }
    }
    }
}
