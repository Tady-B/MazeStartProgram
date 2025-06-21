using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{// 直接挂载在脚本对象上同样也是适用对象池初始化之后并且setAtive(true)才挂载
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
