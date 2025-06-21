
using UnityEngine;

public class PlayerSurroundingJump : PlayerState
{
    float jumpForce = 10f;          // �������������
    float horizontalSpeed = 10f;    // ˮƽ�ƶ��ٶ�
    float gravity = -30f;           // ��ǿ����׹��
    float smoothTime = 0.02f;
    float rotationVelocity = 0.2f;
    bool isJump = false; 

    // ����ʱ����
    Vector3 moveDirection;
    float yVelocity;

    public PlayerSurroundingJump(Player player, PlayerStateMachine playerStateMachine)
        : base(player, playerStateMachine) { }

    public override void Enter()
    {
        // ��ʼ����Ծ
        yVelocity = jumpForce;
        player.rigidbody.useGravity = false;//��ΪҪģ�����������Ƚ��ã��Ȼص�������Exit��������
        isJump = true;
        
        Debug.Log("����������");
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
        // ��ȡ���벢��������
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        moveDirection = new Vector3(h, 0, v).normalized;
        //ƽ��ת��
        float targeRotationAngle = Mathf.Atan2(moveDirection.x,moveDirection.z) * Mathf.Rad2Deg;
        float rotation = Mathf.SmoothDampAngle(player.transform.eulerAngles.y, targeRotationAngle, ref rotationVelocity, smoothTime);
        player.transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);//������y��һ�����ת����Ӧ����rotation�Ǳ仯ֵ��ƽ��ת��һ���ת
        Vector3 targetDirection = moveDirection;
    }
    public override void PhysicsFixedUpdate()
    {
        // ����Ӧ�ó�ʼ�ٶ�
        ApplyMovement();
        // ģ������
        yVelocity += gravity * Time.fixedDeltaTime;
        // ����ˮƽ�ٶȲ���
        Vector3 current = player.rigidbody.velocity;
        current.y = yVelocity; // ֻ�޸�Y���ٶ�
        player.rigidbody.velocity = current;
        // �������˳�����Ҫʵ��IsGrounded��
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
