
using UnityEngine;

public class PlayerSurroundingJump : PlayerState
{
    float jumpForce = 10f;          // 合理的起跳力度
    float horizontalSpeed = 10f;    // 水平移动速度
    float gravity = -30f;           // 更强的下坠感
    float smoothTime = 0.02f;
    float rotationVelocity = 0.2f;
    bool isJump = false; 

    // 运行时变量
    Vector3 moveDirection;
    float yVelocity;

    public PlayerSurroundingJump(Player player, PlayerStateMachine playerStateMachine)
        : base(player, playerStateMachine) { }

    public override void Enter()
    {
        // 初始化跳跃
        yVelocity = jumpForce;
        player.rigidbody.useGravity = false;//因为要模拟重力所以先禁用，等回到地面再Exit里面启用
        isJump = true;
        
        Debug.Log("四周跳进入");
    }
    public override void Exit() { 
     isJump = false;
    }
    void ApplyMovement()
    {
        Vector3 velocity = moveDirection * horizontalSpeed;
        player.rigidbody.velocity = velocity;
    }
    //xuya
    public override void FrameUpdate()
    {
        // 获取输入并锁定方向
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        moveDirection = new Vector3(h, 0, v).normalized;
        //平滑转向
        float targeRotationAngle = Mathf.Atan2(moveDirection.x,moveDirection.z) * Mathf.Rad2Deg;
        float rotation = Mathf.SmoothDampAngle(player.transform.eulerAngles.y, targeRotationAngle, ref rotationVelocity, smoothTime);
        player.transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);//就沿着y轴一点点旋转到对应方向（rotation是变化值）平滑转向一点点转
        Vector3 targetDirection = moveDirection;
    }
    public override void PhysicsFixedUpdate()
    {
        // 立即应用初始速度
        ApplyMovement();
        // 模拟重力
        yVelocity += gravity * Time.fixedDeltaTime;
        // 保持水平速度不变
        Vector3 current = player.rigidbody.velocity;
        current.y = yVelocity; // 只修改Y轴速度
        player.rigidbody.velocity = current;
        // 地面检测退出（需要实现IsGrounded）
        if (yVelocity < 0 && player.IsGrounded())
        {
            player.stateMachine.ChangeStateTo(player.playerIdle);
        }
    }

    

    public override void SetAnimation(Player.AnimationType animationType)
    {
        player.animator.SetTrigger("JumpForward");
    }
}
