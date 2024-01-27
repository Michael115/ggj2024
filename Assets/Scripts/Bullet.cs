using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

public class Bullet : MonoBehaviour
{

    public VisualEffect impactEffect;
    private bool _isimpactEffectNotNull;
    
    // set by gun
    internal float damage;

    private void Start()
    {
        _isimpactEffectNotNull = impactEffect != null;
    }

    void OnCollisionEnter(Collision collision)
    {
        // Do damage to something
        HitEffect(collision.GetContact(0).point);
        Destroy(gameObject);
    }
    
    private void HitEffect(Vector3 collisionPoint)
    {
        if (_isimpactEffectNotNull)
        {
            impactEffect.transform.position = collisionPoint;
            impactEffect.transform.SetParent(null);
            impactEffect.Play();
            impactEffect.AddComponent<TimedDestroy>().duration = 5;
        }
    }
}
