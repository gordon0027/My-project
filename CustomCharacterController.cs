// УПРАВЛЕНИЕ ПЕРСОНАЖЕМ (ДЖОЙСТИК + АНИМАЦИЯ АТАКИ ПРИ ПРИБЛИЖЕНИИ ВРАГА)

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CustomCharacterController : MonoBehaviour

{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private FixedJoystick _joystick;
    [SerializeField] private Animator _animator;

    public float _moveSpeed;
    public bool isMoving;
    //public Transform enemy;
    private float distance;
    private float lastAttackTime;
    private EnemyDamage enemyDamageScript;

    public float rotationSpeed = 5f;

    // Переменные для проверки слоя "Ground"
    public Transform groundCheckPoint; // Пустой объект, представляющий позицию для проверки слоя "Ground"
    public float groundCheckRadius = 0.1f; // Радиус для проверки слоя "Ground"
    public LayerMask groundLayer; // Слой "Ground"

    

    void Start()
    {
        // enemy = GameObject.FindGameObjectWithTag("Enemy");
        lastAttackTime = Time.time;
        // Получаем ссылку на скрипт EnemyDamage
        enemyDamageScript = GetComponent<EnemyDamage>();
        //coinPickUp = GetComponent<AudioSource>();
    }

    public void ChangeLayerWeight(float newLayerWeight)
    {
        _animator.SetLayerWeight(1, newLayerWeight);
    }


    // Движение героя

    private void FixedUpdate()
    {
        // Проверяем, находится ли объект над слоем "Ground"
        if (IsGrounded())
        {
            _rigidbody.velocity = new Vector3(_joystick.Horizontal * _moveSpeed, _rigidbody.velocity.y, _joystick.Vertical * _moveSpeed);

            if (_joystick.Horizontal != 0 || _joystick.Vertical != 0)
            {   
                // Поворот персонажа в сторону врага при приближении
                if (!enemyDamageScript.AreEnemiesInAttackRadius)
                {
                    transform.rotation = Quaternion.LookRotation(_rigidbody.velocity);
                }
                _animator.SetBool("IsMoving", true);
            }
            else
            {
                _animator.SetBool("IsMoving", false);
            }
        }
    }

    private bool IsGrounded()
    {
        // Используем SphereCast для проверки слоя "Ground" вокруг точки groundCheckPoint
        return Physics.CheckSphere(groundCheckPoint.position, groundCheckRadius, groundLayer);
    }

    void Update()
    {
        
        float attackRadius = enemyDamageScript.attackRadius;
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRadius);

        // Флаг, который указывает, был ли обнаружен враг в радиусе
        bool enemyDetected = false;

        Vector3 enemyPosition = Vector3.zero; // Добавлено объявление переменной

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Enemy"))
            {
                // Меняем вес маски
                ChangeLayerWeight(1);
                // Устанавливаем флаг, что враг обнаружен
                enemyDetected = true;
                // Получаем позицию врага
                enemyPosition = hitCollider.transform.position; // Обновлено присвоение переменной
                
            }
        
        }

        if (enemyDetected)
        {
            if (_joystick.Horizontal <= 20 && _joystick.Vertical <= 20) {
            // Плавный поворот к врагу
            Quaternion targetRotation = Quaternion.LookRotation(new Vector3(enemyPosition.x - transform.position.x, 0, enemyPosition.z - transform.position.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            }
            // Воспроизвести анимацию атаки
            _animator.Play("MeleeAttack");
        }
        else
        {
            // Если враг не обнаружен, устанавливаем вес маски в 0
            ChangeLayerWeight(0);
        }
    }   

     

}

    


