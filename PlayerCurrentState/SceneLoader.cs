using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    // 在Inspector中填写场景名称的字段
    public string targetSceneName;

    // 供按钮直接调用的无参方法
    public void LoadTargetScene()
    {
        if (!string.IsNullOrEmpty(targetSceneName))
        {
            SceneManager.LoadScene(targetSceneName);
        }
        else
        {
            Debug.LogError("场景名称未配置!");
        }
    }
}
