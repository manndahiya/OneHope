using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }



    #region Singleton
    public static ObjectPooler instance;


    private void Awake()
    {
        instance = this;
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }
            poolDictionary.Add(pool.tag, objectPool);
        }

    }

    #endregion

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;


    void Start()
    {


    }

    public GameObject SpawnFromPool(string tag, Vector2 position)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            return null;
        }


        GameObject objectToSpawn = poolDictionary[tag].Dequeue();
       
        if (!objectToSpawn.activeInHierarchy)
        {
            objectToSpawn.transform.position = position;
            objectToSpawn.transform.rotation = Quaternion.identity;
            objectToSpawn.SetActive(true);
        }

        poolDictionary[tag].Enqueue(objectToSpawn);

        return objectToSpawn;

    }

    public void DeactivateGameObject(string tag, GameObject collidedObj)
    {
        if (poolDictionary.ContainsKey(tag))
        {

            foreach (GameObject obj in poolDictionary[tag])
            {
                if (obj == collidedObj)
                {

                    obj.SetActive(false);

                    return;
                }
            }


        }
        else
        {
            Debug.Log("TAG NOT PRESENT");
        }


    }
}
