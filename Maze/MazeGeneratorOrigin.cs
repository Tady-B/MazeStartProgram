using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class MazeGeneratorOrigin : MonoBehaviour
{
   
    
    [Header("迷宫设置(记得在代码里面也要修改)")]
    public GameObject wallPrefab;
    public int width = 11; //迷宫宽  
    public int length = 11; //迷宫长
    public int cellSize = 1;
    [Header("对象池设置")]
    public int poolSize = 100;
    ObjectPool wallPool;
    public TextMeshProUGUI markExit;
    GameObject startMarker;
    public GameObject player;

    private static MazeCell[,] grid; //网格二维数组，平面两个方向  
    // 这里需要一起改
    public static Vector3[,] worldVersionGrid = new Vector3[11,11];
    public static bool isMazeGenerated = false;
    private Vector3 startPoint;
   
    private Vector2Int startPos = new Vector2Int(0, 0);//起点设置在迷宫右上角
    private static Vector2Int endPos;
    private Stack<Vector2Int> pathStack = new Stack<Vector2Int>();//栈记录了访问的点,注意是一个点，这是一个网格坐标（不是unity里面的世界坐标）
    private void Awake()
    {
        startPoint = player.transform.position;//从玩家所在位置作为起点开始生成迷宫
        InitializeEndPos();
        InitializePool();
        GenerateMaze();
    }
    private void Start()
    {
       
   
    }
    private void GenerateMaze()
    {
        InitializeEndPos();
        InitializeGrid();
        MazeGenerateWithDFS();
        RenderMaze();
        isMazeGenerated = true;
    }
    //=========生成网格（迷宫前置基础）=============  
    //Cell类，网格类，每一个实例都是网格  
    // 迷宫网格数据结构
  
    public class MazeCell
    {
        public bool[] isWallExist = new bool[4]; //四面墙是否存在  
        public bool isVisited; //是否被访问的状态  
    }
    private void InitializeGrid()//注明：这里生成的网格是单位意义上的，而不是在世界坐标下的。
    {
        grid = new MazeCell[width, length];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < length; y++)
            {
                //初始化网格数组里面的每一个网格  
                grid[x, y] = new MazeCell();
                //将每一个网格设置为没有访问  
                grid[x, y].isVisited = false;
                //并将每一个网格的四周都设置为有墙
                for (int i = 0; i < 4; i++) 
                   grid[x, y].isWallExist[i] = true;

            }
        }
    }
    //===========选择网格起点==============  
    //DFS生成的迷宫通常是在生成迷宫之后选择右下角作为迷宫的终点
    private void InitializeEndPos()
    {
        endPos = new Vector2Int(width - 1, length - 1);
    }
    //根据选择的起点进行DFS显示递推标记
    //使用显式栈进行深度优先搜索
    //===========DFS生成迷宫蓝图===============
    private void MazeGenerateWithDFS()
    {
        //首先将起点压入栈开始深度优先搜索设置状态生成迷宫蓝图
        pathStack.Push(startPos);
        grid[startPos.x,startPos.y].isVisited = true;//起点设置已经访问
        while (pathStack.Count > 0)
        {
            Vector2Int current = pathStack.Peek();//读取栈顶元素
            //询问当前网格是否有未访问邻居
            List<Direction> unvisitedNeighbour = GetUnvisitedNeighbour(current);
            if (unvisitedNeighbour.Count > 0)//列表里的元素个数大于0存在未访问邻居
            {//随机选择未访问邻居（此时便已经确定了方向）
                Direction randomDir = unvisitedNeighbour[UnityEngine.Random.Range(0, unvisitedNeighbour.Count)];//不用担心超出因为最多未访问邻居是3个
                Vector2Int nextCell = GetNeighbourPosition(current, randomDir);
                //打通墙
                RemoveWalls(current,randomDir);
                //标记为已经访问过
                grid[nextCell.x,nextCell.y].isVisited = true;
                pathStack.Push(nextCell);

                
            }
            else//全部都已经访问过了，回溯寻找是否还存在未访问过的
            {
                pathStack.Pop();
            }
            
        }

    }
    private void RemoveWalls(Vector2Int current,Direction dir)
    {
        //移除当前单元格的墙壁
        grid[current.x, current.y].isWallExist[(int)dir] = false;
        //移除当前单元格邻居的对面墙（独立存储的状态需要移动两次）
        Vector2Int neighbour = GetNeighbourPosition(current, dir);
        grid[neighbour.x, neighbour.y].isWallExist[(int)GetOppsiteDirection(dir)] = false;
    }
    private List<Direction> GetUnvisitedNeighbour(Vector2Int cell)//返回没有访问的邻居列表便于随便选取一个进行访问
    {
        List<Direction > unvisitedNeighbour = new List<Direction>();
        //cell的北边南边东边西边查找邻居,需要满足条件：1.未访问 2.不是边界处于有效区域
        CheckDirection(cell, Direction.North, unvisitedNeighbour);
        CheckDirection(cell,Direction.East,unvisitedNeighbour);
        CheckDirection(cell, Direction.West, unvisitedNeighbour);//************************************
        CheckDirection(cell, Direction.South, unvisitedNeighbour);
        
        return unvisitedNeighbour;
    }
    enum Direction { North = 0, South = 1, East = 2, West = 3}
    bool isValidCell(Vector2Int cell)
    {
        if (cell.x >= 0 && cell.x < width && cell.y >= 0 && cell.y < length)
            return true;
        return false;
        
    }//辅助，确定有效非边界
    private void CheckDirection(Vector2Int cell,Direction dir, List<Direction> list)//查找单个方向需要获得邻居的位置
    {//1.在网格里面未访问 2.处于有效区域 满足了就添加到列表之中
        Vector2Int neighbour = GetNeighbourPosition(cell,dir);
        if (isValidCell(neighbour) && !grid[neighbour.x, neighbour.y].isVisited)//************注意边界检查逻辑顺序*****************
            list.Add(dir);
        
    }
    private Direction GetOppsiteDirection(Direction dir)
    {
        switch (dir)
        {
           case Direction.North:return Direction.South;
           case Direction.South:return Direction.North;
           case Direction.West: return Direction.East;
           case Direction.East:return Direction.West;
            default:return dir;
        }

    }
    private Vector2Int GetNeighbourPosition(Vector2Int cell,Direction dir)//获得邻居的位置
    {
        switch (dir)
        {
            case Direction.North:return new Vector2Int(cell.x, cell.y + 1);
            case Direction.South:return new Vector2Int(cell.x, cell.y - 1);
            case Direction.West:return new Vector2Int(cell.x - 1,cell.y);
            case Direction.East:return new Vector2Int(cell.x + 1, cell.y);
            default :return cell;
        }
    }
