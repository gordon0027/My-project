// СКРИПТ ДЛЯ ПОЛОСКИ ЗДОРОВЬЯ НАД ПЕРСОНАЖЕМ (В ИГРЕ)

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InGameHealthBar : MonoBehaviour
{
    public Health Health; // Ссылка на скрипт здоровья
    public Image healthBar; // полоска сверху

    private Camera _camera;

    void Awake()
    {
        _camera = Camera.main;
    }


    void Update()
    {

        
        // Вычисляем fill как процентное отношение текущего здоровья к максимальному
        float fill = Health.health / Health.maxHealth;
        // Обновляем fillAmount в Image
        healthBar.fillAmount = fill;

        
    }

    // Отображение inGameHp бара помещено сюда дабы исключить баги интерфейса
    void FixedUpdate()
    {
        // чтобы хп бар не крутился
        transform.LookAt(new Vector3(transform.position.x, _camera.transform.position.y, _camera.transform.position.z));
        transform.Rotate(0, 180, 0);
    }
}
