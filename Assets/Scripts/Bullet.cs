using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

public class Bullet : MonoBehaviour
{
    [SerializeField] private VisualEffect impactEffect;
    [SerializeField] private VisualEffect impactEffectEnemy;

    private bool _hasImpactEffect;
    private bool _hasImpactEffectEnemy;

    // set by gun
    internal float Damage;

    private void Awake()
    {
        _hasImpactEffect = impactEffect != null;
        _hasImpactEffectEnemy = impactEffectEnemy != null;
    }

    void OnCollisionEnter(Collision collision)
    {
        // Only damage things that have health.
        if (collision.gameObject.TryGetComponent(out Health health))
        {
            health.ApplyDamage(Damage);
            HitEffectEnemy(collision.GetContact(0).point);
        }
        else
        {
            HitEffect(collision.GetContact(0).point);
        }
       
        Destroy(gameObject);
    }

    private void HitEffectEnemy(Vector3 collisionPoint)
    {
        if (_hasImpactEffectEnemy)
        {
            impactEffectEnemy.transform.position = collisionPoint + transform.forward * -0.1f;
            impactEffectEnemy.transform.SetParent(null);
            impactEffectEnemy.Play();
            impactEffectEnemy.AddComponent<TimedDestroy>().duration = 25;
        }
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