//===================渲染迷宫====================
    private void RenderMaze()
    {
        Debug.Log($"开始渲染迷宫，网格尺寸: {width}x{length}");
        // 遍历所有网格渲染四面墙
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < length; y++)
            {
                Vector3 cellPosition = new Vector3(x * cellSize , 0, y * cellSize) + startPoint;
                worldVersionGrid[x,y] = cellPosition;
                if (grid[x, y].isWallExist[(int)Direction.North])
                    CreatWall(cellPosition + new Vector3( 0, 0,cellSize / 2), Quaternion.identity, cellSize);//设置生成位置生成转向生成的大小
                if (grid[x, y].isWallExist[(int)Direction.South])
                    CreatWall(cellPosition + new Vector3( 0, 0, -cellSize / 2), Quaternion.identity, cellSize);
                if (grid[x, y].isWallExist[(int)Direction.West])
                    CreatWall(cellPosition + new Vector3( -cellSize/2, 0 ,0),Quaternion.Euler( 0, 90, 0), cellSize);
                if (grid[x, y].isWallExist[(int)Direction.East])
                    CreatWall(cellPosition + new Vector3(cellSize / 2, 0, 0), Quaternion.Euler(0, 90, 0), cellSize);
            }
        }
       
    }
    private void MarkStartPoint()
    {
        // 计算世界坐标
        Vector3 startWorldPos = startPoint + new Vector3(startPos.x * cellSize, 0, startPos.y * cellSize);

        // 实际标记操作（改变颜色）
        startMarker = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        startMarker.transform.position = startWorldPos;
        startMarker.GetComponent<Renderer>().material.color = Color.green;

        //然后在终点顶部初始化一个文本标记！
        markExit.text = "Exit!";
        markExit.color = Color.yellow;
        markExit.fontSize = 100;
    }
    private void CreatWall(Vector3 position,Quaternion rotation,float scale)
    {
        //从对象池里面取得对象出来进行生成
        GameObject wall = wallPool.GetObject();
        wall.transform.position = position;
        wall.transform.rotation = rotation;
        wall.transform .localScale = new Vector3(scale, scale * 2f, scale * 0.1f);
    }
    //=================对象池=======================
    private void InitializePool()//初始化对象池
    {
        wallPool = new ObjectPool(wallPrefab,poolSize,transform );
    }
}
