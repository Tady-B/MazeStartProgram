using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerState
{
    Vector3 inputDiraction;
    [Header("跳跃参数")]
    [SerializeField] float groundCheckDistance = 0.2f; // 地面检测距离
    [SerializeField] LayerMask groundMask;     // 地面层级
    public float gravity = -9.81f;
    bool isGround {  get; set; } = false;
 
    public PlayerJumpState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
    {
    }

    public override void SetAnimation(Player.AnimationType animationType)
    {

        player.animator.SetTrigger("IsJump");
     
    }

    public override void Enter()
    {
        player.rigidbody.AddForce(0f, 8000.0f, 0.0f);
    }

    public override void Exit()
    {
       
    }

    public override void FrameUpdate()
    {
        if (inputDiraction.magnitude < 0.1f)
        {
            player.stateMachine.ChangeStateTo(player.playerIdle);
        }

        if (Input.GetKey(KeyCode.Space))
        {
            SetAnimation(Player.AnimationType.Jump);
        }
        if (Input.GetKey(KeyCode.Space) && ((Input.GetKey(KeyCode.W)) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A)))
        {

            player.stateMachine.ChangeStateTo(player.playerSurroundingJump);
        }    
        
    }
    public override void PhysicsFixedUpdate()
    {
        
   
    }
}
