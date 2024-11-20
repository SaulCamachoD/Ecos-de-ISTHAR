using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;
using Random = UnityEngine.Random;


public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float spawnInterval = 2f;
    [SerializeField] private float spawnDuration = 300f;
    [SerializeField] private Transform[] PointsSpawn;
    [SerializeField] private int maxEnemies = 10;

    private List<GameObject> activeEnemies = new List<GameObject>();
    private bool spawning = false;
    private bool hasSpawned = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasSpawned)
        {
            hasSpawned = true;
            StartSpawn();
        }
    }

    private void StartSpawn()
    {
        if (!spawning)
        {
            spawning = true;
            StartCoroutine(SpawnEnemies());
        }
    }

    private IEnumerator SpawnEnemies()
    {
        float elapsedTime = 0f;

        while (elapsedTime < spawnDuration)
        {
            if (activeEnemies.Count < maxEnemies)
            {
                SpawnEnemy();
            }

            yield return new WaitForSeconds(spawnInterval);
            elapsedTime += spawnInterval;
        }

        spawning = false;
        Debug.Log("Oleada finalizada");
    }

    private void SpawnEnemy()
    {
        if (activeEnemies.Count >= maxEnemies || !spawning)
        {
            return;
        }

        int spawnIndex = Random.Range(0, PointsSpawn.Length);
        Transform spawnPoint = PointsSpawn[spawnIndex];

        GameObject enemy = ProjectilePool.Instance.GetProjectile(enemyPrefab);
        if (enemy != null)
        {
            enemy.transform.position = spawnPoint.position;

            var navMesh = enemy.GetComponent<NavMeshAgent>();
            if (navMesh != null)
            {
                navMesh.enabled = false;
            }

            enemy.SetActive(true);
            activeEnemies.Add(enemy);

            var enemyBase = enemy.GetComponent<EnemyBase>();
            if (enemyBase != null)
            {
                enemyBase.OnEnemyDeath += HandleEnemyDeath;
            }

            StartCoroutine(ActiveNavmeshAgent(enemy));
        }
    }

    private IEnumerator ActiveNavmeshAgent(GameObject enemy)
    {
        yield return null;

        var navMeshAgent = enemy.GetComponent<NavMeshAgent>();
        if (navMeshAgent != null)
        {
            NavMeshHit hit;
            if (NavMesh.SamplePosition(enemy.transform.position, out hit, 2f, NavMesh.AllAreas))
            {
                navMeshAgent.enabled = true;
                navMeshAgent.Warp(hit.position);
            }
            else
            {
                Debug.LogWarning($"El enemigo fue generado fuera del NavMesh: {enemy.transform.position}");
            }
        }
    }

    private void HandleEnemyDeath(GameObject enemy)
    {
        if (activeEnemies.Contains(enemy))
        {
            activeEnemies.Remove(enemy);
        }
    }
}

