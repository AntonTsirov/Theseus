using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//responsible for defining an edge between two cells

public abstract class MazeCellEdge : MonoBehaviour
{
    public MazeCellv2 cell, otherCell;

    public MazeDirection direction;

    public void Initialize(MazeCellv2 cell, MazeCellv2 otherCell, MazeDirection direction)
    {
        this.cell = cell;
        this.otherCell = otherCell;
        this.direction = direction;
        cell.SetEdge(direction, this);
        transform.parent = cell.transform;
        transform.localPosition = Vector3.zero;
        transform.localRotation = direction.ToRotation();
        transform.name = direction.ToString();
    }

}
