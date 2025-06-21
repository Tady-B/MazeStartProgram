using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour,IDamageable,ICoroutineRunner
{

    public Animator animator;
  
    public int maxHealth { get ; set ; }
    public int currentHealth { get ; set ; }
    #region PlayerStatesAndStateMachineValue
    public PlayerState currentState { get ; set ; }
    public PlayerIdleState playerIdle { set; get; }
    public PlayerAttackState playerAttack { set; get; }
    public PlayerMoveState playerMove { set; get; }
    public PlayerJumpState playerJump { set; get; }
    public PlayerDamageState playerDamage { set; get; }
    public PlayerStateMachine stateMachine { set; get; }
    public PlayerSurroundingJump playerSurroundingJump { set; get; }
    public Transform playerTransform { set; get; }
    #endregion
    public Rigidbody rigidbody { get; set; }
    public Collider cld { get; set; }
    bool isGround { get; set; }
    public LayerMask layerMask { get; set; }
    public RaycastHit hit { get; set; }
   
    public bool IsDead { get; set; } = false;
    public bool isDamage { get ; set ; }

    public void CheckIsDead()
    {
        if (IsDead)
        {
            Debug.Log("你死了！");
        }
    }


    private void Awake()
    {
        playerTransform = transform;
        stateMachine = new PlayerStateMachine();    
        playerAttack = new PlayerAttackState(this,stateMachine);//都是使用这个实例化的状态机
        playerIdle = new PlayerIdleState(this,stateMachine);
        playerMove = new PlayerMoveState(this,stateMachine);
        playerJump = new PlayerJumpState(this,stateMachine);
        playerDamage = new PlayerDamageState(this,stateMachine);
        playerSurroundingJump = new PlayerSurroundingJump(this, stateMachine);
    }
    void Start()
    {
       
        hit = new RaycastHit();
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();
        stateMachine.Initialized(playerIdle);
    }

    void Update()
    {
        stateMachine.currentState.FrameUpdate();
    }
    void FixedUpdate()
    {
       stateMachine.currentState.PhysicsFixedUpdate();
    }
    public bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 0.1f);
    }

    public new void StopCoroutine(Coroutine coroutine)
    {
        base.StopCoroutine(coroutine);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
    }

    public enum AnimationType
    {
        Idle,
        Walk,
        Run,
        Jump,
        Attack,
        Hit,
        Death,
        JumpForward
    }
  
}
