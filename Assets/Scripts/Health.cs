using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float maxHealth;

    private float _health;

    private void Awake()
    {
        _health = maxHealth;
    }

    public bool ApplyDamage(float damage)
    {
        print($"Received {damage} damage");

        _health -= damage;
        if (_health <= 0)
        {
            Destroy(gameObject);
            return true;
        }

        return false;
    }
}