using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AnimatorEventCenter
{// 由于事件中心唯一，则实现单例模式
    private static AnimatorEventCenter instance;
    public static AnimatorEventCenter Instance
    {// 事件实例
        get
        {
            if(instance == null)
            {
                instance = new AnimatorEventCenter();
            }
            return instance;
        }
    }
    // 存储事件和对应的订阅者列表,键为事件，值为订阅者列表，订阅者是未知类型的   
    // 订阅者列表
    private static readonly Dictionary<AnimatorEventType, List<Action<Animator>>> eventSubscribers = new Dictionary<AnimatorEventType, List<Action<Animator>>>();
    // 订阅
    public static void Subscribe(AnimatorEventType eventType,Action<Animator> callback)
    {
        // 如果不存在这个事件
        if (!eventSubscribers.ContainsKey(eventType))
        {// 则新建一个
            eventSubscribers[eventType] = new List<Action<Animator>>();
        }// 同理如果新建值初始化
        var list = eventSubscribers[eventType];
        if (!list.Contains(callback))
        {
            list.Add(callback);
        }
    }// 取消订阅
    public static void Unsubscribe(AnimatorEventType eventType, Action<Animator> callback)
    {
        if (eventSubscribers.TryGetValue(eventType,out var list))// 使用TryGetValue，若找不到对应的说明值不存在那么就没有订阅者，即什么都不操作
        {// 如果可以找到这个事件，则将其从列表中移除实现取消订阅,使用out将其返回
            list.Remove(callback);
        }
    }
    // 发布事件通知
    public static void Publish(AnimatorEventType eventType,Animator animator)
    { 
        if(eventSubscribers.TryGetValue(eventType, out var handlers))
        {// 如果找到了事件和对应的订阅者那么逐个通知事件发生了
            foreach(var handler in handlers.ToArray())// 使用副本进行遍历
            {
                handler?.Invoke(animator);// 确保空委托不会导致崩溃，如果是空委托就什么都不做
            }
        }
        // 这么设计的原因是为了防止中途有订阅者取消订阅导致原始列表长度被修改从而抛出异常无法再遍历
        // 所以对复制件进行操作，即使原件被修改了，照样遍历
    }
    
}

public enum AnimatorEventType// enum实际编译的时候会变成Enum类
{
    EnemyInstanceCreated// 敌人实例化完成：分别传递Animator组件给不同类型敌人的实例
}