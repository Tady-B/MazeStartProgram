using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class MininoEnemyMoveState : EnemyMoveState
{
    
    private float attackDistance = 2f;
    public MininoEnemyMoveState(Enemy enemy, EnemyStateMachine stateMachine) : base(enemy, stateMachine)
    {
    }

    public override void Enter()
    {
        Debug.Log("This is move enter");
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        if (DetectPlayer())
        {
            float distance = (player.transform.position - enemy.transform.position).magnitude;
            Debug.Log(distance);
            if (distance < attackDistance)
            {
                Debug.Log("Attack");
                enemy.SwitchState<MinionEnemyAttackState>();
            }
            else
            {
                Debug.Log("Chase,now is in Move to ");
                enemy.SwitchState<MinionEnemyChaseState>();
            }
        }
        if (enemy.isDamage)
        {
            enemy.SwitchState<MinionEnemyDecayState>();
        }
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
