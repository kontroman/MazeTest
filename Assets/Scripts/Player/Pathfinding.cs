using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    List<Cell> openList = new List<Cell>(); //active
    List<Cell> closeList = new List<Cell>(); //visited
    List<Cell> finalpath = new List<Cell>();

    private Cell startCell;
    private Cell currentCell;
    private Cell targetCell;

    public void SearchForPath()
    {
        //Ќебольша€ задержка нужна что бы успели обновитьс€ данные о том, на какой клетке сейчас шарик
        StartCoroutine(delay());
    }

    IEnumerator delay()
    {
        MazeCreateor.Instance.ResetCellParents();

        closeList = new List<Cell>();
        finalpath = new List<Cell>();
        openList = new List<Cell>();

        yield return new WaitForSeconds(0.05f);

        targetCell = GetComponent<PlayerMovement>().GetTarget;
        currentCell = GetComponent<PlayerSpawner>().CurrentCell;
        startCell = currentCell;

        currentCell.SetDistance(targetCell.xCoord, targetCell.yCoord);

        openList.Add(currentCell);


        while (openList.Any())
        {
            Cell checkCell = openList.OrderBy(x => x.CostDistance).First();

            closeList.Add(checkCell);
            openList.Remove(checkCell);

            if (checkCell.xCoord == targetCell.xCoord && checkCell.yCoord == targetCell.yCoord)
            {
                finalpath.Add(checkCell);

                do
                {
                    finalpath.Add(finalpath.Last().Parent);
                } 
                while (finalpath.Last() != startCell);

                GetComponent<PlayerMovement>().SetPath(finalpath);
                openList.Clear();
                yield return null;
            }

            var walkableTiles = GetWalkableTiles(checkCell, targetCell);

            foreach (var walkableTile in walkableTiles)
            {
                if (closeList.Any(x => x.xCoord == walkableTile.xCoord && x.yCoord == walkableTile.yCoord))
                    continue;

                if (closeList.Any(x => x.xCoord == walkableTile.xCoord && x.yCoord == walkableTile.yCoord))
                {
                    var existingTile = closeList.First(x => x.xCoord == walkableTile.xCoord && x.yCoord == walkableTile.yCoord);
                    if (existingTile.CostDistance > checkCell.CostDistance)
                    {
                        closeList.Remove(existingTile);
                        closeList.Add(walkableTile);
                    }
                }
                else
                {
                    openList.Add(walkableTile);
                }
            }
        }
    }

    private static List<Cell> GetWalkableTiles(Cell currentCell, Cell targetCell)
    {
        var possibleTiles = new List<Cell>();

        Cell c1 = MazeCreateor.Instance.GetCellInPosition(currentCell.xCoord, currentCell.yCoord - 1);
        if (c1.Type == CellType.Road && c1.Parent == null) { c1.Parent = currentCell; c1.Cost = currentCell.Cost + 1; possibleTiles.Add(c1); }
        Cell c2 = MazeCreateor.Instance.GetCellInPosition(currentCell.xCoord, currentCell.yCoord + 1);
        if (c2.Type == CellType.Road && c2.Parent == null) { c2.Parent = currentCell; c2.Cost = currentCell.Cost + 1; possibleTiles.Add(c2); }
        Cell c3 = MazeCreateor.Instance.GetCellInPosition(currentCell.xCoord - 1, currentCell.yCoord);
        if (c3.Type == CellType.Road && c3.Parent == null) { c3.Parent = currentCell; c3.Cost = currentCell.Cost + 1; possibleTiles.Add(c3); }
        Cell c4 = MazeCreateor.Instance.GetCellInPosition(currentCell.xCoord + 1, currentCell.yCoord);
        if (c4.Type == CellType.Road && c4.Parent == null) { c4.Parent = currentCell; c4.Cost = currentCell.Cost + 1; possibleTiles.Add(c4); }


        possibleTiles.ForEach(tile => tile.SetDistance(targetCell.xCoord, targetCell.yCoord));

        return possibleTiles
                .Where(_cell => _cell.xCoord >= 0 && _cell.xCoord <= MazeCreateor.Instance.SizeX)
                .Where(_cell => _cell.yCoord >= 0 && _cell.yCoord <= MazeCreateor.Instance.SizeY)
            .ToList();
    }
}
