using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AnimatorEventCenter
{// �����¼�����Ψһ����ʵ�ֵ���ģʽ
    private static AnimatorEventCenter instance;
    public static AnimatorEventCenter Instance
    {// �¼�ʵ��
        get
        {
            if(instance == null)
            {
                instance = new AnimatorEventCenter();
            }
            return instance;
        }
    }
    // �洢�¼��Ͷ�Ӧ�Ķ������б�,��Ϊ�¼���ֵΪ�������б���������δ֪���͵�   
    // �������б�
    private static readonly Dictionary<AnimatorEventType, List<Action<Animator>>> eventSubscribers = new Dictionary<AnimatorEventType, List<Action<Animator>>>();
    // ����
    public static void Subscribe(AnimatorEventType eventType,Action<Animator> callback)
    {
        // �������������¼�
        if (!eventSubscribers.ContainsKey(eventType))
        {// ���½�һ��
            eventSubscribers[eventType] = new List<Action<Animator>>();
        }// ͬ������½�ֵ��ʼ��
        var list = eventSubscribers[eventType];
        if (!list.Contains(callback))
        {
            list.Add(callback);
        }
    }// ȡ������
    public static void Unsubscribe(AnimatorEventType eventType, Action<Animator> callback)
    {
        if (eventSubscribers.TryGetValue(eventType,out var list))// ʹ��TryGetValue�����Ҳ�����Ӧ��˵��ֵ��������ô��û�ж����ߣ���ʲô��������
        {// ��������ҵ�����¼���������б����Ƴ�ʵ��ȡ������,ʹ��out���䷵��
            list.Remove(callback);
        }
    }
    // �����¼�֪ͨ
    public static void Publish(AnimatorEventType eventType,Animator animator)
    { 
        if(eventSubscribers.TryGetValue(eventType, out var handlers))
        {// ����ҵ����¼��Ͷ�Ӧ�Ķ�������ô���֪ͨ�¼�������
            foreach(var handler in handlers.ToArray())// ʹ�ø������б���
            {
                handler?.Invoke(animator);// ȷ����ί�в��ᵼ�±���������ǿ�ί�о�ʲô������
            }
        }
        // ��ô��Ƶ�ԭ����Ϊ�˷�ֹ��;�ж�����ȡ�����ĵ���ԭʼ�б��ȱ��޸ĴӶ��׳��쳣�޷��ٱ���
        // ���ԶԸ��Ƽ����в�������ʹԭ�����޸��ˣ���������
    }
    
}

public enum AnimatorEventType// enumʵ�ʱ����ʱ�����Enum��
{
    EnemyInstanceCreated// ����ʵ������ɣ��ֱ𴫵�Animator�������ͬ���͵��˵�ʵ��
}