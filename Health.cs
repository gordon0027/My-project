// СКРИПТ ЗДОРОВЬЯ (ПОЛУЧЕНИЕ УРОНА, ЭФФЕКТЫ ОТ УРОНА, ЗВУКИ, КРОВЬ НА ЗЕМЛЕ)

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Health: MonoBehaviour
{
   // HP и регенерация
   public float health;
   public float maxHealth;
   private const float RegenerationRate = 1f;
   public float regenerationPercentage = 1f;
   //public float regenerationAmount = 1f; // Количество здоровья, которое будет регенерироваться за единицу времени
   private float lastRegenerationTime;

    private AudioSource audioSource;
    //private float lastDamageSoundTime;
    
    // звуки //
    // кровь //
    
    // public GameObject bloodPrefab; // Префаб крови // НЕАКТУАЛЬНО, ПОМЕНЯЛ СИСТЕМУ
    
    public GameObject[] bloodGroundPrefabs; // Массив префабов // Префаб крови на земле
    public Transform bloodGroundSpawnPoint; // Точка, где будет создаваться префаб крови на земле
    
    public Transform damageTextSpawnPoint; 

    public GameObject coinPrefab; 
    public Transform coinPrefabSpawnPoint; 
    public Transform bloodSpawnPoint; // Точка, где будет создаваться префаб крови
    public ParticleSystem deathParticles; // эффект после смерти
    
    // кровь //


    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            // Если AudioSource отсутствует, добавьте его динамически
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        lastRegenerationTime = Time.time;
        //lastDamageSoundTime = Time.time;
    }

    void Update()
    {
        if(health > maxHealth)
        {
            health = maxHealth;
        }

        if (regenerationPercentage > 0)
        {
            RegenerateHealth();
        }
        
    }

    public void TakeHit(float damage)
    {
        
        health -= damage;
        // Определяем, кто нанес урон, проверив тег объекта, который вызвал метод TakeHit
        bool isPlayerDamage = gameObject.CompareTag("Player");

        DamageUI.Instance.AddText((int)damage, damageTextSpawnPoint.position, isPlayerDamage);

        if (health <= 0)
        {
            if (gameObject.CompareTag("Player"))
            {
                // Отключаем объект игрока, если это урон игроку
                gameObject.SetActive(false);
            }
            else if (gameObject.CompareTag("Enemy") && !gameObject.CompareTag("Player"))
            {
                Die();
            }
        }
    }

    private void Die()
    {
        // Обработка смерти, например, воспроизведение анимации, вызов события Game Over, и т.д.
        SpawnCoin();
        ShowGroundBloodEffect();
        ShowBloodEffect();
        Destroy(gameObject);
    }

    

    // кровь от урона //
    void ShowBloodEffect()
    {
        Instantiate(deathParticles, bloodSpawnPoint.position, bloodSpawnPoint.rotation);

    }

    // кровь от урона //

    // создание монеты

    void SpawnCoin()
    {
      
      GameObject coinInstance = Instantiate(coinPrefab, coinPrefabSpawnPoint.position, coinPrefabSpawnPoint.rotation);
      coinInstance.transform.rotation = Quaternion.identity;

    }
    

    //
    // кровь на земле //
    void ShowGroundBloodEffect()
    {
            // Создаем экземпляр префаба крови на указанной точке
           GameObject randomBloodPrefab = bloodGroundPrefabs[Random.Range(0, bloodGroundPrefabs.Length)];

            // Создаем экземпляр случайного префаба крови на указанной точке
            GameObject bloodGroundInstance = Instantiate(randomBloodPrefab, bloodGroundSpawnPoint.position, bloodGroundSpawnPoint.rotation);
            
            // Запускаем корутину для удаления крови через 10 секунды
            //StartCoroutine(RemoveGroundBloodAfterDelay(bloodGroundInstance, 1.0f));
    }

    // Минус и плюс макс хп от предметов
    public void UpdateMinusMaxHealth(float amount)
    {
        maxHealth = maxHealth - (maxHealth * amount);
        Debug.Log("Health-");
    }

    public void UpdatePlusMaxHealth(float amount)
    {
        maxHealth = maxHealth + (maxHealth * amount);
        Debug.Log("Health+");
    }

    public void UpdatePlusHpRegen(float amount)
    {
        regenerationPercentage += amount;
    }


    // Регенерация здоровья в секунду

    void RegenerateHealth()
    {
        if(health < maxHealth)
        {
            if (health < maxHealth && Time.time - lastRegenerationTime > 1f / RegenerationRate)
            {
            // Рассчитываем количество времени, прошедшее с последней регенерации
            float timeSinceLastRegeneration = Time.time - lastRegenerationTime;

            // Рассчитываем количество здоровья, которое нужно восстановить
            float regenerationAmount = maxHealth * 0.01f * regenerationPercentage * timeSinceLastRegeneration;

            // Увеличиваем здоровье на соответствующее количество
            health += regenerationAmount;

            // Ограничиваем здоровье максимальным значением
            health = Mathf.Min(health, maxHealth);

            // Обновляем время последней регенерации
            lastRegenerationTime = Time.time;

            // Выводим отладочное сообщение
            Debug.Log($"Regenerating: {regenerationAmount} health");
            }
        }
    }

}
