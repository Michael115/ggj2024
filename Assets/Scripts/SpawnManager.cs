using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private int spawnIntervalInSeconds;
    [SerializeField] private int waveIntervalInSeconds;
    [SerializeField] private GameObject enemyPrefab;

    private int _currentWave;

    private long _nextSpawnTicks;
    private long _intervalTicks;
    private Transform[] _spawnPoints;

    private int _remainingSpawns;

    private void Awake()
    {
        _intervalTicks = TimeSpan.FromSeconds(spawnIntervalInSeconds).Ticks;
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
        if (_remainingSpawns == 0)
        {
            _currentWave += 1;
            _remainingSpawns = GetSpawnCount(_currentWave);
            print($"new wave - {_remainingSpawns}");
        }
        else
        {
            foreach (var spawnPoint in _spawnPoints)
            {
                if (_remainingSpawns == 0) break;
                Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
                _remainingSpawns -= 1;
            }
        }

        _nextSpawnTicks += _intervalTicks;
    }

    private static int GetSpawnCount(int wave) => (int)Math.Pow(10, Math.Cbrt(wave));
}