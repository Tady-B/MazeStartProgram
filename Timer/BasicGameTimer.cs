using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BasicGameTimer:MonoBehaviour
{
    private float unitTime = 60f;
    private float totalTime;
    public TextMeshProUGUI timerText;
    private int count = 0;
    public static bool isWin = false;
    public float SetTime(float min)//返回一个设定好了的秒数计时
    {
        return unitTime * min;
    }
    public float CountDown()//倒计时,在Update调用，Update里面就写countTime >= 1就执行这个然后重置countTime = 0;
    {
        float leftSeconds = totalTime--;
        return leftSeconds;
    }
    private void Start()
    {
        totalTime = SetTime(2);
    }
    void FixedUpdate()//一秒执行五十次数到50次执行一次
    {  
        if(isWin == false)
        {   
            count++;
            if (count == 50 && totalTime > 0)
            {
                CountDown();
                count = 0;
            }
            UpdateDisplay();

        }
        if (IsTimeRunOut())
            Failure.Fail();
    }
    private bool IsTimeRunOut()
    {
        if (totalTime == 0)
            return true;
        return false;
    }
    private void UpdateDisplay()
    {
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(totalTime / 60);
            int seconds = Mathf.FloorToInt(totalTime % 60);
            timerText.text = $"{minutes:00}:{ seconds:00}";
        }
    }
}
//想写个计时器，在玩家进入游戏关卡后开始计时，开发者可以设定计时时间进行调试的那种
//最终计时器显示在UI上面，就在屏幕的右上角，这里只需要计时，UI逻辑写在别的地方
//可以使用Time.deltaTime进行乘法累加得出计时时间来控制
//显示还需要转换一下