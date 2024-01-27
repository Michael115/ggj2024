using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

public class Bullet : MonoBehaviour
{
    [SerializeField] private VisualEffect impactEffect;

    private bool _hasImpactEffect;

    // set by gun
    internal float Damage;

    private void Awake()
    {
        _hasImpactEffect = impactEffect != null;
    }

    void OnCollisionEnter(Collision collision)
    {
        // Only damage things that have health.
        if (collision.gameObject.TryGetComponent(out Health health))
        {
            health.ApplyDamage(Damage);
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