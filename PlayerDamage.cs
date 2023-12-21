// СКРИПТ УРОНА ПО ИГРОКУ ОТ МОБОВ

using System.Collections;
using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    public float attackInterval = 2f; // Интервал между атаками
    public float attackRadius = 3f; // Радиус атаки
    public float enemyDamage = 20f;

    private void Start()
    {
        StartCoroutine(AttackRoutine());
    }

    private IEnumerator AttackRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(attackInterval);

            Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRadius);


            foreach (var collider in hitColliders)
            {
                if (collider.CompareTag("Player"))
                {
                    Health playerHealth = collider.GetComponent<Health>();
                    if (playerHealth != null)
                    {
                        playerHealth.TakeHit(enemyDamage); // Нанести урон
                    }
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}