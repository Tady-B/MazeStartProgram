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
    public float SetTime(float min)//����һ���趨���˵�������ʱ
    {
        return unitTime * min;
    }
    public float CountDown()//����ʱ,��Update���ã�Update�����дcountTime >= 1��ִ�����Ȼ������countTime = 0;
    {
        float leftSeconds = totalTime--;
        return leftSeconds;
    }
    private void Start()
    {
        totalTime = SetTime(2);
    }
    void FixedUpdate()//һ��ִ����ʮ������50��ִ��һ��
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
//��д����ʱ��������ҽ�����Ϸ�ؿ���ʼ��ʱ�������߿����趨��ʱʱ����е��Ե�����
//���ռ�ʱ����ʾ��UI���棬������Ļ�����Ͻǣ�����ֻ��Ҫ��ʱ��UI�߼�д�ڱ�ĵط�
//����ʹ��Time.deltaTime���г˷��ۼӵó���ʱʱ��������
//��ʾ����Ҫת��һ��