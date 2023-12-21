using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinAttraction : MonoBehaviour
{
    public string playerTag = "Player"; // Тэг игрока
    public float attractionDistance = 5f; // Расстояние для притягивания
    public float attractionSpeed = 2f; // Скорость притягивания

    private GameObject player;
    private bool isAttracting = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag(playerTag);

        if (player == null)
        {
            Debug.LogError("Player not found. Make sure the player has the specified tag.");
        }
    }

    void Update()
    {
        if (!isAttracting && player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

            if (distanceToPlayer < attractionDistance)
            {
                StartCoroutine(AttractToPlayer());
            }
        }
    }

    IEnumerator AttractToPlayer()
    {
        isAttracting = true;

        while (player != null && Vector3.Distance(transform.position, player.transform.position) > 0.1f)
        {
            float step = attractionSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, step);
            yield return null;
        }

        // Уничтожаем монету или выполняем другие действия
        Destroy(gameObject);

        isAttracting = false;
    }
}

