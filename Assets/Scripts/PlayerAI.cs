using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent (typeof(Animator))]
public class PlayerAI : Humanoid
{
    private PlayerAIManager _manager;
    private Material _skin;
    private NavMeshAgent _agent;
    private Animator _animator;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
    }

    public void Init(PlayerAIManager manager)
    {
        _manager = manager;
    }

    public override void Freeze()
    {
        base.Freeze();

        _animator.SetBool(Key.Animations.Running.ToString(), false);
        _agent.isStopped = true;
    }

    public void MoveTo(Vector3 position)
    {
        _agent.destination = position;
        _animator.SetBool(Key.Animations.Running.ToString(), true);
    }

    public void FindAnotherRoom()
    {
        foreach (Room room in _manager.GameManager.RoomManager.Rooms)
            if (room.StakeOut == null)
            {
                MoveTo(room.GunPosition.position);
                Debug.Log(room.name.ToString());
            }
        
    }
}
