using System;
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
        _nextDamageTime = Time.time + damageIntervalInSeconds;
        percentage = Math.Min(100, percentage + tickleAmount);
        laughterSlider.value = percentage;
        if (percentage >= 100)
        {
            Time.timeScale = 0;
            retryPanel.SetActive(true);
            _retry = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && Time.time >= _nextDamageTime)
        {
            Tickle(damage);
        }
    }
    
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Enemy") && Time.time >= _nextDamageTime)
        {
            Tickle(damage);
        }
    }
}