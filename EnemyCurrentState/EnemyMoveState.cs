using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMoveState : EnemyState
{
    private NavMeshAgent agent;
    

    // ��һ�����߿��೤�������ڽ���ʱ�ڳ���������һ�������ο����ȶ���ȷ��һ��׷���͹�����Χ
    private float maxDetectDistance = 2f;
    
    private RaycastHit hit;
    
    public EnemyMoveState(Enemy enemy, EnemyStateMachine stateMachine) : base(enemy, stateMachine)
    {

    }

    public override void Enter()
    {
        base.Enter();
        player = GameObject.FindWithTag("Player");
        SetAnimation();// ����״̬��ʼ��������
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
        // ��enemy���������߼��Ӵ����ĵ�һ�����壬�жϸ������Ƿ�Ϊ��ң�����ǣ��л���׷��ģʽ���߹���ģʽ�����������Һ͵��˵ľ����ڹ�����Χ�����л�������ģʽ
        if (collider != null){// ��⵽����ң�layer3Player�� 
            Debug.DrawLine(enemy.transform.position, Vector3.forward * maxDetectDistance,Color.red);
            return true;    
        }
        return false;
    }
}
