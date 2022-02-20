using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5f;

    private Transform targetCell;

    private int cellIndex = 0;
    private Vector3 currentPath;

    private List<Cell> myPath = new List<Cell>();

    public Cell GetTarget { get { return targetCell.gameObject.GetComponent<Cell>(); } }

    private void Start()
    {
        MazeCreateor.Instance.onMazeDeleteAction += StopTargeting;
    }

    private void OnDisable()
    {
        MazeCreateor.Instance.onMazeDeleteAction -= StopTargeting;
    }

    //Если новый лабиринт - останавливаем движение
    public void StopTargeting()
    {
        myPath.Clear();
    }

    //Установили точку к которой движемся
    public void SetTarget(Transform _transform)
    {
        targetCell = _transform;
        GetComponent<Pathfinding>().SearchForPath();
    }

    //Приняли лист клеток и обработали его
    public void SetPath(List<Cell> path)
    {
        myPath.Clear();
        myPath = new List<Cell>();
        currentPath = transform.position;
        myPath = path;
        cellIndex = myPath.Count;

        currentPath = new Vector3(myPath[cellIndex -1 ].transform.position.x,
            0.75f,
            myPath[cellIndex - 1].transform.position.z
        );

        foreach (Cell _cell in myPath)
            _cell.ChangeCellColor();
    }

    //Движемся
    void Update()
    {
        if (myPath.Count == 0) return;
        if (Mathf.Abs(transform.position.x - myPath.First().transform.position.x) < 0.1f 
            && Mathf.Abs(transform.position.z - myPath.First().transform.position.z) < 0.1f) 
            return;

        transform.position = Vector3.MoveTowards(transform.position, currentPath, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, currentPath) < 0.03f && myPath.Count > 0)
            currentPath = GetCurrentPathVector();

    }

    //Получаем координаты следующей точки когда достигли предыдущей
    private Vector3 GetCurrentPathVector()
    {
        cellIndex--;

        currentPath = new Vector3(
            myPath[cellIndex].transform.position.x,
            0.75f,
            myPath[cellIndex].transform.position.z
            );

        return currentPath;
    }
}
