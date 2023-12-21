using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemCardShop : MonoBehaviour
{
    public Text itemNameText;
    public Text itemDescriptionText;
    public Text[] itemStatText;
    private CoinSound coinSoundScript; // Ссылка на скрипт с монетами
    public Text priceText; // Добавлено поле для отображения цены
    public Button buyButton; // Добавлено поле для кнопки "Купить"
    public Image itemImage;
    private ItemScriptableObject currentItem; // Добавлено поле для текущего предмета
    private InventoryManager inventoryManager;

    void Awake()
    {
        // Скрываем карточку при старте
       // gameObject.SetActive(false);

        // Назначаем метод для обработки нажатия кнопки "Купить"
        buyButton.onClick.AddListener(BuyItem);

        // Находим экземпляр InventoryManager
        inventoryManager = FindObjectOfType<InventoryManager>();
        coinSoundScript = FindObjectOfType<CoinSound>();

    }


    

    public void ToggleCard(ItemScriptableObject item)
    {
        
            // Показываем карточку и заполняем информацию о предмете
            gameObject.SetActive(true);
            for (int i = 0; i < itemStatText.Length; i++)
            {
                // Проверяем, чтобы не выйти за пределы массива
                if (i < item.itemStats.Length)
                {
                    itemStatText[i].text = item.itemStats[i];
                }
                else
                {
                    // Если статистики не хватает, оставляем текстовое поле пустым
                    itemStatText[i].text = "";
                }
            }

            
            itemNameText.text = item.itemName;
            itemDescriptionText.text = item.itemDescription;
            priceText.text = "Цена: " + item.price.ToString(); // Отображаем цену
            currentItem = item; // Запоминаем текущий предмет
            itemImage.sprite = currentItem.icon;


           
       
    }

   
    

    public void BuyItem()
    {
        if (currentItem != null)
        {
            // Проверяем, хватает ли у игрока денег
            if (coinSoundScript != null && coinSoundScript.coins >= currentItem.price)
            {
                // Вычитаем деньги
                coinSoundScript.coins -= currentItem.price;

                // Добавляем предмет в инвентарь (просто пример, замените на вашу логику)
                inventoryManager.AddItem(currentItem, 1);

                // Обновляем интерфейс, например, скрываем карточку
                gameObject.SetActive(false);

                // Здесь можно добавить дополнительные действия, связанные с покупкой
            }
            else
            {
                Debug.Log("Not enough coins to buy!");
            }
        }
    }
}
