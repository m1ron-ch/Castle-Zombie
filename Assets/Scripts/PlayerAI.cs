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

    public virtual void MoveTo(Vector3 position)
    {
        Active();

        _agent.destination = position;
        _animator.SetBool(Key.Animations.Running.ToString(), true);

        float delay = Util.CalculateTimeToReachPoint(transform.position, position, _agent.speed);
        Util.Invoke(this, () => Stop(), delay + 0.2f);
    }

    public void Stop()
    {
        _agent.isStopped = true;
        _animator.SetBool(Key.Animations.Running.ToString(), false);
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
