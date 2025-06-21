using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    // ��Inspector����д�������Ƶ��ֶ�
    public string targetSceneName;

    // ����ťֱ�ӵ��õ��޲η���
    public void LoadTargetScene()
    {
        if (!string.IsNullOrEmpty(targetSceneName))
        {
            SceneManager.LoadScene(targetSceneName);
        }
        else
        {
            Debug.LogError("��������δ����!");
        }
    }
}
