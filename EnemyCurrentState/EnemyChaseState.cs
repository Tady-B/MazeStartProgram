using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaseState : EnemyState
{
    private Vector3 playerPos;
    public EnemyChaseState(Enemy enemy, EnemyStateMachine stateMachine) : base(enemy, stateMachine)
    {

    }

    public override void Enter()
    {
        base.Enter();
        SetAnimation();
        Debug.Log("Enter Chase .Come to find Player");
        player = GameObject.FindWithTag("Player");
        Debug.Log("Player name:" + player.name);
    }

    public override void Exit()
    {
        Debug.Log("Exit Chase");
        base.Exit();
        enemy.animator.SetBool(Enemy.run, false);
    }

    public override void FrameUpdate()
    {
        playerPos = player.transform.position;
        base.FrameUpdate();
        Debug.Log("Player:" + playerPos);
        // ��򵥵�ʵʱˢ�£�����ʵ����Ƕ�ʱˢ�²���Ҫһֱˢ��
        enemy.agent.SetDestination(playerPos);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void SetAnimation()
    {
        base.SetAnimation();
        enemy.animator.SetBool(Enemy.run,true);
    }

    

    
}
