using System;
using System.Collections.Generic;
using TMPro;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.AI;

public class MinionEnemy : Enemy// �����Ѿ���ʼ��״̬��
{
    [SerializeField] private float detectionRange = 15f;
    [SerializeField] private float attackRange = 2f;
    // ÿ��ʵ������״̬
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
        // ��ǰ���ڴ����ܻ��������
        if (isProcessingHit)
        {
            // ���ƶ��д�С��ֹ���޶ѻ�
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

    protected override void InitializeStates()// ��ʼ״̬��Enemy�����Ѿ���ɳ�ʼ�������ı�������ĳ�ʼ�����뷵����InitialStateType
    {
        // ��ʼ��״̬ ʵ��״̬this
        chaseState = new MinionEnemyChaseState(this, stateMachine);
        attackState = new MinionEnemyAttackState(this, stateMachine);
        decayState = new MinionEnemyDecayState(this, stateMachine);
        idleState = new MinionEnemyIdleState(this, stateMachine);
        moveState = new MininoEnemyMoveState(this, stateMachine);
        // ע��״̬
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
        InitializeStates();// ��ʼʵ����״̬
        // ���ó�ʼ״̬
        Debug.Log("��ʼ��״̬");
        stateMachine.Initialize(moveState);
        currentState = moveState;
         
    }
    protected override void Update()
    {
        // ���״̬�л���
        base.Update();
        currentState.FrameUpdate();

    }
    public override void TakeDamage(int damage)
    {
        // ���û��෽�������˺�
        base.TakeDamage(damage);
        stateMachine.SwitchState(decayState);
    }

}
