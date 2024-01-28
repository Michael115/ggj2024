using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LaughterMeter : MonoBehaviour
{
    [Range(0, 100)] public float percentage;
    [SerializeField] private float decayPerSecond;
    [SerializeField] private Slider laughterSlider;
    [SerializeField] private float damage;
    [SerializeField] private float damageIntervalInSeconds;
    [SerializeField] private GameObject retryPanel;

    private float _nextDamageTime;
    private InputSystemReader _inputReader;
    private bool _retry;

    private Dictionary<int, float> nextAttacks;

    public AudioSource audioLaugh;

    private float _nextLaughTime;
    public float laughTimeOut = 4f;

    public AudioClip[] clips;
    
    private void Start()
    {
        nextAttacks = new Dictionary<int, float>();
    }

    void OnEnable()
    {
        _inputReader ??= GameController.Instance.InputReader;
        if (_inputReader != null)
        {
            _inputReader.InteractEvent += OnInteract;
        }
    }

    void OnDisable()
    {
        _inputReader ??= GameController.Instance.InputReader;
        if (_inputReader != null)
        {
            _inputReader.InteractEvent -= OnInteract;
        }
    }
    
    
    private void OnInteract()
    {
        if (_retry)
        {
            GameController.Instance.Reload();
        }
    }
    
    private void Update()
    {
        var decay = Time.deltaTime * decayPerSecond;
        percentage = Math.Max(0, percentage - decay);
        laughterSlider.value = percentage;
    }

    private void Tickle(float tickleAmount)
    {
        percentage = Math.Min(100, percentage + tickleAmount);
        laughterSlider.value = percentage;

        if (Time.time >= _nextLaughTime)
        {
            _nextLaughTime = Time.time + laughTimeOut;
            var clip = percentage switch
            {
                < 10 => clips[0],
                < 20 => clips[1],
                < 30 => clips[2],
                < 40 => clips[3],
                < 50 => clips[4],
                < 60 => clips[5],
                < 70 => clips[6],
                < 80 => clips[7],
                < 90 => clips[8],
                _ => clips[9],
            };

            audioLaugh.PlayOneShot(clip);
        }

        if (percentage >= 100)
        {
            Time.timeScale = 0;
            retryPanel.SetActive(true);
            _retry = true;
        }
    }

    // private void OnTriggerEnter(Collider other)
    // {
    //     if (other.CompareTag("Enemy") && Time.time >= _nextDamageTime)
    //     {
    //         Tickle(damage);
    //     }
    // }
    
    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Enemy")) return;

        var instanceId = other.GetInstanceID();
        
        nextAttacks.TryGetValue(instanceId, out float nextDamageTime);
        
        if (Time.time >= nextDamageTime)
        {
            nextAttacks[instanceId] = Time.time + damageIntervalInSeconds;
            Tickle(damage);
        }
        
      
    }
}