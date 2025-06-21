using UnityEngine;

[System.Serializable]
public class EnemyAttributes
{
    // ��������
    public string enemyType;
    public float health = 100f;
    public float moveSpeed = 3f;
    public float damage = 10f;

    // ��Ϊ����
    [Header("��Ϊ����")]
    public float chaseDistance = 10f;
    public float attackDistance = 2f;
    public float patrolRadius = 5f;

    // ��������
    [Header("��������")]
    public string idleAnimName = "Idle";
    public string moveAnimName = "Move";
    public string attackAnimName = "Attack";
    public string decayAnimName = "Decay";
    public string dieAnimName = "Die";
    public float moveAnimSpeedMultiplier = 1f;

    // �Ӿ�����
    [Header("�Ӿ�����")]
    public Material enemyMaterial;
    public GameObject weaponPrefab;

    // ��������
    [Header("��������")]
    public AudioClip attackSound;
    public AudioClip deathSound;
}
