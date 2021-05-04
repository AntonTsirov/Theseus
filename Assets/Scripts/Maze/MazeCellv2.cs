using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//responsible for defining the features of a single cell
public class MazeCellv2 : MonoBehaviour
{
    public IntVector2 coordinates;
    public MazeRoom room;

    private MazeCellEdge[] edges = new MazeCellEdge[MazeDirections.Count];

    public MazeCellEdge GetEdge(MazeDirection direction)
    {
        return edges[(int)direction];
    }

    public void SetEdge(MazeDirection direction, MazeCellEdge edge)
    {
        edges[(int)direction] = edge;
        initializedEdgeCount += 1;
    }

    private int initializedEdgeCount;

    public bool IsFullyInitialized
    {
        get
        {
            return initializedEdgeCount == MazeDirections.Count;
        }
    }

    public MazeDirection RandomUninitializedDirection
    {
        get
        {
            int skips = Random.Range(0, MazeDirections.Count - initializedEdgeCount);
            for (int i = 0; i < MazeDirections.Count; i++)
            {
                if (edges[i] == null)
                {
                    if (skips == 0)
                    {
                        return (MazeDirection)i;
                    }
                    skips -= 1;
                }
            }
            throw new System.InvalidOperationException("MazeCell has no uninitialized directions left.");
        }
    }

    public void Initialize(MazeRoom room)
    {
        if (FindObjectOfType<Maze>().doorProbability != 0) room.Add(this);
    }
}
