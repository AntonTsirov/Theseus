using UnityEngine;

public class MazeConstructor : MonoBehaviour
{
    //1
    public bool showDebug;
    private MazeDataGenerator dataGenerator;
    private MazeMeshGenerator meshGenerator;

    // [SerializeField] private Material mazeMat1;
    // [SerializeField] private Material mazeMat2;

    //2
    public int[,] data
    {
        get; private set;
    }

    //3
    void Awake()
    {
        meshGenerator = new MazeMeshGenerator();
        dataGenerator = new MazeDataGenerator();

    }

    public void GenerateNewMaze(int sizeRows, int sizeCols)
    {
        if (sizeRows % 2 == 0 && sizeCols % 2 == 0)
        {
            Debug.LogError("Odd numbers work better for dungeon size.");
        }

        data = dataGenerator.FromDimensions(sizeRows, sizeCols);

        DisplayMaze();
    }

    private void DisplayMaze()
    {
        GameObject go = new GameObject();
        go.transform.position = Vector3.zero;
        go.name = "Procedural Maze";
        go.tag = "Generated";

        MeshFilter mf = go.AddComponent<MeshFilter>();
        mf.mesh = meshGenerator.FromData(data);

        MeshCollider mc = go.AddComponent<MeshCollider>();
        mc.sharedMesh = mf.mesh;

        MeshRenderer mr = go.AddComponent<MeshRenderer>();
        //mr.materials = new Material[2] { mazeMat1, mazeMat2 };
    }
}