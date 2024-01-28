using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using Random = UnityEngine.Random;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private int initialWaveDelayInSeconds;
    [SerializeField] private int spawnIntervalInSeconds;
    [SerializeField] private int waveIntervalInSeconds;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private TextMeshProUGUI counter;

    private int _currentWave;
    private bool _shouldSpawn;
    private int _remainingSpawns;
    private int _remainingEnemies;
    private DateTime _nextSpawnTime;
    private Transform[] _spawnPoints;

    private void Awake()
    {
        Bullet.OnEnemyDeath += OnEnemyDeath;
    }

    private void Start()
    {
        _spawnPoints = GameObject.FindGameObjectsWithTag("Spawn").Select(x => x.transform).ToArray();
        Assert.IsTrue(_spawnPoints.Length > 0);

        NewWave(initialWaveDelayInSeconds);
    }

    private void Update()
    {
        if (_shouldSpawn && DateTime.UtcNow >= _nextSpawnTime)
        {
            Spawn();
        }
    }

    private void OnEnemyDeath()
    {
        _remainingEnemies -= 1;
        counter.text = $"{_remainingEnemies:N0}";
        if (_remainingEnemies == 0)
        {
            NewWave(waveIntervalInSeconds);
        }
    }

    private void NewWave(int delayInSeconds)
    {
        _currentWave += 1;
        _shouldSpawn = true;
        _remainingSpawns = GetSpawnCount(_currentWave);
        _nextSpawnTime = DateTime.UtcNow.AddSeconds(delayInSeconds);
        print($"wave {_currentWave} starting in {delayInSeconds} seconds ({_remainingSpawns} enemies)");
    }

    private void Spawn()
    {
        Assert.IsTrue(_remainingSpawns > 0);

        var spawnPoint = _spawnPoints[Random.Range(0, _spawnPoints.Length)];
        Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
        _remainingSpawns -= 1;
        _remainingEnemies += 1;
        counter.text = $"{_remainingEnemies:N0}";
        if (_remainingSpawns == 0)
        {
            _shouldSpawn = false;
            return;
        }

        _nextSpawnTime = DateTime.UtcNow.AddSeconds(spawnIntervalInSeconds);
    }

    private static int GetSpawnCount(int wave) => (int)Math.Pow(10, Math.Cbrt(wave));
}