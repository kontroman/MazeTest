using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//�� ������ ���� ���� ��������: http://www.robotron2084guidebook.com/home/games/berzerk/mazegenerator/

public class MazeCreateor : MonoBehaviour
{
    public static MazeCreateor Instance { get; private set; }

    [SerializeField] private int sizeX, sizeY;
    public int SizeX { get { return sizeX; } }
    public int SizeY { get { return sizeY; } }

    private Cell[,] cells;

    [SerializeField]
    public UnityEvent onMazeDeleted;
    public event Action onMazeDeleteAction;

    private void Awake()
    {
        if (Instance != null) return;
        else Instance = this;
    }

    private void Start()
    {
        sizeX = MazeData.MAZE_SIZE_X;
        sizeY = MazeData.MAZE_SIZE_Y;

        GenerateMaze(sizeX, sizeY);
    }

    private void GenerateMaze(int sizeX, int sizeY)
    {
        cells = new Cell[sizeX, sizeY];

        //��������� ��������
        for (int x = 0; x < sizeX; x++)
        {
            for (int y = 0; y < sizeY; y++)
            {
                //��� ������� ����� ������ ������
                cells[x, y] = CreateCellWithType(CellType.Road);
                
                //���� ������� ���������, �� ���������� ���� �������
                if (x == 0 || y == 0 || x == sizeX - 1 || y == sizeY - 1)
                    cells[x, y] = CreateCellWithType(CellType.Border);
                //����� �������� �������� ����� ������ �������, ������ ��� �����
                else if (x % 2 == (sizeX % 2 == 0 ? 1 : 0) && y % 2 == (sizeY % 2 == 0 ? 1 : 0))
                {
                    if (UnityEngine.Random.value > .1f)
                    {
                        cells[x, y] = CreateCellWithType(CellType.Wall); 

                        int offsetX = UnityEngine.Random.value < .5f ? 0 : -1;
                        int offsetY = offsetX != 0 ? 0 : -1;
                        
                        //�� �������������� ������� ���� ������ ����
                        if(cells[x + offsetX, y + offsetY].Type != CellType.Border)
                            cells[x + offsetX, y + offsetY] = CreateCellWithType(CellType.Wall);
                    }
                }
            }
        }
        
        //��������� ����������� �������
        for (int x = 0; x < sizeX; x++)
        {
            for (int y = 0; y < sizeY; y++)
            {
                CreateCell(x, y, cells[x,y].Type);
            }
        }
    }

    private Cell CreateCellWithType(CellType _type)
    {
        GameObject newCell = new GameObject();
        newCell.AddComponent<Cell>().Type = _type;

        Destroy(newCell, .1f);

        return newCell.GetComponent<Cell>();
    }

    public GameObject GetRandomCell(CellType _type)
    {
        List<Cell> typeCells = new List<Cell>();

        //�� �������� �� ���� ��������� �������� � ������������������, ������� ����� ������� For

        //foreach(Cell _cell in cells)
        //{
        //    if (_cell.Type == _type)
        //        typeCells.Add(_cell);
        //}

        for (int x = 0; x < sizeX; x++)
        {
            for (int y = 0; y < sizeY; y++)
            {
                if (cells[x, y].Type == _type)
                    typeCells.Add(cells[x, y]);
            }
        }

        return typeCells[UnityEngine.Random.Range(0,typeCells.Count - 1)].gameObject;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            DeleteMaze(true);
            GenerateMaze(sizeX, sizeY);
        }
    }

    //������� ���������� ������ ���������� ���� � ��������� �����
    //TODO: ObjectPool, �� ��� ��������� ������������ �������� �� �����, ������ ������ � ���,
    //��� ������ ������� ������ � ������������������
    private void CreateCell(int x, int y, CellType type)
    {
        Cell newCell = Instantiate(GetPrefabByCellType(type));

        newCell.name = "Maze Cell " + x + ", " + y;
        newCell.transform.parent = transform;
        newCell.transform.localPosition = GetCellPosition(x,y, type);
        newCell.xCoord = x;
        newCell.yCoord = y;

        cells[x, y] = newCell;
    }

    public Cell GetPrefabByCellType(CellType type)
    {
        return Resources.Load<Cell>("Prefabs/" + type.ToString());
    }

    //���� ����� - �� ��� ������ ������ ����
    private Vector3 GetCellPosition(int x, int y, CellType type)
    {
        return new Vector3(
            x - sizeX * 0.5f + 0.5f,
            type == CellType.Road ? 0 : 1f,
            y - sizeY * 0.5f + 0.5f);
    }

    public Cell GetCellInPosition(int posX, int posY)
    {
        return cells[posX, posY];
    }

    public void SetNewMazeSizeX(int newSize)
    {
        sizeX = newSize > MazeData.MAZE_MIN_SIZE_X ? newSize : MazeData.MAZE_MIN_SIZE_X;
    }

    public void SetNewMazeSizeY(int newSize)
    {
        sizeY = newSize > MazeData.MAZE_MIN_SIZE_Y ? newSize : MazeData.MAZE_MIN_SIZE_Y;
    }

    //����������� ������ �� ���������� ������� ���������
    public int GetHighSize()
    {
        return sizeX > sizeY ? sizeX : sizeY;
    }

    //��� ������ ������ ���� ���� ��������� ������, � ������� ������
    public void ResetCellParents()
    {
        foreach (Cell _cell in cells)
        {
            _cell.Parent = null;
        }
    }

    public void DeleteMaze(bool createNew)
    {
        foreach(Cell _cell in cells)
        {
            Destroy(_cell.gameObject);
        }

        Array.Clear(cells, 0, cells.Length);

        if (createNew)
            GenerateMaze(sizeX, sizeY);

        onMazeDeleted.Invoke();

        if (onMazeDeleteAction != null)
            onMazeDeleteAction();
    }
}
