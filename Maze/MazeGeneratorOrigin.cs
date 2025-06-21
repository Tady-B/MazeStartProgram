using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class MazeGeneratorOrigin : MonoBehaviour
{
   
    
    [Header("�Թ�����(�ǵ��ڴ�������ҲҪ�޸�)")]
    public GameObject wallPrefab;
    public int width = 11; //�Թ���  
    public int length = 11; //�Թ���
    public int cellSize = 1;
    [Header("���������")]
    public int poolSize = 100;
    ObjectPool wallPool;
    public TextMeshProUGUI markExit;
    GameObject startMarker;
    public GameObject player;

    private static MazeCell[,] grid; //�����ά���飬ƽ����������  
    // ������Ҫһ���
    public static Vector3[,] worldVersionGrid = new Vector3[11,11];
    public static bool isMazeGenerated = false;
    private Vector3 startPoint;
   
    private Vector2Int startPos = new Vector2Int(0, 0);//����������Թ����Ͻ�
    private static Vector2Int endPos;
    private Stack<Vector2Int> pathStack = new Stack<Vector2Int>();//ջ��¼�˷��ʵĵ�,ע����һ���㣬����һ���������꣨����unity������������꣩
    private void Awake()
    {
        startPoint = player.transform.position;//���������λ����Ϊ��㿪ʼ�����Թ�
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
    //=========���������Թ�ǰ�û�����=============  
    //Cell�࣬�����࣬ÿһ��ʵ����������  
    // �Թ��������ݽṹ
  
    public class MazeCell
    {
        public bool[] isWallExist = new bool[4]; //����ǽ�Ƿ����  
        public bool isVisited; //�Ƿ񱻷��ʵ�״̬  
    }
    private void InitializeGrid()//ע�����������ɵ������ǵ�λ�����ϵģ������������������µġ�
    {
        grid = new MazeCell[width, length];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < length; y++)
            {
                //��ʼ���������������ÿһ������  
                grid[x, y] = new MazeCell();
                //��ÿһ����������Ϊû�з���  
                grid[x, y].isVisited = false;
                //����ÿһ����������ܶ�����Ϊ��ǽ
                for (int i = 0; i < 4; i++) 
                   grid[x, y].isWallExist[i] = true;

            }
        }
    }
    //===========ѡ���������==============  
    //DFS���ɵ��Թ�ͨ�����������Թ�֮��ѡ�����½���Ϊ�Թ����յ�
    private void InitializeEndPos()
    {
        endPos = new Vector2Int(width - 1, length - 1);
    }
    //����ѡ���������DFS��ʾ���Ʊ��
    //ʹ����ʽջ���������������
    //===========DFS�����Թ���ͼ===============
    private void MazeGenerateWithDFS()
    {
        //���Ƚ����ѹ��ջ��ʼ���������������״̬�����Թ���ͼ
        pathStack.Push(startPos);
        grid[startPos.x,startPos.y].isVisited = true;//��������Ѿ�����
        while (pathStack.Count > 0)
        {
            Vector2Int current = pathStack.Peek();//��ȡջ��Ԫ��
            //ѯ�ʵ�ǰ�����Ƿ���δ�����ھ�
            List<Direction> unvisitedNeighbour = GetUnvisitedNeighbour(current);
            if (unvisitedNeighbour.Count > 0)//�б����Ԫ�ظ�������0����δ�����ھ�
            {//���ѡ��δ�����ھӣ���ʱ���Ѿ�ȷ���˷���
                Direction randomDir = unvisitedNeighbour[UnityEngine.Random.Range(0, unvisitedNeighbour.Count)];//���õ��ĳ�����Ϊ���δ�����ھ���3��
                Vector2Int nextCell = GetNeighbourPosition(current, randomDir);
                //��ͨǽ
                RemoveWalls(current,randomDir);
                //���Ϊ�Ѿ����ʹ�
                grid[nextCell.x,nextCell.y].isVisited = true;
                pathStack.Push(nextCell);

                
            }
            else//ȫ�����Ѿ����ʹ��ˣ�����Ѱ���Ƿ񻹴���δ���ʹ���
            {
                pathStack.Pop();
            }
            
        }

    }
    private void RemoveWalls(Vector2Int current,Direction dir)
    {
        //�Ƴ���ǰ��Ԫ���ǽ��
        grid[current.x, current.y].isWallExist[(int)dir] = false;
        //�Ƴ���ǰ��Ԫ���ھӵĶ���ǽ�������洢��״̬��Ҫ�ƶ����Σ�
        Vector2Int neighbour = GetNeighbourPosition(current, dir);
        grid[neighbour.x, neighbour.y].isWallExist[(int)GetOppsiteDirection(dir)] = false;
    }
    private List<Direction> GetUnvisitedNeighbour(Vector2Int cell)//����û�з��ʵ��ھ��б�������ѡȡһ�����з���
    {
        List<Direction > unvisitedNeighbour = new List<Direction>();
        //cell�ı����ϱ߶������߲����ھ�,��Ҫ����������1.δ���� 2.���Ǳ߽紦����Ч����
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
        
    }//������ȷ����Ч�Ǳ߽�
    private void CheckDirection(Vector2Int cell,Direction dir, List<Direction> list)//���ҵ���������Ҫ����ھӵ�λ��
    {//1.����������δ���� 2.������Ч���� �����˾���ӵ��б�֮��
        Vector2Int neighbour = GetNeighbourPosition(cell,dir);
        if (isValidCell(neighbour) && !grid[neighbour.x, neighbour.y].isVisited)//************ע��߽����߼�˳��*****************
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
    private Vector2Int GetNeighbourPosition(Vector2Int cell,Direction dir)//����ھӵ�λ��
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
//===================��Ⱦ�Թ�====================
    private void RenderMaze()
    {
        Debug.Log($"��ʼ��Ⱦ�Թ�������ߴ�: {width}x{length}");
        // ��������������Ⱦ����ǽ
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < length; y++)
            {
                Vector3 cellPosition = new Vector3(x * cellSize , 0, y * cellSize) + startPoint;
                worldVersionGrid[x,y] = cellPosition;
                if (grid[x, y].isWallExist[(int)Direction.North])
                    CreatWall(cellPosition + new Vector3( 0, 0,cellSize / 2), Quaternion.identity, cellSize);//��������λ������ת�����ɵĴ�С
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
        // ������������
        Vector3 startWorldPos = startPoint + new Vector3(startPos.x * cellSize, 0, startPos.y * cellSize);

        // ʵ�ʱ�ǲ������ı���ɫ��
        startMarker = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        startMarker.transform.position = startWorldPos;
        startMarker.GetComponent<Renderer>().material.color = Color.green;

        //Ȼ�����յ㶥����ʼ��һ���ı���ǣ�
        markExit.text = "Exit!";
        markExit.color = Color.yellow;
        markExit.fontSize = 100;
    }
    private void CreatWall(Vector3 position,Quaternion rotation,float scale)
    {
        //�Ӷ��������ȡ�ö��������������
        GameObject wall = wallPool.GetObject();
        wall.transform.position = position;
        wall.transform.rotation = rotation;
        wall.transform .localScale = new Vector3(scale, scale * 2f, scale * 0.1f);
    }
    //=================�����=======================
    private void InitializePool()//��ʼ�������
    {
        wallPool = new ObjectPool(wallPrefab,poolSize,transform );
    }
}
