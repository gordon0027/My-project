using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemCard : MonoBehaviour
{
    public Text itemNameText;
    public Text itemDescriptionText;
    public Text[] itemStatText;
    public Image itemImage;
    private ItemScriptableObject currentItem; // Добавлено поле для текущего предмета
    private bool isCardOpen = false;

    void Start()
    {
        
    }

    void Awake()
    {
        gameObject.SetActive(false);
    }



    public void ToggleCard(ItemScriptableObject item)
    {
        
        // Переключить открытие/закрытие карточки
        isCardOpen = !isCardOpen;

        if (isCardOpen)
        {
            // Показать карточку и заполнить информацию о предмете
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
            currentItem = item; // Запоминаем текущий предмет
            itemImage.sprite = currentItem.icon;
            
        }
        else
        {
            // Скрыть карточку
            gameObject.SetActive(false);
        }
    }


}

