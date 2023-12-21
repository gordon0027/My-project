using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    public List<GameObject> enemyPrefabs; // Список префабов
    public int poolSize = 10;

    private List<GameObject> objectPool;

    void Start()
    {
        objectPool = new List<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            // Выбираем случайный префаб из списка
            GameObject obj = Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Count)]);
            obj.SetActive(false);
            objectPool.Add(obj);
        }
    }

    public GameObject GetPooledObject()
    {
        for (int i = 0; i < objectPool.Count; i++)
        {
            if (!objectPool[i].activeInHierarchy)
            {
                return objectPool[i];
            }
        }

        // Если не нашли неактивный объект в пуле, создаем новый снова, используя случайный префаб
        GameObject obj = Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Count)]);
        obj.SetActive(false);
        objectPool.Add(obj);
        return obj;
    }
}

