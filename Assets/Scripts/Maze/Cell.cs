using System;
using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class Cell : MonoBehaviour, IPointerDownHandler
{
    public CellType Type; //��� ������, ��������� � �������

    public int Cost; //��������� ��������

    public int xCoord;
    public int yCoord;
    public int Distance { get; set; } //��������� �� ��������
    public int CostDistance => Cost + Distance;
    public Cell Parent { get; set; } //������, � ������� �� ������ � ����

    public void SetDistance(int targetX, int targetY)
    {
        this.Distance = Math.Abs(targetX - xCoord) + Math.Abs(targetY - yCoord);
    }

    //������������ ��� �� ������
    public void OnPointerDown(PointerEventData eventData)
    {
        if (!(Type == CellType.Road)) return;

        GameObject.Find("Sphere").GetComponent<PlayerMovement>().SetTarget(transform);
        GameObject.Find("Sphere").GetComponent<PlayerSpawner>().SetNewCurrentCell();
    }

    //��� ������������ ����.
    public void ChangeCellColor()
    {
        StopAllCoroutines();
        StartCoroutine(ChangeColorForTime());
    }

    //��-�������� �� ������������ �������� ��� ������, �� ��� ������� ���������� �������� 50�50
    //�� ��� ����� ������ �������, ������ ��������������� ������ ������ ����
    private IEnumerator ChangeColorForTime()
    {
        Material _mat = gameObject.GetComponent<MeshRenderer>().material;
        _mat.color = Color.red;
        yield return new WaitForSeconds(2f);
        _mat.color = Color.green;
    }    
}
