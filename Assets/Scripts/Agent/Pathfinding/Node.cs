using UnityEngine;
using System.Collections;

//each node in the grid with its costs
public class Node : IHeapItem<Node>
{
    public bool walkable; //is a node walkable or has an obstacle on in
    public Vector3 worldPosition; //where that node is located in the world position
    public int gridX;
    public int gridY;

    public int gCost; //g cost as per the a* algoritm (start node to current node)
    public int hCost; //h cost as per the a* algoritm (estimated cost from current to target node)
    public Node parent; //var to point to the prev node
    int heapIndexVar;

    public Node(bool _walkable, Vector3 _worldPos, int _gridX, int _gridY)
    {
        walkable = _walkable;
        worldPosition = _worldPos;
        gridX = _gridX;
        gridY = _gridY;
    }

    public int fCost
    { //f cost is the sum of g and h as per a* algorithm
        get
        {
            return gCost + hCost;
        }
    }

    public int heapIndex
    {
        get
        {
            return heapIndexVar;
        }
        set
        {
            heapIndexVar = value;
        }
    }

    public int CompareTo(Node nodeToCompare)
    {
        int compare = fCost.CompareTo(nodeToCompare.fCost);
        if (compare == 0)
        {
            compare = hCost.CompareTo(nodeToCompare.hCost);
        }
        return -compare;
    }
}
