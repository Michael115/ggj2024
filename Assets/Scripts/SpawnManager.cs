using System;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private int intervalSeconds;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform[] spawnPoints;

    private long _nextSpawnTicks;
    private long _intervalTicks;

    private void Awake()
    {
        _intervalTicks = TimeSpan.FromSeconds(intervalSeconds).Ticks;
    }

    private void Update()
    {
        if (DateTime.UtcNow.Ticks >= _nextSpawnTicks)
        {
            Spawn();
        }
    }

    private void Spawn()
    {
        foreach (var spawnPoint in spawnPoints)
        {
            Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
        }

        _nextSpawnTicks = DateTime.UtcNow.Ticks + _intervalTicks;
    }
}