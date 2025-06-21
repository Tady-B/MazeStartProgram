using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionEnemyAttackState : EnemyAttackState
{
   
    public MinionEnemyAttackState(Enemy enemy, EnemyStateMachine stateMachine) : base(enemy, stateMachine)
    {

    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Enter Minion Attack State");
    }
    public override void Exit()
    {
        base.Exit();
        Debug.Log("Exit Minion Attack State");
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();// ²¥·Å¹¥»÷¶¯»­
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void SetAnimation()
    {
        base.SetAnimation();
        
        
    }

    
}
