using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;

[RequireComponent(typeof(NavMeshAgent))]
public class FollowPlayer : MonoBehaviour
{
    private NavMeshAgent _navMeshAgent;
    private Transform[] _playerTransforms;

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
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

    private void Update()
    {
        const int playerIndex = 0;
        _navMeshAgent.destination = _playerTransforms[playerIndex].position;
    }
}