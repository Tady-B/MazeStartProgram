using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
    private GameObject prefab;
    private Queue<GameObject> pool = new Queue<GameObject>();
    // 使用队列主要是确保对象的循环使用，不会出现一部分对象使用频繁另一部分对象完全没有用上的情况
    private Transform parent;// 父对象位置
    private GameObject musicMonster;
    private int initializeSize;

    // 需要获得对象，扩容对象池，归还对象，归还所有对象
    public ObjectPool(GameObject prefab, int initialSize, Transform parent)
    {
        this.prefab = prefab;
        this.parent = parent;
        GrowPool(initialSize);// 对象池实例初始化就是增加对象池里面的对象
    }


    private void GrowPool(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject obj = GameObject.Instantiate(prefab, parent);// HERE!!!!
            obj.SetActive(false);// 需要确保是未激活状态
            pool.Enqueue(obj);
        }
    }
    //从对象池里面获得对象
    public GameObject GetObject()
    {
        if (pool.Count == 0)
            GrowPool(5);// 对象池空了扩容
        GameObject obj = pool.Dequeue();// 从队列中取出
        obj.SetActive(true);//设置激活
        return obj;
    }
    // 归还对象就是设置非激活状态，重新入队列
    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);
        pool.Enqueue(obj);

    }
    // 归还所有对象，遍历所有已经使用的子对象
    public void ReturnAll()
    {
        foreach (Transform child in parent)
        {
            if (child.gameObject.activeSelf && child.gameObject.name.StartsWith(prefab.name))
            {// 如果子对象是激活状态并且子对象的名字是prefab开头的
                ReturnObject(child.gameObject);
            }
        }
    }
}