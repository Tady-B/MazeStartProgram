using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/EnemyConfig")]
public class EnemyConfig : ScriptableObject
{
    public EnemyAttributes attributes;

    // 动画配置
    public RuntimeAnimatorController animatorController;
    public AnimationClip[] animationClips;

    // 预制体引用
    public GameObject enemyPrefab;

    // 获取特定动画剪辑的方法
    public AnimationClip GetAnimationClip(string clipName)
    {
        foreach (var clip in animationClips)
        {
            if (clip.name == clipName)
                return clip;
        }
        return null;
    }
}
