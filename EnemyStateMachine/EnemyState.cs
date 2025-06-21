using UnityEngine;

public abstract class EnemyState
{
    protected GameObject player;
    protected Enemy enemy;
    protected EnemyStateMachine stateMachine;
    public EnemyState(Enemy enemy, EnemyStateMachine stateMachine)
    {
        this.enemy = enemy;
        this.stateMachine = stateMachine;
    }

    public virtual void Enter() { }
    public virtual void Exit() { }
    public virtual void SetAnimation() { }
    public virtual void FrameUpdate(){}
    public virtual void PhysicsUpdate() { }
}

