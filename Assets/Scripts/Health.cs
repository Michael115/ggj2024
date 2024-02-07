using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Health : MonoBehaviour
{
    [SerializeField] private float maxHealth;

    public int moneyOnDeath = 100;
    
    private float _health;
    private PlayerController _playerController;
    public AudioSourceRandomRange audioRandom;
    public AudioSource audioDeath;
    public GameObject ragdoll;
    
    public float forceMultiplier;
    public GameObject[] disables;
    public Collider[] ragdollColliders;

    private FollowPlayer _follow;
    private NavMeshAgent _nav;
    private AudioSource _audio;
    private CapsuleCollider _collider;
    private LayerMask _mask;
    private bool dead;


    private void Awake()
    {
        _health = maxHealth;
        _playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        _follow = GetComponent<FollowPlayer>();
        _nav = GetComponent<NavMeshAgent>();
        _audio = GetComponent<AudioSource>();
        _collider = GetComponent<CapsuleCollider>();
        _mask = LayerMask.GetMask("Ragdoll");

    }

    
    public bool ApplyDamage(float damage, Vector3 pt, Vector3 dmgDirection)
    {
        if (dead) return false;
        
        if (_health <= 0) return false;
        
        _health -= damage;
        var nvAgent = GetComponent<NavMeshAgent>();
        nvAgent.velocity = nvAgent.velocity * 0.25f;

        if (_health <= 0)
        {
            dead = true;
            
            _playerController.AddMoney(moneyOnDeath);

            // Stop any existing chatter first;
            foreach(AudioSource audSource in GetComponentsInChildren<AudioSource>())
            {
                audSource.Stop();
            }
            
            audioDeath.transform.parent = null;
            audioDeath.AddComponent<TimedDestroy>().duration = 5;
            audioRandom.PlayRandom();

            foreach (var disable in disables)
            {
                disable.SetActive(false);
            }

            _follow.enabled = false;
            _nav.enabled = false;
           
            _collider.enabled = false;
            
            ragdoll.SetActive(true);
            
            var randomCollider = ragdollColliders[Random.Range(0, ragdollColliders.Length)];
            
            randomCollider.attachedRigidbody.AddForceAtPosition(dmgDirection*damage*forceMultiplier, randomCollider.ClosestPoint(pt), ForceMode.VelocityChange);
            
            Destroy(gameObject,GameController.Instance.bodyDisappearTime);
            
            return true;
        }

        return false;
    }
}