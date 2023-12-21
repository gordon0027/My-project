using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackScript : MonoBehaviour
{
    private GameObject attackObject;
    public GameObject playerAttackPrefab; // ссылка на префаб атаки ближнего боя
    public Transform spawnPoint; // точка, откуда будет выстрел
    public float attackSpeed = 5f; // скорость атаки
    public float attackForce = 10f; // сила атаки

    

    public void PlayerAttack()
    {
        // Создаем объект из префаба в указанной точке
        attackObject = Instantiate(playerAttackPrefab, spawnPoint.position, spawnPoint.rotation);


        if (attackObject != null)
        {
            Debug.Log("Префаб удара игрока успешно создан.");
        }
        else
        {
            Debug.LogError("Префаб удара игрока не был создан. Проверьте настройки.");
            return;
        }

        // Получаем компонент Rigidbody объекта, чтобы добавить силу
        Rigidbody attackRigidbody = attackObject.GetComponent<Rigidbody>();

        if (attackRigidbody != null)
        {
            // Определяем направление к врагу
            Vector3 enemyDirection = (FindEnemy().position - spawnPoint.position).normalized;

            // Используем MoveTowards для перемещения объекта в сторону игрока
            attackRigidbody.velocity = enemyDirection * attackSpeed;

            // Устанавливаем силу атаки
            attackRigidbody.AddForce(enemyDirection * attackForce, ForceMode.Impulse);

            // Запускаем корутину для удаления префаба через 1 секунду
            StartCoroutine(DestroyAfterDelay(attackObject, 2f));
        }
        else
        {
            Debug.LogError("Префаб должен содержать компонент Rigidbody для работы с физикой.");
        }
    }

    Transform FindEnemy()
    {
        // Находим врага по тегу
        GameObject enemy = GameObject.FindGameObjectWithTag("Enemy");

        if (enemy != null)
        {
            return enemy.transform;
        }
        else
        {
            Debug.LogError("Объект с тегом 'Enemy' не найден.");
            return null;
        }
    }

    // Корутина для удаления объекта после заданной задержки
    IEnumerator DestroyAfterDelay(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(obj);
    }

    // Удаление объекта при столкновении с врагом
    void OnTriggerEnter(Collider other)
    {
    if (other.CompareTag("Enemy"))
    {
        {
            // Если столкнулись с врагом, уничтожаем объект сразу
            Destroy(attackObject);
        }
    }
}
}