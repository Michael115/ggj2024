using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ProximitySoundPlayer : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Sound") && other.TryGetComponent(out AudioSourceRandomRange audioRandom))
        {
            audioRandom.PlayRandom();
        }
    }
}
