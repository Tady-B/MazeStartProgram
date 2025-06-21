using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MySceneManager : MonoBehaviour
{
    private string TeachScene = "MinionScene";
    //游戏开始的场景转换
    #region 按钮使用函数
    public void GameStartButtonClick()//按下之后转换场景到教程关卡
    {
        // 必须使用 StartCoroutine 启动协程
        StartCoroutine(LoadAsyncScene(TeachScene));
        Debug.Log("调用了");
    }
    IEnumerator LoadAsyncScene(string name)//IEnumerator是迭代器的基本接口
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(name);
        while (!asyncLoad.isDone)
        {
            yield return null;
            Debug.Log("asyncLoad haven't done!");
        }
       
    }
    #endregion
    public void ChangeSceneTo(string scene)
    {
        Debug.Log("场景转换调用！");

    }

}
