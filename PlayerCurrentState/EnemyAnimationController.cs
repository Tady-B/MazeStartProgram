using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{// ֱ�ӹ����ڽű�������ͬ��Ҳ�����ö���س�ʼ��֮����setAtive(true)�Ź���
    public Animator animator;
    public AnimationClip idleClip;
    public AnimationClip moveClip;
    public AnimationClip attackClip;
    public AnimationClip deacyClip;
    public AnimationClip deathClip;

    private string GetEnemyType()
    {
        return "";
    }
    
}
