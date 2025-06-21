using System;
using System.Collections.Generic;
using TMPro;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.AI;

public class MinionEnemy : Enemy// 基类已经初始化状态机
{
    [SerializeField] private float detectionRange = 15f;
    [SerializeField] private float attackRange = 2f;
    // 每个实例独享状态
    private MininoEnemyMoveState moveState;
    private MinionEnemyChaseState chaseState;
    private MinionEnemyAttackState attackState;
    private MinionEnemyDecayState decayState;
    private MinionEnemyIdleState idleState;

    private Queue<HitData> hitQueue = new Queue<HitData>();
    private bool isProcessingHit = false;
    private HitData currentHitData;
    public void QueueHit(HitData hitData)
    {
        // 当前正在处理受击加入队列
        if (isProcessingHit)
        {
            // 限制队列大小防止无限堆积
            if (hitQueue.Count < maxConsexutiveHits)
            {
                hitQueue.Enqueue(hitData);
            }
            return;
        }
    }
    private void ProcessHitImmediately(HitData hitData)
    {
        isProcessingHit = true;
        currentHitData = hitData;
        stateMachine.SwitchState(decayState);
    }


    protected override Type InitialStateType => typeof(MininoEnemyMoveState);

    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        currentState.PhysicsUpdate();

    }

    protected override void InitializeStates()// 初始状态在Enemy基类已经完成初始化，该文本给基类的初始化代码返回了InitialStateType
    {
        // 初始化状态 实例状态this
        chaseState = new MinionEnemyChaseState(this, stateMachine);
        attackState = new MinionEnemyAttackState(this, stateMachine);
        decayState = new MinionEnemyDecayState(this, stateMachine);
        idleState = new MinionEnemyIdleState(this, stateMachine);
        moveState = new MininoEnemyMoveState(this, stateMachine);
        // 注册状态
        RegisterState<MinionEnemyIdleState>(idleState);
        RegisterState<MinionEnemyAttackState>(attackState);
        RegisterState<MinionEnemyDecayState>(decayState);
        RegisterState<MininoEnemyMoveState>(moveState);
        RegisterState<MinionEnemyChaseState>(chaseState);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void Start()
    {
        base.Start();
        agent = GetComponent<NavMeshAgent>();
        InitializeStates();// 开始实例化状态
        // 设置初始状态
        Debug.Log("初始化状态");
        stateMachine.Initialize(moveState);
        currentState = moveState;
         
    }
    protected override void Update()
    {
        // 检查状态切换？
        base.Update();
        currentState.FrameUpdate();

    }
    public override void TakeDamage(int damage)
    {
        // 调用基类方法处理伤害
        base.TakeDamage(damage);
        stateMachine.SwitchState(decayState);
    }

}
