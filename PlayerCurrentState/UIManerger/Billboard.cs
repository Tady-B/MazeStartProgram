using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Camera mainCam;

    void Start() => mainCam = Camera.main;

    void LateUpdate()
    {
        // ʼ�������������ѡ����Y����ת��
        transform.rotation = Quaternion.LookRotation(transform.position - mainCam.transform.position);
    }
}
