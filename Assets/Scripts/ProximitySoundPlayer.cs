using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ProximitySoundPlayer : MonoBehaviour
{
    private float _nextSoundTime;
    public float secondsBetweenSounds = 3f;
    
    private void OnTriggerStay(Collider other)
    {
        if (Time.time >= _nextSoundTime)
        {
            print($"Triggering {_nextSoundTime}");
            if (other.CompareTag("Enemy") && other.TryGetComponent(out AudioSourceRandomRange audioRandom))
            {
                audioRandom.PlayRandom();
                _nextSoundTime = Time.time + secondsBetweenSounds;
            }
        }
    }
}
