using System;
using UnityEngine;
using UnityEngine.UI;

public class LaughterMeter : MonoBehaviour
{
    [Range(0, 100)] public float percentage;
    [SerializeField] private float decayPerSecond;

    [SerializeField] private Slider laughterSlider;

    private void Update()
    {
        var decay = Time.deltaTime * decayPerSecond;
        percentage = Math.Max(0, percentage - decay);
        laughterSlider.value = percentage;
    }
}