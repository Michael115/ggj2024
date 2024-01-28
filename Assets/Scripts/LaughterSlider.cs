using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class LaughterSlider : MonoBehaviour
{
    [SerializeField] private Image handle;
    [SerializeField] private Sprite sprite0;
    [SerializeField] private Sprite sprite20;
    [SerializeField] private Sprite sprite40;
    [SerializeField] private Sprite sprite60;
    [SerializeField] private Sprite sprite80;

    private Slider _slider;

    private void Awake()
    {
        _slider = GetComponent<Slider>();
    }

    private void Update()
    {
        handle.sprite = _slider.value switch
        {
            < 20 => sprite0,
            < 40 => sprite20,
            < 60 => sprite40,
            < 80 => sprite60,
            _ => sprite80,
        };
    }
}