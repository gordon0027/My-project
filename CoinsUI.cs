// отображение колва монет на UI

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinsUI : MonoBehaviour
{
    public CoinSound CoinSound; // Ссылка на скрипт коинов
    public TextMeshProUGUI CoinUI; // Ссылка на объект TextMeshPro для отображения rjbyjd

    void Update()
    {
        {
            // Обновляем текст на Canvas в соответствии с здоровьем и максимальным здоровьем
            CoinUI.text = $"{CoinSound.coins}";
        }
    }
}
