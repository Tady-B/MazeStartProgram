using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMoveState : EnemyState
{
    private NavMeshAgent agent;
    

    // 画一个射线看多长或者是在进行时在场景里面拉一个长方形看长度多少确定一下追击和攻击范围
    private float maxDetectDistance = 2f;
    
    private RaycastHit hit;
    
    public EnemyMoveState(Enemy enemy, EnemyStateMachine stateMachine) : base(enemy, stateMachine)
    {

    }

    public override void Enter()
    {
        base.Enter();
        player = GameObject.FindWithTag("Player");
        SetAnimation();// 进入状态开始播放行走
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        enemy.agent.SetDestination(enemy.targetPosition);
     
        
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void SetAnimation()
    {
        base.SetAnimation();
        enemy.animator.SetBool(Enemy.walk, true);
    }
    protected bool DetectPlayer()
    {
        Collider[] collider = Physics.OverlapSphere(enemy.transform.position, maxDetectDistance, 3);
        // 从enemy自身发射射线检测接触到的第一个物体，判断该物体是否为玩家，如果是，切换到追击模式或者攻击模式，如果满足玩家和敌人的距离在攻击范围内则切换到攻击模式
        if (collider != null){// 检测到了玩家（layer3Player） 
            Debug.DrawLine(enemy.transform.position, Vector3.forward * maxDetectDistance,Color.red);
            return true;    
        }
        return false;
    }
}
