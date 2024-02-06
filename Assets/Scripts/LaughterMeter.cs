using System;
using System.Collections.Generic;
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
    private int prevClipID = -1;

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

        var currentClipID = Math.Min((int) ((percentage / 100.0f) * clips.Length), clips.Length - 1);
        var clip = clips[currentClipID];

        if (Time.time >= _nextLaughTime || currentClipID != prevClipID)
        {
            _nextLaughTime = Time.time + laughTimeOut;
            audioLaugh.Stop();
            audioLaugh.PlayOneShot(clip);
        }

        prevClipID = currentClipID;

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