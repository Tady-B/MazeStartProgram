using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
    private GameObject prefab;
    private Queue<GameObject> pool = new Queue<GameObject>();
    // ʹ�ö�����Ҫ��ȷ�������ѭ��ʹ�ã��������һ���ֶ���ʹ��Ƶ����һ���ֶ�����ȫû�����ϵ����
    private Transform parent;// ������λ��
    private GameObject musicMonster;
    private int initializeSize;

    // ��Ҫ��ö������ݶ���أ��黹���󣬹黹���ж���
    public ObjectPool(GameObject prefab, int initialSize, Transform parent)
    {
        this.prefab = prefab;
        this.parent = parent;
        GrowPool(initialSize);// �����ʵ����ʼ���������Ӷ��������Ķ���
    }


    private void GrowPool(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject obj = GameObject.Instantiate(prefab, parent);// HERE!!!!
            obj.SetActive(false);// ��Ҫȷ����δ����״̬
            pool.Enqueue(obj);
        }
    }
    //�Ӷ���������ö���
    public GameObject GetObject()
    {
        if (pool.Count == 0)
            GrowPool(5);// ����ؿ�������
        GameObject obj = pool.Dequeue();// �Ӷ�����ȡ��
        obj.SetActive(true);//���ü���
        return obj;
    }
    // �黹����������÷Ǽ���״̬�����������
    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);
        pool.Enqueue(obj);

    }
    // �黹���ж��󣬱��������Ѿ�ʹ�õ��Ӷ���
    public void ReturnAll()
    {
        foreach (Transform child in parent)
        {
            if (child.gameObject.activeSelf && child.gameObject.name.StartsWith(prefab.name))
            {// ����Ӷ����Ǽ���״̬�����Ӷ����������prefab��ͷ��
                ReturnObject(child.gameObject);
            }
        }
    }
}