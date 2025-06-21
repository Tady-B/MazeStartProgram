using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Failure : MonoBehaviour//¥¶¿Ì ß∞‹¬ﬂº≠
{
    public GameObject failureText;
    public GameObject failureImage;
    private static GameObject staticFailureText;
    private GameObject player;
    private  static GameObject image;
    void Start()
    {
        staticFailureText = failureText ;
        image = failureImage;
        image.SetActive(false);
        failureText.SetActive(false);
               
    }
    public static void Fail()
    {
        staticFailureText.SetActive(true);
        image.SetActive(true);
    }
}
