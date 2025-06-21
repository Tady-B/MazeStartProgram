using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : EnemyState
{
    private float turnSpeed = 180f;//180°每秒
    private bool isTurning = false;

    public EnemyIdleState(Enemy enemy, EnemyStateMachine stateMachine) : base(enemy, stateMachine)
    {
    }

    public override void Enter()//idle模式转身然后切换到移动模式
    {
        SetAnimation();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void SetAnimation()
    {
        base.SetAnimation();
        enemy.animator.SetBool(Enemy.idle, true);
    }
    private void Turn180()//实际需要获取真正承接Mono类挂在脚本的游戏对象
    {
        
    }
}
