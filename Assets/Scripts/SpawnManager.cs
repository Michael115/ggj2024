using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private int intervalSeconds;
    [SerializeField] private GameObject enemyPrefab;

    private long _nextSpawnTicks;
    private long _intervalTicks;
    private Transform[] _spawnPoints;

    private void Awake()
    {
        _intervalTicks = TimeSpan.FromSeconds(intervalSeconds).Ticks;
        _nextSpawnTicks = DateTime.UtcNow.Ticks + _intervalTicks;
    }

    private void Start()
    {
        _spawnPoints = GameObject.FindGameObjectsWithTag("Spawn").Select(x => x.transform).ToArray();
        Assert.IsTrue(_spawnPoints.Length > 0);
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
        foreach (var spawnPoint in _spawnPoints)
        {
            Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
        }

        _nextSpawnTicks += _intervalTicks;
    }
}