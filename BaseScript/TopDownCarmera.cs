using UnityEngine;

public class TopDownCameraController : MonoBehaviour
{
    [Header("���Ŀ��")]
    public Transform target; // ��������Ŀ�꣨ͨ������ң�

    [Header("�������")]
    [Range(30, 90)] public float defaultAngle = 90f; // Ĭ�ϸ��ӽǽǶ�
    [Range(5, 50)] public float minDistance = 10f; // ��С����
    [Range(15, 100)] public float maxDistance = 30f; // ������
    public float rotationSpeed = 120f; // ��ת�ٶ�
    public float zoomSpeed = 10f; // �����ٶ�
    public float smoothTime = 0.3f; // ƽ��ʱ��

    [Header("��ײ���")]
    public LayerMask obstacleMask; // �ϰ����
    public float collisionOffset = 0.5f; // ��ײƫ��
    public float collisionRadius = 0.3f; // ��ײ���뾶

    private float currentDistance; // �û��趨�ľ���
    private float appliedDistance; // ʵ��Ӧ�õ�������루������ײ��
    private float currentRotation; // ��ǰ��ת�Ƕ�
    private float distanceVelocity; // ����ƽ���ٶ�

    void Start()
    {
        // ��ʼ������
        currentDistance = (minDistance + maxDistance) / 2;
        appliedDistance = currentDistance;

        // ��ʼ����ת�Ƕ�
        currentRotation = transform.eulerAngles.y;

        // �������ó�ʼλ��
        UpdateCameraPositionImmediately();
    }

    void LateUpdate()
    {
        if (target == null) return;

        // ������ת���루�����Ҽ�����ʱ��
        HandleRotationInput();

        // ������������
        HandleZoomInput();

        // ����������루������ײ��
        UpdateCameraDistance();

        // �������λ��
        UpdateCameraPosition();
    }

    // ������ת���루��������Ҽ�����ʱ��
    private void HandleRotationInput()
    {
        // ��������Ҽ�����ʱ������ת
        if (Input.GetMouseButton(1))
        {
            float rotationInput = Input.GetAxis("Mouse X");
            currentRotation += rotationInput * rotationSpeed * Time.deltaTime;
        }
        // ������ת����ѡ��������������
        else
        {
            if (Input.GetKey(KeyCode.Q)) currentRotation -= rotationSpeed * Time.deltaTime;
            if (Input.GetKey(KeyCode.E)) currentRotation += rotationSpeed * Time.deltaTime;
        }
    }

    // ������������
    private void HandleZoomInput()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scrollInput) > 0.01f)
        {
            currentDistance -= scrollInput * zoomSpeed;
            currentDistance = Mathf.Clamp(currentDistance, minDistance, maxDistance);
        }
    }

    // ����������루������ײ��
    private void UpdateCameraDistance()
    {
        // ��������λ��
        Vector3 idealPosition = CalculateCameraPosition(target.position, currentDistance);

        // ��ȡ��ײ������ľ���
        float collisionAdjustedDistance = GetCollisionAdjustedDistance(target.position, idealPosition);

        // ƽ��Ӧ�����վ���
        appliedDistance = Mathf.SmoothDamp(
            appliedDistance,
            collisionAdjustedDistance,
            ref distanceVelocity,
            smoothTime
        );
    }

    // ��ȡ��ײ������ľ���
    private float GetCollisionAdjustedDistance(Vector3 targetPos, Vector3 idealPos)
    {
        Vector3 direction = (idealPos - targetPos).normalized;
        float maxDistance = Vector3.Distance(targetPos, idealPos);

        // ʹ��SphereCast���и��ȶ�����ײ���
        if (Physics.SphereCast(
            targetPos,
            collisionRadius,
            direction,
            out RaycastHit hit,
            maxDistance,
            obstacleMask))
        {
            // ���㰲ȫ���루��ȥƫ������
            float safeDistance = hit.distance - collisionOffset;

            // ȷ�������ں���Χ��
            return Mathf.Clamp(safeDistance, minDistance, currentDistance);
        }

        return currentDistance;
    }

    // �������λ��
    private void UpdateCameraPosition()
    {
        // ����Ŀ��λ��
        Vector3 targetPosition = target.position;

        // �������λ�ã����̶ֹ����ӽǣ�
        Vector3 desiredPosition = CalculateCameraPosition(targetPosition, appliedDistance);
        transform.position = desiredPosition;

        // ȷ���̶����ӽ�
        MaintainFixedPerspective(targetPosition);
    }

    // ȷ���̶����ӽ�
    private void MaintainFixedPerspective(Vector3 targetPosition)
    {
        // ����������Ŀ�������
        Vector3 toTarget = targetPosition - transform.position;

        // �������ӽ���ת
        Quaternion targetRotation = Quaternion.LookRotation(toTarget);

        // ��ȡŷ���ǲ��̶�X��Ϊ���ӽǽǶ�
        Vector3 euler = targetRotation.eulerAngles;
        euler.x = defaultAngle;

        // Ӧ����ת
        transform.rotation = Quaternion.Euler(euler);
    }

    // �����������λ�ã����ڳ�ʼ����
    private void UpdateCameraPositionImmediately()
    {
        Vector3 targetPosition = target.position;
        Vector3 desiredPosition = CalculateCameraPosition(targetPosition, appliedDistance);
        transform.position = desiredPosition;

        MaintainFixedPerspective(targetPosition);
    }

    // �������λ��
    private Vector3 CalculateCameraPosition(Vector3 targetPos, float distance)
    {
        // 1. ������������Ŀ���ƫ�Ʒ���
        // - ˮƽ����ʹ�õ�ǰ��ת�Ƕ�
        // - ��ֱ����ʹ�ù̶����ӽ�
        Vector3 horizontalDirection = Quaternion.Euler(0, currentRotation, 0) * Vector3.back;

        // 2. ���㴹ֱƫ���������ڸ��ӽǣ�
        float verticalOffset = Mathf.Tan(defaultAngle * Mathf.Deg2Rad) * distance;

        // 3. �������λ��
        Vector3 finalPosition = targetPos + horizontalDirection * distance;
        finalPosition.y = targetPos.y + verticalOffset;

        return finalPosition;
    }

    // �������Ŀ��
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    // ���Կ��ӻ�
    void OnDrawGizmosSelected()
    {
        if (target != null)
        {
            // ���������Ŀ�����
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, target.position);

            // ��������λ��
            if (Application.isPlaying)
            {
                Vector3 idealPos = CalculateCameraPosition(target.position, currentDistance);
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(idealPos, 0.5f);
                Gizmos.DrawLine(target.position, idealPos);
            }
        }
    }
}