using UnityEngine;

[System.Serializable]
public class EnemyAttributes
{
    // 基础属性
    public string enemyType;
    public float health = 100f;
    public float moveSpeed = 3f;
    public float damage = 10f;

    // 行为属性
    [Header("行为设置")]
    public float chaseDistance = 10f;
    public float attackDistance = 2f;
    public float patrolRadius = 5f;

    // 动画属性
    [Header("动画设置")]
    public string idleAnimName = "Idle";
    public string moveAnimName = "Move";
    public string attackAnimName = "Attack";
    public string decayAnimName = "Decay";
    public string dieAnimName = "Die";
    public float moveAnimSpeedMultiplier = 1f;

    // 视觉属性
    [Header("视觉设置")]
    public Material enemyMaterial;
    public GameObject weaponPrefab;

    // 声音属性
    [Header("声音设置")]
    public AudioClip attackSound;
    public AudioClip deathSound;
}
