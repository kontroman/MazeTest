using System;
using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class Cell : MonoBehaviour, IPointerDownHandler
{
    public CellType Type; //Тип клетки, указываем в префабе

    public int Cost; //Стоимость движения

    public int xCoord;
    public int yCoord;
    public int Distance { get; set; } //Дистанция до конечной
    public int CostDistance => Cost + Distance;
    public Cell Parent { get; set; } //Клетка, с которой мы пришли к этой

    public void SetDistance(int targetX, int targetY)
    {
        this.Distance = Math.Abs(targetX - xCoord) + Math.Abs(targetY - yCoord);
    }

    //Обрабатываем тап по клетке
    public void OnPointerDown(PointerEventData eventData)
    {
        if (!(Type == CellType.Road)) return;

        GameObject.Find("Sphere").GetComponent<PlayerMovement>().SetTarget(transform);
        GameObject.Find("Sphere").GetComponent<PlayerSpawner>().SetNewCurrentCell();
    }

    //Тут отрисовываем путь.
    public void ChangeCellColor()
    {
        StopAllCoroutines();
        StartCoroutine(ChangeColorForTime());
    }

    //По-хорошему бы закэшировать материал при старте, но при больших лабиринтах например 50х50
    //На это уйдет больше времени, нежели ГетКомпонентить только клетки пути
    private IEnumerator ChangeColorForTime()
    {
        Material _mat = gameObject.GetComponent<MeshRenderer>().material;
        _mat.color = Color.red;
        yield return new WaitForSeconds(2f);
        _mat.color = Color.green;
    }    
}
