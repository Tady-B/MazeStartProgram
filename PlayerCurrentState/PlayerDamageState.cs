using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamageState : PlayerState
{
    public PlayerDamageState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
    }

    public override void PhysicsFixedUpdate()
    {
        base.PhysicsFixedUpdate();
    }

    public override void SetAnimation(Player.AnimationType animationType)
    {
        base.SetAnimation(animationType);
    }

    public override void SetAnimation(Player.AnimationType animationType, bool isDoing)
    {
        base.SetAnimation(animationType, isDoing);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
