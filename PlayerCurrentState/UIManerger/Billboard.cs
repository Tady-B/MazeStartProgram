using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Camera mainCam;

    void Start() => mainCam = Camera.main;

    void LateUpdate()
    {
        // 始终面向相机（可选：仅Y轴旋转）
        transform.rotation = Quaternion.LookRotation(transform.position - mainCam.transform.position);
    }
}
