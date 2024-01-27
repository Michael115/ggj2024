using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;

[RequireComponent(typeof(NavMeshAgent))]
public class FollowPlayer : MonoBehaviour
{
    private NavMeshAgent _navMeshAgent;
    private Transform[] _players;

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        _players = GameObject.FindGameObjectsWithTag("Player").Select(x => x.transform).ToArray();
        Assert.IsTrue(_players.Length > 0);
    }

    private void Update()
    {
        _navMeshAgent.destination = _players[0].position;
    }
}