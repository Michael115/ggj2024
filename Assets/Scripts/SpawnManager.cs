using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using Random = UnityEngine.Random;

// TODO: Health/tickle bar/laugh resistance
// TODO: Death

// TODO: Restart game button/menu
// TODO: Pause game (time state to zero)

// TODO: Points for killing enemies
// TODO: Display points

// TODO: Wave indicator
// TODO: Wave notification (new wave starts, wave cooldown timer)

// TODO: Use points for loot crate
// TODO: Press "A" notification popup

// TODO: Run functionality
// TODO: Run stamina?

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private float initialWaveDelayInSeconds;
    [SerializeField] private float spawnIntervalInSeconds;
    [SerializeField] private float waveIntervalInSeconds;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private TextMeshProUGUI counter;

    [SerializeField] private float checkSpawnInterval;
    private int _currentWave;
    private bool _shouldSpawn;
    private int _remainingSpawns;
    private int _remainingEnemies;
    private float _nextSpawnTime;
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
        if (_shouldSpawn && Time.time >= _nextSpawnTime)
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

    private void NewWave(float delayInSeconds)
    {
        _currentWave += 1;
        _shouldSpawn = true;
        _remainingSpawns = GetSpawnCount(_currentWave);
        _nextSpawnTime = Time.time + delayInSeconds;
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

        checkSpawnInterval = Math.Max(spawnIntervalInSeconds - (_currentWave * 0.3f), 0.1f);
        _nextSpawnTime = Time.time + checkSpawnInterval;
    }

    private static int GetSpawnCount(int wave) => (int)Math.Pow(10, Math.Cbrt(wave));
}