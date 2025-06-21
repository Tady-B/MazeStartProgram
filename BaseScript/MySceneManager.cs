using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MySceneManager : MonoBehaviour
{
    private string TeachScene = "MinionScene";
    //��Ϸ��ʼ�ĳ���ת��
    #region ��ťʹ�ú���
    public void GameStartButtonClick()//����֮��ת���������̳̹ؿ�
    {
        // ����ʹ�� StartCoroutine ����Э��
        StartCoroutine(LoadAsyncScene(TeachScene));
        Debug.Log("������");
    }
    IEnumerator LoadAsyncScene(string name)//IEnumerator�ǵ������Ļ����ӿ�
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
        Debug.Log("����ת�����ã�");

    }

}
