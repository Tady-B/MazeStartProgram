using System.Collections;
using System.Collections.Generic;
using System.Net.Mail;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;


public class PlayerAttackState : PlayerState
{
    public int attackDamge = 25;
    private bool inAttackState;
    private bool comboWindowOpen;
    [Header("Attack Settings")]
    public float attackRange = 2f;
    public LayerMask enemyLayers;// 记得设置碰撞层级
    public Vector3 attackOffset = new Vector3(0, 1, 0.5f);
    public PlayerAttackState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
    {
    }
    public virtual void ExecuteAttack()
    {
        Debug.Log("ExecuteAttack");
        Vector3 attackPosition = player.transform.position 
            + player.transform.forward * attackOffset.z
            + player.transform.up * attackOffset.y;
        Collider[] hitEnemies = Physics.OverlapSphere(
            attackPosition,
            attackRange,
            enemyLayers
        );
        foreach (Collider hit in hitEnemies)
        {
           IDamageable damageable = hit.GetComponent<IDamageable>();// 可以直接获取身上有无接口
            if (damageable != null)
            {
                damageable.TakeDamage(attackDamge);
                damageable.isDamage = true;
            }
        }
    }
    public override void Enter()
    {   
        Debug.Log("Enter PlayerAttackState");
        base.Enter();
        PlayAttackAnimation();
        inAttackState = true;
        comboWindowOpen = false;
    }

    public override void Exit()
    {
        Debug.Log("Exit PlayerAttackState");
        base.Exit();
    }


    public override void FrameUpdate()
    {
        base.FrameUpdate();
        ExecuteAttack();
        if (!inAttackState) return;
        AnimatorStateInfo stateInfo = player.animator.GetCurrentAnimatorStateInfo(0);// 0是基本主动画层
        // 计算归一化时间
        float normalizedTime = stateInfo.normalizedTime % 1;
        // 连击窗口检测后30%的动画
        if(normalizedTime > 0.7f && !comboWindowOpen)// 开启检测
        {
            comboWindowOpen = true;
            Debug.Log("连击窗口开启");
        }
        if(normalizedTime > 0.8f && stateInfo.normalizedTime >= 1)
        {
            EndAttackAmimation();
        }
    }

    public override void PhysicsFixedUpdate()
    {
        base.PhysicsFixedUpdate();
    }
    protected virtual bool CheckMoveInput()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.A))
        {
            return true;
        }
        else return false;
    }
    private void PlayAttackAnimation()
    {
        // 替换SetBool为直接播放动画
        player.animator.Play("Attack", -1, 0f); // 强制从头播放
    }
    private void EndAttackAmimation()
    {
        inAttackState = false;
        // 切换状态
        if(comboWindowOpen && Input.GetKeyDown(KeyCode.J))
        {
            StartNextCombo();
           

        }
        else if(IsMoving()){
            playerStateMachine.ChangeStateTo(player.playerMove);
        }
        else
        {
            playerStateMachine.ChangeStateTo(player.playerIdle);
        }
    }
    private void StartNextCombo() => player.animator.Play("Attack", -1, 0f);
    private bool IsMoving() => Input.GetAxis("Horizontal") != 0f || Input.GetAxis("Vertical") != 0f;
}
