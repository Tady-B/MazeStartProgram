using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class EnemyDecayState : EnemyState
{
    
    private Transform player;
    private bool isDecayState;
    private bool isContinueDecay;
    public EnemyDecayState(Enemy enemy, EnemyStateMachine stateMachine) : base(enemy, stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();      
        SetAnimation();
        isContinueDecay = false;
        isDecayState = true;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        SetAnimation(); 
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
 

    public override void SetAnimation()
    {
        base.SetAnimation();
        enemy.animator.SetTrigger(Enemy.hit);
    }
}
