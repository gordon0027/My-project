using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public ItemCardShop itemCardShopPrefab;
    public Transform cardContainer;
    public Button rerollButton;
    public GameObject ShopPanel;
    public GameObject OtherPanel;

    private List<ItemCardShop> currentCards = new List<ItemCardShop>();

    private CoinSound coinSoundScript; // Ссылка на скрипт с монетами

    public ItemScriptableObject[] availableItems; // Добавлено
    public int rerollCost = 25; // Добавлено
    

    private void Start()
    {


        coinSoundScript = FindObjectOfType<CoinSound>(); // Ищем скрипт с монетами

        GenerateRandomCards();

        // Назначаем метод для обработки нажатия кнопки ре-ролла
        rerollButton.onClick.AddListener(RerollItems);
    }

    void Awake()
    {
        ShopPanel.SetActive(false);
    }

    /// <summary>
    /// Вывод на экран рандомных карточек предметов
    /// </summary>
    public void GenerateRandomCards()
    {
        // Очищаем текущие карточки
        foreach (ItemCardShop card in currentCards)
        {
            Destroy(card.gameObject);
        }
        currentCards.Clear();

        // Выбираем случайные 3 предмета из доступных
        List<ItemScriptableObject> selectedItems = new List<ItemScriptableObject>();
        List<int> usedIndices = new List<int>();
        for (int i = 0; i < 4; i++)
        {
            int randomIndex;
            do
            {
                randomIndex = Random.Range(0, availableItems.Length);
            } while (usedIndices.Contains(randomIndex));

            usedIndices.Add(randomIndex);
            selectedItems.Add(availableItems[randomIndex]);
        }

        foreach (ItemScriptableObject item in selectedItems)
        {
            ItemCardShop card = Instantiate(itemCardShopPrefab, cardContainer);
            card.ToggleCard(item);
            currentCards.Add(card);
        }
    }
    
    /// <summary>
    /// Реролл предметов
    /// </summary>
    void RerollItems()
    {
        // Проверяем, хватает ли у игрока денег для ре-ролла
        if (coinSoundScript != null && coinSoundScript.coins >= rerollCost)
        {
            // Вычитаем деньги
            coinSoundScript.coins -= rerollCost;

            // Генерируем новые карточки
            GenerateRandomCards();
        }
        else
        {
            Debug.Log("Not enough coins to reroll!");
        }
    }


    // Закрыть открыть кнопки инвентаря //
    void CloseShop()
    {
        {
            ShopPanel.SetActive(false); // Скрыть меню
            OtherPanel.SetActive(true);
        }
    }

    void OpenShop()
    {
        {
            ShopPanel.SetActive(true); // Открыть меню
            OtherPanel.SetActive(false);
        }
    }
    // Закрыть открыть кнопки инвентаря //
}
   




