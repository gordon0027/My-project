using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private NavMeshAgent myAgent;
    private Animator myAnimator;
    private float distance;
    private Transform target; // Убрал public, так как мы будем искать игрока автоматически
    private bool isAttacking = false;
    public float rotationSpeed = 5f; // Скорость поворота
    public float attackDistance = 1.9f;

    void Start()
    {
        myAgent = GetComponent<NavMeshAgent>();
        myAnimator = GetComponent<Animator>();

        // Находим игрока с тегом "Player" и устанавливаем его в качестве цели
        target = GameObject.FindGameObjectWithTag("Player").transform;
        
        // Проверка, чтобы избежать ошибки, если игрок не найден
        if (target == null)
        {
            Debug.LogError("Player not found. Make sure the player has the 'Player' tag.");
        }
    }

    void FixedUpdate()
    {
        if (target == null)
        {
            return; // Если цель не установлена, просто выходим из Update
        }

        distance = Vector3.Distance(target.position, transform.position);

        if (distance > 200)
        {
            myAgent.enabled = false;
            myAnimator.Play("Idle");
        }
        else if (distance < 200 && distance > 2f)
        {
            if (!isAttacking)
            {
                myAgent.enabled = true;
                myAgent.SetDestination(target.position);
                myAnimator.Play("Walk");
            }
        }
        else if (distance <= attackDistance)
        {
            myAgent.enabled = false;
            if (!isAttacking)
            {
                myAnimator.Play("Attack");

                Vector3 direction = (target.position - transform.position).normalized;


                // ПОВОРОТ ВРАГА В СТОРОНУ ИГРОКА, НУЖНО ДОДУМАТЬ И ОПТИМИЗИРОВАТЬ
                float angle = Vector3.Angle(transform.forward, direction);

                if (angle > 1f)
                {
                    // Игнорируем изменения по оси Y при помощи Vector3.ProjectOnPlane
                    Vector3 newDirection = Vector3.ProjectOnPlane(direction, Vector3.up);

                    // Создаем новую кватернионную ротацию, используя измененное направление
                    Quaternion newRotation = Quaternion.LookRotation(newDirection);

                    // Применяем новую ротацию к объекту
                    transform.rotation = newRotation;
                }
            }
        }
    }
}

    

