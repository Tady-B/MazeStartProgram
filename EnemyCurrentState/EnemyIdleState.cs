using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : EnemyState
{
    private float turnSpeed = 180f;//180��ÿ��
    private bool isTurning = false;

    public EnemyIdleState(Enemy enemy, EnemyStateMachine stateMachine) : base(enemy, stateMachine)
    {
    }

    public override void Enter()//idleģʽת��Ȼ���л����ƶ�ģʽ
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
    private void Turn180()//ʵ����Ҫ��ȡ�����н�Mono����ڽű�����Ϸ����
    {
        
    }
}
