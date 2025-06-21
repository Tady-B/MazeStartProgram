using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCarmera : MonoBehaviour
{
    [Header("Reference")]    
    public Transform player;
    public Rigidbody rb;
    
    public float rotationSpeed;
    private void Update()
    {
        transform.position = player.position;
    }
}
