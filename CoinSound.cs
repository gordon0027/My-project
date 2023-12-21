// СКРИПТ МОНЕТ ИГРОКА, ДОБАВЛЕНИЕ, ЗВУК ПРИТЯГИВАНИЕ И ТД. ИЗМЕНИТЬ НАЗВАНИЕ.
//


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSound : MonoBehaviour
{
    public int coins;
    private AudioSource audioSource;

    public AudioClip coinPickUpSound;
    
    private static Transform playerTransform;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Coin")
        {
            Vector3 originalPosition = transform.position;
            transform.position = Vector3.Lerp(originalPosition, playerTransform.position, 0.1f);
            coins++;
            audioSource.clip = coinPickUpSound;
            audioSource.Play();
            Destroy(other.gameObject);
        }
    }
}
