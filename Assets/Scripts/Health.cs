using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float maxHealth;

    public int moneyOnDeath = 100;
    
    private float _health;
    private PlayerController _playerController;
    public AudioSourceRandomRange audioRandom;
    

    private void Awake()
    {
        _health = maxHealth;
        _playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }

    public bool ApplyDamage(float damage)
    {
        _health -= damage;
        if (_health <= 0)
        {
            _playerController.AddMoney(moneyOnDeath);
            audioRandom.PlayRandom();
            
            Destroy(gameObject);
            
            return true;
        }

        return false;
    }
}