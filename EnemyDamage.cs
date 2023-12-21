// СКРИПТ АВТОАТАКИ ИГРОКА .. Здесь хранятся обновления переменных характеристик игрока, 
// которые обновляются в скрипте Inventory Slot

using System.Collections;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    public float attackInterval = 2f; // Интервал между атаками
    public float attackRadius = 3f; // Радиус атаки
    private Animator myAnimator;
    public float playerDamage = 50f; // Урон игрока
    public float splashRate = 0f; // уменьшение или увеличиния урона по радиусу
    private float maxSplashRate = 0.5f;
    public Health playerHealth;
    public CustomCharacterController customCharacterController;
    public bool AreEnemiesInAttackRadius { get; private set; }



    // звук удара
    public AudioClip hitSound;  // Присвойте аудиофайл в параметре "Audio Clip"
    public AudioClip drunkPunchSound;
    public AudioClip vortexSound;
    public AudioClip frostRainSound;
    private AudioSource audioSource;
    private bool hasPlayedHitSound = false;
    private bool hasPlayedDPSound = false;      
    private bool hasPlayedVortexParticle = false;   
    // звук удара
    public ParticleSystem hitParticlesPrefab;
    public ParticleSystem autoAttackPrefab;
    public ParticleSystem vortexPrefab;
    private bool hasPlayedVortexSound = false;  
    public GameObject frostRainPrefab;
    private bool hasPlayedFRParticle = false;
    private bool hasPlayedFRSound = false;

    private void Start()
    {
        StartCoroutine(AttackRoutine());
        myAnimator = GetComponent<Animator>();

        // находим игрока и его компонент здоровья, чтобы после поднятия предметов хп менялось у него
        GameObject playerObject = GameObject.FindWithTag("Player");
        playerHealth = playerObject.GetComponent<Health>();

        // звуки
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.clip = hitSound;
        audioSource.volume = 0.1f; // ГРОМКОСТЬ ЗВУКА УДАРА
    }

    void Update()
    {
        CheckEnemiesInAttackRadius();

        // В данной формуле, чем меньше attackInterval, тем больше значение animationSpeed, что приведет к увеличению скорости анимации.
        float animationSpeed = 1.5f / attackInterval;
        ChangeAnimationSpeed("MeleeAttack", animationSpeed);

        if(splashRate > maxSplashRate)
        {
            splashRate = maxSplashRate;
        }
    }

    /// <summary>
    /// Проверка на наличие врагов в радиусе атаки
    /// </summary>
    void CheckEnemiesInAttackRadius()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRadius);

        AreEnemiesInAttackRadius = false;

        foreach (var collider in hitColliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                AreEnemiesInAttackRadius = true;
                break;
            }
        }
    }

    /// <summary>
    /// Функция изменения скорости анимации
    /// </summary>
    /// <param name="animationName"></param>
    /// <param name="newSpeed"></param>
    void ChangeAnimationSpeed(string animationName, float newSpeed)
    {
        // Проверяем, что аниматор не равен null
        if (myAnimator != null)
        {
            // Добавляем "_speed" к имени параметра
            string parameterName = animationName;

            // Устанавливаем новую скорость для конкретной анимации
            myAnimator.SetFloat(parameterName, newSpeed);
        }
        else
        {
            Debug.LogError("Animator component is missing!");
        }
    }
    


    // Добавление урона от предметов к базовому урону героя
    public void ApplyDamage(float amount)
    {
        playerDamage += amount;
    }

    public void ApplyMinusDamage(float amount)
    {
        playerDamage -= amount;
    }

    public void ApplySplashRate(float amount)
    {
        splashRate += amount;
    }

    // Добавление скорости атаки от предметов и изменение скорости анимации атаки
    public void ApplyAttackSpeed(float amount)
    {
        attackInterval -= amount;
    }

    public void ApplyMinusAttackSpeed(float amount)
    {
        attackInterval += amount;
    }

    // +Скорость движения от предметов (добавление)
    public void ApplyMoveSpeed(float amount)
    {
        customCharacterController._moveSpeed += amount;
    }

    // +Скорость движения от предметов (добавление)
    public void ApplyMinusMoveSpeed(float amount)
    {
        customCharacterController._moveSpeed -= amount;
    }



    // Минус Радиуса атаки
    public void ApplyAttackRadius(float amount)
    {
        attackRadius -= amount;
    }

    // Плюс Радиуса атаки
    public void ApplyAddAttackRadius(float amount)
    {
        attackRadius += amount;
    }

    public void ApplyMinusMaxHealth(float amount)
    {
        if (playerHealth != null)
        {
            playerHealth.UpdateMinusMaxHealth(amount);
            Debug.Log("HealthItem-" + amount);
        }
        else
        {
            Debug.LogError("PlayerHealth component is missing!");
        }
    }

    public void ApplyPlusMaxHealth(float amount)
    {
        if (playerHealth != null)
        {
            playerHealth.UpdatePlusMaxHealth(amount);
            Debug.Log("HealthItem+" + amount);
        }
        else
        {
            Debug.LogError("PlayerHealth component is missing!");
        }
    }

    public void UpdatePlusHpRegen(float amount)
    {
        playerHealth.UpdatePlusHpRegen(amount);
    }

    // 

    private IEnumerator AttackRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(attackInterval);

            Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRadius);

            int numberOfEnemies = 0;

            // Подсчет количества врагов в радиусе
            foreach (var collider in hitColliders)
            {
                if (collider.CompareTag("Enemy"))
                {
                    numberOfEnemies++;
                }
            }

            if (numberOfEnemies > 1)
            {
                // Распределение урона между врагами
                float damagePerEnemy = playerDamage / (numberOfEnemies - (numberOfEnemies * splashRate));

                // формула усиления урона по радиусу = playerDamage / (numberOfEnemies - (numberOfEnemies * 0.10f));
                hasPlayedHitSound = false;
                // Применение урона к каждому врагу
                foreach (var collider in hitColliders)
                {
                    if (collider.CompareTag("Enemy"))
                    {
                        Health enemyHealth = collider.GetComponent<Health>();
                        if (enemyHealth != null)
                        {
                            enemyHealth.TakeHit(damagePerEnemy); // Нанести урон
                            Instantiate(autoAttackPrefab, collider.transform.position, Quaternion.identity);
                        }
                        if (!hasPlayedHitSound)
                        {
                            audioSource.Play();
                            hasPlayedHitSound = true; // Устанавливаем флаг, что звук уже проигран
                        }
                    }
                }
                
            }
            else 
            {
                hasPlayedHitSound = false;
                foreach (var collider in hitColliders)
                {
                    if (collider.CompareTag("Enemy"))
                    {
                        Health enemyHealth = collider.GetComponent<Health>();
                        if (enemyHealth != null)
                        {
                            enemyHealth.TakeHit(playerDamage); // Нанести урон
                            Instantiate(autoAttackPrefab, collider.transform.position, Quaternion.identity);
                        }
                        if (!hasPlayedHitSound)
                        {
                        audioSource.Play();
                        hasPlayedHitSound = true; // Устанавливаем флаг, что звук уже проигран
                        }
                    }
                }
            }
        }
    }


    // DRUNK PUNCH SKILL SETTINGS
    public IEnumerator DrunkPunch(float interval, float damage, float radius)
    {
        while (true)
        {
            yield return new WaitForSeconds(interval + attackInterval);

            Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius + attackRadius);

            int numberOfEnemies = 0;

            // Подсчет количества врагов в радиусе
            foreach (var collider in hitColliders)
            {
                if (collider.CompareTag("Enemy"))
                {
                    numberOfEnemies++;
                }
            }

            if (numberOfEnemies > 1)
            {
                // Распределение урона между врагами
                float damagePerEnemy = (damage + playerDamage) / (numberOfEnemies - (numberOfEnemies * splashRate));

                // формула усиления урона по радиусу = damage / (numberOfEnemies - (numberOfEnemies * 0.10f));

                // Применение урона к каждому врагу
                hasPlayedDPSound = false;
                foreach (var collider in hitColliders)
                {
                    if (collider.CompareTag("Enemy"))
                    {
                        Health enemyHealth = collider.GetComponent<Health>();
                        if (enemyHealth != null)
                        {
                            enemyHealth.TakeHit(damagePerEnemy); // Нанести урон
                            // Создание Particle System на месте врага
                            Instantiate(hitParticlesPrefab, collider.transform.position, Quaternion.identity);
                        }
                        if (!hasPlayedDPSound)
                        {
                            audioSource.PlayOneShot(drunkPunchSound);
                            hasPlayedDPSound = true; // Устанавливаем флаг, что звук уже проигран
                        }
                        
                    }
                }
            }
            else
            {
                hasPlayedDPSound = false;
                foreach (var collider in hitColliders)
                {
                    if (collider.CompareTag("Enemy"))
                    {
                        Health enemyHealth = collider.GetComponent<Health>();
                        if (enemyHealth != null)
                        {
                            enemyHealth.TakeHit(damage + playerDamage); // Нанести урон
                            Instantiate(hitParticlesPrefab, collider.transform.position, Quaternion.identity);
                        }
                        if (!hasPlayedDPSound)
                        {
                            audioSource.PlayOneShot(drunkPunchSound);
                            hasPlayedDPSound = true; // Устанавливаем флаг, что звук уже проигран
                        }
                    }
                }
                
            }
        }
    }

    // VORTEX SKILL // 

    public IEnumerator Vortex(float interval, float damage, float radius)
    {
        while (true)
        {
            yield return new WaitForSeconds(interval + attackInterval);

            //Collider nearestEnemyCollider = FindNearestEnemy();

            //Vector3 closestPoint = nearestEnemyCollider.ClosestPoint(transform.position);

            Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius + attackRadius);

            int numberOfEnemies = 0;

            // Подсчет количества врагов в радиусе
            foreach (var collider in hitColliders)
            {
                if (collider.CompareTag("Enemy"))
                {
                    numberOfEnemies++;
                }
            }

            if (numberOfEnemies > 1)
            {
                // Распределение урона между врагами
                float damagePerEnemy = (damage + playerDamage) / (numberOfEnemies - (numberOfEnemies * splashRate));

                // формула усиления урона по радиусу = damage / (numberOfEnemies - (numberOfEnemies * 0.10f));

                // Применение урона к каждому врагу
                hasPlayedVortexSound = false;
                hasPlayedVortexParticle = false;
                foreach (var collider in hitColliders)
                {
                    if (collider.CompareTag("Enemy"))
                    {
                        Health enemyHealth = collider.GetComponent<Health>();
                        if (enemyHealth != null)
                        {
                            enemyHealth.TakeHit(damagePerEnemy); // Нанести урон
                            // Создание Particle System на месте врага
                        }
                        if (!hasPlayedVortexSound && !hasPlayedVortexParticle)
                        {
                            audioSource.PlayOneShot(vortexSound);
                            hasPlayedVortexSound = true; // Устанавливаем флаг, что звук уже проигран
                            Instantiate(vortexPrefab, collider.transform.position, Quaternion.identity);
                            hasPlayedVortexParticle = true;
                        }
                        
                    }
                }
            }
            else
            {
                hasPlayedVortexSound = false;
                hasPlayedVortexParticle = false;
                foreach (var collider in hitColliders)
                {
                    if (collider.CompareTag("Enemy"))
                    {
                        Health enemyHealth = collider.GetComponent<Health>();
                        if (enemyHealth != null)
                        {
                            enemyHealth.TakeHit(damage + playerDamage); // Нанести урон
                            Instantiate(vortexPrefab, collider.transform.position, Quaternion.identity);
                        }
                        if (!hasPlayedVortexSound && !hasPlayedVortexParticle)
                        {
                            audioSource.PlayOneShot(vortexSound);
                            hasPlayedVortexSound = true; // Устанавливаем флаг, что звук уже проигран
                            Instantiate(vortexPrefab, collider.transform.position, Quaternion.identity);
                            hasPlayedVortexParticle = true;
                        }
                    }
                }
                
            }
        }
    }

    // FROST RAIN SKILL // 
    
    public IEnumerator FrostRain(float interval, float damage, float radius)
    {
        while (true)
        {
            yield return new WaitForSeconds(interval + attackInterval);

            Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius + attackRadius);

            int numberOfEnemies = 0;
            float slowAmount = 0.3f;

            // Подсчет количества врагов в радиусе
            foreach (var collider in hitColliders)
            {
                if (collider.CompareTag("Enemy"))
                {
                    numberOfEnemies++;
                }
            }

            if (numberOfEnemies > 1)
            {
                // Распределение урона между врагами
                float damagePerEnemy = (damage + playerDamage) / (numberOfEnemies - (numberOfEnemies * splashRate));

                // Применение урона к каждому врагу
                hasPlayedFRSound = false;
                hasPlayedFRParticle = false;
                foreach (var collider in hitColliders)
                {
                    if (collider.CompareTag("Enemy"))
                    {
                        Health enemyHealth = collider.GetComponent<Health>();
                        UnityEngine.AI.NavMeshAgent enemyNavMeshAgent = collider.GetComponent<UnityEngine.AI.NavMeshAgent>();

                        if (enemyHealth != null && enemyNavMeshAgent != null)
                        {
                            enemyHealth.TakeHit(damagePerEnemy); // Нанести урон

                            // Уменьшение скорости врага
                            enemyNavMeshAgent.speed *= (1f - slowAmount);

                            if (!hasPlayedFRSound && !hasPlayedFRParticle)
                            {
                                audioSource.PlayOneShot(frostRainSound);
                                hasPlayedFRSound = true; // Устанавливаем флаг, что звук уже проигран
                                Instantiate(frostRainPrefab, transform.position, Quaternion.identity);
                                hasPlayedFRParticle = true;
                            }
                        }
                    }
                }
            }
            else
            {
                hasPlayedFRSound = false;
                hasPlayedFRParticle = false;
                foreach (var collider in hitColliders)
                {
                    if (collider.CompareTag("Enemy"))
                    {
                        Health enemyHealth = collider.GetComponent<Health>();
                        UnityEngine.AI.NavMeshAgent enemyNavMeshAgent = collider.GetComponent<UnityEngine.AI.NavMeshAgent>();

                        if (enemyHealth != null && enemyNavMeshAgent != null)
                        {
                            enemyHealth.TakeHit(damage + playerDamage); // Нанести урон

                            // Уменьшение скорости врага
                            enemyNavMeshAgent.speed *= (1f - slowAmount);

                            if (!hasPlayedFRSound && !hasPlayedFRParticle)
                            {
                                audioSource.PlayOneShot(frostRainSound);
                                hasPlayedFRSound = true; // Устанавливаем флаг, что звук уже проигран
                                Instantiate(frostRainPrefab, transform.position, Quaternion.identity);
                                hasPlayedFRParticle = true;
                            }
                        }
                    }
                }
            }
        }
    }


    /*private Collider FindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        float closestDistance = float.MaxValue;
        Collider nearestEnemyCollider = null;

        foreach (var enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);

            // Проверяем, находится ли враг внутри заданного радиуса
            if (distance < attackRadius && distance < closestDistance)
            {
                closestDistance = distance;
                nearestEnemyCollider = enemy.GetComponent<Collider>();
            }
        }

        return nearestEnemyCollider;
    }*/

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }

}
