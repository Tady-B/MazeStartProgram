using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerState
{bool isGround {  get; set; }
    public PlayerIdleState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
    {
    }

    public override void SetAnimation(Player.AnimationType animationType)
    {
      
    }

    public override void Enter()
    {

    }

    public override void Exit()
    {

    }

    public override void FrameUpdate()
    {
        if ( (Input.GetKey(KeyCode.W)|| Input.GetKey(KeyCode.A)|| Input.GetKey(KeyCode.S)||Input.GetKey(KeyCode.D)) && !Input.GetKey(KeyCode.Space) && Raycast())
        {

            player.stateMachine.ChangeStateTo(player.playerMove);
        }
        if (Input.GetKey(KeyCode.J))
        {
            player.stateMachine.ChangeStateTo(player.playerAttack);
        }
        if (Input.GetKey(KeyCode.Space) && Raycast())
        {

            player.stateMachine.ChangeStateTo(player.playerJump);
        }
        if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)) && Input.GetKey(KeyCode.Space) && Raycast())
        {
            player.stateMachine.ChangeStateTo(player.playerSurroundingJump);
        }

    }
    private bool Raycast() // 射线检测是否可以跳跃  
    {
        RaycastHit hit;
        Vector3 origin = player.transform.position; // 玩家位置
        Physics.Raycast(origin, Vector3.down, out hit, 0.1f); // 向下发射射线
        Debug.DrawLine(origin, hit.point, Color.red); // 可视化射线
        isGround = hit.transform;
        return isGround;
    }

    public override void PhysicsFixedUpdate()
    {
        base.PhysicsFixedUpdate();
    }
}
