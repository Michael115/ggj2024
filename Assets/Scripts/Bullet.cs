using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

public class Bullet : MonoBehaviour
{
    [SerializeField] private VisualEffect impactEffect;

    private bool _hasImpactEffect;
    private int _enemyLayer;

    // set by gun
    internal float Damage;

    private void Awake()
    {
        _hasImpactEffect = impactEffect != null;
        _enemyLayer = LayerMask.NameToLayer("Enemy");
    }

    void OnCollisionEnter(Collision collision)
    {
        // Only damage things on the enemy layer.
        if (((1 << collision.gameObject.layer) & _enemyLayer) != 0)
        {
            print("OnCollisionEnter");
        }

        HitEffect(collision.GetContact(0).point);
        Destroy(gameObject);
    }

    private void HitEffect(Vector3 collisionPoint)
    {
        if (_hasImpactEffect)
        {
            impactEffect.transform.position = collisionPoint + transform.forward * -0.1f;
            impactEffect.transform.SetParent(null);
            impactEffect.Play();
            impactEffect.AddComponent<TimedDestroy>().duration = 25;
        }
    }
}