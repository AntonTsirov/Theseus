using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//responsible for creating the grid of walkable nodes for the AI
public class GridForPath : MonoBehaviour
{
    public LayerMask unwalkableMask; //mask for the unwalkable terrain
    public Vector2 gridWorldSize; //the size of the grid
    public float nodeRadius; //how much space each node will take
    Node[,] grid; //the grid of the map
    float nodeDiameter; //node diamater
    int gridSizeX, gridSizeY;

    public void Awake()
    {
    }

    public int MaxSize
    {
        get { return gridSizeX * gridSizeY; }
    }

    public void CreateGrid()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        grid = new Node[gridSizeX, gridSizeY]; //create the node array or the so called grid
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2; //get the bottom-left edge of the grid's word position

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask));
                grid[x, y] = new Node(walkable, worldPoint, x, y); //with double loot populate the node array with nodes that are unwalkable if the sphere caster returns object with mask unwalkable
            }
        }
    }

    public Node NodeFromWorldPoint(Vector3 worldPosition) //method that returns node for a give world position of an object
    {
        float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (worldPosition.z + gridWorldSize.y / 2) / gridWorldSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
        return grid[x, y];
    }

    public List<Node> GetNeighbours(Node node) //return neighbours of a given node (8 at most)
    {
        List<Node> neighbours = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                    continue;

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    neighbours.Add(grid[checkX, checkY]);
                }
            }
        }

        return neighbours;
    }

    public List<Node> path;
    void OnDrawGizmos() //test to show the grid in the gizmos of Unity (debug)
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y)); // draw the size of the grid


        if (grid != null)
        {
            foreach (Node n in grid)
            {
                Gizmos.color = (n.walkable) ? Color.white : Color.red;
                if (path != null)
                {
                    if (path.Contains(n))
                    {
                        Gizmos.color = Color.gray;
                    }
                }
                Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - .1f)); //draw cube for each node: white for walkable and red for unwalkable
            }
        }
    }
}
