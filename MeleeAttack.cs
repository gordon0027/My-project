using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    public GameObject meleeAttackPrefab; // ссылка на префаб атаки ближнего боя
    public Transform spawnPoint; // точка, откуда будет выстрел
    public float attackSpeed = 5f; // скорость атаки
    public float attackForce = 10f; // сила атаки

    

    public void Attack()
    {
        // Создаем объект из префаба в указанной точке
        GameObject attackObject = Instantiate(meleeAttackPrefab, spawnPoint.position, spawnPoint.rotation);

        if (attackObject != null)
        {
            Debug.Log("Префаб успешно создан.");
        }
        else
        {
            Debug.LogError("Префаб не был создан. Проверьте настройки.");
            return;
        }

        // Получаем компонент Rigidbody объекта, чтобы добавить силу
        Rigidbody attackRigidbody = attackObject.GetComponent<Rigidbody>();

        if (attackRigidbody != null)
        {
            // Определяем направление к игроку
            Vector3 playerDirection = (FindPlayer().position - spawnPoint.position).normalized;

            // Используем MoveTowards для перемещения объекта в сторону игрока
            attackRigidbody.velocity = playerDirection * attackSpeed;

            // Устанавливаем силу атаки
            attackRigidbody.AddForce(playerDirection * attackForce, ForceMode.Impulse);

            // Запускаем корутину для удаления префаба через 1 секунду
            StartCoroutine(DestroyAfterDelay(attackObject, 0.5f));
        }
        else
        {
            Debug.LogError("Префаб должен содержать компонент Rigidbody для работы с физикой.");
        }
    }

    Transform FindPlayer()
    {
        // Находим игрока по тегу
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            return player.transform;
        }
        else
        {
            Debug.LogError("Объект с тегом 'Player' не найден.");
            return null;
        }
    }

    // Корутина для удаления объекта после заданной задержки
    IEnumerator DestroyAfterDelay(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(obj);
    }

    // Удаление объекта при столкновении с игроком
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Если столкнулись с игроком, уничтожаем объект сразу
            Destroy(gameObject);
        }
    }
}
