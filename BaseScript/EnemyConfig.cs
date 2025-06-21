using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/EnemyConfig")]
public class EnemyConfig : ScriptableObject
{
    public EnemyAttributes attributes;

    // ��������
    public RuntimeAnimatorController animatorController;
    public AnimationClip[] animationClips;

    // Ԥ��������
    public GameObject enemyPrefab;

    // ��ȡ�ض����������ķ���
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
