
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerState
{
    float moveSpeed = 5.0f;
    float x, y;
    float smoothTime = 0.02f;
    float rotationVelocity = 0.2f;
    float targetRotationAngle;
    Vector3 targetDirection;


    public PlayerMoveState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
    {
    }

    public override void SetAnimation(Player.AnimationType animationType)
    {
        player.animator.SetFloat("X", x);
        player.animator.SetFloat("Y", y);
    }

    public override void Enter()
    {
       
    }

    public override void Exit()
    {
        player.animator.SetFloat("X", 0);
        player.animator.SetFloat("Y", 0);
    
    }
    public override void FrameUpdate()
    {
        y = Input.GetAxisRaw("Vertical");
        x = Input.GetAxisRaw("Horizontal");
        SetAnimation(Player.AnimationType.Walk);
        Vector3 inputDiraction = new Vector3(x, 0.0f, y).normalized;
        if (inputDiraction.magnitude > 0.001f)
        {
            targetRotationAngle = Mathf.Atan2(inputDiraction.x, inputDiraction.z) * Mathf.Rad2Deg;
            float rotation = Mathf.SmoothDampAngle(player.transform.eulerAngles.y, targetRotationAngle, ref rotationVelocity, smoothTime);
            //具体改变实现移动
            player.transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);//就沿着y轴一点点旋转到对应方向（rotation是变化值）平滑转向一点点转
            targetDirection = inputDiraction;
        }

        if (inputDiraction.magnitude < 1f)
        {
            player.stateMachine.ChangeStateTo(player.playerIdle);
        }
        if (Input.GetKey(KeyCode.Space))
        {

            player.stateMachine.ChangeStateTo(player.playerJump);
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            player.stateMachine.ChangeStateTo(player.playerAttack);
        }
    }



    public override void PhysicsFixedUpdate()
    {
        player.transform.position += targetDirection * Time.deltaTime * moveSpeed;
    }
}