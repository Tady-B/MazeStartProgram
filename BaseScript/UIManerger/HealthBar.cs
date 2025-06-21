using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;  // 拖拽赋值Slider组件

    // 设置最大生命值
    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;  // 初始满血
    }

    // 更新当前生命值
    public void SetHealth(int health)
    {
        slider.value = health;
    }
}
