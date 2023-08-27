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

        Deactivate();
    }

    public void MoveTo(Vector3 position)
    {
        Active();

        _agent.destination = position;
        _animator.SetBool(Key.Animations.Running.ToString(), true);
    }

    public void Active()
    {
        _agent.enabled = true;
    }

    public void Deactivate()
    {
        _agent.enabled = false;
    }
}
