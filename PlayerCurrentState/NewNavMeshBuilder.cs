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
/*该脚本用于动态生成Mesh以便给Enemy烘焙出可游荡区域*/
