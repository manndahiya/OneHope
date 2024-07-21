using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    ObjectPooler op;
    Vector3 spawnPos = Vector2.zero;
    bool canSpawn = false;

    [SerializeField] LayerMask layersEnemyCannotSpawnOn;

    void Start()
    {
        op = ObjectPooler.instance;
        StartCoroutine(SpawnEnemies());
    }

    
    void Update()
    {
        
    }

   IEnumerator SpawnEnemies()
    {
        while (true)
        {
            GetRandomSpawnPosition();
            canSpawn = CheckForSpawnPos(spawnPos);

            if (canSpawn)
            {

                op.SpawnFromPool("Enemy", spawnPos);
                op.SpawnFromPool("Enemy2", spawnPos);
                op.SpawnFromPool("Enemy3", spawnPos);
                op.SpawnFromPool("Enemy4", spawnPos);


            }


            //float time = Random.Range(1f);

            yield return new WaitForSeconds(1f);
        }
        
    }

    Vector3 GetRandomSpawnPosition()
    {
        spawnPos = new Vector3(Random.Range(-10, 10), 0f, Random.Range(0, 10));
        return spawnPos;
    }

    bool CheckForSpawnPos(Vector3 spawnPosition)
    {
        bool isSpawnPosValid = false;
        int attemptCount = 0;
        int maxAttempts = 10; 
        Collider[] colliders;

        colliders = Physics.OverlapSphere(spawnPosition, 1f);
        while (!isSpawnPosValid && attemptCount < maxAttempts)
        {

            bool isInvalidCollision = false;

            colliders = Physics.OverlapSphere(spawnPos, 1f);
            foreach (Collider col in colliders)
            {
                if (((1 << col.gameObject.layer) & layersEnemyCannotSpawnOn) != 0)
                {
                    // Invalid collision found
                    isInvalidCollision = true;

                    break;
                }
            }

            if (!isInvalidCollision)
            {
                isSpawnPosValid = true;
            }
            attemptCount++;
        }



        // If no invalid collisions found, spawn position is valid
        if (!isSpawnPosValid)
        {

            return false;
        }


        return true;

    }

}

