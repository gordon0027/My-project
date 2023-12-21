using System.Collections;
using UnityEngine;
using TMPro;

public class WaveSpawner : MonoBehaviour
{
    int waveCount = 1;

    public float spawnRate = 1.0f;
    public float timeBetweenWaves = 3.0f;
    public int enemyCount;

    public Transform[] spawnPoints;
    bool waveIsDone = true;
    //public Health health;
    public ObjectPooler objectPooler; // Ссылка на объект пула

    void Start()
    {
        // Поиск объекта пула в сцене
        objectPooler = FindObjectOfType<ObjectPooler>();
    }

    void Update()
    {
            if (waveIsDone == true)
            {
                StartCoroutine(waveSpawner());
            }
    }

    IEnumerator waveSpawner()
    {
        waveIsDone = false;

        for (int i = 0; i < enemyCount; i++)
        {
            Transform randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

            // Используйте метод GetPooledObject для получения объекта из пула
            GameObject enemyClone = objectPooler.GetPooledObject();
            enemyClone.transform.position = randomSpawnPoint.position;
            enemyClone.SetActive(true);

            yield return new WaitForSeconds(spawnRate);
        }

        spawnRate -= 0.01f;
        //enemyCount += 1;
        waveCount += 1;

        yield return new WaitForSeconds(timeBetweenWaves);

        waveIsDone = true;
    }
}


