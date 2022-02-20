using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    private Cell currentCell; //������ �� ������� ��� ����� �����, �� ����������� ���������
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

    //��������� ������� ������ ��� �� ��������� ���������� �������
    public void SetNewCurrentCell()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit))
        {
            currentCell = hit.transform.gameObject.GetComponent<Cell>();
        }
    }

    //��� ��������� ��������� �������� ����� � ��������� ��������� ������
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
