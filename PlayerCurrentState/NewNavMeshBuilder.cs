using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
public class NewNavMeshBuilder : MonoBehaviour
{
    public NavMeshSurface navMeshSurface;
    void Start()
    {
        navMeshSurface.BuildNavMesh();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
/*�ýű����ڶ�̬����Mesh�Ա��Enemy�決�����ε�����*/
