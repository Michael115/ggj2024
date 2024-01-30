using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;

[RequireComponent(typeof(NavMeshAgent))]
public class FollowPlayer : MonoBehaviour
{
    private NavMeshAgent _navMeshAgent;
    private Transform[] _playerTransforms;

    public float speed_mod_spread = 0.5f;
    public float radius_low = 0.25f;
    public float radius_high = 1.0f;

    public float reaction_speed_low = 0.05f;
    public float reaction_speed_high = 0.25f;
    private float reaction_speed;

    private float time_till_next_dest_upd = 0.0f;

    private Vector3 destination_offset;

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();

        var orig_speed = _navMeshAgent.speed;
        var speed_mod = Random.Range(-speed_mod_spread, speed_mod_spread);
        var new_speed = orig_speed + speed_mod;
        _navMeshAgent.speed = new_speed;

        Animator anim = GetComponentInChildren<Animator>();
        var base_anim_speed = 1.5f;
        var anim_multiplier = new_speed / orig_speed;
        anim.speed = base_anim_speed * anim_multiplier;

        _navMeshAgent.radius = Random.Range(radius_low, radius_high);
        reaction_speed = Random.Range(reaction_speed_low, reaction_speed_high);

        destination_offset = new Vector3(Random.Range(-1.5f, 1.5f), 0.0f, Random.Range(-1.5f, 1.5f));



    }

    private void Start()
    {
        var players = GameObject.FindGameObjectsWithTag("Player");
        Assert.IsTrue(players.Length > 0);

        _playerTransforms = new Transform[players.Length];
        for (var i = 0; i < players.Length; i++)
        {
            _playerTransforms[i] = players[i].transform;
        }
    }

    private void FixedUpdate()
    {
        const int playerIndex = 0;

        time_till_next_dest_upd -= Time.deltaTime;

        if (time_till_next_dest_upd <= 0.0){
            time_till_next_dest_upd = reaction_speed;
            _navMeshAgent.destination = _playerTransforms[playerIndex].position;

            // If the enemy is a little bit farther away, add some noise into their navigation to mix up swarm shapes
            if(Vector3.Distance(_playerTransforms[playerIndex].position, transform.position) >= 5.0f){
                _navMeshAgent.destination = _playerTransforms[playerIndex].position + destination_offset; 
            }
        }
    }
}