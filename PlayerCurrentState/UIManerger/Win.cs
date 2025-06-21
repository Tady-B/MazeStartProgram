using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Win : MonoBehaviour
{
    public GameObject winImage;
    public GameObject winText;
    public Collider endMarker;
    private static GameObject staticWinImage;
    private static GameObject staticWinText;

    void Start()
    {
        
        staticWinImage = winImage;
        staticWinText = winText;
        staticWinImage.SetActive(false);
        staticWinText.SetActive(false);
    }
  
    static void Winnig()
    {
        staticWinText.SetActive(true);
        staticWinImage.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger");
        if (other.CompareTag("Player"))
        {
            Debug.Log("玩家进入碰撞区域");
            Winnig();
            BasicGameTimer.isWin = true;
        }
    }
}
