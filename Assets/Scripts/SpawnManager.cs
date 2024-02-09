using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using Random = UnityEngine.Random;

// TODO: Wave notification (new wave starts, wave cooldown timer)

// TODO: Run functionality
// TODO: Run stamina?

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private float initialWaveDelayInSeconds;
    [SerializeField] private float spawnIntervalInSeconds;
    [SerializeField] private float waveIntervalInSeconds;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject waveUI;

    [SerializeField] private float checkSpawnInterval;
    private int _currentWave;
    private int _remainingSpawns;
    private float _nextSpawnTime;
    private Transform[] _spawnPoints;
    public Transform playerTransform;

    private void Start()
    {
        _spawnPoints = GameObject.FindGameObjectsWithTag("Spawn").Select(x => x.transform).ToArray();
        Assert.IsTrue(_spawnPoints.Length > 0);

        NewWave(initialWaveDelayInSeconds);
    }

    private void Update()
    {
        var remainingEnemies = GameObject.FindGameObjectsWithTag("Enemy").ToList();
        var numRemainingEnemies = 0;
        foreach (var e in remainingEnemies)
        {
            var e_health = e.GetComponent<Health>();
            if (e_health != null && !e_health.dead)
            {
                numRemainingEnemies += 1;
            }
        }

        if (_remainingSpawns > 0 && Time.time >= _nextSpawnTime)
        {
            Spawn();
        }
        else if (numRemainingEnemies == 0 && _remainingSpawns == 0)
        {
            NewWave(waveIntervalInSeconds);
        }
    }

    private void NewWave(float delayInSeconds)
    {
        _currentWave += 1;
        _remainingSpawns = GetSpawnCount(_currentWave);
        _nextSpawnTime = Time.time + delayInSeconds;
        print($"wave {_currentWave} starting in {delayInSeconds} seconds ({_remainingSpawns} enemies)");

        waveUI.GetComponent<TextMeshProUGUI>().text = $"Wave {_currentWave:n0}";

        if(_currentWave > 1)
        {
            waveUI.GetComponent<UITextPop>().PopText();
            waveUI.GetComponent<AudioSource>().Play();
        }
    }

    private void Spawn()
    {
        Assert.IsTrue(_remainingSpawns > 0);

        // Try not to spawn directly on top of the player
        var valid_spawn_points = _spawnPoints.ToList().Where(sp => Vector3.Distance(sp.transform.position, playerTransform.position) > 6.0f).ToList();
        print("Num Valid Spawn Points..." + valid_spawn_points.Count());
        var spawnPoint = valid_spawn_points[Random.Range(0, valid_spawn_points.Count())];


        Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
        _remainingSpawns -= 1;

        checkSpawnInterval = Math.Max(spawnIntervalInSeconds - (_currentWave * 0.3f), 0.1f);
        _nextSpawnTime = Time.time + checkSpawnInterval;
    }

    private static int GetSpawnCount(int wave) => (int)Math.Pow(10, Math.Cbrt(wave));
}