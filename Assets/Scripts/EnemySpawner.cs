using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject spawnParticle;

    public enum SpawnMode
    {
        Continous,
        Waves
    }

    public SpawnMode mode;

    public GameObject[] enemyPrefabs;

    [Space]
    [Header("General Settings")]
    public float spawnDistance;
    public float spawnCooldown;

    [Space]
    [Header("Wave Settings")]
    public int waveMultiplier;
    public float waveSpread;
    public int waveIncrement;

    void Start()
    {
        StartCoroutine("Spawn", spawnCooldown);
    }

    IEnumerator Spawn(float delay)
    {
        while (true)
        {
            switch (mode)
            {
                case SpawnMode.Continous:
                    SpawnSingleEnemy();
                    break;
                case SpawnMode.Waves:
                    SpawnEnemyWave(enemyPrefabs);
                    break;
                default:
                    break;
            }
            
            yield return new WaitForSeconds(delay);
        }
    }

    void SpawnEnemyWave(GameObject[] enemies)
    {
        Vector2 uc = Random.insideUnitCircle.normalized * spawnDistance;
        Vector3 pos = new Vector3(uc.x, 0.5f, uc.y);

        for (int j = 0; j < waveMultiplier; j++)
        {
            for (int i = 0; i < enemies.Length; i++)
            {
                Vector3 uc2 = Random.onUnitSphere.normalized * waveSpread;
                Vector3 dpos = new Vector3(uc2.x, 0, uc2.z) + pos;

                Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)], dpos, Quaternion.identity);
            }
        }

        waveMultiplier += waveIncrement;
    }

    void SpawnSingleEnemy()
    {
        Vector2 uc = Random.insideUnitCircle.normalized * spawnDistance;
        Vector3 pos = new Vector3(uc.x, 0.5f, uc.y);

        Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)], pos, Quaternion.identity);
    }
}
