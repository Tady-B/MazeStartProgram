using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;  // ��ק��ֵSlider���

    // �����������ֵ
    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;  // ��ʼ��Ѫ
    }

    // ���µ�ǰ����ֵ
    public void SetHealth(int health)
    {
        slider.value = health;
    }
}
