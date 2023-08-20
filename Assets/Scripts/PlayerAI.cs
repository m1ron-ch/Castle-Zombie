using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent (typeof(Animator))]
public class PlayerAI : Humanoid
{
    private Material _skin;
    private NavMeshAgent _agent;
    private Animator _animator;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
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
}
