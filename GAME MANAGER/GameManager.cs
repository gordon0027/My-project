using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int totalRounds = 20;
    public float roundDurationSeconds = 60f;
    public GameObject[] waveSpawnerPrefabs;
    public GameObject shopMenu;
    public Text roundText;
    public Text timerText; // Добавлено новое текстовое поле для отображения таймера

    private int currentRound = 0;
    private float timer = 0f; // Добавлен новый счетчик времени текущего раунда
    private bool isPaused = false;

    void Start()
    {
        StartCoroutine(StartRounds());
    }

    void Update()
    {
        // Обновление таймера текущего раунда
        if (!isPaused)
        {
            timer += Time.deltaTime;
            UpdateTimerText();
        }
    }

    void UpdateTimerText()
    {
        float remainingTime = Mathf.Max(roundDurationSeconds - timer, 0f);
        int seconds = Mathf.FloorToInt(remainingTime % 60f);
        timerText.text = string.Format("{0:00}", seconds);
    }

    IEnumerator StartRounds()
    {
        while (currentRound < totalRounds)
        {
            yield return StartCoroutine(StartRound());
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            /*/ Получить компонент ShopManager, предполагая, что он находится на том же объекте, что и этот скрипт
            *  Для обновления карточек при открытии магазина после раунда
            /*/

            if (player != null)
            {
                // Получить компонент ShopManager с объекта Player
                ShopManager shopManager = player.GetComponent<ShopManager>();

                if (shopManager != null)
                {
                    // Вызвать функцию RerollItems
                    shopManager.GenerateRandomCards();
                }
            }
            yield return StartCoroutine(OpenShop());
        }

        Debug.Log("Game Over");
    }

    IEnumerator StartRound()
    {
        currentRound++;
        roundText.text = "Round " + currentRound;

        timer = 0f; // Сброс таймера при старте нового раунда

        GameObject player = GameObject.FindGameObjectWithTag("Player");

        

        if (player != null)
        {
            // Получить компонент Health и установить текущее здоровье равным максимальному
            Health playerHealth = player.GetComponent<Health>();
            
            if (playerHealth != null)
            {
                playerHealth.health = playerHealth.maxHealth;
            }
        }

        if (waveSpawnerPrefabs.Length > 0)
        {
            foreach (GameObject spawnerPrefab in waveSpawnerPrefabs)
            {
                Instantiate(spawnerPrefab);
            }
        }

        yield return new WaitForSeconds(roundDurationSeconds);

        DestroyAllGameObjects();
    }

    IEnumerator OpenShop()
    {
        shopMenu.SetActive(true);
        Time.timeScale = 0;

        while (shopMenu.activeSelf)
        {
            yield return null;
        }

        Time.timeScale = 1;
        isPaused = false; // Снятие паузы после закрытия магазина
    }

    void DestroyAllGameObjects()
    {
        // Находим все объекты с тегом "Enemy"
        GameObject[] enemyObjects = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemyObjects)
        {
            Destroy(enemy);
        }

        // Находим все объекты с тегом "Coin"
        GameObject[] coinObjects = GameObject.FindGameObjectsWithTag("Coin");
        foreach (GameObject coin in coinObjects)
        {
            Destroy(coin);
        }
    }
}


