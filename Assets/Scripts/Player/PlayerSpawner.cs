using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    private Cell currentCell; // летка на которой щас стоит шарик, не обновл€етс€ посто€нно
    public Cell CurrentCell { get { return currentCell; } }

    private void Start()
    {
        SetNewPositiion();

        MazeCreateor.Instance.onMazeDeleteAction += SetNewPositiion;
    }

    private void OnDisable()
    {
        MazeCreateor.Instance.onMazeDeleteAction -= SetNewPositiion;
    }

    //ќбновл€ем текущую клетку что бы проложить правильный маршрут
    public void SetNewCurrentCell()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit))
        {
            currentCell = hit.transform.gameObject.GetComponent<Cell>();
        }
    }

    //ѕри генерации лабиринта помещаем шарик в рандомную свободную клетку
    public void SetNewPositiion()
    {
        GameObject randomCell = MazeCreateor.Instance.GetRandomCell(CellType.Road);

        currentCell = randomCell.GetComponent<Cell>();

        currentCell.Parent = currentCell;

        transform.position = new Vector3(
            randomCell.transform.position.x,
            transform.position.y,
            randomCell.transform.position.z
        );
    }
}
