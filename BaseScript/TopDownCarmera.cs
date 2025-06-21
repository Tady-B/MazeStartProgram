using UnityEngine;

public class TopDownCameraController : MonoBehaviour
{
    [Header("相机目标")]
    public Transform target; // 相机跟随的目标（通常是玩家）

    [Header("相机参数")]
    [Range(30, 90)] public float defaultAngle = 90f; // 默认俯视角角度
    [Range(5, 50)] public float minDistance = 10f; // 最小距离
    [Range(15, 100)] public float maxDistance = 30f; // 最大距离
    public float rotationSpeed = 120f; // 旋转速度
    public float zoomSpeed = 10f; // 缩放速度
    public float smoothTime = 0.3f; // 平滑时间

    [Header("碰撞检测")]
    public LayerMask obstacleMask; // 障碍物层
    public float collisionOffset = 0.5f; // 碰撞偏移
    public float collisionRadius = 0.3f; // 碰撞检测半径

    private float currentDistance; // 用户设定的距离
    private float appliedDistance; // 实际应用的相机距离（考虑碰撞）
    private float currentRotation; // 当前旋转角度
    private float distanceVelocity; // 距离平滑速度

    void Start()
    {
        // 初始化距离
        currentDistance = (minDistance + maxDistance) / 2;
        appliedDistance = currentDistance;

        // 初始化旋转角度
        currentRotation = transform.eulerAngles.y;

        // 立即设置初始位置
        UpdateCameraPositionImmediately();
    }

    void LateUpdate()
    {
        if (target == null) return;

        // 处理旋转输入（仅在右键按下时）
        HandleRotationInput();

        // 处理缩放输入
        HandleZoomInput();

        // 更新相机距离（考虑碰撞）
        UpdateCameraDistance();

        // 更新相机位置
        UpdateCameraPosition();
    }

    // 处理旋转输入（仅在鼠标右键按下时）
    private void HandleRotationInput()
    {
        // 仅在鼠标右键按下时处理旋转
        if (Input.GetMouseButton(1))
        {
            float rotationInput = Input.GetAxis("Mouse X");
            currentRotation += rotationInput * rotationSpeed * Time.deltaTime;
        }
        // 键盘旋转（可选，根据需求保留）
        else
        {
            if (Input.GetKey(KeyCode.Q)) currentRotation -= rotationSpeed * Time.deltaTime;
            if (Input.GetKey(KeyCode.E)) currentRotation += rotationSpeed * Time.deltaTime;
        }
    }

    // 处理缩放输入
    private void HandleZoomInput()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scrollInput) > 0.01f)
        {
            currentDistance -= scrollInput * zoomSpeed;
            currentDistance = Mathf.Clamp(currentDistance, minDistance, maxDistance);
        }
    }

    // 更新相机距离（考虑碰撞）
    private void UpdateCameraDistance()
    {
        // 计算理想位置
        Vector3 idealPosition = CalculateCameraPosition(target.position, currentDistance);

        // 获取碰撞调整后的距离
        float collisionAdjustedDistance = GetCollisionAdjustedDistance(target.position, idealPosition);

        // 平滑应用最终距离
        appliedDistance = Mathf.SmoothDamp(
            appliedDistance,
            collisionAdjustedDistance,
            ref distanceVelocity,
            smoothTime
        );
    }

    // 获取碰撞调整后的距离
    private float GetCollisionAdjustedDistance(Vector3 targetPos, Vector3 idealPos)
    {
        Vector3 direction = (idealPos - targetPos).normalized;
        float maxDistance = Vector3.Distance(targetPos, idealPos);

        // 使用SphereCast进行更稳定的碰撞检测
        if (Physics.SphereCast(
            targetPos,
            collisionRadius,
            direction,
            out RaycastHit hit,
            maxDistance,
            obstacleMask))
        {
            // 计算安全距离（减去偏移量）
            float safeDistance = hit.distance - collisionOffset;

            // 确保距离在合理范围内
            return Mathf.Clamp(safeDistance, minDistance, currentDistance);
        }

        return currentDistance;
    }

    // 更新相机位置
    private void UpdateCameraPosition()
    {
        // 计算目标位置
        Vector3 targetPosition = target.position;

        // 计算相机位置（保持固定俯视角）
        Vector3 desiredPosition = CalculateCameraPosition(targetPosition, appliedDistance);
        transform.position = desiredPosition;

        // 确保固定俯视角
        MaintainFixedPerspective(targetPosition);
    }

    // 确保固定俯视角
    private void MaintainFixedPerspective(Vector3 targetPosition)
    {
        // 计算从相机到目标的向量
        Vector3 toTarget = targetPosition - transform.position;

        // 创建俯视角旋转
        Quaternion targetRotation = Quaternion.LookRotation(toTarget);

        // 提取欧拉角并固定X轴为俯视角角度
        Vector3 euler = targetRotation.eulerAngles;
        euler.x = defaultAngle;

        // 应用旋转
        transform.rotation = Quaternion.Euler(euler);
    }

    // 立即更新相机位置（用于初始化）
    private void UpdateCameraPositionImmediately()
    {
        Vector3 targetPosition = target.position;
        Vector3 desiredPosition = CalculateCameraPosition(targetPosition, appliedDistance);
        transform.position = desiredPosition;

        MaintainFixedPerspective(targetPosition);
    }

    // 计算相机位置
    private Vector3 CalculateCameraPosition(Vector3 targetPos, float distance)
    {
        // 1. 计算相机相对于目标的偏移方向
        // - 水平方向：使用当前旋转角度
        // - 垂直方向：使用固定俯视角
        Vector3 horizontalDirection = Quaternion.Euler(0, currentRotation, 0) * Vector3.back;

        // 2. 计算垂直偏移量（基于俯视角）
        float verticalOffset = Mathf.Tan(defaultAngle * Mathf.Deg2Rad) * distance;

        // 3. 组合最终位置
        Vector3 finalPosition = targetPos + horizontalDirection * distance;
        finalPosition.y = targetPos.y + verticalOffset;

        return finalPosition;
    }

    // 设置相机目标
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    // 调试可视化
    void OnDrawGizmosSelected()
    {
        if (target != null)
        {
            // 绘制相机到目标的线
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, target.position);

            // 绘制理想位置
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