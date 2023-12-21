// СКРИПТ ДЛЯ ПОЛОСКИ ЗДОРОВЬЯ + текст на главном UI

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{
    public Health Health; // Ссылка на скрипт здоровья
    public TextMeshProUGUI healthText; // Ссылка на объект TextMeshPro для отображения здоровья
    public Image healthBar; // полоска сверху

    


    void Update()
    {
        if (Health != null && healthText != null && healthBar != null)
        {
            // Вычисляем fill как процентное отношение текущего здоровья к максимальному
            float fill = Health.health / Health.maxHealth;

            

            // Обновляем fillAmount в Image
            healthBar.fillAmount = fill;

            // Обновляем текст на Canvas в соответствии с здоровьем и максимальным здоровьем
            healthText.text = $"{Health.health:0}/{Health.maxHealth:0}";
        }
    }
}


